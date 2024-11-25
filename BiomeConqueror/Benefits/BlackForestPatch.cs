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
                if (__instance.name.Contains("Troll"))
                {
                    if (BiomeConquerorUtils.isElderDefeatedForPlayer() &&
                        Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.BlackForest)
                    {
                        if (ConfigurationFile.elderBenefitMultiplierTrollDmg.Value >= 1f)
                        {
                            Logger.Log("Increasing damage hit to attacked trol by x"+ ConfigurationFile.elderBenefitMultiplierTrollDmg.Value);
                            hit.m_damage.Modify(ConfigurationFile.elderBenefitMultiplierTrollDmg.Value);
                        }
                    }
                }
            }
        }

    }
}