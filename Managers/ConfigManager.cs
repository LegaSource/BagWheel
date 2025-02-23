using BagWheel.Behaviours;
using BepInEx.Configuration;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BagWheel.Managers
{
    public class ConfigManager
    {
        // GLOBAL
        public static ConfigEntry<string> slot1;
        public static ConfigEntry<string> slot2;
        public static ConfigEntry<string> slot3;
        public static ConfigEntry<string> slot4;
        public static ConfigEntry<string> slot5;
        public static ConfigEntry<string> slot6;
        public static ConfigEntry<string> slot7;
        public static ConfigEntry<string> slot8;

        public static void Load()
        {
            slot1 = BagWheel.configFile.Bind("Global",
                "Slot 1",
                $"{Constants.FLASHLIGHT}:{Constants.PRO_FLASHLIGHT},{Constants.FLASHLIGHT}",
                $"Slot 1 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
            slot2 = BagWheel.configFile.Bind("Global",
                "Slot 2",
                $"{Constants.SHOVEL}:{Constants.SHOVEL}",
                $"Slot 2 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
            slot3 = BagWheel.configFile.Bind("Global",
                "Slot 3",
                $"{Constants.SPRAY_PAINT}:{Constants.SPRAY_PAINT}",
                $"Slot 3 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
            slot4 = BagWheel.configFile.Bind("Global",
                "Slot 4",
                $"{Constants.WALKIE_TALKIE}:{Constants.WALKIE_TALKIE}",
                $"Slot 4 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
            slot5 = BagWheel.configFile.Bind("Global",
                "Slot 5",
                $"{Constants.STUN_GRENADE}:{Constants.STUN_GRENADE}",
                $"Slot 5 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
            slot6 = BagWheel.configFile.Bind("Global",
                "Slot 6",
                $"{Constants.BOOMBOX}:{Constants.BOOMBOX}",
                $"Slot 6 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
            slot7 = BagWheel.configFile.Bind("Global",
                "Slot 7",
                $"{Constants.ZAP_GUN}:{Constants.ZAP_GUN}",
                $"Slot 7 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
            slot8 = BagWheel.configFile.Bind("Global",
                "Slot 8",
                $"{Constants.TZP}:{Constants.TZP}",
                $"Slot 8 configuration.\nThe format is ImageName:ItemNameList, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}, {Constants.KNIFE}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the image name.");
        }

        public static void ConfigureWheelButtons()
        {
            BagWheel.bagWheelInterface = Object.Instantiate(BagWheel.bagWheelPrefab, Vector3.zero, Quaternion.identity, HUDManager.Instance.HUDContainer.transform.parent);
            BagWheel.bagWheelInterface.transform.localPosition = Vector3.zero;

            if (BagWheel.bagWheelInterface == null)
            {
                BagWheel.mls.LogError($"bagWheelInterface not initialized");
                return;
            }

            ConfigEntry<string>[] slots = [slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8];
            for (int i = 0; i < slots.Length; i++) ConfigureSlot(i + 1, slots[i]);
        }

        private static void ConfigureSlot(int slotId, ConfigEntry<string> slot)
        {
            string[] values = slot.Value.Split(':');
            if (values.Length == 2)
            {
                BagWheelButtonController bagButton = BagWheel.bagWheelInterface.GetComponentsInChildren<BagWheelButtonController>().FirstOrDefault(b => b.gameObject.name.Equals($"BagWheelButton{slotId}"));
                if (bagButton == null)
                {
                    BagWheel.mls.LogError($"Script not found for the {slot.Value} config");
                    return;
                }

                ConfigureWheelButton(ref bagButton, values[1].Split(','), values[0]);
            }
        }

        private static void ConfigureWheelButton(ref BagWheelButtonController bagButton, string[] itemNames, string spriteName)
        {
            Image imageButton = bagButton.gameObject.GetComponentsInChildren<Image>().FirstOrDefault(b => b.gameObject.name.Equals("Icon"));
            if (imageButton == null)
            {
                BagWheel.mls.LogError($"Image not found for {spriteName}");
                return;
            }

            if (Constants.DISABLED.Equals(spriteName))
            {
                bagButton.gameObject.GetComponent<Button>().interactable = false;
                return;
            }

            bagButton.eligibleItems = itemNames.ToList();
            bagButton.itemName = spriteName;
            imageButton.sprite = spriteName switch
            {
                Constants.FLASHLIGHT => BagWheel.flashlightSprite,
                Constants.SHOVEL => BagWheel.shovelSprite,
                Constants.SPRAY_PAINT => BagWheel.sprayPaintSprite,
                Constants.WALKIE_TALKIE => BagWheel.walkieTalkieSprite,
                Constants.STUN_GRENADE => BagWheel.stunGrenadeSprite,
                Constants.BOOMBOX => BagWheel.boomboxSprite,
                Constants.ZAP_GUN => BagWheel.zapGunSprite,
                Constants.TZP => BagWheel.tzpSprite,
                Constants.LOCKPICKER => BagWheel.lockpickerSprite,
                Constants.JETPACK => BagWheel.jetpackSprite,
                Constants.EXTENSION_LADDER => BagWheel.extensionLadderSprite,
                Constants.RADAR_BOOSTER => BagWheel.radarBoosterSprite,
                Constants.WEED_KILLER => BagWheel.weedKillerSprite,
                Constants.KNIFE => BagWheel.knifeSprite,
                _ => BagWheel.shovelSprite
            };
        }
    }
}
