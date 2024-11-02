using System.Collections.Generic;
using System;
using UnityEngine;
using HarmonyLib;

namespace BiomeConqueror
{
    public class PlayerBuffs
    {
        private static Dictionary<string, Sprite> cachedSprites = new Dictionary<string, Sprite>();

        public static void AddBenefitBuff(Player player, string name, string spriteName)
        {
            RemoveAllBenefitBuffs();

            SEMan seMan = player.GetSEMan();

            // Create new custom status effect
            SE_Stats customBuff = ScriptableObject.CreateInstance<SE_Stats>();
            customBuff.m_name = name;
            customBuff.m_tooltip = getBenefitTooltipBySpriteName(spriteName); // same description compendium text value
            customBuff.m_icon = getSprite(spriteName);
            customBuff.name = name; // to produce distinct hash values

            // Apply buff to player
            int nameHash = customBuff.NameHash();
            Logger.Log($"name: {customBuff.name}, m_name: {customBuff.m_name}, nameHash: {nameHash}");

            seMan.AddStatusEffect(customBuff);
            Logger.Log($"Added buff: {customBuff.m_name}");
        }

        public static void RemoveAllBenefitBuffs()
        {
            List<string> benefitBuffsDef = new List<string> {
                "$event_boss03_end",
                "$event_boss04_end",
                "$event_boss05_end",
                "$enemy_boss_queen_deathmessage"
             };

            foreach (var def in benefitBuffsDef)
            {
                Player player = Player.m_localPlayer;
                SEMan seMan = player.GetSEMan();

                // Find and delete buff
                StatusEffect existingBuff = seMan.GetStatusEffect(def.GetHashCode());
                if (existingBuff != null)
                {
                    seMan.RemoveStatusEffect(existingBuff);
                    Logger.Log($"Deleted buff: {existingBuff.m_name}");
                }
            }
        }

        public static Sprite getSprite(String name)
        {
            if (!cachedSprites.ContainsKey(name))
            {
                Logger.Log($"Finding {name} sprite...");
                var allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
                for (var i = 0; i < allSprites.Length; i++)
                {
                    var sprite = allSprites[i];
                    if (sprite.name == name)
                    {
                        Logger.Log($"{name} sprite found.");
                        cachedSprites.Add(name, sprite);
                        return sprite;
                    }
                }
                Logger.Log($"{name} sprite NOT found.");
                return null;
            }
            else
            {
                return cachedSprites.GetValueSafe(name);
            }
        }

        private static string getBenefitTooltipBySpriteName(string spriteName)
        {
            if (spriteName == "TrophyBonemass")
            {
                return Localization.instance.Localize("$biome_swamp") + " / " + Localization.instance.Localize("$se_wet_name") + " = " + Localization.instance.Localize("$menu_none");
            }else if (spriteName == "TrophyDragonQueen")
            {
                return Localization.instance.Localize("$biome_mountain") + " / " + Localization.instance.Localize("$se_freezing_name") + " = " + Localization.instance.Localize("$menu_none");
            }
            else if (spriteName == "TrophyGoblinKing")
            {
                return Localization.instance.Localize("$biome_plains") + " / " + Localization.instance.Localize("$enemy_deathsquito") + " = " + Localization.instance.Localize("$menu_none");
            }
            else if (spriteName == "TrophySeekerQueen")
            {
                return Localization.instance.Localize("$item_demister") + " = " + ConfigurationFile.queenBenefitEligibleRange.Value + "m.";
            }
            return "";
        }

        public static void ActivateCurrentBiomeBenefitBuff()
        {
            if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Swamp && BiomeConquerorUtils.isBonemassDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$event_boss03_end", "TrophyBonemass");
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Mountain && BiomeConquerorUtils.isModerDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$event_boss04_end", "TrophyDragonQueen");
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Plains && BiomeConquerorUtils.isYagluthDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$event_boss05_end", "TrophyGoblinKing");
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Mistlands && BiomeConquerorUtils.isQueenDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$enemy_boss_queen_deathmessage", "TrophySeekerQueen");
            }
            else
            {
                RemoveAllBenefitBuffs();
            }
        }

        public static void AddPlayerBiomeBenefitBuff()
        {
            if (BiomeConquerorUtils.isBonemassDefeatedForPlayer() && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Swamp)
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$event_boss03_end", "TrophyBonemass");
            }
            else if (BiomeConquerorUtils.isModerDefeatedForPlayer() && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mountain)
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$event_boss04_end", "TrophyDragonQueen");
            }
            else if (BiomeConquerorUtils.isYagluthDefeatedForPlayer() && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Plains)
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$event_boss05_end", "TrophyGoblinKing");
            }
            else if (BiomeConquerorUtils.isQueenDefeatedForPlayer() && Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mistlands)
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, "$enemy_boss_queen_deathmessage", "TrophySeekerQueen");
            }
        }
    }
}
