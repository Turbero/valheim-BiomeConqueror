using System;
using HarmonyLib;

namespace BiomeConqueror.Benefits
{
    [HarmonyPatch(typeof(Character), "RPC_Damage")]
    public static class FireDamageReducePatch
    {
        static void Prefix(ref HitData hit, Character __instance)
        {
            if (__instance.name.Contains("Player"))
            {
                if (Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.AshLands &&
                    hit.m_hitType == HitData.HitType.Burning &&
                    ConfigurationFile.faderBenefitDamageFireResistant.Value >= 1f && 
                    BiomeConquerorUtils.isFaderDefeatedForPlayer())
                {
                    hit.m_damage.Modify(Math.Min(100, Math.Max(0, 100 - ConfigurationFile.faderBenefitDamageFireResistant.Value)) / 100f);
                }
            }
        }
    }
}
