using API;
using HarmonyLib;
using Player;
using SNetwork;

namespace CombatIndicator.Patches
{
    [HarmonyPatch]
    internal class Pulse_Patches
    {
        [HarmonyPatch(typeof(PlayerChatManager), nameof(PlayerChatManager.DoSendChatMessage))]
        [HarmonyPrefix]
        public static bool DoSendChatMessage_Prefix(PlayerChatManager __instance, PlayerChatManager.pChatMessage data)
        {
            if (ConfigManager.Debug) APILogger.Debug(Module.Name, $"Sent message {data.message.data}");
            if (data.message.data == ".toggle")
            {
                ConfigManager.Enabled = !ConfigManager.Enabled;
                APILogger.Debug(Module.Name, $"Set Enabled to {ConfigManager.Enabled}");
                return false;
            }
            return true;
        }

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

            __instance.m_pulseText.text += $" | " + state;
            if (ConfigManager.Debug) __instance.m_pulseText.text += $" ({DramaManager.CurrentStateEnum.ToString()})";
        }
    }
}