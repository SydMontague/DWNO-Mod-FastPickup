using HarmonyLib;
using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWNOPlugin
{
    class Patches
    {
        [HarmonyPatch(typeof(uItemPickPanel), "PickDefault")]
        [HarmonyPostfix]
        static void PickDefault_Postfix(uItemPickPanel __instance)
        {
            // set timers to 0, so we don't have to wait
            __instance.m_itemPickTimer = 0f;
            __instance.m_waitTime = 0f;
        }


        [HarmonyPatch(typeof(uItemPickPanel), "Update")]
        [HarmonyPostfix]
        static void Update_Postfix(uItemPickPanel __instance)
        {
            // the wait timer can get set in this function, make sure it is reset on exit
            __instance.m_waitTime = 0f;
            // only true on material pickup, makes pickup instant
            if(__instance.m_targetPartnerCtrl != null)
                __instance.m_targetPartnerCtrl.EmotionIdx = -1;
        }

        [HarmonyPatch(typeof(PlayerCtrl), "StartFeelingAndChangeFace")]
        [HarmonyPrefix]
        static bool StartFeelingAndChangeFace_Prefix(PlayerCtrl __instance, PlayerCtrl.FeelAnimID _animId)
        {
            // prevent player item pickup animation from playing
            if (_animId == PlayerCtrl.FeelAnimID.FeelAnimID_Pickup)
                return false;

            return true;
        }
    }
}
