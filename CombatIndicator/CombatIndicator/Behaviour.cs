using API;
using UnityEngine;

namespace CombatIndicator
{
    public class Behaviour : MonoBehaviour
    {
        private void Start()
        {
#if DEBUG
            APILogger.Debug(Module.Name, $"Behaviour started.");
#endif
        }

        private void Update()
        {
            if (DramaManager.CurrentStateEnum == DRAMA_State.Combat)
            {
                GuiManager.CrosshairLayer?.m_circleCrosshair.SetColor(Color.red);
            }
            else
            {
                GuiManager.CrosshairLayer?.m_circleCrosshair.ResetColor();
            }
        }

        private void OnDestroy()
        {
#if DEBUG
            APILogger.Debug(Module.Name, $"Behaviour destroyed.");
#endif
        }
    }
}
