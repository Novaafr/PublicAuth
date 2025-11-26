
using MelonLoader;
using Photon.Pun;
using PlayFab;
using PublicAuth;
using PublicAuth.Main;
using System.IO;
using UnityEngine;

[assembly: MelonInfo(typeof(Plugin), "PublicAuth", "1.0.0", "Nova")]
[assembly: MelonGame()]
namespace PublicAuth
{
    public class Plugin : MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            URLS.SetUp();
            if (!File.Exists(Path.Combine(Application.persistentDataPath, "CustomId.txt")))
            {
                File.Create(Path.Combine(Application.persistentDataPath, "CustomId.txt"));
            }
        }

        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();
            Auth.CustomID = File.ReadAllText(Path.Combine(Application.persistentDataPath, "CustomId.txt"));

            if (string.IsNullOrEmpty(Auth.CustomID))
            {
                Log("Custom id is blank");
                return;
            }

            Auth.AuthFully();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (PhotonNetwork.IsConnected && !PlayFabSettings.staticPlayer.IsClientLoggedIn())
            {
                URLS.HudText.text = $"Playfab not connected\nPlease make a customid at |{URLS.CustomIDMaker}| and make a valid custom id and put it in the CustomId.txt in your game files\n\nJoin the discord {URLS.Discord}\nMod made by Nova (@novaafr)";
            }
            if (!PhotonNetwork.IsConnected && PlayFabSettings.staticPlayer.IsClientLoggedIn())
            {
                URLS.HudText.text = $"Photon not connected\nPlease make a customid at |{URLS.CustomIDMaker}| and make a valid custom id and put it in the CustomId.txt in your game files\n\nJoin the discord {URLS.Discord}\nMod made by Nova (@novaafr)";
            }
            if (!PhotonNetwork.IsConnected && !PlayFabSettings.staticPlayer.IsClientLoggedIn())
            {
                URLS.HudText.text = $"Somehow nothing is connected\nPlease make a customid at |{URLS.CustomIDMaker}| and make a valid custom id and put it in the CustomId.txt in your game files\n\nJoin the discord {URLS.Discord}\nMod made by Nova (@novaafr)";
            }

            if (PhotonNetwork.IsConnected && PlayFabSettings.staticPlayer.IsClientLoggedIn())
            {
                URLS.HudText.text = "";
            }
        }

        public static void Log(string msg) => MelonLogger.Msg($"[PUBLIC AUTH] Log : {msg}");
    }
}