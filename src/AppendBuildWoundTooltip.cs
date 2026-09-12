using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Pool;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace MoreImplantSlots
{

    [HarmonyPatch(typeof(TooltipFactory), nameof(TooltipFactory.BuildWoundTooltip))]
    public class AppendBuildWoundTooltip
    {

        //passive effect is kept

        static bool ignore_implant_injury = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Ignore_Implant_Injury", false);

        public static bool Prefix(ref TooltipFactory __instance, string woundSlotId, List<BodyPartWound> wounds, List<ImplicitAugEffect> implicitAugEffects, EffectsController effectsController, int maxSockets, ImplantSocketData socketData)
        {

            if (wounds.Any((BodyPartWound wound) => wound.IsCompletelyGone))
            {
                return false;
            }
            __instance._attachedEffectsIds.Clear();
            bool flag = wounds.Count > 0;
            PropertiesTooltip propertiesTooltip = __instance.BuildEmptyTooltip(wide: false, flag);
            propertiesTooltip.SetCaption1(Localization.Get("woundslot." + woundSlotId + ".name"), flag ? __instance._captionFirstLetterRedColor : __instance._captionFirstLetterColor);
            WoundSlotRecord record = Data.WoundSlots.GetRecord(woundSlotId);
            string nature = record.NatureType;
            bool num = record.ImplicitBonusEffects.Count == 0 && record.ImplicitPenaltyEffects.Count == 0;
            bool flag2 = socketData.InstalledImplantsData.Count == 0;
            bool flag3 = ignore_implant_injury || !wounds.Any() || wounds.All((BodyPartWound wound) => wound.IsMinor);
            if (num && flag2)
            {
                if (wounds.Count == 0)
                {
                    __instance.AddPanelToTooltip().SetIcon("common_no_effects").LocalizeName("woundtype.noeffects").SetNameColor(Colors.AltGreen);
                }
            }
            else
            {
                Dictionary<string, float> dictionary = CollectionPool<Dictionary<string, float>, KeyValuePair<string, float>>.Get();
                Dictionary<string, float> dictionary2 = CollectionPool<Dictionary<string, float>, KeyValuePair<string, float>>.Get();
                foreach (ImplicitAugEffect implicitAugEffect in implicitAugEffects)
                {
                    if (flag3)
                    {
                        foreach (int bonusEffect in implicitAugEffect.BonusEffects)
                        {
                            WoundEffect woundEffect = effectsController.First<WoundEffect>(bonusEffect);
                            if (IsValidEffect(woundEffect))
                            {
                                dictionary.TryGetValue(woundEffect.EffectId, out float currentVal);
                                dictionary[woundEffect.EffectId] = currentVal + woundEffect.ViewValue;
                            }
                        }
                    }
                    foreach (int penaltyEffect in implicitAugEffect.PenaltyEffects)
                    {
                        WoundEffect woundEffect2 = effectsController.First<WoundEffect>(penaltyEffect);
                        if (IsValidEffect(woundEffect2))
                        {
                            dictionary2.TryGetValue(woundEffect2.EffectId, out float currentVal2);
                            dictionary2[woundEffect2.EffectId] = currentVal2 + woundEffect2.ViewValue;
                        }
                    }
                }
                __instance.AddEffectsBlock(dictionary, dictionary2, offset: false, altGreen: false);
                CollectionPool<Dictionary<string, float>, KeyValuePair<string, float>>.Release(dictionary);
                CollectionPool<Dictionary<string, float>, KeyValuePair<string, float>>.Release(dictionary2);
            }
            List<string> woundsCache = CollectionPool<List<string>, string>.Get();
            bool flag4 = wounds.Any((BodyPartWound w) => w.WoundCategory != WoundCategory.Minor);
            foreach (BodyPartWound wound in wounds)
            {
                if (!woundsCache.Contains(wound.DmgType))
                {
                    woundsCache.Add(wound.DmgType);
                }
            }
            AddHeaders(WoundCategory.Normal, isFixated: true, wounds, woundsCache, nature, ref __instance);
            AddHeaders(WoundCategory.Amputation, isFixated: true, wounds, woundsCache, nature, ref __instance);
            if (!flag4)
            {
                AddHeaders(WoundCategory.Minor, isFixated: true, wounds, woundsCache, nature, ref __instance);
            }
            AddHeaders(WoundCategory.Normal, isFixated: false, wounds, woundsCache, nature, ref __instance);
            AddHeaders(WoundCategory.Amputation, isFixated: false, wounds, woundsCache, nature, ref __instance);
            if (!flag4)
            {
                AddHeaders(WoundCategory.Minor, isFixated: false, wounds, woundsCache, nature, ref __instance);
            }
            CollectionPool<List<string>, string>.Release(woundsCache);
            Dictionary<string, float> dictionary3 = CollectionPool<Dictionary<string, float>, KeyValuePair<string, float>>.Get();
            foreach (BodyPartWound wound2 in wounds)
            {
                if (wound2.WoundCategory == WoundCategory.Minor && flag4)
                {
                    continue;
                }
                foreach (int subWound in wound2.SubWounds)
                {
                    WoundEffect woundEffect3 = effectsController.First<WoundEffect>(subWound);

                    // Check condition first
                    if (!woundEffect3.IsFixable || !wound2.IsFixated)
                    {
                        // TryGetValue gets existing value, or 0f if the key doesn't exist yet
                        dictionary3.TryGetValue(woundEffect3.EffectId, out float currentVal3);

                        // Add the value once
                        dictionary3[woundEffect3.EffectId] = currentVal3 + woundEffect3.ViewValue;
                    }
                }
            }
            foreach (KeyValuePair<string, float> item in dictionary3)
            {
                __instance.AddWoundEffectProperty(item.Key, item.Value);
            }
            CollectionPool<Dictionary<string, float>, KeyValuePair<string, float>>.Release(dictionary3);
            foreach (BodyPartWound wound3 in wounds)
            {
                if (wound3.HasAttachedEffect && !__instance._attachedEffectsIds.Contains(wound3.AttachedEffectId))
                {
                    string text = Localization.Get("wound." + wound3.AttachedEffectId + ".name");
                    StatusEffect statusEffect = StatusEffectsSystem.FindById(effectsController, wound3.AttachedEffectId);
                    if (statusEffect != null)
                    {
                        __instance.AddPanelToTooltip().SetName(text.SafeFormat(FormatHelper.To100Percent(statusEffect.ViewValue))).SetNameColor(Colors.Yellow);
                        __instance._attachedEffectsIds.Add(wound3.AttachedEffectId);
                    }
                }
            }
            foreach (BodyPartWound wound4 in wounds)
            {
                if (wound4.IsFixated)
                {
                    string text2 = FormatHelper.To100Percent(wound4.IsAmputation ? 0f : WoundSystem.GetWoundSlotAvgResolutionChance(woundSlotId, effectsController));
                    string text3 = Localization.Get("tooltip.TicksToResolution");
                    string text4 = FormatHelper.ToInt(wound4.ResolutionTick) + " " + Localization.Get("ui.label.turns");
                    __instance.AddPanelToTooltip().SetIcon("common_time_with_plus").SetName(text3.SafeFormat(text4.WrapInColor(Colors.White))).SetNameColor(Colors.Green)
                        .SetValue(text2.WrapInColor(Colors.White))
                        .SetValueColor(Colors.White);
                    break;
                }
            }
            if (flag2)
            {
                __instance.AddPanelToTooltip().SetIcon("common_implant_sockets_green").LocalizeName("woundtype.noimplants").SetNameColor(Colors.AltGreen);
            }
            else
            {
                /*
                foreach (InstalledImplantData installedImplantsDatum in socketData.InstalledImplantsData)
                {
                    ImplantRecord simpleRecord = Data.Items.GetSimpleRecord<ImplantRecord>(installedImplantsDatum.ImplantId);
                    TooltipProperty tooltipProperty = __instance.AddPanelToTooltip().SetIcon("common_implant_sockets").SetNameColor(Colors.Green);

                    LocalizeNameAndNum("item." + installedImplantsDatum.ImplantId + ".name", 3, ref tooltipProperty);
                    if (simpleRecord.HasCharges)
                    {
                        tooltipProperty.SetValue($"{installedImplantsDatum.CurrentCharges}/{simpleRecord.Charges}").SetValueColor(Colors.White);
                    }
                }
                */
                var groupedImplants = socketData.InstalledImplantsData.GroupBy(data => data.ImplantId);

                foreach (var group in groupedImplants)
                {
                    string implantId = group.Key;
                    int implantCount = group.Count();

                    ImplantRecord simpleRecord = Data.Items.GetSimpleRecord<ImplantRecord>(implantId);
                    TooltipProperty tooltipProperty = __instance.AddPanelToTooltip().SetIcon("common_implant_sockets").SetNameColor(Colors.Green);

                    // Set localized name and total count
                    LocalizeNameAndNum("item." + implantId + ".name", implantCount, ref tooltipProperty);

                    if (simpleRecord.HasCharges)
                    {
                        // Sum current charges across all duplicate implants of this type
                        int totalCurrentCharges = group.Sum(d => d.CurrentCharges);

                        // Sum max charges (simpleRecord.Charges * number of implants)
                        int totalMaxCharges = simpleRecord.Charges * implantCount;

                        tooltipProperty.SetValue($"{totalCurrentCharges}/{totalMaxCharges}").SetValueColor(Colors.White);
                    }
                }




            }
            propertiesTooltip.SetCaption2(Localization.Get("woundslot.nature." + nature));
            

            if (socketData.InstalledImplantsData.Count > 0)
            {
                //__instance.AddPanelToTooltip().SetIcon("common_implant_sockets_green").LocalizeName("woundtype.noimplants").SetNameColor(Colors.AltGreen);
                //bring back the old
                __instance.AddPanelToTooltip().SetIcon("common_implant_sockets_green").LocalizeName("tooltip.ImplantSockets").SetNameColor(Colors.AltGreen)
                .SetValue(string.Format("{0}{1}", socketData.InstalledImplantsData.Count, ("/" + socketData.TotalSockets).WrapInColor(Colors.DarkYellow)));
            }
            return false;
        }

        public static void AddHeaders(WoundCategory category, bool isFixated, List<BodyPartWound> wounds, List<string> woundsCache, string nature, ref TooltipFactory __instance)
        {
            foreach (string dmgType in woundsCache)
            {
                int num2 = wounds.Count((BodyPartWound w) => w.DmgType == dmgType && w.IsFixated == isFixated && w.WoundCategory == category);
                if (num2 > 0)
                {
                    __instance.AddWoundHeaderProperty(category, nature, dmgType, num2, isFixated);
                }
            }
        }
        static bool IsValidEffect(BaseEffect effect)
        {
            if (effect == null)
            {
                return false;
            }
            if (effect is WoundEffectMaxHealth { MaxHealthPenalty: 0 })
            {
                return false;
            }
            return true;
        }

        static public TooltipProperty LocalizeNameAndNum(string tag, int num, ref TooltipProperty ttp)
        {
            string addString = "";
            if (num > 1) 
            {
                addString = " X " + num;
            }
            return ttp.SetName(Localization.Get(tag, true) + addString);
        }

        /*
        public static void Postfix(ref TooltipFactory __instance, string woundSlotId, List<BodyPartWound> wounds, List<ImplicitAugEffect> implicitAugEffects, EffectsController effectsController, int maxSockets, ImplantSocketData socketData)
        {
            
            if (socketData.InstalledImplantsData.Count > 0)
            {
                //__instance.AddPanelToTooltip().SetIcon("common_implant_sockets_green").LocalizeName("woundtype.noimplants").SetNameColor(Colors.AltGreen);
                //bring back the old
                __instance.AddPanelToTooltip().SetIcon("common_implant_sockets_green").LocalizeName("tooltip.ImplantSockets").SetNameColor(Colors.AltGreen)
                .SetValue(string.Format("{0}{1}", socketData.InstalledImplantsData.Count, ("/" + socketData.TotalSockets).WrapInColor(Colors.DarkYellow)));
            }
        }
        */



    }
}
