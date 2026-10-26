using EFT.UI;
using System.Reflection;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MenuOverhaul.Infrastructure.Reflection;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;
using MenuOverhaul.Helpers.Services;

namespace MenuOverhaul.Helpers
{
    internal static class MainMenuButtonRuntime
    {
        private static readonly string[] ButtonNames =
        [
            MenuOverhaulConstants.MenuButtons.PlayButton,
            MenuOverhaulConstants.MenuButtons.CharacterButton,
            MenuOverhaulConstants.MenuButtons.TradeButton,
            MenuOverhaulConstants.MenuButtons.HideoutButton,
            MenuOverhaulConstants.MenuButtons.ExitButtonGroup
        ];

        public static void SetupButtonIcons(MenuScreen menuScreenInstance)
        {
            if (menuScreenInstance == null) return;
            
            float iconScale = Settings.ButtonIconScale.Value;
            float iconPosX = Settings.ButtonIconPositionX.Value;
            float iconPosY = Settings.ButtonIconPositionY.Value;

            SetButtonIconTransform(menuScreenInstance, MenuOverhaulConstants.MenuButtons.PlayButton, new Vector3(iconScale, iconScale, iconScale), new Vector3(-48f + iconPosX, iconPosY, 0f));
            SetButtonIconTransform(menuScreenInstance, MenuOverhaulConstants.MenuButtons.CharacterButton, new Vector3(iconScale, iconScale, iconScale), new Vector3(iconPosX, iconPosY, 0f));
            SetButtonIconTransform(menuScreenInstance, MenuOverhaulConstants.MenuButtons.TradeButton, new Vector3(iconScale, iconScale, iconScale), new Vector3(iconPosX, iconPosY, 0f));
            SetButtonIconTransform(menuScreenInstance, MenuOverhaulConstants.MenuButtons.HideoutButton, new Vector3(iconScale, iconScale, iconScale), new Vector3(iconPosX, iconPosY, 0f));
            SetButtonIconTransform(menuScreenInstance, MenuOverhaulConstants.MenuButtons.ExitButtonGroup, new Vector3(iconScale, iconScale, iconScale), new Vector3(iconPosX, iconPosY, 0f));
        }

        public static void RefreshButtonIdleState(MenuScreen menuScreenInstance)
        {
            if (menuScreenInstance == null) return;
            if (!EftReflectionMap.IsInitialized) EftReflectionMap.Initialize();
            var animations = menuScreenInstance.gameObject.GetComponentsInChildren<DefaultUIButtonAnimation>(true);
            if (animations == null || animations.Length == 0) return;
            var method = EftReflectionMap.DefaultButtonIdleMethod;
            if (method == null) return;
            object[] args = new object[] { false };
            foreach (var anim in animations)
            {
                if (anim == null) continue;
                try { method.Invoke(anim, args); } catch { }
            }
            UpdateMenuButtonIconVisibility(menuScreenInstance);
        }

        public static void RefreshButtonsState()
        {
            GameObject menuScreenObject = GameObject.Find(MenuOverhaulConstants.MenuScreen.ScenePath);
            if (menuScreenObject == null) return;

            foreach (var animation in menuScreenObject.GetComponentsInChildren<DefaultUIButtonAnimation>(true))
            {
                if (animation != null)
                {
                    MainMenuButtonAnimationService.ApplyIdle(animation, false);
                }
            }
        }

        public static void UpdateMenuButtonGroupPositions(MenuScreen menuScreenInstance = null)
        {
            GameObject menuScreenObject = menuScreenInstance != null ? menuScreenInstance.gameObject : GameObject.Find(MenuOverhaulConstants.MenuScreen.ScenePath);
            if (menuScreenObject == null) return;

            foreach (var buttonName in ButtonNames)
            {
                Transform buttonTransform = menuScreenObject.transform.Find(buttonName);
                GameObject buttonObject = buttonTransform != null ? buttonTransform.gameObject : null;
                if (buttonObject != null) ApplyButtonTransform(buttonObject, buttonName);
            }
        }

