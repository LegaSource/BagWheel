using HarmonyLib;
using System.Reflection;

namespace BagWheel.Patches.ModsPatches
{
    [HarmonyPatch]
    internal class BetterSprayPaintPatch
    {
        public static bool HeldByLocalPlayer(ref object __instance, ref bool __result)
        {
            bool isSprayPaintActive = CheckSprayPaintActivity(__instance);
            if (!isSprayPaintActive) __result = true;
            return isSprayPaintActive;
        }

        public static bool InActiveSlot(ref object __instance, ref bool __result)
        {
            bool isSprayPaintActive = CheckSprayPaintActivity(__instance);
            if (!isSprayPaintActive) __result = true;
            return isSprayPaintActive;
        }

        public static bool CheckSprayPaintActivity(object instance)
        {
            if (!PlayerControllerBPatch.bagItem.HasValue) return true;
            if (PlayerControllerBPatch.bagItem.Value.Value is not SprayPaintItem) return true;

            var instanceProperty = instance.GetType().GetProperty("instance", BindingFlags.NonPublic | BindingFlags.Instance);
            if (instanceProperty == null) return true;

            if (instanceProperty.GetValue(instance) is SprayPaintItem sprayPaintItem && sprayPaintItem == PlayerControllerBPatch.bagItem.Value.Value) return false;
            return true;
        }
    }
}
