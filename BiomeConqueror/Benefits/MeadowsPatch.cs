using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BiomeConqueror.Benefits
{
    public class MeadowsPatch
    {
        [HarmonyPatch(typeof(Character), "OnDeath")]
        public static class DeerLootPatch
        {
            public static void Postfix(Character __instance)
            {
                if (__instance.name.StartsWith("Deer") &&
                    Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Meadows &&
                    BiomeConquerorUtils.isEikthyrDefeatedForPlayer())
                {
                    Logger.Log("Eikthyr defeated: dropping extra meat.");
                    // Manually instantiate after death animation
                    GameObject deerMeatObject = ZNetScene.instance.GetPrefab("DeerMeat");
                    if (deerMeatObject == null)
                    {
                        Logger.LogError("Prefab 'DeerMeat' not found.");
                        return;
                    }

                    GameObject spawnedItem = Object.Instantiate(deerMeatObject, __instance.transform.position,
                        Quaternion.identity);
                    ItemDrop itemDrop = spawnedItem.GetComponent<ItemDrop>();
                    if (itemDrop != null)
                    {
                        itemDrop.m_itemData.m_stack =
                            ConfigurationFile.eikthyrBenefitEligibleExtraDrop.Value; // Quantity to drop
                        Logger.Log("Ítem DeerMeat successfully generated.");
                    }
                    else
                    {
                        Logger.LogError("Prefab 'DeerMeat' doesn't have an ItemDrop.");
                    }
                }
            }
        }
    }
}