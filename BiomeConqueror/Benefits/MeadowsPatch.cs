using BiomeConqueror.Helpers;
using HarmonyLib;
using UnityEngine;

namespace BiomeConqueror.Benefits
{
    public class MeadowsPatch
    {
        [HarmonyPatch(typeof(CharacterDrop), "GenerateDropList")]
        public static class BonesPatch
        {
            public static void Prefix(CharacterDrop __instance)
            {
                if (ConfigurationFile.eikthyrBenefitExtraDrop.Value <= 0) return;
            
                Character character = GameManager.GetPrivateValue(__instance, "m_character") as Character;
                if (character == null) return;

                string characterName = character.name.Replace("(Clone)", "").Trim();
                if (!__instance.name.StartsWith("Deer") ||
                    Player.m_localPlayer.GetCurrentBiome() != Heightmap.Biome.Meadows ||
                    !BiomeConquerorUtils.isEikthyrDefeatedForPlayer())
                    return;
                
                GameObject boneFragments = ZNetScene.instance.GetPrefab("BoneFragments");

                if (boneFragments == null)
                {
                    Logger.LogError("Prefab 'BoneFragments' not found.");
                    return;
                }

                CharacterDrop.Drop extraDrop = new CharacterDrop.Drop
                {
                    m_prefab = boneFragments,
                    m_amountMin = ConfigurationFile.eikthyrBenefitExtraDrop.Value,
                    m_amountMax = ConfigurationFile.eikthyrBenefitExtraDrop.Value,
                    m_chance = 1f,
                    m_onePerPlayer = false
                };
            
                __instance.m_drops.Add(extraDrop);

                Logger.Log($"Added {ConfigurationFile.eikthyrBenefitExtraDrop.Value} extra bones to {characterName}.");
            }
        }
    }
}