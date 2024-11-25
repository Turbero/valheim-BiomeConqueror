using BiomeConqueror.Helpers;

namespace BiomeConqueror
{
    public class BiomeConquerorUtils
    {
        private static bool hasUniqueKey(string key, bool configValue)
        {
            return ConfigurationFile.modEnabled.Value && configValue &&
                Player.m_localPlayer.HaveUniqueKey(key);
        }

        private static bool hasGlobalKey(string key)
        {
            return ConfigurationFile.modEnabled.Value && ConfigurationFile.worldProgression.Value &&
                ZoneSystem.instance.GetGlobalKey(key);
        }

        public static bool isEikthyrDefeatedForPlayer()
        {
            return hasUniqueKey(Constants.EIKTHYR_DEFEATED_PLAYER_KEY, ConfigurationFile.eikthyrBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_eikthyr");
        }

        public static bool isElderDefeatedForPlayer()
        {
            return hasUniqueKey(Constants.ELDER_DEFEATED_PLAYER_KEY, ConfigurationFile.elderBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_gdking");
        }

        public static bool isBonemassDefeatedForPlayer()
        {
            return hasUniqueKey(Constants.BONEMASS_DEFEATED_PLAYER_KEY, ConfigurationFile.bonemassBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_bonemass");
        }

        public static bool isModerDefeatedForPlayer()
        {
            return hasUniqueKey(Constants.MODER_DEFEATED_PLAYER_KEY, ConfigurationFile.moderBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_dragon");
        }

        public static bool isYagluthDefeatedForPlayer()
        {
            return hasUniqueKey(Constants.YAGLUTH_DEFEATED_PLAYER_KEY, ConfigurationFile.yagluthBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_goblinking");
        }

        public static bool isQueenDefeatedForPlayer()
        {
            return hasUniqueKey(Constants.QUEEN_DEFEATED_PLAYER_KEY, ConfigurationFile.queenBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_queen");
        }
        
        public static bool isFaderDefeatedForPlayer()
        {
            return hasUniqueKey(Constants.FADER_DEFEATED_PLAYER_KEY, ConfigurationFile.faderBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_fader");
        }
    }
}
