using HarmonyLib;
using System.Linq;
using BiomeConqueror.Helpers;
using static BiomeConqueror.Helpers.Constants;

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
                    if (__instance.m_name == "$enemy_eikthyr" && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Meadows)
                    {
                        Player.m_localPlayer.AddUniqueKey(EIKTHYR_DEFEATED_PLAYER_KEY);
                        Logger.Log("** Eikthyr defeated");
                        if (ConfigurationFile.benefitIcons.Value && ConfigurationFile.eikthyrBenefitEligibleEnabled.Value)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, EIKTHYR_DEFEATED_MESSAGE_KEY, TROPHY_EIKTHYR);
                    }
                    else if (__instance.m_name == "$enemy_gdking" && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.BlackForest)
                    {
                        Player.m_localPlayer.AddUniqueKey(ELDER_DEFEATED_PLAYER_KEY);
                        Logger.Log("** Elder defeated");
                        if (ConfigurationFile.benefitIcons.Value && ConfigurationFile.elderBenefitEligibleEnabled.Value)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, ELDER_DEFEATED_MESSAGE_KEY, TROPHY_ELDER);
                    }
                    else if (__instance.m_name == "$enemy_bonemass" && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Swamp)
                    {
                        Player.m_localPlayer.AddUniqueKey(BONEMASS_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Bonemass defeated");
                        if (ConfigurationFile.benefitIcons.Value && ConfigurationFile.bonemassBenefitEligibleEnabled.Value)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, BONEMASS_DEFEATED_MESSAGE_KEY, TROPHY_BONEMASS);
                    }
                    else if (__instance.m_name == "$enemy_dragon" && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mountain)
                    {
                        Player.m_localPlayer.AddUniqueKey(MODER_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Moder defeated");
                        if (ConfigurationFile.benefitIcons.Value && ConfigurationFile.moderBenefitEligibleEnabled.Value)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, MODER_DEFEATED_MESSAGE_KEY, TROPHY_MODER);
                    }
                    else if (__instance.m_name == "$enemy_goblinking" && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Plains)
                    {
                        Player.m_localPlayer.AddUniqueKey(YAGLUTH_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Yagluth defeated");
                        if (ConfigurationFile.benefitIcons.Value && ConfigurationFile.yagluthBenefitEligibleEnabled.Value)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, YAGLUTH_DEFEATED_MESSAGE_KEY, TROPHY_YAGLUTH);
                    }
                    else if (__instance.m_name == "$enemy_seekerqueen" && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mistlands)
                    {
                        Player.m_localPlayer.AddUniqueKey(QUEEN_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Queen defeated");
                        if (ConfigurationFile.benefitIcons.Value && ConfigurationFile.queenBenefitEligibleEnabled.Value)
                        {
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, QUEEN_DEFEATED_MESSAGE_KEY, TROPHY_QUEEN);
                            //Refresh wisp light immediately
                            var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");
                            if (itemData != null)
                            {
                                Player.m_localPlayer.UnequipItem(itemData);
                                Player.m_localPlayer.EquipItem(itemData);
                            }
                        }
                    }else if (__instance.m_name == "$enemy_fader" && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.AshLands)
                    {
                        Player.m_localPlayer.AddUniqueKey(FADER_DEFEATED_PLAYER_KEY);
                        Logger.Log($"** Fader defeated");
                        if (ConfigurationFile.benefitIcons.Value && ConfigurationFile.faderBenefitEligibleEnabled.Value)
                            PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, FADER_DEFEATED_MESSAGE_KEY, TROPHY_FADER);
                    }
                }
            }
        }


    }
}
