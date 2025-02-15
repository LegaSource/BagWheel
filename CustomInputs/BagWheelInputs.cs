using LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils.BindingPathEnums;
using UnityEngine.InputSystem;
using BagWheel.Behaviours;
using GameNetcodeStuff;

namespace BagWheel.CustomInputs
{
    internal class BagWheelInputs : LcInputActions
    {
        private static BagWheelInputs instance;

        public static BagWheelInputs Instance
        {
            get
            {
                instance ??= new BagWheelInputs();
                return instance;
            }
            private set { instance = value; }
        }

        [InputAction(KeyboardControl.Z, GamepadControl = GamepadControl.ButtonEast, Name = "Bag Wheel")]
        public InputAction BagWheelKey { get; set; }

        public void EnableInputs()
            => BagWheelKey.performed += OpenBagWheel;

        public void OpenBagWheel(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (BagWheel.bagWheelInterface == null) return;

            PlayerControllerB player = GameNetworkManager.Instance?.localPlayerController;
            if (player == null) return;
            if (!HasBeltBagItem(player)) return;

            BagWheelController.OpenBagWheel(!BagWheelController.bagWheelSelected);
        }

        public bool HasBeltBagItem(PlayerControllerB player)
        {
            for (int i = 0; i < player.ItemSlots.Length; i++)
            {
                GrabbableObject grabbableObject = player.ItemSlots[i];
                if (grabbableObject == null) continue;
                if (grabbableObject is BeltBagItem) return true;
            }
            return false;
        }
    }
}
