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
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace MoreImplantSlots
{

    [HarmonyPatch(typeof(TooltipFactory), nameof(TooltipFactory.BuildWoundTooltip))]
    public class AppendBuildWoundTooltip
    {
        static bool ignore_implant_injury = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Ignore_Implant_Injury", false);

        //passive effect is kept
        public static void Postfix(ref TooltipFactory __instance, string woundSlotId, List<BodyPartWound> wounds, List<ImplicitAugEffect> implicitAugEffects, EffectsController effectsController, int maxSockets, List<string> installedImplants)
        {
            if (installedImplants.Count > 0)
            {
                //__instance.AddPanelToTooltip().SetIcon("common_implant_sockets_green").LocalizeName("woundtype.noimplants").SetNameColor(Colors.AltGreen);
                //bring back the old
                __instance.AddPanelToTooltip().SetIcon("common_implant_sockets_green").LocalizeName("tooltip.ImplantSockets").SetNameColor(Colors.AltGreen)
                .SetValue(string.Format("{0}{1}", installedImplants.Count, ("/" + maxSockets).WrapInColor(Colors.DarkYellow)));
            }
        }



    }
}
