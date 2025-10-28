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

    [HarmonyPatch(typeof(WoundSystem), nameof(WoundSystem.HasAnyWoundInSlot))]
    public class IgnoreInjuryonImplantActive
    {
        static bool ignore_implant_injury = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Ignore_Implant_Injury", false);

        //passive effect is kept

        public static bool Prefix(CreatureData creatureData, string woundSlotId)
        {
            if (ignore_implant_injury)
            {
                return false;
            }
            return true;
        }

        public static void Postfix(ref bool __result)
        {
            if (ignore_implant_injury)
            {
                __result = false;
            }
        }


    }
}
