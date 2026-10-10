using BiomeConqueror.Helpers;
using HarmonyLib;

namespace BiomeConqueror.Benefits
{
    [HarmonyPatch(typeof(EnvMan), "UpdateEnvironment")]
    public class WeatherPatch
    {
        static bool Prefix(EnvMan __instance, long sec, BiomeSector biome)
        {
            Player player = Player.m_localPlayer;

            if (player != null)
            {
                //Update wisplight if equipped
                var demisters = Demister.GetDemisters();
                if (demisters != null)
                {
                    foreach (var demister in demisters)
                        MistlandsPatch.updateDemisterRangeAndText(demister); //is this all active demisters of all players in the game???
                }


                if (player.GetCurrentBiome() == Heightmap.Biome.Swamp)
                {
                    if (BiomeConquerorUtils.isBonemassDefeatedForPlayer())
                    {
                        __instance.GetCurrentEnvironment().m_isWet = false;
                        if (Player.m_localPlayer.GetSEMan().HaveStatusEffect("Wet".GetHashCode()))
                        {
                            Player.m_localPlayer.GetSEMan().RemoveStatusEffect("Wet".GetHashCode(), true);
                        }
                    }
                    else
                    {
                        __instance.GetCurrentEnvironment().m_isWet = true;
                    }
                }
                else if (player.GetCurrentBiome() == Heightmap.Biome.Mountain)
                {
                    if (BiomeConquerorUtils.isModerDefeatedForPlayer())
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
                else if (player.GetCurrentBiome() == Heightmap.Biome.DeepNorth)
                {
                    if (BiomeConquerorUtils.isKallDefeatedForPlayer())
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
                else
                {
                    __instance.GetCurrentEnvironment().m_isFreezing = false;
                    __instance.GetCurrentEnvironment().m_isFreezingAtNight = false;
                }
            }

            return true;
        }

        private static Heightmap.Biome previousBiome;

        public static void Postfix(long sec, BiomeSector biome)
        {
            if (biome.Biome != previousBiome)
            {
                Logger.Log($"Biome changed: {previousBiome} -> {biome.Biome}");
                
                //Force to remove wet when exiting Swamp
                if (previousBiome == Heightmap.Biome.Swamp)
                {
                    Logger.Log("Coming from swamp, forcing wet false");
                    EnvMan.instance.GetCurrentEnvironment().m_isWet = false;
                } else if (previousBiome == Heightmap.Biome.Mountain)
                {
                    Logger.Log("Coming from mountain, forcing freezing false");
                    EnvMan.instance.GetCurrentEnvironment().m_isFreezing = false;
                } else if (previousBiome == Heightmap.Biome.DeepNorth)
                {
                    Logger.Log("Coming from deep north, forcing freezing false");
                    EnvMan.instance.GetCurrentEnvironment().m_isFreezing = false;
                }

                previousBiome = biome.Biome;
                PlayerBuffs.ActivateCurrentEnvBiomeBenefitBuff();
            }
        }
    }
}
