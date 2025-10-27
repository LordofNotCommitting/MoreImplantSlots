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

    [HarmonyPatch(typeof(AugmentationSystem), nameof(AugmentationSystem.GetImplantSocketsCount))]
    public class NewLimbs
    {
        //steam mod ID 3594238447
        static int implant_slot_newval = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Implant_Count", 1);
        //[HarmonyPatch(typeof(WoundSlotRecord), "ImplantSocketsDefault", MethodType.Getter)]

        public static int Postfix(int __result)
        {
            return implant_slot_newval;
        }



    }
}
