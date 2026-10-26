using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

namespace MenuOverhaul.Utils
{
    public class Settings
    {
        public static ConfigFile Config;

        public static ConfigEntry<bool> EnableExtraShadows;
        public static ConfigEntry<bool> EnableLargerPlayerModel;
        public static ConfigEntry<bool> EnableHighQualityPlayerPreview;
        public static ConfigEntry<bool> EnableDefaultPlayerAnimation;
        public static ConfigEntry<bool> EnableMenuButtonIcons;
        
        public static ConfigEntry<float> HoverLabelSlideDistance;
        public static ConfigEntry<float> HoverIconScaleMultiplier;
        public static ConfigEntry<float> HoverIndicatorSizeMultiplier;
        public static ConfigEntry<float> HoverIndicatorSpacing;
        public static ConfigEntry<string> HoverAnimationDurationPreset;
        
        public static ConfigEntry<bool> EnableDebugDiagnostics;
        public static ConfigEntry<bool> EnableVerboseLifecycleDiagnostics;
        
        public static ConfigEntry<float> PositionPlayButtonHorizontal;
        public static ConfigEntry<float> PositionCharacterButtonHorizontal;
        public static ConfigEntry<float> PositionTradeButtonHorizontal;
        public static ConfigEntry<float> PositionHideoutButtonHorizontal;
        public static ConfigEntry<float> PositionExitButtonHorizontal;
        
        public static ConfigEntry<float> CameraInventoryPositionX;
        public static ConfigEntry<float> CameraInventoryPositionY;
        public static ConfigEntry<float> CameraInventoryPositionZ;
        public static ConfigEntry<float> CameraInventoryRotationX;
        public static ConfigEntry<float> CameraInventoryRotationY;
        public static ConfigEntry<float> CameraInventoryRotationZ;
        
        public static ConfigEntry<float> PositionPlayerModelHorizontal;
        public static ConfigEntry<float> PositionBottomFieldHorizontal;
        public static ConfigEntry<float> PositionBottomFieldVertical;
        public static ConfigEntry<float> RotationPlayerModelHorizontal;
        
        public static ConfigEntry<float> PositionBattlePassHorizontal;
        public static ConfigEntry<float> PositionBattlePassVertical;

        public static ConfigEntry<float> ButtonVerticalSpacing;
        public static ConfigEntry<float> ButtonIconScale;
        public static ConfigEntry<float> ButtonIconPositionX;
        public static ConfigEntry<float> ButtonIconPositionY;
        public static ConfigEntry<float> ButtonTextScale;
        public static ConfigEntry<float> ButtonTextPositionX;
        public static ConfigEntry<float> ButtonTextPositionY;

        public static ConfigEntry<string> AccentColorHex;

        public static Color AccentColor
        {
            get
            {
                if (string.IsNullOrEmpty(AccentColorHex?.Value)) 
                    return new Color(1f, 0.75f, 0.3f, 1f);
                
                string[] parts = AccentColorHex.Value.Split(',');
                if (parts.Length >= 3)
                {
                    try
                    {
                        float r = float.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
                        float g = float.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
                        float b = float.Parse(parts[2].Trim(), CultureInfo.InvariantCulture);
                        float a = parts.Length > 3 ? float.Parse(parts[3].Trim(), CultureInfo.InvariantCulture) : 1f;
                        return new Color(r, g, b, a);
                    }
                    catch { }
                }
                return new Color(1f, 0.75f, 0.3f, 1f);
            }
        }

        public static List<ConfigEntryBase> ConfigEntries = new List<ConfigEntryBase>();

