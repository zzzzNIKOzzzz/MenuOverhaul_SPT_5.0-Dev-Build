using EFT;
using EFT.UI;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Infrastructure.Lifecycle;
using MenuOverhaul.Utils;
using SPTushonka.Reflection.Utils;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MenuOverhaul.Helpers;

namespace MenuOverhaul.Helpers.Services
{
    internal static class MainMenuLayoutService
    {
        public static void ApplyMainMenuLayout(MenuScreen menuScreen)
        {
            MenuLifecycleCoordinator.EnsureScreenSubscription();
            MenuLifecycleCoordinator.EnsureLayoutSettingsSubscription();
            MenuScreenVisibilityPolicy.MarkMainMenuActive();

            HideMenuWarnings(menuScreen);
            AdjustBattlePassPosition(); 
            UpdateFactionLogo(); 
        }

        public static void SubscribeToLayoutSettingsChanges()
        {
            Settings.PositionPlayButtonHorizontal.SettingChanged += OnButtonGroupPositionChanged;
            Settings.PositionCharacterButtonHorizontal.SettingChanged += OnButtonGroupPositionChanged;
            Settings.PositionTradeButtonHorizontal.SettingChanged += OnButtonGroupPositionChanged;
            Settings.PositionHideoutButtonHorizontal.SettingChanged += OnButtonGroupPositionChanged;
            Settings.PositionExitButtonHorizontal.SettingChanged += OnButtonGroupPositionChanged;
            Settings.EnableMenuButtonIcons.SettingChanged += OnMenuIconVisibilityChanged;
            
            Settings.PositionBattlePassHorizontal.SettingChanged += OnBattlePassPositionChanged;
            Settings.PositionBattlePassVertical.SettingChanged += OnBattlePassPositionChanged;

            Settings.ButtonVerticalSpacing.SettingChanged += OnButtonGroupPositionChanged;
            Settings.ButtonIconScale.SettingChanged += OnButtonGroupPositionChanged;
            Settings.ButtonIconPositionX.SettingChanged += OnButtonGroupPositionChanged;
            Settings.ButtonIconPositionY.SettingChanged += OnButtonGroupPositionChanged;
            Settings.ButtonTextScale.SettingChanged += OnButtonGroupPositionChanged;
            Settings.ButtonTextPositionX.SettingChanged += OnButtonGroupPositionChanged;
            Settings.ButtonTextPositionY.SettingChanged += OnButtonGroupPositionChanged;
        }

        public static void UnsubscribeFromLayoutSettingsChanges()
        {
            Settings.PositionPlayButtonHorizontal.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.PositionCharacterButtonHorizontal.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.PositionTradeButtonHorizontal.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.PositionHideoutButtonHorizontal.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.PositionExitButtonHorizontal.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.EnableMenuButtonIcons.SettingChanged -= OnMenuIconVisibilityChanged;
            
            Settings.PositionBattlePassHorizontal.SettingChanged -= OnBattlePassPositionChanged;
            Settings.PositionBattlePassVertical.SettingChanged -= OnBattlePassPositionChanged;

            Settings.ButtonVerticalSpacing.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.ButtonIconScale.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.ButtonIconPositionX.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.ButtonIconPositionY.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.ButtonTextScale.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.ButtonTextPositionX.SettingChanged -= OnButtonGroupPositionChanged;
            Settings.ButtonTextPositionY.SettingChanged -= OnButtonGroupPositionChanged;
        }

        private static void HideMenuWarnings(MenuScreen menuScreen)
        {
            if (menuScreen == null) return;
            MainMenuLayoutRuntime.HideGameObject(menuScreen, MenuOverhaulConstants.MenuScreen.WarningField);
        }

