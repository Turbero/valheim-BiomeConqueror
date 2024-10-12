using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace BiomeConqueror
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BiomeConqueror
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
            if (__instance != null && __instance.IsBoss() && __instance.name == "Boss_Queen") //prefab name
            {
                OnQueenDefeated();
            }
            // Verificamos si el personaje muerto es Bonemass por su nombre
            if (__instance != null && __instance.IsBoss() && __instance.m_name == "$enemy_bonemass")
            {
                // Bonemass ha sido derrotado, ejecutar lógica
                OnBonemassDefeated();
            }
        }

        // Método que se ejecuta al derrotar a la Reina
        public static void OnQueenDefeated()
        {
            if (Player.m_localPlayer != null)
            {
                // Guardamos el estado de que la Reina ha sido derrotada para este jugador
                PlayerPrefs.SetInt("QueenDefeated", 1);
                RemoveMistlandsFog(); // Quitamos la niebla solo para este jugador
            }
        }
        // Método que se ejecuta al derrotar a Bonemass
        public static void OnBonemassDefeated()
        {
            if (Player.m_localPlayer != null)
            {
                // Guardamos el estado de que Bonemass ha sido derrotado para este jugador
                PlayerPrefs.SetInt("BonemassDefeated", 1);
                RemoveSwampRain(); // Quitamos la lluvia permanente solo para este jugador
            }
        }

        public static void CheckAndRemoveFog()
        {
            if (PlayerPrefs.GetInt("QueenDefeated", 0) == 1)
            {
                // Si el jugador ha derrotado a la Reina, eliminamos la niebla
                RemoveMistlandsFog();
            }
        }

        public static void RemoveMistlandsFog()
        {
            // Obtenemos la instancia de EnvMan que controla los efectos climáticos
            var envMan = EnvMan.instance;

            // Comprobamos si el jugador está en Mistlands
            if (Player.m_localPlayer != null && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mistlands)
            {
                // Accedemos al entorno actual (EnvSetup) del juego
                var currentEnv = envMan.GetCurrentEnvironment();

                if (currentEnv != null)
                {
                    // Modificamos la densidad de la niebla en Mistlands
                    currentEnv.m_fogDensityDay = 0f;    // Niebla durante el día
                    currentEnv.m_fogDensityNight = 0f;  // Niebla durante la noche
                    currentEnv.m_fogColorDay = Color.clear;  // Color de la niebla durante el día
                    currentEnv.m_fogColorNight = Color.clear; // Color de la niebla durante la noche
                }
            }
        }

        public static void RemoveSwampRain()
        {
            // Obtenemos la instancia de EnvMan que controla los efectos climáticos
            var envMan = EnvMan.instance;

            // Comprobamos si el jugador está en los Pantanos
            if (Player.m_localPlayer != null && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Swamp)
            {
                // Accedemos al entorno actual (EnvSetup) del juego
                var currentEnv = envMan.GetCurrentEnvironment();

                if (currentEnv != null)
                {
                    // Ajustamos los efectos visuales de lluvia manipulando el viento y la niebla
                    currentEnv.m_fogDensityDay = 0f;    // Eliminamos la niebla de día
                    currentEnv.m_fogDensityNight = 0f;  // Eliminamos la niebla de noche

                    // Reducimos el viento al mínimo, lo que también afecta el ambiente de lluvia
                    currentEnv.m_windMin = 0f;
                    currentEnv.m_windMax = 0f;
                }
            }
        }
    }
}