        public static void UpdateMenuButtonIconVisibility(MenuScreen menuScreenInstance = null)
        {
            bool showIcons = Settings.EnableMenuButtonIcons.Value;
            GameObject menuScreenObject = menuScreenInstance != null ? menuScreenInstance.gameObject : GameObject.Find(MenuOverhaulConstants.MenuScreen.ScenePath);
            if (menuScreenObject == null) return;

            foreach (var animation in menuScreenObject.GetComponentsInChildren<DefaultUIButtonAnimation>(true))
            {
                if (animation != null && animation.Icon != null) animation.Icon.gameObject.SetActive(showIcons);
            }

            foreach (string buttonName in ButtonNames)
            {
                Transform buttonTransform = menuScreenObject.transform.Find(buttonName);
                GameObject buttonObject = buttonTransform != null ? buttonTransform.gameObject : null;
                if (buttonObject == null) continue;

                Transform sizeLabelTransform = buttonObject.transform.Find(MenuOverhaulConstants.MenuButtons.SizeLabel);
                if (buttonName == MenuOverhaulConstants.MenuButtons.ExitButtonGroup)
                    sizeLabelTransform = buttonObject.transform.Find(MenuOverhaulConstants.MenuButtons.ExitButtonSizeLabelPath);

                if (sizeLabelTransform == null) continue;

                Transform iconContainerTransform = sizeLabelTransform.Find(MenuOverhaulConstants.MenuButtons.IconContainer);
                if (iconContainerTransform != null) iconContainerTransform.gameObject.SetActive(showIcons);
            }
        }

        public static void ProcessButtons(MenuScreen menuScreenInstance)
        {
            if (menuScreenInstance == null) return;

            Canvas.ForceUpdateCanvases();

            foreach (var buttonName in ButtonNames)
            {
                Transform buttonTransform = menuScreenInstance.gameObject.transform.Find(buttonName);
                GameObject buttonObject = buttonTransform != null ? buttonTransform.gameObject : null;
                if (buttonObject != null)
                {
                    ApplyButtonTransform(buttonObject, buttonName);
                    MainMenuLayoutRuntime.SetIconImages(buttonObject, buttonName);
                    
                    switch (buttonName)
                    {
                        case MenuOverhaulConstants.MenuButtons.PlayButton:
                            HandlePlayButtonLogic(menuScreenInstance, buttonObject);
                            break;
                        case MenuOverhaulConstants.MenuButtons.ExitButtonGroup:
                            HandleExitButtonGroupLogic(buttonObject);
                            break;
                        default:
                            HideButtonBackgroundAndActivateIcon(buttonObject, buttonName);
                            break;
                    }
                }
            }
            Canvas.ForceUpdateCanvases();
        }

