using BepInEx.Configuration;
using BepInEx;

namespace CombatIndicator
{
    public static class ConfigManager
    {
        static ConfigManager()
        {
            string text = Path.Combine(Paths.ConfigPath, $"{Module.Name}.cfg");
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

            bpm = configFile.Bind(
                "Settings",
                "bpm",
                false,
                "Shows current bpm instead of combat state");
        }

        public static bool Debug
        {
            get { return debug.Value; }
            set { debug.Value = value; }
        }
        public static bool Enabled
        {
            get { return enabled.Value; }
            set { enabled.Value = value; }
        }

        public static bool BPM
        {
            get { return bpm.Value; }
            set { bpm.Value = value; }
        }

        private static ConfigEntry<bool> debug;
        private static ConfigEntry<bool> enabled;
        private static ConfigEntry<bool> bpm;
    }
}