using System.Collections.Generic;
using System.Linq;
using BiomeConqueror.Helpers;
using HarmonyLib;

namespace BiomeConqueror
{
    [HarmonyPatch(typeof(Chat), "InputText")]
    public static class ChatCommandPatch
    {
        static bool Prefix(Chat __instance)
        {
            string text = __instance.m_input.text;
            if (!text.StartsWith("/")) return true;

            string[] args = text.Substring(1).Split(' ');
            string command = args[0].ToLower();
            if (command == "update-old-keys")
            {
                updateOldKeys();
                return false;
            }

            return true;
        }

        private static void updateOldKeys()
        {
            Player player = Player.m_localPlayer;
            if (player == null) return;
            List<string> keys = player.GetUniqueKeys();

            List<string> keysToRemove = new List<string>();
            List<string> keysToAdd = new List<string>();
            foreach (var key in keys.Where(key => Constants.benefitDefeatedPlayerOldKeys.Contains(key)))
            {
                Logger.Log($"Old key detected. Refreshing {key} -> {key + "_BC"}");

                Logger.Log("key to remove: " + key);
                keysToRemove.Add(key);

                Logger.Log("key to add: " + key + "_BC");
                keysToAdd.Add(key + "_BC");
            }

            keysToRemove.ForEach(key => player.RemoveUniqueKey(key));
            keysToAdd.ForEach(key => player.AddUniqueKey(key));
        }
    }
}