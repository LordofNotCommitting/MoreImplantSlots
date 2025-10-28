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

    [HarmonyPatch(typeof(AugmentationSystem), nameof(AugmentationSystem.Implant))]
    public class IgnoreImplantType
    {
        //steam mod ID 3594238447
        static bool ignore_implant_type = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Ignore_Implant_Type", false);
        //[HarmonyPatch(typeof(WoundSlotRecord), "ImplantSocketsDefault", MethodType.Getter)]

        public static bool Prefix(Mercenary mercenary, BasePickupItem item, ItemStorage activeCargo, PerkFactory perkFactory)
        {
            ImplantRecord implantRecord = item.Record<ImplantRecord>();
            CreatureData creatureData = mercenary.CreatureData;
            string woundSlotBySlotType = WoundSystem.GetWoundSlotBySlotType(creatureData.WoundSlotMap, implantRecord.SlotType);
            ImplantSocketData implantSocketData;
            if (!creatureData.WoundSlotMap.TryGetValue(woundSlotBySlotType, out implantSocketData))
            {
                SingletonMonoBehaviour<SoundController>.Instance.PlayUiSound(SingletonMonoBehaviour<SoundsStorage>.Instance.EmptyAttack, false, 0f);
                return false;
            }
            if (!ignore_implant_type) {
                WoundSlotRecord record = Data.WoundSlots.GetRecord(woundSlotBySlotType, true);
                if (record == null || !implantRecord.NatureTypes.Contains(record.NatureType))
                {
                    SingletonMonoBehaviour<SoundController>.Instance.PlayUiSound(SingletonMonoBehaviour<SoundsStorage>.Instance.EmptyAttack, false, 0f);
                    return false;
                }
            }
            if (implantSocketData.HasEmptySocket())
            {
                implantSocketData.InstalledImplants.Add(implantRecord.Id);
                if (implantRecord.IsActive)
                {
                    AugmentationSystem.AddActiveImplant(creatureData, implantRecord.Id, perkFactory);
                }
                ItemInteractionSystem.ConsumeItem(item);
                AugmentationSystem.ConfigureImplicitEffects(creatureData);
                return false;
            }
            if (implantSocketData.InstalledImplants.Count > 0)
            {
                string implantId = implantSocketData.InstalledImplants.First<string>();
                AugmentationSystem.RemoveImplant(creatureData, implantSocketData, implantId, activeCargo, true);
                implantSocketData.InstalledImplants.Add(implantRecord.Id);
                if (implantRecord.IsActive)
                {
                    AugmentationSystem.AddActiveImplant(creatureData, implantRecord.Id, perkFactory);
                }
                ItemInteractionSystem.ConsumeItem(item);
                AugmentationSystem.ConfigureImplicitEffects(creatureData);
                return false;
            }
            SingletonMonoBehaviour<SoundController>.Instance.PlayUiSound(SingletonMonoBehaviour<SoundsStorage>.Instance.EmptyAttack, false, 0f);

            return false;
        }



    }
}