        public static void Init(ConfigFile config)
        {
            Localization.DetectLanguage();

            string secGeneral = Localization.T("Section_General");
            string secText = Localization.T("Section_TextAndButtons");
            string secCharacter = Localization.T("Section_Character");
            string secUI = Localization.T("Section_UI");

            // ========== 1. ОБЩИЕ ==========
            ConfigEntries.Add(EnableDebugDiagnostics = config.Bind(secGeneral, "Enable Debug Diagnostics", false, new ConfigDescription(Localization.T("Desc_EnableDebugDiagnostics"), null, new ConfigurationManagerAttributes { DispName = Localization.T("Name_EnableDebugDiagnostics"), IsAdvanced = true })));
            ConfigEntries.Add(EnableVerboseLifecycleDiagnostics = config.Bind(secGeneral, "Enable Verbose Lifecycle Diagnostics", false, new ConfigDescription(Localization.T("Desc_EnableVerboseLifecycleDiagnostics"), null, new ConfigurationManagerAttributes { DispName = Localization.T("Name_EnableVerboseLifecycleDiagnostics"), IsAdvanced = true })));

            // ========== 2. ТЕКСТ И КНОПКИ ==========
            ConfigEntries.Add(EnableMenuButtonIcons = config.Bind(secText, "Enable Menu Button Icons", true, new ConfigDescription(Localization.T("Desc_EnableMenuButtonIcons"), null, new ConfigurationManagerAttributes { DispName = Localization.T("Name_EnableMenuButtonIcons") })));
            
            ConfigEntries.Add(PositionPlayButtonHorizontal = config.Bind(secText, "Position Play Button Horizontal", 142f, new ConfigDescription(Localization.T("Desc_PositionPlayButtonHorizontal"), new AcceptableValueRange<float>(-800f, 1200f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionPlayButtonHorizontal") })));
            ConfigEntries.Add(PositionCharacterButtonHorizontal = config.Bind(secText, "Position Character Button Horizontal", 140f, new ConfigDescription(Localization.T("Desc_PositionCharacterButtonHorizontal"), new AcceptableValueRange<float>(-800f, 1200f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionCharacterButtonHorizontal") })));
            ConfigEntries.Add(PositionTradeButtonHorizontal = config.Bind(secText, "Position Trade Button Horizontal", 140f, new ConfigDescription(Localization.T("Desc_PositionTradeButtonHorizontal"), new AcceptableValueRange<float>(-800f, 1200f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionTradeButtonHorizontal") })));
            ConfigEntries.Add(PositionHideoutButtonHorizontal = config.Bind(secText, "Position Hideout Button Horizontal", 140f, new ConfigDescription(Localization.T("Desc_PositionHideoutButtonHorizontal"), new AcceptableValueRange<float>(-800f, 1200f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionHideoutButtonHorizontal") })));
            ConfigEntries.Add(PositionExitButtonHorizontal = config.Bind(secText, "Position Exit Button Horizontal", 140f, new ConfigDescription(Localization.T("Desc_PositionExitButtonHorizontal"), new AcceptableValueRange<float>(-800f, 1200f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionExitButtonHorizontal") })));
            
            ConfigEntries.Add(ButtonVerticalSpacing = config.Bind(secText, "Menu Button Vertical Spacing", 60f, new ConfigDescription(Localization.T("Desc_ButtonVerticalSpacing"), new AcceptableValueRange<float>(20f, 150f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_ButtonVerticalSpacing") })));
            ConfigEntries.Add(ButtonIconScale = config.Bind(secText, "Menu Button Icon Scale", 0.8f, new ConfigDescription(Localization.T("Desc_ButtonIconScale"), new AcceptableValueRange<float>(0.1f, 2f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_ButtonIconScale") })));
            ConfigEntries.Add(ButtonIconPositionX = config.Bind(secText, "Menu Button Icon Position X", -10f, new ConfigDescription(Localization.T("Desc_ButtonIconPositionX"), new AcceptableValueRange<float>(-200f, 100f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_ButtonIconPositionX") })));
            ConfigEntries.Add(ButtonIconPositionY = config.Bind(secText, "Menu Button Icon Position Y", 0f, new ConfigDescription(Localization.T("Desc_ButtonIconPositionY"), new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_ButtonIconPositionY") })));
            ConfigEntries.Add(ButtonTextScale = config.Bind(secText, "Menu Button Text Scale", 36f, new ConfigDescription(Localization.T("Desc_ButtonTextScale"), new AcceptableValueRange<float>(10f, 72f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_ButtonTextScale") })));
            ConfigEntries.Add(ButtonTextPositionX = config.Bind(secText, "Menu Button Text Position X", 0f, new ConfigDescription(Localization.T("Desc_ButtonTextPositionX"), new AcceptableValueRange<float>(-200f, 100f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_ButtonTextPositionX") })));
            ConfigEntries.Add(ButtonTextPositionY = config.Bind(secText, "Menu Button Text Position Y", 0f, new ConfigDescription(Localization.T("Desc_ButtonTextPositionY"), new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_ButtonTextPositionY") })));
            
            ConfigEntries.Add(HoverLabelSlideDistance = config.Bind(secText, "Hover Label Slide Distance", 10f, new ConfigDescription(Localization.T("Desc_HoverLabelSlideDistance"), new AcceptableValueRange<float>(0f, 40f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_HoverLabelSlideDistance"), IsAdvanced = true })));
            ConfigEntries.Add(HoverIconScaleMultiplier = config.Bind(secText, "Hover Icon Scale Multiplier", 1.4f, new ConfigDescription(Localization.T("Desc_HoverIconScaleMultiplier"), new AcceptableValueRange<float>(0.5f, 2f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_HoverIconScaleMultiplier"), IsAdvanced = true })));
            ConfigEntries.Add(HoverIndicatorSizeMultiplier = config.Bind(secText, "Hover Indicator Size Multiplier", 0.5f, new ConfigDescription(Localization.T("Desc_HoverIndicatorSizeMultiplier"), new AcceptableValueRange<float>(0.1f, 1.5f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_HoverIndicatorSizeMultiplier"), IsAdvanced = true })));
            ConfigEntries.Add(HoverIndicatorSpacing = config.Bind(secText, "Hover Indicator Spacing", 10f, new ConfigDescription(Localization.T("Desc_HoverIndicatorSpacing"), new AcceptableValueRange<float>(0f, 32f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_HoverIndicatorSpacing"), IsAdvanced = true })));
            ConfigEntries.Add(HoverAnimationDurationPreset = config.Bind(secText, "Hover Animation Duration Preset", "Normal", new ConfigDescription(Localization.T("Desc_HoverAnimationDurationPreset"), new AcceptableValueList<string>("Fast", "Normal", "Slow"), new ConfigurationManagerAttributes { DispName = Localization.T("Name_HoverAnimationDurationPreset"), IsAdvanced = true })));

            // ========== 3. ПЕРСОНАЖ И КАМЕРА ==========
            ConfigEntries.Add(EnableExtraShadows = config.Bind(secCharacter, "Enable Extra Shadows", true, new ConfigDescription(Localization.T("Desc_EnableExtraShadows"), null, new ConfigurationManagerAttributes { DispName = Localization.T("Name_EnableExtraShadows") })));
            ConfigEntries.Add(EnableLargerPlayerModel = config.Bind(secCharacter, "Enable Larger Player Model", false, new ConfigDescription(Localization.T("Desc_EnableLargerPlayerModel"), null, new ConfigurationManagerAttributes { DispName = Localization.T("Name_EnableLargerPlayerModel") })));
            ConfigEntries.Add(EnableHighQualityPlayerPreview = config.Bind(secCharacter, "Enable High Quality Player Preview", true, new ConfigDescription(Localization.T("Desc_EnableHighQualityPlayerPreview"), null, new ConfigurationManagerAttributes { DispName = Localization.T("Name_EnableHighQualityPlayerPreview") })));
            ConfigEntries.Add(EnableDefaultPlayerAnimation = config.Bind(secCharacter, "Enable Default Player Animation", false, new ConfigDescription(Localization.T("Desc_EnableDefaultPlayerAnimation"), null, new ConfigurationManagerAttributes { DispName = Localization.T("Name_EnableDefaultPlayerAnimation") })));
            
            ConfigEntries.Add(PositionPlayerModelHorizontal = config.Bind(secCharacter, "Position Player Model Horizontal", 400f, new ConfigDescription(Localization.T("Desc_PositionPlayerModelHorizontal"), new AcceptableValueRange<float>(-600f, 1800f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionPlayerModelHorizontal") })));
            ConfigEntries.Add(RotationPlayerModelHorizontal = config.Bind(secCharacter, "Rotate Player Model", 200f, new ConfigDescription(Localization.T("Desc_RotationPlayerModelHorizontal"), new AcceptableValueRange<float>(0f, 360f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_RotationPlayerModelHorizontal") })));
            ConfigEntries.Add(PositionBottomFieldHorizontal = config.Bind(secCharacter, "Position Player Info Horizontal", 460f, new ConfigDescription(Localization.T("Desc_PositionBottomFieldHorizontal"), new AcceptableValueRange<float>(-800f, 1200f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionBottomFieldHorizontal") })));
            ConfigEntries.Add(PositionBottomFieldVertical = config.Bind(secCharacter, "Position Player Info Vertical", -150f, new ConfigDescription(Localization.T("Desc_PositionBottomFieldVertical"), new AcceptableValueRange<float>(-300f, 300f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionBottomFieldVertical") })));
            
            ConfigEntries.Add(CameraInventoryPositionX = config.Bind(secCharacter, "Camera Player Position X", -0.2f, new ConfigDescription(Localization.T("Desc_CameraInventoryPosition"), new AcceptableValueRange<float>(-3f, 3f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_CameraInventoryPositionX"), IsAdvanced = true })));
            ConfigEntries.Add(CameraInventoryPositionY = config.Bind(secCharacter, "Camera Player Position Y", 0.15f, new ConfigDescription(Localization.T("Desc_CameraInventoryPosition"), new AcceptableValueRange<float>(-3f, 3f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_CameraInventoryPositionY"), IsAdvanced = true })));
            ConfigEntries.Add(CameraInventoryPositionZ = config.Bind(secCharacter, "Camera Player Position Z", 0.4f, new ConfigDescription(Localization.T("Desc_CameraInventoryPosition"), new AcceptableValueRange<float>(-3f, 3f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_CameraInventoryPositionZ"), IsAdvanced = true })));
            ConfigEntries.Add(CameraInventoryRotationX = config.Bind(secCharacter, "Camera Player Rotation X", 0f, new ConfigDescription(Localization.T("Desc_CameraInventoryRotation"), new AcceptableValueRange<float>(-180f, 180f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_CameraInventoryRotationX"), IsAdvanced = true })));
            ConfigEntries.Add(CameraInventoryRotationY = config.Bind(secCharacter, "Camera Player Rotation Y", 0f, new ConfigDescription(Localization.T("Desc_CameraInventoryRotation"), new AcceptableValueRange<float>(-180f, 180f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_CameraInventoryRotationY"), IsAdvanced = true })));
            ConfigEntries.Add(CameraInventoryRotationZ = config.Bind(secCharacter, "Camera Player Rotation Z", 0f, new ConfigDescription(Localization.T("Desc_CameraInventoryRotation"), new AcceptableValueRange<float>(-180f, 180f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_CameraInventoryRotationZ"), IsAdvanced = true })));

            // ========== 4. ИНТЕРФЕЙС И ЦВЕТА ==========
            ConfigEntries.Add(PositionBattlePassHorizontal = config.Bind(secUI, "Position Battle Pass Horizontal", 0.255f, new ConfigDescription(Localization.T("Desc_PositionBattlePassHorizontal"), new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionBattlePassHorizontal") })));
            ConfigEntries.Add(PositionBattlePassVertical = config.Bind(secUI, "Position Battle Pass Vertical", 0.410f, new ConfigDescription(Localization.T("Desc_PositionBattlePassVertical"), new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { DispName = Localization.T("Name_PositionBattlePassVertical") })));

            ConfigEntries.Add(AccentColorHex = config.Bind(
                secUI,
                "Accent Color (RGBA)",
                "1, 0.75, 0.3, 1",
                new ConfigDescription(
                    Localization.T("Desc_AccentColorHex"),
                    null,
                    new ConfigurationManagerAttributes { DispName = Localization.T("Name_AccentColorHex") })));

            RecalcOrder();
        }

        private static void RecalcOrder()
        {
            int settingOrder = ConfigEntries.Count;
            foreach (var entry in ConfigEntries)
            {
                ConfigurationManagerAttributes attributes = entry.Description.Tags[0] as ConfigurationManagerAttributes;
                if (attributes != null)
                {
                    attributes.Order = settingOrder;
                }
                settingOrder--;
            }
        }
    }
}