        private static void AdjustBattlePassPosition()
        {
            GameObject menuScreen = GameObject.Find(MenuOverhaulConstants.MenuScreen.ScenePath);
            if (menuScreen == null) return;

            RectTransform menuRect = menuScreen.GetComponent<RectTransform>();
            float screenWidth = menuRect != null ? menuRect.rect.width : Screen.width;
            float screenHeight = menuRect != null ? menuRect.rect.height : Screen.height;

            var transforms = menuScreen.GetComponentsInChildren<RectTransform>(true);
            foreach (var t in transforms)
            {
                if (t.name.ToLower().Contains("kord") || t.name.ToLower().Contains("breach") || t.name.ToLower().Contains("season"))
                {
                    Transform parent = t.parent;
                    while (parent != null)
                    {
                        var layoutGroup = parent.GetComponent<LayoutGroup>();
                        if (layoutGroup != null) layoutGroup.enabled = false;
                        var csf = parent.GetComponent<ContentSizeFitter>();
                        if (csf != null) csf.enabled = false;
                        parent = parent.parent;
                    }

                    t.anchorMin = new Vector2(0f, 1f);
                    t.anchorMax = new Vector2(0f, 1f);
                    t.pivot = new Vector2(0.5f, 0.5f);
                    
                    float xPos = screenWidth * Settings.PositionBattlePassHorizontal.Value; 
                    float yPos = -screenHeight * Settings.PositionBattlePassVertical.Value; 
                    
                    t.anchoredPosition = new Vector2(xPos, yPos); 
                }
            }
        }

        public static bool UpdateFactionLogo()
        {
            var profile = PatchConstants.BackEndSession?.Profile;
            if (profile == null) return false;

            string sideString = profile.Side.ToString().ToLower();
            string iconName = sideString.Contains("bear") ? "bear_logo" : "usec_logo";
            
            string iconsDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BepInEx", "plugins", "MenuOverhaul", "Resources", "icons");
            Sprite newSprite = UIAssetLoader.LoadSpriteFromDirectory(iconsDir, iconName);
            
            if (newSprite == null)
            {
                string fallbackDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BepInEx", "plugins", "MenuOverhaul", "Resources");
                newSprite = UIAssetLoader.LoadSpriteFromDirectory(fallbackDir, iconName);
            }

            if (newSprite == null) return false;

            bool logoFound = false;
            GameObject envUI = GameObject.Find(MenuOverhaulConstants.Environment.EnvironmentUI);
            GameObject menuScreen = GameObject.Find(MenuOverhaulConstants.MenuScreen.ScenePath);
            
            var images = new System.Collections.Generic.List<Image>();
            if (envUI != null) images.AddRange(envUI.GetComponentsInChildren<Image>(true));
            if (menuScreen != null) images.AddRange(menuScreen.GetComponentsInChildren<Image>(true));

            foreach (var img in images)
            {
                if (img.GetComponentInParent<DefaultUIButtonAnimation>() != null) continue;

                bool isMatch = false;
                string objName = img.gameObject.name.ToLower();
                
                if (objName.Contains("sideicon") || objName.Contains("faction") || objName.Contains("usec") || objName.Contains("bear") || objName.Contains("logo") || objName.Contains("side"))
                    isMatch = true;

                if (!isMatch && img.sprite != null)
                {
                    string spriteName = img.sprite.name.ToLower();
                    if (spriteName.Contains("usec") || spriteName.Contains("bear") || spriteName.Contains("side_icon") || spriteName.Contains("faction"))
                        isMatch = true;
                }

                if (isMatch)
                {
                    img.sprite = newSprite;
                    img.overrideSprite = newSprite;
                    img.preserveAspect = true;
                    logoFound = true;
                }
            }

            return logoFound;
        }

        private static void OnBattlePassPositionChanged(object sender, System.EventArgs e) => AdjustBattlePassPosition();
        
        private static void OnMenuIconVisibilityChanged(object sender, System.EventArgs e) => MainMenuButtonsService.UpdateMenuIconVisibility();
        
        private static void OnButtonGroupPositionChanged(object sender, System.EventArgs e)
        {
            ButtonHoverEffects.ClearCache();

            GameObject menuScreenObj = GameObject.Find(MenuOverhaulConstants.MenuScreen.ScenePath);
            if (menuScreenObj != null)
            {
                MenuScreen menuScreen = menuScreenObj.GetComponent<MenuScreen>();
                if (menuScreen != null)
                {
                    MainMenuButtonsService.ApplyMainMenuButtons(menuScreen);
                    
                    Canvas.ForceUpdateCanvases();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(menuScreenObj.GetComponent<RectTransform>());
                }
            }
        }
    }
}