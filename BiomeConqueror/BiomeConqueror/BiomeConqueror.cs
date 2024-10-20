using BepInEx;
using HarmonyLib;
using System.Linq;

namespace BiomeConqueror
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BiomeConqueror : BaseUnityPlugin
    {
        public const string GUID = "Turbero.BiomeConqueror";
        public const string NAME = "Biome Conqueror";
        public const string VERSION = "1.0.1";

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
}
