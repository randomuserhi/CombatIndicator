using HarmonyLib;

namespace CombatIndicator.Patches
{
    [HarmonyPatch(typeof(PUI_LocalPlayerStatus))]
    internal class Pulse_Patches
    {
        [HarmonyPatch(nameof(PUI_LocalPlayerStatus.UpdateBPM))]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        public static void Initialize_Postfix(PUI_LocalPlayerStatus __instance)
        {
            __instance.m_pulseText.text += $" | " + (DramaManager.CurrentStateEnum == DRAMA_State.Combat ? "In Combat" : "Out of Combat"); 
        }
    }
}