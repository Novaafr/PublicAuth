using Photon.Pun;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

namespace PublicAuth.Main
{
    public class URLS
    {
        public const string CustomIDMaker = "https://meowmeowmeowmeow.pythonanywhere.com/";
        public const string Discord = "https://discord.gg/M4NQ6BTywD";
        public const string COCText = "Thank you for using public auth\nI made this auth public for apk method games since there kinda getting boring\nhave fun modding\n\nThis mod was made by Nova (@novaafr)";
        public const string COCTextTop = "Public Auth";
        public static string UserId;
        public static string TitleId;
        public static string RT;
        public static string VC;
        public static string V;

        public static GameObject Hud;
        public static Text HudText;

        public static void SetUp()
        {
            UserId = PhotonNetwork.IsConnected ? PhotonNetwork.LocalPlayer.UserId : "Not connected to photon";

            TitleId = PlayFabSettings.TitleId;
            RT = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            VC = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;
            V = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;

            (Hud, HudText) = CreateTextGUI("", "HUD", TextAnchor.MiddleCenter, new Vector3(0, 0, 2));

            if (GameObject.Find("COC Text"))
            {
                GameObject.Find("COC Text").GetComponent<Text>().text = COCText.ToUpper();
            }
            if(GameObject.Find("CodeOfConduct"))
            {
                GameObject.Find("CodeOfConduct").GetComponent<Text>().text = COCTextTop.ToUpper();
            }

            if (GameObject.Find("motdtext"))
            {
                GameObject.Find("motdtext").GetComponent<Text>().text = $"TitleId: {TitleId}\nRealtime: {RT}\nVoice: {VC}\nVersion: {V}\nPlayerId: {UserId}.ToUpper()";
            }
            if (GameObject.Find("motd"))
            {
                GameObject.Find("motd").GetComponent<Text>().text = "Public Auth".ToUpper();
            }
        }


        // Credit to ColossusYTTV
        private static Material mat = new Material(Shader.Find("GUI/Text Shader"));

        public static (GameObject, Text) CreateTextGUI(string text, string name, TextAnchor alignment, Vector3 loctrans)
        {
            GameObject HUDObj = new GameObject();
            HUDObj.name = name;

            Canvas canvas = HUDObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            HUDObj.AddComponent<CanvasScaler>();
            HUDObj.AddComponent<GraphicRaycaster>();

            RectTransform rectTransform = HUDObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(5, 5);
            HUDObj.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);

            GameObject menuTextObj = new GameObject();
            menuTextObj.transform.SetParent(HUDObj.transform);
            Text MenuText = menuTextObj.AddComponent<Text>();
            MenuText.text = text;
            MenuText.fontSize = 10;
            MenuText.font = GameObject.Find("COC Text").GetComponent<Text>().font;
            MenuText.rectTransform.sizeDelta = new Vector2(260, 180);
            MenuText.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            MenuText.rectTransform.localPosition = loctrans;
            MenuText.material = mat;
            MenuText.alignment = alignment;

            // Set the parent and adjust for camera position
            HUDObj.transform.SetParent(Camera.main.transform);
            HUDObj.transform.position = Camera.main.transform.position;
            HUDObj.transform.rotation = Camera.main.transform.rotation;

            return (HUDObj, MenuText);
        }
    }
}
