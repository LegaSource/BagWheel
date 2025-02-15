using BagWheel.Managers;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace BagWheel.Patches
{
    internal class StartOfRoundPatch
    {
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Start))]
        [HarmonyBefore(["evaisa.lethallib"])]
        [HarmonyPostfix]
        private static void StartRound(ref StartOfRound __instance)
        {
            if (!NetworkManager.Singleton.IsHost || BagWheelNetworkManager.Instance != null) return;

            GameObject gameObject = Object.Instantiate(BagWheel.managerPrefab, __instance.transform.parent);
            gameObject.GetComponent<NetworkObject>().Spawn();
            BagWheel.mls.LogInfo("Spawning BagWheelNetworkManager");
        }
    }
}