        private static void ApplyButtonTransform(GameObject buttonObject, string buttonName)
        {
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            if (rectTransform == null) return;

            Transform current = rectTransform;
            while (current != null)
            {
                var layoutGroup = current.GetComponent<LayoutGroup>();
                if (layoutGroup != null) layoutGroup.enabled = false;
                
                var csf = current.GetComponent<ContentSizeFitter>();
                if (csf != null) csf.enabled = false;
                
                current = current.parent;
            }

            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.pivot = new Vector2(0, 0.5f);

            int index = Array.IndexOf(ButtonNames, buttonName);
            if (index < 0) return;

            float yOffset = -index * Settings.ButtonVerticalSpacing.Value;
            float xOffset = GetButtonXOffset(buttonName);
            
            rectTransform.anchoredPosition = new Vector2(xOffset, yOffset);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        private static float GetButtonXOffset(string buttonName)
        {
            return buttonName switch
            {
                MenuOverhaulConstants.MenuButtons.PlayButton => Settings.PositionPlayButtonHorizontal.Value,
                MenuOverhaulConstants.MenuButtons.CharacterButton => Settings.PositionCharacterButtonHorizontal.Value,
                MenuOverhaulConstants.MenuButtons.TradeButton => Settings.PositionTradeButtonHorizontal.Value,
                MenuOverhaulConstants.MenuButtons.HideoutButton => Settings.PositionHideoutButtonHorizontal.Value,
                MenuOverhaulConstants.MenuButtons.ExitButtonGroup => Settings.PositionExitButtonHorizontal.Value,
                _ => 250f
            };
        }

        private static void HandlePlayButtonLogic(MenuScreen menuScreenInstance, GameObject buttonObject)
        {
            if (!EftReflectionMap.IsInitialized) EftReflectionMap.Initialize();

            MemberInfo playButtonField = EftReflectionMap.MenuScreenPlayButtonField;
            if (playButtonField != null) ModifyButtonTextComponent(playButtonField, menuScreenInstance);

            MainMenuLayoutRuntime.SetChildActive(buttonObject, MenuOverhaulConstants.MenuButtons.Background, false);
            Transform sizeLabelTransform = buttonObject.transform.Find(MenuOverhaulConstants.MenuButtons.SizeLabel);
            GameObject sizeLabel = sizeLabelTransform != null ? sizeLabelTransform.gameObject : null;
            if (sizeLabel != null)
                MainMenuLayoutRuntime.SetChildActive(sizeLabel, MenuOverhaulConstants.MenuButtons.IconContainer, Settings.EnableMenuButtonIcons.Value);
        }

        private static void HandleExitButtonGroupLogic(GameObject buttonGroupObject)
        {
            Transform exitButtonTransform = buttonGroupObject.transform.Find(MenuOverhaulConstants.MenuButtons.ExitButton);
            GameObject exitButton = exitButtonTransform != null ? exitButtonTransform.gameObject : null;
            if (exitButton != null)
            {
                MainMenuLayoutRuntime.SetChildActive(exitButton, MenuOverhaulConstants.MenuButtons.Background, false);
                Transform sizeLabelTransform = exitButton.transform.Find(MenuOverhaulConstants.MenuButtons.SizeLabel);
                GameObject sizeLabel = sizeLabelTransform != null ? sizeLabelTransform.gameObject : null;
                if (sizeLabel != null)
                {
                    Transform iconContainerTransform = sizeLabel.transform.Find(MenuOverhaulConstants.MenuButtons.IconContainer);
                    GameObject iconContainer = iconContainerTransform != null ? iconContainerTransform.gameObject : null;
                    if (iconContainer != null)
                        MainMenuLayoutRuntime.SetChildActive(iconContainer, MenuOverhaulConstants.MenuButtons.Icon, Settings.EnableMenuButtonIcons.Value);
                }
                MainMenuLayoutRuntime.SetIconImages(exitButton, MenuOverhaulConstants.MenuButtons.ExitButton);
            }
        }

        private static void HideButtonBackgroundAndActivateIcon(GameObject buttonObject, string buttonName)
        {
            MainMenuLayoutRuntime.SetChildActive(buttonObject, MenuOverhaulConstants.MenuButtons.Background, false);
            Transform sizeLabelTransform = buttonObject.transform.Find(MenuOverhaulConstants.MenuButtons.SizeLabel);
            GameObject sizeLabel = sizeLabelTransform != null ? sizeLabelTransform.gameObject : null;
            if (sizeLabel != null)
            {
                Transform iconContainerTransform = sizeLabel.transform.Find(MenuOverhaulConstants.MenuButtons.IconContainer);
                GameObject iconContainer = iconContainerTransform != null ? iconContainerTransform.gameObject : null;
                if (iconContainer != null)
                    MainMenuLayoutRuntime.SetChildActive(iconContainer, MenuOverhaulConstants.MenuButtons.Icon, Settings.EnableMenuButtonIcons.Value);
            }
        }

        private static void SetButtonIconTransform(MenuScreen menuScreenInstance, string buttonName, Vector3? localScale = null, Vector3? anchoredPosition = null)
        {
            if (menuScreenInstance == null) return;
            Transform buttonTransform = menuScreenInstance.gameObject.transform.Find(buttonName);
            GameObject button = buttonTransform != null ? buttonTransform.gameObject : null;
            if (button == null) return;

            Transform iconOwnerTransform = button.transform;
            if (buttonName == MenuOverhaulConstants.MenuButtons.ExitButtonGroup)
            {
                Transform exitButtonTransform = button.transform.Find(MenuOverhaulConstants.MenuButtons.ExitButton);
                if (exitButtonTransform == null) return;
                iconOwnerTransform = exitButtonTransform;
            }

            DefaultUIButtonAnimation anim = iconOwnerTransform.GetComponent<DefaultUIButtonAnimation>();
            Transform iconTransform = (anim != null && anim.Icon != null) ? anim.Icon.transform : iconOwnerTransform.Find(MenuOverhaulConstants.MenuButtons.IconPath);

            if (iconTransform == null) return;

            if (localScale.HasValue) iconTransform.localScale = localScale.Value;

            if (anchoredPosition.HasValue)
            {
                RectTransform iconRectTransform = iconTransform.GetComponent<RectTransform>();
                if (iconRectTransform != null) iconRectTransform.anchoredPosition = anchoredPosition.Value;
            }
        }

        private static void ModifyButtonTextComponent(MemberInfo memberInfo, MenuScreen screenInstance)
        {
            if (memberInfo == null || screenInstance == null) return;
            try
            {
                DefaultUIButton button = (DefaultUIButton)EftReflectionMap.GetValue(memberInfo, screenInstance);
                if (button != null)
                {
                    var textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.fontSize = Settings.ButtonTextScale.Value;
                        textComponent.alignment = TextAlignmentOptions.Left;
                        
                        float textPosX = Settings.ButtonTextPositionX.Value;
                        float textPosY = Settings.ButtonTextPositionY.Value;
                        
                        if (textComponent.fontSize == 36f)
                        {
                            textComponent.margin = new Vector4(-30f + textPosX, textPosY, 0f, 0f);
                        }
                        else
                        {
                            textComponent.margin = new Vector4(textPosX, textPosY, 0f, 0f);
                        }
                    }
                }
            }
            catch (Exception ex) { MenuDiagnosticsLogger.Error(LogSubsystem.Buttons, $"Error modifying button text: {ex.Message}"); }
        }
    }
}