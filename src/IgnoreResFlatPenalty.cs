using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace MoreImplantSlots
{


    [HarmonyPatch(typeof(WoundEffectResist))]
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(string), typeof(string), typeof(string), typeof(float) })]
    public class IgnoreResFlatPenalty
    {
        static int flat_Res_Malus_Multiplier = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Flat_Res_Malus_Multiplier", 100);

        public static void Postfix(ref WoundEffectResist __instance, string slotType, string parentWoundId, string effectId, float value)
        {
            if (flat_Res_Malus_Multiplier < 100)
            {
                if (__instance.Value < 0)
                {
                    float temp_multiplier = (float)flat_Res_Malus_Multiplier / 100.0f;
                    __instance.Value = (int)(__instance.Value * temp_multiplier);
                }

            }
        }

    }
}
