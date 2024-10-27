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

        public static bool isBonemassDefeatedForPlayer()
        {
            return hasUniqueKey("BonemassDefeated", ConfigurationFile.bonemassBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_bonemass");
        }

        public static bool isModerDefeatedForPlayer()
        {
            return hasUniqueKey("ModerDefeated", ConfigurationFile.moderBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_dragon");
        }

        public static bool isQueenDefeatedForPlayer()
        {
            return hasUniqueKey("QueenDefeated", ConfigurationFile.queenBenefitEligibleEnabled.Value) ||
                   hasGlobalKey("defeated_queen");
        }
    }
}
