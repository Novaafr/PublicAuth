using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oculus.Platform;
using PlayFab;
using PlayFab.Internal;

namespace PublicAuth.Patches
{
    [HarmonyPatch(typeof(PlayFabDeviceUtil), "SendDeviceInfoToPlayFab")]
    internal class DeviceInfoV2
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            return false;
        }
    }
}
