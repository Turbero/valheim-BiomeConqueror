using HarmonyLib;

namespace BiomeConqueror.Benefits
{
    [HarmonyPatch(typeof(EnvMan), "UpdateEnvironment")]
    public class WeatherPatch
    {
        static bool Prefix(EnvMan __instance)
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
            }

            return true;
        }
    }
}
