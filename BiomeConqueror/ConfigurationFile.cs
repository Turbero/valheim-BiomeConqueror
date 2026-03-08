using BepInEx.Configuration;
using BepInEx;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using BiomeConqueror.Helpers;
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
        public static ConfigEntry<bool> eikthyrBenefitEligibleEnabled;
        public static ConfigEntry<int> eikthyrBenefitExtraDrop;
        public static ConfigEntry<bool> elderBenefitEligibleEnabled;
        public static ConfigEntry<float> elderBenefitMultiplierTrollDmg;
        public static ConfigEntry<float> elderBenefitMultiplierBearDmg;
        public static ConfigEntry<bool> bonemassBenefitEligibleEnabled;
        public static ConfigEntry<bool> moderBenefitEligibleEnabled;
        public static ConfigEntry<bool> yagluthBenefitEligibleEnabled;
        public static ConfigEntry<bool> queenBenefitEligibleEnabled;
        public static ConfigEntry<float> queenBenefitRangeAfter;
        public static ConfigEntry<float> queenBenefitRangeBefore;
        public static ConfigEntry<bool> faderBenefitEligibleEnabled;
        public static ConfigEntry<float> faderBenefitDamageFireResistant;
        public static ConfigEntry<int> bossPowerReduction;
        public static ConfigEntry<bool> bossPowerReductionBumpedByStars;
        public static ConfigEntry<string> enemiesForReduction;
        public static ConfigEntry<string> reductionMessageSuccess;

        private static ConfigFile configFile;
        private static string ConfigFileName = BiomeConqueror.GUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

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

                _serverConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
                _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

                modEnabled = config("1 - General", "Enabled", true, "Enabling/Disabling the mod (default = true)");
                debug = config("1 - General", "Debug Mode", false, "Enabling/Disabling the debugging in the console (default = false)", false);
                
                worldProgression = config("2 - Config", "World Progression", false, "Enabling/Disabling the benefits with the world progression (default = false, which is by player personal battle wins)");
                hotKey = config("2 - Config", "HotKey", KeyCode.F3, "Hot key to open the compendium (default = F3)", false);
                
                benefitIcons = config("3 - Victories", "Benefit Icons", true, "Enabling/Disabling the benefits as permanent buffs (default = false)", false);
                eikthyrBenefitEligibleEnabled   = config("3.1 - Eikthyr", "Benefit Eligible Enabled", true, "Allows to earn the benefit that double drops deer meat in all meadows after killing Eikthyr (default = true)");
                eikthyrBenefitExtraDrop         = config("3.1 - Eikthyr", "Benefit Extra Drop", 1, "Establishes the extra meat drop from deers in all meadows after killing Eikthyr (default = +1)");
                elderBenefitEligibleEnabled     = config("3.2 - The Elder", "Elder Benefit Eligible Enabled", true, "Allows to earn the benefit that TBD in all black forests after killing the Elder (default = true)");
                elderBenefitMultiplierTrollDmg  = config("3.2 - The Elder", "Elder Benefit Multiplier Troll Dmg", 2.0f, "Sets up the multiplier damage applied to trolls in black forests after killing the Elder (default = 2x)");
                elderBenefitMultiplierBearDmg   = config("3.2 - The Elder", "Elder Benefit Multiplier Bear Dmg", 2.0f, "Sets up the multiplier damage applied to bears in black forests after killing the Elder (default = 2x)");
                bonemassBenefitEligibleEnabled  = config("3.3 - Bonemass", "Bonemass Benefit Eligible Enabled", true, "Allows to earn the benefit that stops getting wet by rain in all swamps after killing Bonemass (default = true)");
                moderBenefitEligibleEnabled     = config("3.4 - Moder", "Moder Benefit Eligible Enabled", true, "Allows to earn the benefit that stops getting frozen without protection effects in all mountains after killing Moder (default = true)");
                yagluthBenefitEligibleEnabled   = config("3.5 - Yagluth", "Yagluth Benefit Eligible Enabled", true, "Allows to earn the benefit that stops deathsquitos attacking you (default = true)");
                queenBenefitEligibleEnabled     = config("3.6 - Queen", "Queen Benefit Eligible Enabled", true, "Allows to earn the benefit that increases the wisp light range after killing The Seeker Queen (default = true)");
                queenBenefitRangeAfter          = config("3.6 - Queen", "Queen Benefit Range After Defeat", 100f, "Establishes the new wisp light range after killing The Seeker Queen (default = 100)");
                queenBenefitRangeBefore         = config("3.6 - Queen", "Queen Benefit Range Before Defeat", 10f, "Establishes the base wisp light range before killing The Seeker Queen (default = 10)");
                faderBenefitEligibleEnabled     = config("3.7 - Fader", "Fader Benefit Eligible Enabled", true, "Allows to earn the benefit that gives you burning damage protection in lava from Ashlands after killing the Fader (default = true)");
                faderBenefitDamageFireResistant = config("3.7 - Fader", "Fader Benefit Damage Fire Resistant", 100f, new ConfigDescription("Gives extra percentage of burning damage protection from lava in Ashlands after defeating Fader (default = 100)", new AcceptableValueRange<float>(1f, 100f)));
                
                bossPowerReduction              = config("4 - Boss Power Cooldown power", "Cooldown Reduction", 60, "Number of seconds to reduce the power cooldown when one of the specified monsters is defeated (default = 60)");
                bossPowerReductionBumpedByStars = config("4 - Boss Power Cooldown power", "Cooldown Reduction Bumped By Stars", true, "Choose if high-level monsters must decrease more time to the power cooldown proportional to their number of starts (default = true)");
                enemiesForReduction             = config("4 - Boss Power Cooldown power", "Enemies For Reduction", "Troll,Bjorn,Serpent,Abomination,StoneGolem,Unbjorn,Gjall,BonemawSerpent", "Comma-separated list of enemies to apply power cooldown reduction when defeated (leave blank to deactivate)");
                reductionMessageSuccess         = config("4 - Boss Power Cooldown power", "Reduction Message Success", "Cooldown reduced by {0} seconds", "Message on the top left corner when killed one of the specified monsters to reduce boss power cooldown");
                
                SetupWatcher();
            }
        }
        
        private static void SetupWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }
        
        private static void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                Logger.LogInfo("Attempting to reload configuration...");
                configFile.Reload();
                SettingChanged(null, null);
                Logger.LogInfo("Reloaded configuration...");
            }
            catch(Exception ex)
            {
                Logger.LogError($"There was an issue loading {ConfigFileName}. {ex}");
            }
        }

        private static void SettingChanged(object sender, EventArgs e)
        {
            BenefitIcons_SettingChanged(sender, e);
            Configuration_SettingChanged(sender, e);
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
            await Task.Delay((int)0.15 * 1000); // to milliseconds
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
