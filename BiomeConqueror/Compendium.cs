using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static TextsDialog;

namespace BiomeConqueror
{
    [HarmonyPatch]
    public class CompendiumPatch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(TextsDialog), "AddActiveEffects");
        }

        static void Postfix(ref TextsDialog __instance)
        {
            var field = typeof(TextsDialog).GetField("m_texts", BindingFlags.NonPublic | BindingFlags.Instance);
            List<TextInfo> texts = (List<TextInfo>)field.GetValue(__instance);
            TextInfo activeEffectsText = texts[0];

            string currentText = activeEffectsText.m_text;

            var benefitBonemass = BiomeConquerorUtils.isBonemassDefeatedForPlayer();
            var benefitModer = BiomeConquerorUtils.isModerDefeatedForPlayer();
            var benefitYagluth = BiomeConquerorUtils.isYagluthDefeatedForPlayer();
            var benefitQueen = BiomeConquerorUtils.isQueenDefeatedForPlayer();

            if (benefitBonemass || benefitModer || benefitQueen)
            {

                StringBuilder stringBuilder = new StringBuilder(256);
                stringBuilder.Append("\n\n");
                stringBuilder.Append("<color=yellow>BiomeConqueror Mod</color>\n");
                if (benefitBonemass)
                {
                    stringBuilder.Append("<color=orange>" + Localization.instance.Localize("$se_bonemass_name") + "</color>\n");
                    stringBuilder.Append(Localization.instance.Localize("$biome_swamp") + " / " + Localization.instance.Localize("$se_wet_name") + " = " + Localization.instance.Localize("$menu_none"));
                    stringBuilder.Append("\n");
                }
                if (benefitModer)
                {
                    stringBuilder.Append("<color=orange>" + Localization.instance.Localize("$se_moder_name") + "</color>\n");
                    stringBuilder.Append(Localization.instance.Localize("$biome_mountain") + " / " + Localization.instance.Localize("$se_freezing_name") + " = " + Localization.instance.Localize("$menu_none"));
                    stringBuilder.Append("\n");
                }
                if (benefitYagluth)
                {
                    stringBuilder.Append("<color=orange>" + Localization.instance.Localize("$se_yagluth_name") + "</color>\n");
                    stringBuilder.Append(Localization.instance.Localize("$biome_plains") + " / " + Localization.instance.Localize("$enemy_deathsquito") + " = " + Localization.instance.Localize("$menu_none"));
                    stringBuilder.Append("\n");
                }
                if (benefitQueen)
                {
                    stringBuilder.Append("<color=orange>" + Localization.instance.Localize("$se_queen_name") + "</color>\n");
                    stringBuilder.Append(Localization.instance.Localize("$item_demister") + " = " + ConfigurationFile.queenBenefitEligibleRange.Value + "m.");
                    stringBuilder.Append("\n");
                }

                texts.RemoveAt(0);
                texts.Insert(0, new TextInfo(Localization.instance.Localize("$inventory_activeeffects"), currentText + stringBuilder.ToString()));
            }
        }
    }
}
