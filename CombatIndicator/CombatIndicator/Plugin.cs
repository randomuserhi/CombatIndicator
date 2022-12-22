using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using API;

using UnityEngine;
using System.ComponentModel;

namespace CombatIndicator;

public static class Module
{
    public const string GUID = "randomuserbruh.CombatIndicator";
    public const string Name = "CombatIndicator";
    public const string Version = "0.0.1";
}

[BepInPlugin(Module.GUID, Module.Name, Module.Version)]
public class Plugin : BasePlugin
{
#if CURSOR
    private static GameObject container = null;
    private static Behaviour instance = null;
#endif

    public override void Load()
    {
        APILogger.Debug(Module.Name, "Plugin is loaded!");

#if CURSOR
        ClassInjector.RegisterTypeInIl2Cpp<Behaviour>();
        RundownManager.add_OnExpeditionGameplayStarted((Action)OnGameplayStart);
#endif

        harmony = new Harmony(Module.GUID);
        harmony.PatchAll();
    }

    private static Harmony harmony;

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