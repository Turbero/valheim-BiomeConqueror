using BepInEx.Configuration;
using BepInEx;
using System;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using ServerSync;

namespace BiomeConqueror
{
    internal class ConfigurationFile
    {
        private static ConfigEntry<bool> _serverConfigLocked = null;

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

        private static ConfigFile configFile;

        private static readonly ConfigSync ConfigSync = new ConfigSync(BiomeConqueror.GUID)
        {
            DisplayName = BiomeConqueror.NAME,
            CurrentVersion = BiomeConqueror.VERSION,
            MinimumRequiredVersion = BiomeConqueror.VERSION
        };

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                configFile = plugin.Config;

                _serverConfigLocked = config("1 - General", "Lock Configuration", true,
                "If on, the configuration is locked and can be changed by server admins only.");
                _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

                modEnabled = config("1 - General", "Enabled", true, "Enabling/Disabling the mod (default = true)");
                debug = config("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)", false);
                worldProgression = config("2 - Config", "WorldProgression", false, "Enabling/Disabling the benefits with the world progression (default = false, which is by player personal battle wins)");
                hotKey = config("2 - Config", "HotKey", KeyCode.F3, "Hot key to open the compendium (default = F3)", false);
                benefitIcons = config("2 - Config", "BenefitIcons", true, "Enabling/Disabling the benefits as permanent buffs (default = false)", false);
                bonemassBenefitEligibleEnabled = config("3 - Victories", "BonemassBenefitEligibleEnabled", true, "Allows to earn the benefit that stops getting wet by rain in all swamps after killing Bonemass (default = true)");
                moderBenefitEligibleEnabled = config("3 - Victories", "ModerBenefitEligibleEnabled", true, "Allows to earn the benefit that stops getting frozen without protection effects in all mountains after killing Moder (default = true)");
                yagluthBenefitEligibleEnabled = config("3 - Victories", "YagluthBenefitEligibleEnabled", true, "Allows to earn the benefit that stops deathsquitos attacking you (default = true)");
                queenBenefitEligibleEnabled = config("3 - Victories", "QueenBenefitEligibleEnabled", true, "Allows to earn the benefit that increases the wisp light range after killing The Seeker Queen (default = true)");
                queenBenefitEligibleRange = config("3 - Victories", "QueenBenefitRange", 100f, "Establishes the new wisp light range after killing The Seeker Queen (default = 100)");

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

                _ = ReloadWispLight();
            }
            else
            {
                PlayerBuffs.RemoveAllBenefitBuffs();
            }
        }

        private static async Task ReloadWispLight()
        {
            await Task.Delay((int)0.15 * 1000); // to miliseconds
            var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");
            if (itemData == null) return;
            Player.m_localPlayer.UnequipItem(itemData);
            Player.m_localPlayer.EquipItem(itemData); // triggers MistlandsPatch
        }

        private static ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private static ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new ConfigDescription(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = configFile.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
    }
}
