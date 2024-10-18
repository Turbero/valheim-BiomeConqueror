using BepInEx.Configuration;
using BepInEx;

namespace BiomeConqueror
{
    internal class ConfigurationFile
    {
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> debug;
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
                bonemassBenefitEnabled = config.Bind<bool>("2 - Victories", "BonemassBenefitEnabled", true, "Stops getting wet by rain in all swamps after killing Bonemass (default = true)");
                moderBenefitEnabled = config.Bind<bool>("2 - Victories", "ModerBenefitEnabled", true, "Stops freezing without protection effects in all mountains after killing Moder (default = true)");
                queenBenefitEnabled = config.Bind<bool>("2 - Victories", "QueenBenefitEnabled", true, "Increases the wisp light range after killing The Seeker Queen (default = true)");
                queenBenefitRange = config.Bind<float>("2 - Victories", "QueenBenefitRange", 100f, "Establishes the new wisp light range after killing The Seeker Queen (default = true)");
            }
        }
    }
}
