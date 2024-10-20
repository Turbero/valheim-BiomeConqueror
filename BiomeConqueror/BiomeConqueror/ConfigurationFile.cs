using BepInEx.Configuration;
using BepInEx;
using System;
using System.Linq;

namespace BiomeConqueror
{
    internal class ConfigurationFile
    {
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> debug;
        //public static ConfigEntry<bool> serverProgress;
        public static ConfigEntry<bool> bonemassBenefitEnabled;
        public static ConfigEntry<bool> moderBenefitEnabled;
        public static ConfigEntry<bool> queenBenefitEnabled;
        public static ConfigEntry<float> queenBenefitRange;

        private static ConfigFile config;

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                config = plugin.Config;

                modEnabled = config.Bind<bool>("1 - General", "Enabled", true, "Enabling/Disabling the mod (default = true)");
                debug = config.Bind<bool>("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)");
                //serverProgress = config.Bind<bool>("1 - General", "ServerProgress", false, "Enabling/Disabling the benefits with the server progression (default = false, which is by player personal battle wins)");
                bonemassBenefitEnabled = config.Bind<bool>("2 - Victories", "BonemassBenefitEnabled", true, "Stops getting wet by rain in all swamps after killing Bonemass (default = true)");
                moderBenefitEnabled = config.Bind<bool>("2 - Victories", "ModerBenefitEnabled", true, "Stops freezing without protection effects in all mountains after killing Moder (default = true)");
                queenBenefitEnabled = config.Bind<bool>("2 - Victories", "QueenBenefitEnabled", true, "Increases the wisp light range after killing The Seeker Queen (default = true)");
                queenBenefitRange = config.Bind<float>("2 - Victories", "QueenBenefitRange", 100f, "Establishes the new wisp light range after killing The Seeker Queen (default = true)");

                queenBenefitRange.SettingChanged += QueenBenefitRange_SettingChanged;
            }
        }

        private static void QueenBenefitRange_SettingChanged(object sender, EventArgs e)
        {
            var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");
            if (itemData == null) return;
            Player.m_localPlayer.UnequipItem(itemData);
            Player.m_localPlayer.EquipItem(itemData); // triggers MistlandsPatch
        }
    }
}
