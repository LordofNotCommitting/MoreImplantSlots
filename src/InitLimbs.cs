using HarmonyLib;
using MGSC;
using System;
using System.Collections;
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

    [HarmonyPatch(typeof(CreatureSystem), nameof(CreatureSystem.InitWoundSlots))]
    public class InitLimbs
    {
        //steam mod ID 3594238447
        static int implant_slot_newval = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Implant_Count", 1);
        //[HarmonyPatch(typeof(WoundSlotRecord), "ImplantSocketsDefault", MethodType.Getter)]

        public static Dictionary<string, ImplantSocketData> Postfix(Dictionary<string, ImplantSocketData> __result)
        {
            foreach (string key in __result.Keys.ToList())
            {
                __result[key].TotalSockets = implant_slot_newval;
            }
            return __result;
        }
    }
}
