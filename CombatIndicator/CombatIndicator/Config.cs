using BepInEx.Configuration;
using BepInEx;

namespace CombatIndicator
{
    public static class ConfigManager
    {
        static ConfigManager()
        {
            string text = Path.Combine(Paths.ConfigPath, "CombatIndicator.cfg");
            ConfigFile configFile = new ConfigFile(text, true);

            debug = configFile.Bind(
                "Debug",
                "enable",
                false,
                "Enables debug messages when true.");

            enabled = configFile.Bind(
                "Settings",
                "enabled",
                true,
                "Determines if the plugin is enabled or not.");
        }

        public static bool Debug
        {
            get { return debug.Value; }
        }
        public static bool Enabled
        {
            get { return enabled.Value; }
            set { enabled.Value = value; }
        }

        private static ConfigEntry<bool> debug;
        private static ConfigEntry<bool> enabled;
    }
}