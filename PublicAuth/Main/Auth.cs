using GorillaNetworking;
using MelonLoader;
using Photon.Realtime;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace PublicAuth.Main
{
    internal class Auth
    {
        // cred to covid

        public static string CustomID;

        public static void AuthFully()
        {
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
            {
                CustomId = CustomID
            }, new System.Action<LoginResult>(OnAuthed), new System.Action<PlayFabError>(OnLoginError));
        }

        private static void OnLoginError(PlayFabError error)
        {
            HandleLoginError(error);
        }
        private static void HandleLoginError(PlayFabError error)
        {
            currentRetryCount++;
            string text = "Login failed: " + error.ErrorMessage;
            string text2 = ((error.ErrorDetails != null) ? string.Format("\nError Details: {0}", error.ErrorDetails) : "");
            if (currentRetryCount <= 3)
            {
                MelonLogger.Error("Failed : " + text);
                return;
            }
            ClearCredentials();
        }

        private static void ClearCredentials()
        {
            PlayFabId = null;
            SessionTicket = null;
        }
        private static int currentRetryCount;
        public static bool LoggedIn;
        static void OnAuthed(LoginResult result)
        {
            PhotonNetwork.ConnectToRegion("usw");
            PhotonNetwork.ConnectUsingSettings();
            PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
            PlayFabAuthenticator.instance.RequestPhotonToken(result.PlayFabId, result.SessionTicket);
            MelonLoader.MelonLogger.Msg($"Authed to playfab {PlayFabSettings.staticPlayer.IsClientLoggedIn()} | Photon Authed {PhotonNetwork.IsConnected}");
            if (result == null)
            {
                return;
            }
            LoggedIn = true;
            PlayFabId = result.PlayFabId;
            SessionTicket = result.SessionTicket;
            GetPhotonToken();
            if (CosmeticsController.instance != null)
            {
                CosmeticsController.instance.Initialize();
            }
            if (GorillaTagger.Instance.offlineVRRig != null)
            {
                GorillaTagger.Instance.offlineVRRig.GetUserCosmeticsAllowed();
            }
            GorillaComputer.instance.OnConnectedToMasterStuff();
            GameObject.Find("Photon Manager").GetComponent<PhotonNetworkController>().InitiateConnection();
            MelonLoader.MelonLogger.Msg($"2: Authed to playfab {PlayFabSettings.staticPlayer.IsClientLoggedIn()} | Photon Authed {PhotonNetwork.IsConnected}");
        }
        public static string PlayFabId;
        public static string Nonce;
        public static string UserID;
        public static string OrgID;
        private static string SessionTicket;
        private static void OnPhotonTokenReceived(GetPhotonAuthenticationTokenResult result)
        {
            if (result == null)
            {
                return;
            }
            PhotonStart(result);
        }
        private static void PhotonStart(GetPhotonAuthenticationTokenResult obj)
        {
            if (obj == null)
            {
                return;
            }
            AuthenticationValues authenticationValues = new AuthenticationValues();
            authenticationValues.AuthType = CustomAuthenticationType.Custom;
            authenticationValues.AddAuthParameter("username", PlayFabId);
            authenticationValues.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);
            PhotonNetwork.AuthValues = authenticationValues;
            SetupGameSpecificInitializations();
        }
        private static void SetupGameSpecificInitializations()
        {
            GorillaTagger.Instance.offlineVRRig.GetUserCosmeticsAllowed();
            CosmeticsController.instance.Initialize();
            GameObject.Find("Photon Manager").GetComponent<PhotonNetworkController>().wrongVersion = false;
            GorillaComputer.instance.OnConnectedToMasterStuff();
            GameObject.Find("Photon Manager").GetComponent<PhotonNetworkController>().InitiateConnection();
        }
        public static void GetPhotonToken()
        {
            PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest
            {
                PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
            }, new System.Action<GetPhotonAuthenticationTokenResult>(OnPhotonTokenReceived), new System.Action<PlayFabError>(OnPhotonTokenError), null, null);
        }
        private static void OnPhotonTokenError(PlayFabError error)
        {
            MelonLoader.MelonLogger.Msg(string.Format("Error getting photon token: {0}, {1}", error.ErrorMessage, error.ErrorDetails));
        }
    }
}
