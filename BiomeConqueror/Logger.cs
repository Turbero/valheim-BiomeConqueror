using BepInEx.Logging;

namespace BiomeConqueror
{
    public static class Logger
    {
        private static readonly ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(BiomeConqueror.NAME);
        internal static void Log(object s)
        {
            if (!ConfigurationFile.debug.Value)
            {
                return;
            }

            logger.LogInfo(s?.ToString());
        }
        
        internal static void LogInfo(object s)
        {
            logger.LogInfo(s?.ToString());
        }

        internal static void LogWarning(object s)
        {
            logger.LogWarning(s?.ToString());
        }

        internal static void LogError(object s)
        {
            logger.LogError(s?.ToString());
        }
    }
}
