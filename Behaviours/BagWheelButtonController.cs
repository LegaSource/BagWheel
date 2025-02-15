using BagWheel.Managers;
using BagWheel.Patches;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BagWheel.Behaviours
{
    public class BagWheelButtonController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Type itemType;
        public int Id;
        private Animator animator;
        public string itemName;
        public TextMeshProUGUI itemText;

        private void Start()
            => animator = GetComponent<Animator>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            animator.SetBool("Hover", true);
            itemText.text = itemName;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetBool("Hover", false);
            itemText.text = "";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BagWheelController.OpenBagWheel(false);

            if (SearchItem()) itemText.text = itemName;
            else itemText.text = "";
        }

        public bool SearchItem()
        {
            if (itemType == null)
            {
                BagWheel.mls.LogError("SearchItem: itemType is not initialized!");
                return false;
            }

            PlayerControllerB localPlayer = GameNetworkManager.Instance.localPlayerController;
            for (int i = 0; i < localPlayer.ItemSlots.Length; i++)
            {
                GrabbableObject grabbableObject = localPlayer.ItemSlots[i];
                if (grabbableObject == null) continue;
                if (grabbableObject is not BeltBagItem beltBagItem) continue;

                for (int j = beltBagItem.objectsInBag.Count - 1; j >= 0; j--)
                {
                    GrabbableObject grabbableObjectBag = beltBagItem.objectsInBag[j];
                    if (grabbableObjectBag == null) continue;
                    if (!itemType.IsAssignableFrom(grabbableObjectBag.GetType())) continue;

                    PlayerControllerBPatch.beltBagItem = beltBagItem;
                    PlayerControllerBPatch.bagItem = new KeyValuePair<int, GrabbableObject>(j, grabbableObjectBag);

                    BagWheelNetworkManager.Instance.SwitchToItemServerRpc((int)localPlayer.playerClientId, grabbableObjectBag.GetComponent<NetworkObject>(), i);
                    return true;
                }
            }
            return false;
        }
    }
}
