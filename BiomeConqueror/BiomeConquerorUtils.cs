using BiomeConqueror.Helpers;

namespace BiomeConqueror
{
    public class BiomeConquerorUtils
    {
        private static bool hasUniqueKey(string key)
        {
            return ConfigurationFile.modEnabled.Value &&
                Player.m_localPlayer.HaveUniqueKey(key);
        }

        private static bool hasGlobalKey(string key)
        {
            return ConfigurationFile.modEnabled.Value && 
                   ConfigurationFile.worldProgression.Value &&
                   ZoneSystem.instance.GetGlobalKey(key);
        }

        public static bool isEikthyrDefeatedForPlayer()
        {
            return ConfigurationFile.eikthyrBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.EIKTHYR_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_eikthyr") || hasGlobalKey("DeerGod"));
        }

        public static bool isElderDefeatedForPlayer()
        {
            return ConfigurationFile.elderBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.ELDER_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_gdking") || hasGlobalKey("TreeGod"));
        }

        public static bool isBonemassDefeatedForPlayer()
        {
            return ConfigurationFile.bonemassBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.BONEMASS_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_bonemass") || hasGlobalKey("BlobGod"));
        }

        public static bool isModerDefeatedForPlayer()
        {
            return ConfigurationFile.moderBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.MODER_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_dragon") || hasGlobalKey("DrakeGod"));
        }

        public static bool isYagluthDefeatedForPlayer()
        {
            return ConfigurationFile.yagluthBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.YAGLUTH_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_goblinking") || hasGlobalKey("GobGod"));
        }

        public static bool isQueenDefeatedForPlayer()
        {
            return ConfigurationFile.queenBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.QUEEN_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_queen") || hasGlobalKey("BugGod"));
        }
        
        public static bool isFaderDefeatedForPlayer()
        {
            return ConfigurationFile.faderBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.FADER_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_fader") || hasGlobalKey("SkeleGod"));
        }
        
        public static bool isKallDefeatedForPlayer()
        {
            return ConfigurationFile.kallBenefitEligibleEnabled.Value && (
                hasUniqueKey(Constants.KALL_DEFEATED_PLAYER_KEY) ||
                hasGlobalKey("defeated_frozenking_p3"));
        }
    }
}
