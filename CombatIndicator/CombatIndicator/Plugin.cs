using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using API;

using BetterChat;

namespace CombatIndicator;

public static class Module
{
    public const string GUID = "randomuserbruh.CombatIndicator";
    public const string Name = "CombatIndicator";
    public const string Version = "0.0.2";
}

[BepInPlugin(Module.GUID, Module.Name, Module.Version)]
[BepInDependency(BetterChatGUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BasePlugin
{
#if CURSOR
    private static GameObject container = null;
    private static Behaviour instance = null;
#endif

    const string BetterChatGUID = "randomuserhi.BetterChat";

    public override void Load()
    {
        APILogger.Debug(Module.Name, "Plugin is loaded!");
        harmony = new Harmony(Module.GUID);
        harmony.PatchAll();

        if (IL2CPPChainloader.Instance.Plugins.TryGetValue(BetterChatGUID, out _))
        {
            APILogger.Debug(Module.Name, "BetterChat is installed, adding commands.");

            ChatLogger.root.AddCommand("CombatIndicator/", null);
            ChatLogger.root.AddCommand("CombatIndicator/Enabled", new ChatLogger.Command()
            {
                action = (ChatLogger.CmdNode n, ChatLogger.Command cmd, string[] args) =>
                {
                    if (args.Length == 0)
                    {
                        n.Debug(cmd.help);
                        return;
                    }
                    int value;
                    if (int.TryParse(args[0], out value))
                    {
                        if (value != 0 && value != 1)
                        {
                            n.Debug(cmd.help);
                            return;
                        }
                        ConfigManager.Enabled = value == 1;
                        n.Debug($"Enabled set to {(ConfigManager.Enabled ? "True" : "False")}");
                    }
                    else
                    {
                        n.Debug(cmd.help);
                        return;
                    }
                },
                description = "1 for enable, 0 for disable",
                syntax = "<value>"
            });
        }

#if CURSOR
        ClassInjector.RegisterTypeInIl2Cpp<Behaviour>();
        RundownManager.add_OnExpeditionGameplayStarted((Action)OnGameplayStart);
#endif

        APILogger.Debug(Module.Name, "Debug is " + (ConfigManager.Debug ? "Enabled" : "Disabled"));
    }

    private static Harmony? harmony;

#if CURSOR
    [HarmonyPatch(typeof(RundownManager))]
    private class RundownPatch
    {
        [HarmonyPatch(nameof(RundownManager.OnExpeditionEnded))]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        private static void OnExpeditionEnded()
        {
            OnExpeditionEnd();
        }
    }

    private static void OnGameplayStart()
    {
#if DEBUG
        APILogger.Debug(Module.Name, "Gameplay started!");
#endif

        if (instance == null && container == null)
        {
            container = new GameObject();
            instance = container.AddComponent<Behaviour>();
        }
        else APILogger.Debug(Module.Name, "Behaviour instance already exists!");
    }
    private static void OnExpeditionEnd()
    {
#if DEBUG
        APILogger.Debug(Module.Name, "Cleanup started!");
#endif

        if (container != null)
        {
            GameObject.Destroy(container);
            container = null;
            instance = null;
        }
        else APILogger.Debug(Module.Name, "Object has already been destroyed!");
    }
#endif
}