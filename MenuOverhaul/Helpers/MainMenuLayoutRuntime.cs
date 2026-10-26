using EFT.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Infrastructure.Reflection;
using MenuOverhaul.Utils;
using System.Reflection;

namespace MenuOverhaul.Helpers
{
    public static class MainMenuLayoutRuntime
    {
        private static readonly Dictionary<string, string> ButtonNameToFileNameMap = new Dictionary<string, string>
        {
            { MenuOverhaulConstants.MenuButtons.PlayButton, "icon_play" },
            { MenuOverhaulConstants.MenuButtons.CharacterButton, "icon_mainmenu_character" },
            { MenuOverhaulConstants.MenuButtons.TradeButton, "icon_trade" },
            { MenuOverhaulConstants.MenuButtons.HideoutButton, "hideout_icon_black" },
            { MenuOverhaulConstants.MenuButtons.ExitButton, "exit_status_runner" },
            { MenuOverhaulConstants.MenuButtons.ExitButtonGroup, "exit_status_runner" }
        };

        private static readonly string PluginResourcesRoot = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx", "plugins", "MenuOverhaul", "Resources");
        private static readonly string IconsDirectory = System.IO.Path.Combine(PluginResourcesRoot, "icons");

        public class EnvironmentObjects
        {
            public GameObject EnvironmentUI { get; set; }
            public GameObject CommonObj { get; set; }
        }

        public static EnvironmentObjects FindEnvironmentObjects()
        {
            GameObject environmentUI = GameObject.Find(MenuOverhaulConstants.Environment.EnvironmentUI);
            if (environmentUI == null) return null;

            Transform commonTransform = environmentUI.transform.Find(MenuOverhaulConstants.Environment.Common);
            GameObject commonObj = commonTransform != null ? commonTransform.gameObject : null;
            
            if (commonObj == null) return null;

            return new EnvironmentObjects
            {
                EnvironmentUI = environmentUI,
                CommonObj = commonObj
            };
        }

        public static void HideGameObject(MenuScreen instance, string fieldName)
        {
            if (instance == null || string.IsNullOrEmpty(fieldName)) return;

            if (!EftReflectionMap.IsInitialized)
            {
                EftReflectionMap.Initialize();
            }

            if (!EftReflectionMap.TryGetMenuScreenField(fieldName, out var member, out var fieldKey))
            {
                return;
            }

            EftReflectionMap.WarnIfMissing(member, fieldKey);
            if (member != null)
            {
                var gameObject = EftReflectionMap.GetValue(member, instance) as GameObject;
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public static void SetChildActive(GameObject parent, string childName, bool isActive)
        {
            if (parent == null || string.IsNullOrEmpty(childName)) return;
            
            Transform childTransform = parent.transform.Find(childName);
            if (childTransform != null)
            {
                childTransform.gameObject.SetActive(isActive);
            }
        }

        public static void SetIconImages(GameObject buttonObject, string buttonName)
        {
            if (buttonObject == null) return;

            if (!ButtonNameToFileNameMap.TryGetValue(buttonName, out string fileName)) return;

            Sprite newIconSprite = UIAssetLoader.LoadSpriteFromDirectory(IconsDirectory, fileName);
            if (newIconSprite == null) return;

            SetImageComponentSprite(buttonObject, newIconSprite);
            Transform iconTransform = buttonObject.transform.Find(MenuOverhaulConstants.MenuButtons.IconPath);
            if (iconTransform != null)
            {
                SetImageComponentSprite(iconTransform.gameObject, newIconSprite);
            }
        }

        private static void SetImageComponentSprite(GameObject obj, Sprite sprite)
        {
            if (obj == null) return;
            Image image = obj.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = sprite;
                image.overrideSprite = sprite;
                image.type = Image.Type.Simple;
                image.preserveAspect = true;
            }
        }

        public static bool IsPartOfMenuScreen(DefaultUIButtonAnimation buttonAnimation)
        {
            if (buttonAnimation == null) return false;
            Transform currentTransform = buttonAnimation.transform;
            while (currentTransform != null)
            {
                if (currentTransform.name == MenuOverhaulConstants.MenuScreen.Name) return true;
                currentTransform = currentTransform.parent;
            }
            return false;
        }

        public static void DisposeResources()
        {
            UIAssetLoader.ClearCaches();
        }
    }
}