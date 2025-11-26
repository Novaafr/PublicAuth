using System;
using GorillaNetworking;
using HarmonyLib;

[HarmonyPatch(typeof(PlayFabAuthenticator), "AuthenticateWithPlayFab")]
internal class PlayFabPatch
{
    [HarmonyPrefix]
    private static bool Prefix()
    {
        return false;
    }
}
