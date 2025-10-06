using System;
using HarmonyLib;
using System.Reflection;

namespace BiomeConqueror.Benefits
{

    [HarmonyPatch(typeof(Character), "OnDeath")]
    public static class ReduceBossCooldownPatch
    {
        static void Postfix(Character __instance)
        {
            int baseReduction = ConfigurationFile.bossPowerReduction.Value;
            if (baseReduction <= 0) return;

            // Only enemies, not PvP
            if (__instance is Player) return;

            
            string monsterName = __instance.name.Replace("(Clone)", "");
            if (!ConfigurationFile.enemiesForReduction.Value.Contains(monsterName)) return;
            Logger.Log($"Killed {monsterName}.");

            // Last attacker
            HitData m_lastHit = (HitData) typeof(Character).GetField("m_lastHit", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

            Character killer = m_lastHit.GetAttacker();
            if (killer == Player.m_localPlayer)
            {
                
                Player lastPlayer = killer as Player;
                float m_guardianPowerCooldown = lastPlayer.m_guardianPowerCooldown;
                if (m_guardianPowerCooldown > 0)
                {
                    if (ConfigurationFile.bossPowerReductionBumpedByStars.Value)
                    {
                        //GetLevel() starts with 1
                        baseReduction *= __instance.GetLevel();
                    }
                    
                    lastPlayer.m_guardianPowerCooldown = Math.Max(0, lastPlayer.m_guardianPowerCooldown - baseReduction);

                    // Visual feedback
                    lastPlayer.Message(MessageHud.MessageType.TopLeft, ConfigurationFile.reductionMessageSuccess.Value.Replace("{0}", ""+baseReduction));
                }
            }
        }
    }
}