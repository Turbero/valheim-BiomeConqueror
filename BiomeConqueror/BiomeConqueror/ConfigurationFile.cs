using BepInEx.Configuration;
using BepInEx;
using System;
using System.Linq;
using UnityEngine;

namespace BiomeConqueror
{
    internal class ConfigurationFile
    {
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> debug;
        public static ConfigEntry<KeyCode> hotKey;
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
                hotKey = config.Bind<KeyCode>("2 - Config", "HotKey", KeyCode.F3, "Hot key to open the compendium (default = F3)");
                benefitIcons = config.Bind<bool>("2 - Config", "BenefitIcons", false, "Enabling/Disabling the benefits as permanent buffs (default = false)");
                bonemassBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "BonemassBenefitEligibleEnabled", true, "Allows to earn the benefit that stops getting wet by rain in all swamps after killing Bonemass (default = true)");
                moderBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "ModerBenefitEligibleEnabled", true, "Allows to earn the benefit that stops getting frozen without protection effects in all mountains after killing Moder (default = true)");
                yagluthBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "YagluthBenefitEligibleEnabled", true, "Allows to earn the benefit that stops deathsquitos attacking you (default = true)");
                queenBenefitEligibleEnabled = config.Bind<bool>("3 - Victories", "QueenBenefitEligibleEnabled", true, "Allows to earn the benefit that increases the wisp light range after killing The Seeker Queen (default = true)");
                queenBenefitEligibleRange = config.Bind<float>("3 - Victories", "QueenBenefitRange", 100f, "Establishes the new wisp light range after killing The Seeker Queen (default = true)");

                modEnabled.SettingChanged += Configuration_SettingChanged;
                worldProgression.SettingChanged += Configuration_SettingChanged;
                benefitIcons.SettingChanged += BenefitIcons_SettingChanged;
                bonemassBenefitEligibleEnabled.SettingChanged += Configuration_SettingChanged;
                moderBenefitEligibleEnabled.SettingChanged += Configuration_SettingChanged;
                yagluthBenefitEligibleEnabled.SettingChanged += Configuration_SettingChanged;
                queenBenefitEligibleEnabled.SettingChanged += Configuration_SettingChanged;
                queenBenefitEligibleRange.SettingChanged += Configuration_SettingChanged;
            }
        }

        private static void BenefitIcons_SettingChanged(object sender, EventArgs e)
        {
            if (Player.m_localPlayer == null) return;

            if (!benefitIcons.Value)
            {
                PlayerBuffs.RemoveAllBenefitBuffs();
            } else
            {
                PlayerBuffs.AddPlayerBiomeBenefitBuff();
            }
        }

        private static void Configuration_SettingChanged(object sender, EventArgs e)
        {
            if (modEnabled.Value)
            {
                PlayerBuffs.RemoveAllBenefitBuffs();
                PlayerBuffs.AddPlayerBiomeBenefitBuff();

                var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");
                if (itemData == null) return;
                Player.m_localPlayer.UnequipItem(itemData);
                Player.m_localPlayer.EquipItem(itemData); // triggers MistlandsPatch
            } else
            {
                PlayerBuffs.RemoveAllBenefitBuffs();
            }
        }
    }
}
