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

    [HarmonyPatch(typeof(ImplicitAugEffect), nameof(ImplicitAugEffect.RemoveBonusEffects))]
    public class IgnoreInjuryonImplantPassive
    {
        static bool ignore_implant_injury = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Ignore_Implant_Injury", false);

        //passive effect is kept
        public static bool Prefix(EffectsController effectsController, string woundSlotId)
        {
            return !ignore_implant_injury;
        }



    }
}
