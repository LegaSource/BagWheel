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
                $"1:{Constants.FLASHLIGHT}:{Constants.FLASHLIGHT}," +
                    $"2:{Constants.SHOVEL}:{Constants.SHOVEL}," +
                    $"3:{Constants.SPRAY_PAINT}:{Constants.SPRAY_PAINT}," +
                    $"4:{Constants.WALKIE_TALKIE}:{Constants.WALKIE_TALKIE}," +
                    $"5:{Constants.STUN_GRENADE}:{Constants.STUN_GRENADE}," +
                    $"6:{Constants.BOOMBOX}:{Constants.BOOMBOX}," +
                    $"7:{Constants.ZAP_GUN}:{Constants.ZAP_GUN}," +
                    $"8:{Constants.TZP}:{Constants.TZP}",
                $"Bag wheel configuration.\nThe format is Slot:ItemName:ImageName, accepted image names: {Constants.FLASHLIGHT}, {Constants.SHOVEL}, {Constants.SPRAY_PAINT}, {Constants.WALKIE_TALKIE}, {Constants.STUN_GRENADE}, {Constants.BOOMBOX}, {Constants.ZAP_GUN}, {Constants.TZP}, {Constants.LOCKPICKER}, {Constants.JETPACK}, {Constants.EXTENSION_LADDER}, {Constants.RADAR_BOOSTER}, {Constants.WEED_KILLER}." +
                $"\nTo deactivate a slot put {Constants.DISABLED} in place of the name.");
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
                if (values.Length == 3)
                {
                    BagWheelButtonController bagButton = BagWheel.bagWheelInterface.GetComponentsInChildren<BagWheelButtonController>().FirstOrDefault(b => b.gameObject.name.Equals($"BagWheelButton{values[0]}"));
                    if (bagButton == null)
                    {
                        BagWheel.mls.LogError($"Script not found for the {item} config");
                        return;
                    }
                    ConfigureWheelButton(ref bagButton, values[1], values[2]);
                }
            }
        }

        private static void ConfigureWheelButton(ref BagWheelButtonController bagButton, string name, string spriteName)
        {
            Image imageButton = bagButton.gameObject.GetComponentsInChildren<Image>().FirstOrDefault(b => b.gameObject.name.Equals("Icon"));
            if (imageButton == null)
            {
                BagWheel.mls.LogError($"Image not found for the {name} item");
                return;
            }

            if (Constants.DISABLED.Equals(name))
            {
                bagButton.gameObject.GetComponent<Button>().interactable = false;
                return;
            }

            bagButton.itemName = name;
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
                _ => BagWheel.shovelSprite
            };
        }
    }
}
