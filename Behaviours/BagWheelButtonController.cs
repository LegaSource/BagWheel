using BagWheel.Managers;
using BagWheel.Patches;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BagWheel.Behaviours
{
    public class BagWheelButtonController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
    {
        public int Id;
        public string itemName;

        public Button button;
        private Animator animator;
        public TextMeshProUGUI itemText;

        private void Start()
        {
            button = GetComponent<Button>();
            animator = GetComponent<Animator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!button.interactable) return;

            animator.SetBool("Hover", true);
            itemText.text = itemName;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!button.interactable) return;

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
            if (string.IsNullOrEmpty(itemName))
            {
                BagWheel.mls.LogError("SearchItem: itemName is not assigned!");
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
                    if (grabbableObjectBag.itemProperties.itemName.IndexOf(itemName, StringComparison.OrdinalIgnoreCase) == -1) continue;
                    
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
