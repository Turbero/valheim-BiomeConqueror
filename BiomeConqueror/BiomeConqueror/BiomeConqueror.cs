using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static TextsDialog;

namespace BiomeConqueror
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BiomeConqueror : BaseUnityPlugin
    {
        public const string GUID = "Turbero.BiomeConqueror";
        public const string NAME = "Biome Conqueror";
        public const string VERSION = "1.0.3";

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
                if (__instance.m_name == "$enemy_bonemass")
                {
                    Player.m_localPlayer.AddUniqueKey("BonemassDefeated");
                    Logger.Log($"** Bonemass defeated");
                }
                else if (__instance.m_name == "$enemy_dragon")
                {
                    Player.m_localPlayer.AddUniqueKey("ModerDefeated");
                    Logger.Log($"** Moder defeated");
                }
                else if (__instance.m_name == "$enemy_seekerqueen")
                {
                    Player.m_localPlayer.AddUniqueKey("QueenDefeated");
                    Logger.Log($"** Queen defeated");

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

    [HarmonyPatch]
    public class TextsDialogActiveEffectsPatch
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
            var benefitQueen = BiomeConquerorUtils.isQueenDefeatedForPlayer();

            if (benefitBonemass || benefitModer || benefitQueen) {

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
