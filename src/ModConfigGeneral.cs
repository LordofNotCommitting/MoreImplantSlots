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
            this.ModData.AddConfigHeader("General Settings", "general");
            this.ModData.AddConfigValue("general", "about", "<color=#f51b1b>On existing save, existing clone with low implant count must have new augment installed to have implant count updated.</color>\n");
            this.ModData.AddConfigValue("general", "Implant_Count", 1, 1, 50, "Set Implant Multiplier", "Set the number of implant count multiplier you want per body part. (currently vanilla only has limit of 1 for all body part)");
            this.ModData.AddConfigValue("general", "Ignore_Implant_Type", false, "Ignore Implant type", "Make the system ignore implant type limitation when installing on a body part/augment.\n");
            this.ModData.AddConfigValue("general", "Ignore_Implant_Injury", false, "Ignore Implant Injury penalty", "Turn off disabling of active/passive implant from injuries. Health screen will suggest that they are off but they will continue to be active.\n");
            this.ModData.AddConfigValue("general", "Ignore_Implant_Charge_Use", false, "Implant use consume no Charge", "Implant charge will not be used upon activation.\n");

            this.ModData.AddConfigValue("general", "about_res", " Negative % turn malus into bonus upon installation. Below res malus negation apply to the enemies. Choose wisely. <color=#f51b1b>On existing save, existing clone must have new augment/implant installed to have below value updated.</color>\n");
            this.ModData.AddConfigValue("general", "Flat_Res_Malus_Multiplier", 100, -100, 100, "Flat Res Malus Multiplier %", "Set % of flat resistance malus from augments/implants. Lowest is -100%, Multiplies with steel within.");
            this.ModData.AddConfigValue("general", "Mult_Res_Malus_Multiplier", 100, -100, 100, "Mult Res Malus Multiplier %", "Set % of Percentage based resistance malus from augments/implants..");
            this.ModData.AddConfigValue("general", "Quasigen_Malus_Multiplier", 100, -100, 100, "Quasimorphosis Gain Multiplier %", "Set % of Quasimorphosis gain from augments/implants (rounded down).");
            this.ModData.AddConfigValue("general", "about_final", "<color=#f51b1b>The game must be restarted after setting then saving this config to take effect.</color>\n");
            this.ModData.RegisterModConfigData(ModName);
        }

        // Token: 0x04000011 RID: 17
        private string ModName;

        // Token: 0x04000012 RID: 18
        public ModConfigData ModData;

    }
}
