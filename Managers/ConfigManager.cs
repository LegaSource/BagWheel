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
        public static ConfigEntry<string> bagWheel;
        
        public static void Load()
        {
            bagWheel = BagWheel.configFile.Bind("Global",
                "Bag wheel",
                $"1:{Constants.FLASHLIGHT},2:{Constants.SHOVEL},3:{Constants.SPRAY_PAINT},4:{Constants.WALKIE_TALKIE},5:{Constants.STUN_GRENADE},6:{Constants.BOOMBOX},7:{Constants.ZAP_GUN},8:{Constants.TZP}",
                $"Bag wheel configuration.\nAccepted item names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}.");
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

            string[] items = bagWheel.Value.Split(',');
            foreach (string item in items)
            {
                string[] values = item.Split(':');
                if (values.Length == 2)
                {
                    BagWheelButtonController bagButton = BagWheel.bagWheelInterface.GetComponentsInChildren<BagWheelButtonController>().FirstOrDefault(b => b.gameObject.name.Equals($"BagWheelButton{values[0]}"));
                    if (bagButton == null)
                    {
                        BagWheel.mls.LogError($"Script not found for the {item} config");
                        return;
                    }
                    ConfigureWheelButton(ref bagButton, values[1]);
                }
            }
        }

        private static void ConfigureWheelButton(ref BagWheelButtonController bagButton, string name)
        {
            Image imageButton = bagButton.gameObject.GetComponentsInChildren<Image>().FirstOrDefault(b => b.gameObject.name.Equals("Icon"));
            if (imageButton == null)
            {
                BagWheel.mls.LogError($"Image not found for the {name} item");
                return;
            }

            bagButton.itemName = name;
            switch (bagButton.itemName)
            {
                case Constants.FLASHLIGHT:
                    bagButton.itemType = typeof(FlashlightItem);
                    imageButton.sprite = BagWheel.flashlightSprite;
                    break;
                case Constants.SHOVEL:
                    bagButton.itemType = typeof(Shovel);
                    imageButton.sprite = BagWheel.shovelSprite;
                    break;
                case Constants.SPRAY_PAINT:
                    bagButton.itemType = typeof(SprayPaintItem);
                    imageButton.sprite = BagWheel.sprayPaintSprite;
                    break;
                case Constants.WALKIE_TALKIE:
                    bagButton.itemType = typeof(WalkieTalkie);
                    imageButton.sprite = BagWheel.walkieTalkieSprite;
                    break;
                case Constants.STUN_GRENADE:
                    bagButton.itemType = typeof(StunGrenadeItem);
                    imageButton.sprite = BagWheel.stunGrenadeSprite;
                    break;
                case Constants.BOOMBOX:
                    bagButton.itemType = typeof(BoomboxItem);
                    imageButton.sprite = BagWheel.boomboxSprite;
                    break;
                case Constants.ZAP_GUN:
                    bagButton.itemType = typeof(PatcherTool);
                    imageButton.sprite = BagWheel.zapGunSprite;
                    break;
                case Constants.TZP:
                    bagButton.itemType = typeof(TetraChemicalItem);
                    imageButton.sprite = BagWheel.tzpSprite;
                    break;
                case Constants.LOCKPICKER:
                    bagButton.itemType = typeof(LockPicker);
                    imageButton.sprite = BagWheel.lockpickerSprite;
                    break;
                case Constants.JETPACK:
                    bagButton.itemType = typeof(JetpackItem);
                    imageButton.sprite = BagWheel.jetpackSprite;
                    break;
                case Constants.EXTENSION_LADDER:
                    bagButton.itemType = typeof(ExtensionLadderItem);
                    imageButton.sprite = BagWheel.extensionLadderSprite;
                    break;
                case Constants.RADAR_BOOSTER:
                    bagButton.itemType = typeof(RadarBoosterItem);
                    imageButton.sprite = BagWheel.radarBoosterSprite;
                    break;
            }
        }
    }
}
