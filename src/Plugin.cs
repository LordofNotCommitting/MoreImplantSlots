using HarmonyLib;
using MGSC;
using ModLoader_Bootstrap_MoreImplantSlots;
using MoreImplantSlots;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreImplantSlots
{
    public class Plugin : BootstrapMod
    {
        public Plugin(HookEvents hookEvents, bool isBeta) : base(hookEvents, isBeta)
        {
            hookEvents.AfterConfigsLoaded += AfterConfig;
        }
        public static string ModAssemblyName
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000043 RID: 67 RVA: 0x000032FB File Offset: 0x000014FB
        private static string ModPersistenceFolder
        {
            get
            {
                return Path.Combine(Application.persistentDataPath + "/../Quasimorph_ModConfigs", "LoC_MoreImplantSlots");
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000044 RID: 68 RVA: 0x00003316 File Offset: 0x00001516
        private static string ConfigPath
        {
            get
            {
                return Path.Combine(Plugin.ModPersistenceFolder, "config.txt");
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000045 RID: 69 RVA: 0x00003327 File Offset: 0x00001527
        private static string SavePath
        {
            get
            {
                return Path.Combine(Plugin.ModPersistenceFolder, "savedata.json");
            }
        }

        public static Logger Logger { get; private set; } = new Logger("");

        public static ModConfigGeneral ConfigGeneral { get; set; }

        public static ModSave Save { get; set; }

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            Plugin.ConfigGeneral = new ModConfigGeneral("More Implant Slots", Plugin.ConfigPath);
            Plugin.Save = new ModSave(Plugin.SavePath);

            new Harmony("LoC_" + Plugin.ModAssemblyName).PatchAll();

            ApplyImplantInject();
        }

        public static void ApplyImplantInject()
        {
            if (Data.WoundSlots == null)
            {
                Plugin.Logger.Log("Data is not ready.");
                return;
            }

            // Read the value here, AFTER ConfigGeneral is initialized
            int implant_slot_newval = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Implant_Count", 1);

            foreach (WoundSlotRecord slot in Data.WoundSlots.Records)
            {
                slot.ImplantSocketsMax *= implant_slot_newval;
                slot.ImplantSocketsDefault *= implant_slot_newval;
            }
        }

    }


}
