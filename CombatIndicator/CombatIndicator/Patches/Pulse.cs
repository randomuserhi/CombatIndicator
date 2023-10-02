using API;
using HarmonyLib;

namespace CombatIndicator.Patches
{
    [HarmonyPatch]
    internal class Pulse_Patches
    {
        [HarmonyPatch(typeof(PUI_LocalPlayerStatus), nameof(PUI_LocalPlayerStatus.UpdateBPM))]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        public static void Initialize_Postfix(PUI_LocalPlayerStatus __instance)
        {
            if (!ConfigManager.Enabled) return;

            string state = "Out of Combat";
            switch(DramaManager.CurrentStateEnum)
            {
                case DRAMA_State.Encounter:
                    // Removed due to desync with client
                    //state = "In Encounter";
                    //break;

                case DRAMA_State.Survival: // Unsure if this is correct - Assumed to be when you teleport between Alpha zones
                case DRAMA_State.IntentionalCombat:
                case DRAMA_State.Combat:
                    state = "In Combat";
                    break;
            }

            if (ConfigManager.BPM) __instance.m_pulseText.text += $" | {state}";
            else __instance.m_pulseText.text += $" | {(int)__instance.m_currentBPM}";
            if (ConfigManager.Debug) __instance.m_pulseText.text += $" ({DramaManager.CurrentStateEnum.ToString()}) | ({__instance.m_currentBPM})";
        }
    }
}