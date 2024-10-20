namespace BiomeConqueror
{
    public class BiomeConquerorUtils
    {
        public static bool hasUniqueKey(string key, bool configValue)
        {
            return ConfigurationFile.modEnabled.Value && configValue &&
                Player.m_localPlayer.HaveUniqueKey(key);
        }

        public static bool hasGlobalKey(string key)
        {
            return ConfigurationFile.modEnabled.Value && ConfigurationFile.worldProgression.Value &&
                ZoneSystem.instance.GetGlobalKey(key);
        }
    }
}
