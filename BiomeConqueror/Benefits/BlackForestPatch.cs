using HarmonyLib;

namespace BiomeConqueror.Benefits
{
    public class BlackForestPatch
    {
        [HarmonyPatch(typeof(Character), "RPC_Damage")]
        public static class TrollDamageBoostPatch
        {
            static void Prefix(ref HitData hit, Character __instance)
            {
                Logger.Log("RPC_Damage name: " + __instance.name);
                if (BiomeConquerorUtils.isElderDefeatedForPlayer() &&
                    Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.BlackForest)
                {
                    if (__instance.name.Contains("Troll"))
                    {
                        {
                            if (ConfigurationFile.elderBenefitMultiplierTrollDmg.Value >= 1f)
                            {
                                Logger.Log("Increasing damage hit to attacked troll by x" +
                                           ConfigurationFile.elderBenefitMultiplierTrollDmg.Value);
                                hit.m_damage.Modify(ConfigurationFile.elderBenefitMultiplierTrollDmg.Value);
                            }
                        }
                    }
                    else if (__instance.name.Contains("Bjorn"))
                    {
                        if (ConfigurationFile.elderBenefitMultiplierBearDmg.Value >= 1f)
                        {
                            Logger.Log("Increasing damage hit to attacked bear by x" +
                                       ConfigurationFile.elderBenefitMultiplierBearDmg.Value);
                            hit.m_damage.Modify(ConfigurationFile.elderBenefitMultiplierBearDmg.Value);
                        }
                    }
                }
            }
        }

    }
}