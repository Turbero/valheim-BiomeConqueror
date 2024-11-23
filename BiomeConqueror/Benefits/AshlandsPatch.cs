using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace BiomeConqueror.Benefits
{
    /*[HarmonyPatch(typeof(SE_Burning), "AddFireDamage")]
    public class AshlandsPatch
    {
        // Este método intercepta la aplicación del modificador de quemadura (burning)
        static bool Prefix(SE_Burning __instance, float damage)
        {
            // Verificamos si el Character afectado es el jugador
            var field = typeof(SE_Burning).GetField("m_character", BindingFlags.NonPublic | BindingFlags.Instance);
            var character = field.GetValue(__instance);

            if (character is Player)
            {
                Debug.Log("Es player");
                Player player = character as Player;

                // Aquí puedes aplicar cualquier lógica que necesites.
                // Por ejemplo, verificar si el jugador está en una zona de lava o en contacto con lava.
                // Si es así, prevenimos que se aplique el debuff.

                if (IsPlayerOnLava(player))
                {
                    Debug.Log("Es LAVA");
                    // Evitar la aplicación del debuff de quemadura si está sobre lava
                    return false; // Bloquea la aplicación del debuff.
                }
            }

            // Permitir la aplicación del debuff si no está sobre lava
            return true;
        }

        // Método personalizado para detectar si el jugador está en una zona de lava
        private static bool IsPlayerOnLava(Player player)
        {
            // Dependiendo de cómo esté configurada la lava, puedes realizar una comprobación aquí.
            // Por ejemplo, usar raycasts para detectar si el jugador está sobre un área de lava
            Vector3 playerPosition = player.transform.position;

            // Realiza un raycast hacia abajo desde la posición del jugador
            RaycastHit hit;
            if (Physics.Raycast(playerPosition, Vector3.down, out hit, 2f))
            {
                // Aquí puedes verificar si el "hit" corresponde a un área de lava
                // Usualmente, la lava puede tener un tag o una capa específica
                Debug.Log($"hit.collider: {hit.collider.ToString()}");
                Debug.Log($"hit.collider: {hit.collider.name}");
                if (hit.collider.CompareTag("Lava"))
                {
                    Debug.Log("** fire true");
                    return true; // El jugador está sobre lava
                }
            }
            Debug.Log("** fire false");
            return false; // No está sobre lava
        }
    }*/
}
