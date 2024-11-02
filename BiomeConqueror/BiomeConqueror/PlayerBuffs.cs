using System.Collections.Generic;
using System;
using UnityEngine;
using HarmonyLib;
using static BiomeConqueror.Constants;

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
                BONEMASS_DEFEATED_MESSAGE_KEY,
                MODER_DEFEATED_MESSAGE_KEY,
                YAGLUTH_DEFEATED_MESSAGE_KEY,
                QUEEN_DEFEATED_MESSAGE_KEY
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
            if (spriteName == TROPHY_BONEMASS)
            {
                return Localization.instance.Localize("$biome_swamp") + " / " + Localization.instance.Localize("$se_wet_name") + " = " + Localization.instance.Localize("$menu_none");
            }else if (spriteName == TROPHY_MODER)
            {
                return Localization.instance.Localize("$biome_mountain") + " / " + Localization.instance.Localize("$se_freezing_name") + " = " + Localization.instance.Localize("$menu_none");
            }
            else if (spriteName == TROPHY_YAGLUTH)
            {
                return Localization.instance.Localize("$biome_plains") + " / " + Localization.instance.Localize("$enemy_deathsquito") + " = " + Localization.instance.Localize("$menu_none");
            }
            else if (spriteName == TROPHY_QUEEN)
            {
                return Localization.instance.Localize("$item_demister") + " = " + ConfigurationFile.queenBenefitEligibleRange.Value + "m.";
            }
            return "";
        }

        public static void ActivateCurrentEnvBiomeBenefitBuff()
        {
            if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Swamp && BiomeConquerorUtils.isBonemassDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, BONEMASS_DEFEATED_MESSAGE_KEY, TROPHY_BONEMASS);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Mountain && BiomeConquerorUtils.isModerDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, MODER_DEFEATED_MESSAGE_KEY, TROPHY_MODER);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Plains && BiomeConquerorUtils.isYagluthDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, YAGLUTH_DEFEATED_MESSAGE_KEY, TROPHY_YAGLUTH);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Mistlands && BiomeConquerorUtils.isQueenDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, QUEEN_DEFEATED_MESSAGE_KEY, TROPHY_QUEEN);
            }
            else
            {
                RemoveAllBenefitBuffs();
            }
        }

        public static void AddPlayerBiomeBenefitBuff()
        {
            if (Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Swamp && BiomeConquerorUtils.isBonemassDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, BONEMASS_DEFEATED_MESSAGE_KEY, TROPHY_BONEMASS);
            }
            else if (Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mountain && BiomeConquerorUtils.isModerDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, MODER_DEFEATED_MESSAGE_KEY, TROPHY_MODER);
            }
            else if (Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Plains && BiomeConquerorUtils.isYagluthDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, YAGLUTH_DEFEATED_MESSAGE_KEY, TROPHY_YAGLUTH);
            }
            else if (Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mistlands && BiomeConquerorUtils.isQueenDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, QUEEN_DEFEATED_MESSAGE_KEY, TROPHY_QUEEN);
            }
        }
    }
}
