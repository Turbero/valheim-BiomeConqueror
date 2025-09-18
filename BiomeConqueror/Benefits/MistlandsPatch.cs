using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace BiomeConqueror.Benefits
{
    [HarmonyPatch]
    public class MistlandsPatch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Demister), "OnEnable");
        }

        static void Postfix(ref Demister __instance)
        {
            if (!ConfigurationFile.modEnabled.Value || Player.m_localPlayer == null) return;
            try
            {
                updateDemisterRangeAndText(__instance);
            }
            catch (Exception ex){}
        }

        public static void updateDemisterRangeAndText(Demister __instance)
        {
            if (BiomeConquerorUtils.isQueenDefeatedForPlayer())
            {
                var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");

                if (!__instance.isActiveAndEnabled || itemData == null) return;
                __instance.m_forceField.endRange = 
                    Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Mistlands
                        ? ConfigurationFile.queenBenefitRange.Value
                        : ConfigurationFile.queenBenefitBaseRange.Value;
            }

            float demisterRange = __instance.m_forceField.endRange;

            //Update wisp light buff text
            var demisterSE = Player.m_localPlayer.GetSEMan().GetStatusEffects().FirstOrDefault(effect => effect.name == "Demister");
            if (demisterSE != null)
            {
                demisterSE.m_name = $"$item_demister: {demisterRange}m.";
            }
        }
    }
}