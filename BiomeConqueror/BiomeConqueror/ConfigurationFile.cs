using BepInEx.Configuration;
using BepInEx;
using System;
using System.Linq;

namespace BiomeConqueror
{
    internal class ConfigurationFile
    {
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> debug;
        public static ConfigEntry<bool> worldProgression;
        public static ConfigEntry<bool> benefitIcons;
        public static ConfigEntry<bool> bonemassBenefitEligibleEnabled;
        public static ConfigEntry<bool> moderBenefitEligibleEnabled;
        public static ConfigEntry<bool> yagluthBenefitEligibleEnabled;
        public static ConfigEntry<bool> queenBenefitEligibleEnabled;
        public static ConfigEntry<float> queenBenefitEligibleRange;

        private static ConfigFile config;

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                config = plugin.Config;

                modEnabled = config.Bind<bool>("1 - General", "Enabled", true, "Enabling/Disabling the mod (default = true)");
                debug = config.Bind<bool>("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)");
                worldProgression = config.Bind<bool>("2 - Config", "WorldProgression", false, "Enabling/Disabling the benefits with the world progression (default = false, which is by player personal battle wins)");
                benefitIcons = config.Bind<bool>("2 - Config", "BenefitIcons", false, "Enabling/Disabling the benefits as permanent buffs (default = false)");
                bonemassBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "BonemassBenefitEligibleEnabled", true, "Allows to earn the benefit that stops getting wet by rain in all swamps after killing Bonemass (default = true)");
                moderBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "ModerBenefitEligibleEnabled", true, "Allows to earn the benefit that stops getting frozen without protection effects in all mountains after killing Moder (default = true)");
                yagluthBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "YagluthBenefitEligibleEnabled", true, "Allows to earn the benefit that stops deathsquitos attacking you (default = true)");
                queenBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "QueenBenefitEligibleEnabled", true, "Allows to earn the benefit that increases the wisp light range after killing The Seeker Queen (default = true)");
                queenBenefitEligibleRange = config.Bind<float>("3 - Victories", "QueenBenefitRange", 100f, "Establishes the new wisp light range after killing The Seeker Queen (default = true)");

                benefitIcons.SettingChanged += BenefitIcons_SettingChanged;
                modEnabled.SettingChanged += QueenBenefitRange_SettingChanged;
                worldProgression.SettingChanged += QueenBenefitRange_SettingChanged;
                queenBenefitEligibleEnabled.SettingChanged += QueenBenefitRange_SettingChanged;
                queenBenefitEligibleRange.SettingChanged += QueenBenefitRange_SettingChanged;
            }
        }

        private static void BenefitIcons_SettingChanged(object sender, EventArgs e)
        {
            if (Player.m_localPlayer == null) return;

            if (!benefitIcons.Value)
            {
                PlayerBuffs.RemoveBenefitBuffIfExists(Player.m_localPlayer, "$event_boss03_end");
                PlayerBuffs.RemoveBenefitBuffIfExists(Player.m_localPlayer, "$event_boss04_end");
                PlayerBuffs.RemoveBenefitBuffIfExists(Player.m_localPlayer, "$event_boss05_end");
                PlayerBuffs.RemoveBenefitBuffIfExists(Player.m_localPlayer, "$enemy_boss_queen_deathmessage");
            } else
            {
                if (BiomeConquerorUtils.isBonemassDefeatedForPlayer()) PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$event_boss03_end", "TrophyBonemass");
                if (BiomeConquerorUtils.isModerDefeatedForPlayer())    PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$event_boss04_end", "TrophyBonemass");
                if (BiomeConquerorUtils.isYagluthDefeatedForPlayer())  PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$event_boss05_end", "TrophyBonemass");
                if (BiomeConquerorUtils.isQueenDefeatedForPlayer())    PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$event_boss03_end", "TrophyBonemass");
            }
        }

        private static void QueenBenefitRange_SettingChanged(object sender, EventArgs e)
        {
            if (modEnabled.Value)
            {
                var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");
                if (itemData == null) return;
                Player.m_localPlayer.UnequipItem(itemData);
                Player.m_localPlayer.EquipItem(itemData); // triggers MistlandsPatch
            }
        }
    }
}
