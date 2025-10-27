using MGSC;
using ModConfigMenu.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreImplantSlots
{
    // Token: 0x02000006 RID: 6
    public class ModConfigGeneral
    {
        // Token: 0x0600001D RID: 29 RVA: 0x00002840 File Offset: 0x00000A40
        public ModConfigGeneral(string ModName, string ConfigPath)
        {
            this.ModName = ModName;
            this.ModData = new ModConfigData(ConfigPath);
            this.ModData.AddConfigHeader("STRING:General Settings", "general");
            this.ModData.AddConfigValue("general", "about", "STRING:<color=#f51b1b>On existing save, existing clone with low implant count must have new augment installed to have implant count updated.</color>\n");
            this.ModData.AddConfigValue("general", "Implant_Count", 1, 1, 10, "STRING:Set Implant Count", "STRING:Set the number of implant count you want per body part.");
            this.ModData.AddConfigValue("general", "about2", "STRING:<color=#f51b1b>The game must be restarted after setting then saving this config to take effect.</color>\n");
            this.ModData.RegisterModConfigData(ModName);
        }

        // Token: 0x04000011 RID: 17
        private string ModName;

        // Token: 0x04000012 RID: 18
        public ModConfigData ModData;

    }
}
