using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace BiomeConqueror.Benefits
{
    [HarmonyPatch]
    public class MistlandsPatch
    {
        public static float demisterRange;

        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Demister), "OnEnable");
        }

        static void Postfix(ref Demister __instance)
        {
            if (!ConfigurationFile.modEnabled.Value || Player.m_localPlayer == null) return;

            try
            {
                if (BiomeConquerorUtils.isQueenDefeatedForPlayer())
                {
                    var itemData = Player.m_localPlayer.GetInventory().GetEquippedItems()
                        .FirstOrDefault(i => i.m_dropPrefab.name == "Demister");

                    if (!__instance.isActiveAndEnabled || itemData == null) return;
                    __instance.m_forceField.endRange = ConfigurationFile.queenBenefitRange.Value;
                }

                demisterRange = __instance.m_forceField.endRange;

                //Update wisp light buff text
                var demisterSE = Player.m_localPlayer.GetSEMan().GetStatusEffects()
                    .First(effect => effect.name == "Demister");
                demisterSE.m_name = $"$item_demister: {demisterRange}m.";
                Logger.Log($"demister buff text updated to {demisterSE.m_name}");
            }
            catch (Exception ex){}
        }
    }


}