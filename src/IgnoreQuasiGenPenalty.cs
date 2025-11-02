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


    [HarmonyPatch(typeof(WoundEffectQmorph))]
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(string), typeof(string), typeof(string), typeof(float) })]
    public class IgnoreQuasiGenPenalty
    {
        static int quasigen_Malus_Multiplier = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Quasigen_Malus_Multiplier", 100);

        public static void Postfix(ref WoundEffectQmorph __instance, string slotType, string parentWoundId, string effectId, float value)
        {
            if (quasigen_Malus_Multiplier < 100)
            {
                if (__instance.QmorphValue > 0)
                {
                    float temp_multiplier = (float)quasigen_Malus_Multiplier / 100.0f;
                    __instance.QmorphValue = (int)(__instance.QmorphValue * temp_multiplier);
                }

            }
        }

    }
}
