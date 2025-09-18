using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using System.Threading.Tasks;
using BiomeConqueror.Helpers;
using UnityEngine;

namespace BiomeConqueror
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BiomeConqueror : BaseUnityPlugin
    {
        public const string GUID = "Turbero.BiomeConqueror";
        public const string NAME = "Biome Conqueror";
        public const string VERSION = "1.3.0";
        
        private readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
            ConfigurationFile.LoadConfig(this);

            harmony.PatchAll();
        }

        void onDestroy()
        {
            harmony.UnpatchSelf();
        }
        void Update()
        {
            if (!Player.m_localPlayer || !InventoryGui.instance) return;

            // Check if certain keys are hit to close Almanac GUI
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab) || Player.m_localPlayer.IsDead())
            {
                InventoryGui.instance.m_textsDialog.gameObject.SetActive(false);
            }

            // Hotkey to open/close skills dialog (if game is not paused)
            if (Input.GetKeyDown(ConfigurationFile.hotKey.Value) && Time.timeScale > 0)
            {
                if (InventoryGui.instance.m_textsDialog.gameObject.activeSelf)
                {
                    InventoryGui.instance.m_textsDialog.gameObject.SetActive(false);
                    InventoryGui.instance.Hide();
                }
                else
                {
                    InventoryGui.instance.Show(null);
                    _ = WaitForSecondsAsync(0.15f); // Small delay to avoid coroutine issue in log to wait for showing skills dialog until it is active
                }
            }
        }
        private static async Task WaitForSecondsAsync(float seconds)
        {
            await Task.Delay((int)(Math.Max(0f, seconds) * 1000)); // to milisegundos
            InventoryGui.instance.m_textsDialog.Setup(Player.m_localPlayer);
            InventoryGui.instance.m_textsDialog.gameObject.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Show))]
    public class InventoryGui_Show_Patch
    {
        static void Postfix(InventoryGui __instance)
        {
            if (!ConfigurationFile.modEnabled.Value) return;

            if (__instance.m_player != null)
            {
                var transform = __instance
                    .transform.Find("root")
                    .transform.Find("Info")
                    .transform.Find("Texts");
                if (transform != null)
                {
                    UITooltip buttonTooltip = transform.GetComponent<UITooltip>();

                    if (buttonTooltip != null)
                    {
                        string originalTooltip = "$inventory_texts";
                        string customText = $" ({ConfigurationFile.hotKey.Value})";

                        buttonTooltip.m_text = originalTooltip + customText;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
    public class Player_OnSpawned_Patch
    {
        static void Postfix(Player __instance, bool spawnValkyrie)
        {
            PlayerBuffs.ActivateCurrentEnvBiomeBenefitBuff();
        }
    }
    
    [HarmonyPatch(typeof(EnvMan))]
    public static class BiomeChangePatch
    {
        private static Heightmap.Biome previousBiome;

        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(EnvMan), "UpdateEnvironment");
        }

        public static void Postfix(long sec, Heightmap.Biome biome)
        {
            if (biome != previousBiome)
            {
                Logger.Log($"Biome changed: {previousBiome} -> {biome}");
                previousBiome = biome;
                PlayerBuffs.ActivateCurrentEnvBiomeBenefitBuff();
            }
        }
    }
}
