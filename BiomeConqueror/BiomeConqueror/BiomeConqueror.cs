using BepInEx;
using HarmonyLib;
using System;
using TMPro;
using UnityEngine;
using static Skills;
using static Utils;

namespace BiomeConqueror
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class DetailedLevels : BaseUnityPlugin
    {
        public const string GUID = "Turbero.BiomeConqueror";
        public const string NAME = "Biome Conqueror";
        public const string VERSION = "1.0.0";

        private readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
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
            if (__instance != null && __instance.IsBoss())
            {
                if (__instance.m_name == "$enemy_bonemass")
                {
                    Player.m_localPlayer.AddUniqueKey("BonemassDefeated");
                    Debug.Log($"** Bonemass defeated");
                }
                else if (__instance.m_name == "$enemy_dragon")
                {
                    Player.m_localPlayer.AddUniqueKey("ModerDefeated");
                    Debug.Log($"** Moder defeated");
                }
                else if (__instance.m_name == "$enemy_seekerqueen")
                {
                    Player.m_localPlayer.AddUniqueKey("QueenDefeated");
                    Debug.Log($"** Queen defeated");
                }
            }
        }
    }

    [HarmonyPatch(typeof(EnvMan), "UpdateEnvironment")]
    public class NoRainInSwampsPatch
    {
        static void Prefix(EnvMan __instance)
        {
            Player player = Player.m_localPlayer;

            if (player != null)
            {
                if (player.GetCurrentBiome() == Heightmap.Biome.Swamp)
                {
                    if (Player.m_localPlayer.HaveUniqueKey("BonemassDefeated"))
                    {
                        __instance.GetCurrentEnvironment().m_isWet = false;
                    }
                    else
                    {
                        __instance.GetCurrentEnvironment().m_isWet = true;
                    }
                }
                else if (player.GetCurrentBiome() == Heightmap.Biome.Mountain)
                {
                    if (Player.m_localPlayer.HaveUniqueKey("ModerDefeated"))
                    {
                        __instance.GetCurrentEnvironment().m_isFreezing = false;
                        __instance.GetCurrentEnvironment().m_isFreezingAtNight = false;
                    }
                    else
                    {
                        __instance.GetCurrentEnvironment().m_isFreezing = true;
                        __instance.GetCurrentEnvironment().m_isFreezingAtNight = true;
                    }
                }
            }
        }
    }
}
