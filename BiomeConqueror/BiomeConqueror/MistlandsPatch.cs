using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace BiomeConqueror
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
            if (!ConfigurationFile.modEnabled.Value) return;
            
            try
            {
                if (BiomeConquerorUtils.hasUniqueKey("QueenDefeated", ConfigurationFile.queenBenefitEnabled.Value) ||
                    BiomeConquerorUtils.hasGlobalKey("defeated_queen"))
                {
                    var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems().FirstOrDefault(i => i.m_dropPrefab.name == "Demister");

                    if (!__instance.isActiveAndEnabled || itemData == null) return;
                    __instance.m_forceField.endRange = ConfigurationFile.queenBenefitRange.Value;
                }
            }
            catch (Exception ex) {}
        }
    }
}