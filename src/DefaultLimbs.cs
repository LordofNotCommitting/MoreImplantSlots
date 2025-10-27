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

    [HarmonyPatch(typeof(AugmentationSystem), nameof(AugmentationSystem.RestoreDefaultWoundSlots))]
    public class DefaultLimbs
    {
        //steam mod ID 3594238447
        static int implant_slot_newval = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Implant_Count", 1);
        //[HarmonyPatch(typeof(WoundSlotRecord), "ImplantSocketsDefault", MethodType.Getter)]


        public static void Postfix(ref Mercenary mercenary)
        {
            Dictionary<string, ImplantSocketData> woundSlotMap = mercenary.CreatureData.WoundSlotMap;
            foreach (string key in woundSlotMap.Keys.ToList())
            {
                if (woundSlotMap[key].TotalSockets != implant_slot_newval)
                {
                    woundSlotMap[key].TotalSockets = implant_slot_newval;
                }
            }
        }


    }
}
