using HarmonyLib;
using System.Reflection;

namespace BiomeConqueror.Benefits
{
    [HarmonyPatch]
    public class PlainsPatch
    {

        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(MonsterAI), "PheromoneFleeCheck");
        }

        static bool Prefix(MonsterAI __instance, Character target, ref bool __result)
        {
            if (BiomeConquerorUtils.isYagluthDefeatedForPlayer() &&
                __instance.name.Contains("Deathsquito"))
            {
                __result = true; //override method result
                return false; //Stop executing real method
            }
            else
            {
                return true;
            }
        }
    }
}
