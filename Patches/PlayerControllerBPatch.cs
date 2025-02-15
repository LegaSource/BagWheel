using BagWheel.Managers;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using Unity.Netcode;

namespace BagWheel.Patches
{
    internal class PlayerControllerBPatch
    {
        public static BeltBagItem beltBagItem;
        public static KeyValuePair<int, GrabbableObject>? bagItem;

        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.DiscardHeldObject))]
        [HarmonyAfter(["Lega.CursedScraps"])]
        [HarmonyPrefix]
        private static bool PreDropObject(ref PlayerControllerB __instance)
        {
            if (!bagItem.HasValue || beltBagItem == null) return true;

            GrabbableObject grabbableObject = bagItem.Value.Value;
            if (grabbableObject != __instance.currentlyHeldObjectServer) return true;

            BagWheelNetworkManager.Instance.PocketItem(__instance, grabbableObject);
            BagWheelNetworkManager.Instance.PocketItemServerRpc((int)__instance.playerClientId, grabbableObject.GetComponent<NetworkObject>());

            beltBagItem.RemoveObjectFromBag(bagItem.Value.Key);
            bagItem = null;

            return false;
        }

        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SwitchToItemSlot))]
        [HarmonyPrefix]
        private static void PreSwitchItem(ref PlayerControllerB __instance)
        {
            if (!bagItem.HasValue || beltBagItem == null) return;

            GrabbableObject grabbableObject = bagItem.Value.Value;
            BagWheelNetworkManager.Instance.PocketItem(__instance, grabbableObject);
            BagWheelNetworkManager.Instance.PocketItemServerRpc((int)__instance.playerClientId, grabbableObject.GetComponent<NetworkObject>());

            bagItem = null;
        }
    }
}