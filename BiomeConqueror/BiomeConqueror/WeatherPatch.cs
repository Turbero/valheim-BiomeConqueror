using HarmonyLib;

namespace BiomeConqueror
{
    [HarmonyPatch(typeof(EnvMan), "UpdateEnvironment")]
    public class WeatherPatch
    {
        static void Prefix(EnvMan __instance)
        {
            if (!ConfigurationFile.modEnabled.Value) return;

            Player player = Player.m_localPlayer;

            if (player != null)
            {
                if (player.GetCurrentBiome() == Heightmap.Biome.Swamp)
                {
                    if (BiomeConquerorUtils.hasUniqueKey("BonemassDefeated", ConfigurationFile.bonemassBenefitEnabled.Value)/* || hasGlobalKey("defeated_bonemass")*/)
                    {
                        __instance.GetCurrentEnvironment().m_isWet = false;
                    }
                    else
                    {
                        __instance.GetCurrentEnvironment().m_isWet = true;
                    }
                }
                else if ((player.GetCurrentBiome() == Heightmap.Biome.Mountain))
                {
                    if (BiomeConquerorUtils.hasUniqueKey("ModerDefeated", ConfigurationFile.moderBenefitEnabled.Value)/* || hasGlobalKey("defeated_dragon")*/)
                    {
                        __instance.GetCurrentEnvironment().m_isFreezing = false;
                        __instance.GetCurrentEnvironment().m_isFreezingAtNight = false;
                    }
                    else
                    {
                        __instance.GetCurrentEnvironment().m_isFreezing = true;
                        __instance.GetCurrentEnvironment().m_isFreezingAtNight = true;
                    }
                }
            }
        }
    }
}
