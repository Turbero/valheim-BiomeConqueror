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
                    if (Player.m_localPlayer.HaveUniqueKey("BonemassDefeated") && ConfigurationFile.bonemassBenefitEnabled.Value)
                    {
                        __instance.GetCurrentEnvironment().m_isWet = false;
                    }
                    else
                    {
                        __instance.GetCurrentEnvironment().m_isWet = true;
                    }
                }
                else if (player.GetCurrentBiome() == Heightmap.Biome.Mountain && ConfigurationFile.moderBenefitEnabled.Value)
                {
                    if (Player.m_localPlayer.HaveUniqueKey("ModerDefeated"))
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
