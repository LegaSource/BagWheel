using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using BepInEx.Logging;
using BagWheel.CustomInputs;
using LethalLib.Modules;
using BagWheel.Managers;
using BagWheel.Patches;
using System.IO;
using BepInEx.Configuration;
using System;
using BagWheel.Patches.ModsPatches;

namespace BagWheel
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class BagWheel : BaseUnityPlugin
    {
        private const string modGUID = "Lega.BagWheel";
        private const string modName = "Bag Wheel";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        private readonly static AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "bagwheel"));
        internal static ManualLogSource mls;
        public static ConfigFile configFile;

        // UI
        public static GameObject bagWheelInterface;
        public static GameObject bagWheelPrefab;
        // Sprites
        public static Sprite flashlightSprite;
        public static Sprite shovelSprite;
        public static Sprite sprayPaintSprite;
        public static Sprite walkieTalkieSprite;
        public static Sprite stunGrenadeSprite;
        public static Sprite boomboxSprite;
        public static Sprite zapGunSprite;
        public static Sprite tzpSprite;
        public static Sprite lockpickerSprite;
        public static Sprite jetpackSprite;
        public static Sprite extensionLadderSprite;
        public static Sprite radarBoosterSprite;

        public static GameObject managerPrefab = NetworkPrefabs.CreateNetworkPrefab("BagWheelNetworkManager");

        public void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource("BagWheel");
            configFile = Config;
            ConfigManager.Load();

            _ = BagWheelInputs.Instance;
            BagWheelInputs.Instance.EnableInputs();

            LoadManager();
            NetcodePatcher();
            LoadUI();
            LoadSprites();

            harmony.PatchAll(typeof(HUDManagerPatch));
            harmony.PatchAll(typeof(StartOfRoundPatch));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            PatchOtherMods(harmony);
        }

        public static void LoadManager()
        {
            Utilities.FixMixerGroups(managerPrefab);
            managerPrefab.AddComponent<BagWheelNetworkManager>();
        }

        private static void NetcodePatcher()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length == 0) continue;
                    method.Invoke(null, null);
                }
            }
        }

        public static void LoadUI()
            => bagWheelPrefab = bundle.LoadAsset<GameObject>("Assets/UI/BagWheel.prefab");

        public static void LoadSprites()
        {
            flashlightSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/FlashlightIcon.png");
            shovelSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/ShovelIcon.png");
            sprayPaintSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/SpraycanIcon.png");
            walkieTalkieSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/WalkieTalkieIcon.png");
            stunGrenadeSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/StunGrenadeIcon.png");
            boomboxSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/BoomboxIcon.png");
            zapGunSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/ZapGunIcon.png");
            tzpSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/TZPIcon.png");
            lockpickerSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/LockpickerIcon.png");
            jetpackSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/JetpackIcon.png");
            extensionLadderSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/ExtensionLadderIcon.png");
            radarBoosterSprite = bundle.LoadAsset<Sprite>("Assets/Sprites/Icons/RadarBoosterIcon.png");
        }

        public static void PatchOtherMods(Harmony harmony)
            => BetterSprayPaintPatch(harmony);

        public static void BetterSprayPaintPatch(Harmony harmony)
        {
            Type sprayPaintItemNetExtClass = Type.GetType("BetterSprayPaint.Ngo.SprayPaintItemNetExt, BetterSprayPaint");
            if (sprayPaintItemNetExtClass == null) return;

            var heldByLocalPlayerGetter = AccessTools.Property(sprayPaintItemNetExtClass, "HeldByLocalPlayer")?.GetGetMethod();
            if (heldByLocalPlayerGetter != null)
            {
                harmony.Patch(
                    heldByLocalPlayerGetter,
                    prefix: new HarmonyMethod(typeof(BetterSprayPaintPatch).GetMethod("HeldByLocalPlayer"))
                );
            }

            var inActiveSlotGetter = AccessTools.Property(sprayPaintItemNetExtClass, "InActiveSlot")?.GetGetMethod();
            if (inActiveSlotGetter != null)
            {
                harmony.Patch(
                    inActiveSlotGetter,
                    prefix: new HarmonyMethod(typeof(BetterSprayPaintPatch).GetMethod("InActiveSlot"))
                );
            }
        }
    }
}
