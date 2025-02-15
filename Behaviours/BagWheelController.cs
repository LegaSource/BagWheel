using GameNetcodeStuff;
using UnityEngine;

namespace BagWheel.Behaviours
{
    public class BagWheelController : MonoBehaviour
    {
        public Animator animator;
        public static bool bagWheelSelected = false;

        public static void OpenBagWheel(bool enable)
        {
            bagWheelSelected = enable;

            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            player.inSpecialMenu = enable;

            Cursor.lockState = bagWheelSelected ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = enable;
        }

        public void Update()
        {
            if (bagWheelSelected) animator.SetBool("OpenBagWheel", true);
            else animator.SetBool("OpenBagWheel", false);
        }
    }
}
