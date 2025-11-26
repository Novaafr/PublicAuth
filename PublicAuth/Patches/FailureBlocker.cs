using System;
using GorillaNetworking;
using HarmonyLib;

namespace PublicAuth
{
    [HarmonyPatch(typeof(GorillaComputer), "GeneralFailureMessage")]
    public class FailureBlocker
    {
        [HarmonyPrefix]
        private static bool Prefix()
        {
            return false;
        }
    }
}
