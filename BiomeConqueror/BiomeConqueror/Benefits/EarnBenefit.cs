using HarmonyLib;
using System.Linq;
using static BiomeConqueror.Constants;

namespace BiomeConqueror.Benefits
{
    public class EarnBenefit
    {
        [HarmonyPatch(typeof(Character), "OnDeath")]
        public class CharacterDeathPatch
        {
            static void Postfix(Character __instance)
            {
                if (!ConfigurationFile.modEnabled.Value) return;

                if (__instance != null && __instance.IsBoss())
                {
                    Logger.Log($"defeated name: {__instance.m_name}");
                    if (__instance.m_name == "$enemy_bonemass")
                    {
                        Player.m_localPlayer.AddUniqueKey(BONEMASS_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Bonemass defeated");
                        if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Swamp)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, BONEMASS_DEFEATED_MESSAGE_KEY, TROPHY_BONEMASS);
                    }
                    else if (__instance.m_name == "$enemy_dragon")
                    {
                        Player.m_localPlayer.AddUniqueKey(MODER_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Moder defeated");
                        if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mountain)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, MODER_DEFEATED_MESSAGE_KEY, TROPHY_MODER);
                    }
                    else if (__instance.m_name == "$enemy_goblinking")
                    {
                        Player.m_localPlayer.AddUniqueKey(YAGLUTH_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Yagluth defeated");
                        if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Plains)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, YAGLUTH_DEFEATED_MESSAGE_KEY, TROPHY_YAGLUTH);
                    }
                    else if (__instance.m_name == "$enemy_seekerqueen")
                    {
                        Player.m_localPlayer.AddUniqueKey(QUEEN_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Queen defeated");
                        if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mistlands)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, QUEEN_DEFEATED_MESSAGE_KEY, TROPHY_QUEEN);

                        var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");
                        if (itemData != null)
                        {
                            Player.m_localPlayer.UnequipItem(itemData);
                            Player.m_localPlayer.EquipItem(itemData);
                        }
                    }
                }
            }
        }


    }
}
