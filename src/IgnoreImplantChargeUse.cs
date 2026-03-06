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

    [HarmonyPatch(typeof(ActiveAbilitySystem), nameof(ActiveAbilitySystem.SpendImplantCharge))]
    public class IgnoreImplantChargeUse
    {
        //steam mod ID 3594238447
        static bool ignore_implant_charge_use = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Ignore_Implant_Charge_Use", false);
        //[HarmonyPatch(typeof(WoundSlotRecord), "ImplantSocketsDefault", MethodType.Getter)]

        public static bool Prefix(CreatureData creatureData, ImplantRecord record)
        {
            return !ignore_implant_charge_use;
        }



    }
}
