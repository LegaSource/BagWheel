using BagWheel.Managers;
using HarmonyLib;

namespace BagWheel.Patches
{
    internal class HUDManagerPatch
    {
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.Start))]
        [HarmonyPostfix]
        private static void StartHUD()
            => ConfigManager.ConfigureWheelButtons();
    }
}
