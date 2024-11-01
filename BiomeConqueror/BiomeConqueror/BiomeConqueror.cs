using BepInEx;
using HarmonyLib;
using System.Linq;
using System.Reflection;

namespace BiomeConqueror
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BiomeConqueror : BaseUnityPlugin
    {
        public const string GUID = "Turbero.BiomeConqueror";
        public const string NAME = "Biome Conqueror";
        public const string VERSION = "1.1.0";

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
    }

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
                    Player.m_localPlayer.AddUniqueKey("BonemassDefeated");
                    Logger.Log($"** Bonemass defeated");
                    if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Swamp)
                        PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$event_boss03_end", "TrophyBonemass");
                }
                else if (__instance.m_name == "$enemy_dragon")
                {
                    Player.m_localPlayer.AddUniqueKey("ModerDefeated");
                    Logger.Log($"** Moder defeated");
                    if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mountain)
                        PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$event_boss04_end", "TrophyDragonQueen");
                }
                else if (__instance.m_name == "$enemy_goblinking")
                {
                    Player.m_localPlayer.AddUniqueKey("YagluthDefeated");
                    Logger.Log($"** Yagluth defeated");
                    if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Plains)
                        PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$event_boss05_end", "TrophyGoblinKing");
                }
                else if (__instance.m_name == "$enemy_seekerqueen")
                {
                    Player.m_localPlayer.AddUniqueKey("QueenDefeated");
                    Logger.Log($"** Queen defeated");
                    if (ConfigurationFile.benefitIcons.Value && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mistlands)
                        PlayerBuffs.AddBenefitBuff(Player.m_localPlayer, "$enemy_boss_queen_deathmessage", "TrophySeekerQueen");

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

    [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
    public class Player_OnSpawned_Patch
    {
        static void Postfix(Player __instance, bool spawnValkyrie)
        {
            PlayerBuffs.ActivateCurrentBiomeBenefitBuff();
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
                PlayerBuffs.ActivateCurrentBiomeBenefitBuff();
            }
        }
    }
}
