using System.Collections.Generic;
using System;
using UnityEngine;
using HarmonyLib;
using static BiomeConqueror.Helpers.Constants;

namespace BiomeConqueror.Helpers
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
            foreach (var def in benefitDefeatedMessageKeys)
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
            if (spriteName == TROPHY_EIKTHYR)
            {
                return Localization.instance.Localize("$biome_meadows") + " / " + Localization.instance.Localize("$item_deermeat") + " +" + ConfigurationFile.eikthyrBenefitExtraDrop.Value;
            }
            if (spriteName == TROPHY_ELDER)
            {
                return Localization.instance.Localize("$biome_blackforest") + " / " + Localization.instance.Localize("$enemy_troll") + " +" + Localization.instance.Localize("$inventory_damage");
            }
            if (spriteName == TROPHY_BONEMASS)
            {
                return Localization.instance.Localize("$biome_swamp") + " / " + Localization.instance.Localize("$se_wet_name") + " = " + Localization.instance.Localize("$menu_none");
            }
            if (spriteName == TROPHY_MODER)
            {
                return Localization.instance.Localize("$biome_mountain") + " / " + Localization.instance.Localize("$se_freezing_name") + " = " + Localization.instance.Localize("$menu_none");
            }
            if (spriteName == TROPHY_YAGLUTH)
            {
                return Localization.instance.Localize("$biome_plains") + " / " + Localization.instance.Localize("$enemy_deathsquito") + " = " + Localization.instance.Localize("$menu_none");
            }
            if (spriteName == TROPHY_QUEEN)
            {
                return Localization.instance.Localize("$item_demister") + " = " + ConfigurationFile.queenBenefitRange.Value + "m.";
            }
            if (spriteName == TROPHY_FADER)
            {
                return Localization.instance.Localize("$biome_ashlands") + " / " + Localization.instance.Localize("$item_durability") + " " + Localization.instance.Localize("$inventory_fire") + " +" + ConfigurationFile.faderBenefitDamageFireResistant.Value + "%";
            }
            return "";
        }

        public static void ActivateCurrentEnvBiomeBenefitBuff()
        {
            Player player = Player.m_localPlayer;
            if (player == null) return;

            if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Meadows && BiomeConquerorUtils.isEikthyrDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(player, EIKTHYR_DEFEATED_MESSAGE_KEY, TROPHY_EIKTHYR);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.BlackForest && BiomeConquerorUtils.isElderDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(player, ELDER_DEFEATED_MESSAGE_KEY, TROPHY_ELDER);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Swamp && BiomeConquerorUtils.isBonemassDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(player, BONEMASS_DEFEATED_MESSAGE_KEY, TROPHY_BONEMASS);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Mountain && BiomeConquerorUtils.isModerDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(player, MODER_DEFEATED_MESSAGE_KEY, TROPHY_MODER);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Plains && BiomeConquerorUtils.isYagluthDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(player, YAGLUTH_DEFEATED_MESSAGE_KEY, TROPHY_YAGLUTH);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.Mistlands && BiomeConquerorUtils.isQueenDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(player, QUEEN_DEFEATED_MESSAGE_KEY, TROPHY_QUEEN);
            }
            else if (EnvMan.instance.GetCurrentBiome() == Heightmap.Biome.AshLands && BiomeConquerorUtils.isFaderDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(player, FADER_DEFEATED_MESSAGE_KEY, TROPHY_FADER);
            }
            else
            {
                RemoveAllBenefitBuffs();
            }
        }

        public static void AddPlayerBiomeBenefitBuff()
        {
            Player player = Player.m_localPlayer;
            if (player == null) return;

            if (player.GetCurrentBiome() == Heightmap.Biome.Meadows && BiomeConquerorUtils.isEikthyrDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, EIKTHYR_DEFEATED_MESSAGE_KEY, TROPHY_EIKTHYR);
            }
            else if (player.GetCurrentBiome() == Heightmap.Biome.BlackForest && BiomeConquerorUtils.isElderDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, ELDER_DEFEATED_MESSAGE_KEY, TROPHY_ELDER);
            }
            else if (player.GetCurrentBiome() == Heightmap.Biome.Swamp && BiomeConquerorUtils.isBonemassDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, BONEMASS_DEFEATED_MESSAGE_KEY, TROPHY_BONEMASS);
            }
            else if (player.GetCurrentBiome() == Heightmap.Biome.Mountain && BiomeConquerorUtils.isModerDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, MODER_DEFEATED_MESSAGE_KEY, TROPHY_MODER);
            }
            else if (player.GetCurrentBiome() == Heightmap.Biome.Plains && BiomeConquerorUtils.isYagluthDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, YAGLUTH_DEFEATED_MESSAGE_KEY, TROPHY_YAGLUTH);
            }
            else if (player.GetCurrentBiome() == Heightmap.Biome.Mistlands && BiomeConquerorUtils.isQueenDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, QUEEN_DEFEATED_MESSAGE_KEY, TROPHY_QUEEN);
            }
            else if (player.GetCurrentBiome() == Heightmap.Biome.AshLands && BiomeConquerorUtils.isFaderDefeatedForPlayer())
            {
                if (ConfigurationFile.benefitIcons.Value) AddBenefitBuff(Player.m_localPlayer, FADER_DEFEATED_MESSAGE_KEY, TROPHY_FADER);
            }
        }
    }
}
