using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace BagWheel.Managers
{
    internal class BagWheelNetworkManager : NetworkBehaviour
    {
        public static BagWheelNetworkManager Instance;

        public void Awake() => Instance = this;

        [ServerRpc(RequireOwnership = false)]
        public void SwitchToItemServerRpc(int playerId, NetworkObjectReference obj, int currentSlot)
            => SwitchToItemClientRpc(playerId, obj, currentSlot);

        [ClientRpc]
        private void SwitchToItemClientRpc(int playerId, NetworkObjectReference obj, int currentSlot)
        {
            if (!obj.TryGet(out var networkObject)) return;

            GrabbableObject grabbableObject = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();
            SwitchToItem(StartOfRound.Instance.allPlayerObjects[playerId].GetComponent<PlayerControllerB>(), grabbableObject, currentSlot);
        }

        public void SwitchToItem(PlayerControllerB player, GrabbableObject grabbableObject, int currentSlot)
        {
            player.currentItemSlot = currentSlot;
            if (player.IsOwner)
            {
                for (int i = 0; i < HUDManager.Instance.itemSlotIconFrames.Length; i++)
                {
                    HUDManager.Instance.itemSlotIconFrames[i].GetComponent<Animator>().SetBool("selectedSlot", value: false);
                }
                HUDManager.Instance.itemSlotIconFrames[currentSlot].GetComponent<Animator>().SetBool("selectedSlot", value: true);
            }
            if (player.currentlyHeldObjectServer != null)
            {
                player.currentlyHeldObjectServer.playerHeldBy = player;
                if (player.IsOwner)
                {
                    player.SetSpecialGrabAnimationBool(setTrue: false, player.currentlyHeldObjectServer);
                }
                player.currentlyHeldObjectServer.PocketItem();
                if (!string.IsNullOrEmpty(grabbableObject.itemProperties.pocketAnim))
                {
                    player.playerBodyAnimator.SetTrigger(grabbableObject.itemProperties.pocketAnim);
                }
            }
            grabbableObject.playerHeldBy = player;
            grabbableObject.EquipItem();
            if (player.IsOwner)
            {
                player.SetSpecialGrabAnimationBool(setTrue: true, grabbableObject);
            }
            if (player.currentlyHeldObjectServer != null)
            {
                if (grabbableObject.itemProperties.twoHandedAnimation || player.currentlyHeldObjectServer.itemProperties.twoHandedAnimation)
                {
                    player.playerBodyAnimator.ResetTrigger("SwitchHoldAnimationTwoHanded");
                    player.playerBodyAnimator.SetTrigger("SwitchHoldAnimationTwoHanded");
                }
                player.playerBodyAnimator.ResetTrigger("SwitchHoldAnimation");
                player.playerBodyAnimator.SetTrigger("SwitchHoldAnimation");
            }
            player.twoHandedAnimation = grabbableObject.itemProperties.twoHandedAnimation;
            player.twoHanded = grabbableObject.itemProperties.twoHanded;
            player.playerBodyAnimator.SetBool("GrabValidated", value: true);
            player.playerBodyAnimator.SetBool("cancelHolding", value: false);
            player.isHoldingObject = true;
            player.currentlyHeldObjectServer = grabbableObject;
            if (player.IsOwner)
            {
                if (player.twoHanded)
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 0.1f, 0.13f, 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = true;
                }
                else
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 1.5f, 1f, 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = false;
                }
                grabbableObject.parentObject = player.localItemHolder;
            }
            grabbableObject.EnablePhysics(enable: false);
            grabbableObject.isHeld = true;
            grabbableObject.hasHitGround = false;
            grabbableObject.isInFactory = player.isInsideFactory;
            player.SetItemInElevator(player.isInHangarShipRoom, player.isInElevator, grabbableObject);
            player.twoHanded = grabbableObject.itemProperties.twoHanded;
            player.twoHandedAnimation = grabbableObject.itemProperties.twoHandedAnimation;
            if (!player.IsOwner)
            {
                grabbableObject.parentObject = player.serverItemHolder;
                player.isHoldingObject = true;
                player.carryWeight = Mathf.Clamp(player.carryWeight + (grabbableObject.itemProperties.weight - 1f), 1f, 10f);
                if (grabbableObject.itemProperties.grabSFX != null)
                {
                    player.itemAudio.PlayOneShot(grabbableObject.itemProperties.grabSFX, 1f);
                }
            }
            if (GameNetworkManager.Instance.localPlayerController.IsServer || GameNetworkManager.Instance.localPlayerController.IsHost)
            {
                grabbableObject.NetworkObject.ChangeOwnership(player.playerClientId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void PocketItemServerRpc(int playerId, NetworkObjectReference obj)
            => PocketItemClientRpc(playerId, obj);

        [ClientRpc]
        private void PocketItemClientRpc(int playerId, NetworkObjectReference obj)
        {
            if (!obj.TryGet(out var networkObject)) return;

            PlayerControllerB player = StartOfRound.Instance.allPlayerObjects[playerId].GetComponent<PlayerControllerB>();
            if (player.IsOwner) return;

            GrabbableObject grabbableObject = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();
            PocketItem(player, grabbableObject);
        }

        public void PocketItem(PlayerControllerB player, GrabbableObject grabbableObject)
        {
            if (player.IsOwner)
            {
                player.SetSpecialGrabAnimationBool(setTrue: false, grabbableObject);
                HUDManager.Instance.holdingTwoHandedItem.enabled = false;
            }
            player.playerBodyAnimator.SetBool("cancelHolding", value: true);
            player.isHoldingObject = false;
            player.twoHanded = false;
            player.twoHandedAnimation = false;
            grabbableObject.EnablePhysics(enable: false);
            grabbableObject.EnableItemMeshes(enable: true);
            grabbableObject.isHeld = false;
            grabbableObject.parentObject = null;
            grabbableObject.DiscardItemOnClient();
            grabbableObject.targetFloorPosition = new Vector3(3000f, -400f, 3000f);
            grabbableObject.startFallingPosition = new Vector3(3000f, -400f, 3000f);
            player.currentlyHeldObjectServer = null;
            player.currentlyHeldObject = null;
            player.playerBodyAnimator.SetTrigger(grabbableObject.itemProperties.twoHandedAnimation ? "SwitchHoldAnimationTwoHanded" : "SwitchHoldAnimation");
        }
    }
}
