using HarmonyLib;
using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicAuth.Patches
{
    [HarmonyPatch(typeof(Message), "get_IsError")]
    internal class AuthPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result)
        {
            __result = false;
        }
    }
}
