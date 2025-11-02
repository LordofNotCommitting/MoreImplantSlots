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
            this.ModData.AddConfigValue("general", "Ignore_Implant_Type", false, "STRING:Ignore Implant type", "STRING:Make the system ignore implant type limitation when installing on a body part/augment.\n");
            this.ModData.AddConfigValue("general", "Ignore_Implant_Injury", false, "STRING:Ignore Implant Injury penalty", "STRING:Turn off disabling of active/passive implant from injuries. Health screen will suggest that they are off but they will continue to be active.\n");

            this.ModData.AddConfigValue("general", "about_res", "STRING:Below res malus negation may apply to the enemies. Choose wisely. <color=#f51b1b>On existing save, existing clone must have new augment/implant installed to have below value updated.</color>\n");
            this.ModData.AddConfigValue("general", "Flat_Res_Malus_Multiplier", 100, 0, 100, "STRING:Flat Res Malus Multiplier %", "STRING:Set % of flat resistance malus from augments/implants. Lowest is 0%, Multiplies with steel within.");
            this.ModData.AddConfigValue("general", "Mult_Res_Malus_Multiplier", 100, 0, 100, "STRING:Mult Res Malus Multiplier %", "STRING:Set % of Percentage based resistance malus from augments/implants. Lowest is 0%.");
            this.ModData.AddConfigValue("general", "Quasigen_Malus_Multiplier", 100, 0, 100, "STRING:Quasimorphosis Gain Multiplier %", "STRING:Set % of Quasimorphosis gain from augments/implants (rounded down). Lowest is 0%.");
            this.ModData.AddConfigValue("general", "about_final", "STRING:<color=#f51b1b>The game must be restarted after setting then saving this config to take effect.</color>\n");
            this.ModData.RegisterModConfigData(ModName);
        }

        // Token: 0x04000011 RID: 17
        private string ModName;

        // Token: 0x04000012 RID: 18
        public ModConfigData ModData;

    }
}
