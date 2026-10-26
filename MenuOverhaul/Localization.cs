using System;
using System.Collections.Generic;
using UnityEngine;

namespace MenuOverhaul.Utils
{
    public static class Localization
    {
        private static string _currentLang = "En";

        private static readonly Dictionary<string, Dictionary<string, string>> _table =
            new Dictionary<string, Dictionary<string, string>>
            {
                ["En"] = new Dictionary<string, string>
                {
                    ["Section_General"] = "1. General",
                    ["Section_TextAndButtons"] = "2. Text & Buttons",
                    ["Section_Character"] = "3. Character & Camera",
                    ["Section_UI"] = "4. UI & Colors",

                    // General
                    ["Name_EnableDebugDiagnostics"] = "Enable Debug Diagnostics",
                    ["Desc_EnableDebugDiagnostics"] = "Enable debug-level diagnostics logs for subsystems.",
                    ["Name_EnableVerboseLifecycleDiagnostics"] = "Enable Verbose Lifecycle Logs",
                    ["Desc_EnableVerboseLifecycleDiagnostics"] = "Enable extra lifecycle coordinator debug traces.",

                    // Text & Buttons
                    ["Name_EnableMenuButtonIcons"] = "Enable Menu Button Icons",
                    ["Desc_EnableMenuButtonIcons"] = "Show or hide icons on the main menu buttons.",
                    ["Name_HoverLabelSlideDistance"] = "Hover Label Slide Distance",
                    ["Desc_HoverLabelSlideDistance"] = "Horizontal slide distance for menu label hover animation.",
                    ["Name_HoverIconScaleMultiplier"] = "Hover Icon Scale Multiplier",
                    ["Desc_HoverIconScaleMultiplier"] = "Scale multiplier applied to button icons while hovered.",
                    ["Name_HoverIndicatorSizeMultiplier"] = "Hover Indicator Size Multiplier",
                    ["Desc_HoverIndicatorSizeMultiplier"] = "Size multiplier for the hover indicator relative to icon size.",
                    ["Name_HoverIndicatorSpacing"] = "Hover Indicator Spacing",
                    ["Desc_HoverIndicatorSpacing"] = "Horizontal spacing between icon edge and hover indicator.",
                    ["Name_HoverAnimationDurationPreset"] = "Animation Duration Preset",
                    ["Desc_HoverAnimationDurationPreset"] = "Duration preset for hover/idle button animation timing.",
                    ["Name_PositionPlayButtonHorizontal"] = "Play Button Position",
                    ["Desc_PositionPlayButtonHorizontal"] = "Adjust the horizontal position of the Play button.",
                    ["Name_PositionCharacterButtonHorizontal"] = "Character Button Position",
                    ["Desc_PositionCharacterButtonHorizontal"] = "Adjust the horizontal position of the Character button.",
                    ["Name_PositionTradeButtonHorizontal"] = "Trade Button Position",
                    ["Desc_PositionTradeButtonHorizontal"] = "Adjust the horizontal position of the Trade button.",
                    ["Name_PositionHideoutButtonHorizontal"] = "Hideout Button Position",
                    ["Desc_PositionHideoutButtonHorizontal"] = "Adjust the horizontal position of the Hideout button.",
                    ["Name_PositionExitButtonHorizontal"] = "Exit Button Position",
                    ["Desc_PositionExitButtonHorizontal"] = "Adjust the horizontal position of the Exit button.",
                    ["Name_ButtonVerticalSpacing"] = "Menu Button Vertical Spacing",
                    ["Desc_ButtonVerticalSpacing"] = "Distance between menu buttons.",
                    ["Name_ButtonIconScale"] = "Menu Button Icon Scale",
                    ["Desc_ButtonIconScale"] = "Size of the icons on the menu buttons.",
                    ["Name_ButtonIconPositionX"] = "Menu Button Icon Position X",
                    ["Desc_ButtonIconPositionX"] = "Horizontal offset of the button icons.",
                    ["Name_ButtonIconPositionY"] = "Menu Button Icon Position Y",
                    ["Desc_ButtonIconPositionY"] = "Vertical offset of the button icons.",
                    ["Name_ButtonTextScale"] = "Menu Button Text Scale",
                    ["Desc_ButtonTextScale"] = "Font size of the button text.",
                    ["Name_ButtonTextPositionX"] = "Menu Button Text Position X",
                    ["Desc_ButtonTextPositionX"] = "Horizontal offset of the button text.",
                    ["Name_ButtonTextPositionY"] = "Menu Button Text Position Y",
                    ["Desc_ButtonTextPositionY"] = "Vertical offset of the button text.",

                    // Character & Camera
                    ["Name_EnableExtraShadows"] = "Enable Extra Shadows",
                    ["Desc_EnableExtraShadows"] = "Enable more shadows to make the player in the menu more detailed.",
                    ["Name_EnableLargerPlayerModel"] = "Enable Larger Player Model",
                    ["Desc_EnableLargerPlayerModel"] = "Make the player model larger and closer in the main menu.",
                    ["Name_EnableHighQualityPlayerPreview"] = "Enable High Quality Preview",
                    ["Desc_EnableHighQualityPlayerPreview"] = "Sharper player preview rendering (higher GPU cost).",
                    ["Name_EnableDefaultPlayerAnimation"] = "Enable Default Animation",
                    ["Desc_EnableDefaultPlayerAnimation"] = "Use EFT's default animated player preview behavior.",
                    ["Name_PositionPlayerModelHorizontal"] = "Player Model Position",
                    ["Desc_PositionPlayerModelHorizontal"] = "Adjust the horizontal position of the player model.",
                    ["Name_RotationPlayerModelHorizontal"] = "Player Model Rotation",
                    ["Desc_RotationPlayerModelHorizontal"] = "Adjust the horizontal rotation of the player model.",
                    ["Name_PositionBottomFieldHorizontal"] = "Player Info Position X",
                    ["Desc_PositionBottomFieldHorizontal"] = "Adjust the horizontal position of the player info text.",
                    ["Name_PositionBottomFieldVertical"] = "Player Info Position Y",
                    ["Desc_PositionBottomFieldVertical"] = "Adjust the vertical position of the player info text.",
                    ["Name_CameraInventoryPositionX"] = "Camera Position X",
                    ["Name_CameraInventoryPositionY"] = "Camera Position Y",
                    ["Name_CameraInventoryPositionZ"] = "Camera Position Z",
                    ["Desc_CameraInventoryPosition"] = "Offset Camera_inventory position for the player preview.",
                    ["Name_CameraInventoryRotationX"] = "Camera Rotation X",
                    ["Name_CameraInventoryRotationY"] = "Camera Rotation Y",
                    ["Name_CameraInventoryRotationZ"] = "Camera Rotation Z",
                    ["Desc_CameraInventoryRotation"] = "Set Camera_inventory rotation for the player preview.",

                    // UI & Colors
                    ["Name_PositionBattlePassHorizontal"] = "Battle Pass Position X",
                    ["Desc_PositionBattlePassHorizontal"] = "Position of the Battle Pass banner from the left edge (0.0 to 1.0).",
                    ["Name_PositionBattlePassVertical"] = "Battle Pass Position Y",
                    ["Desc_PositionBattlePassVertical"] = "Position of the Battle Pass banner from the top edge (0.0 to 1.0).",
                    ["Name_AccentColorHex"] = "Accent Color (RGBA)",
                    ["Desc_AccentColorHex"] = "The accent color used for texts and highlights. Format: R, G, B, A (0.0 to 1.0)."
                },
                ["Ru"] = new Dictionary<string, string>
                {
                    ["Section_General"] = "1. Общие",
                    ["Section_TextAndButtons"] = "2. Текст и кнопки",
                    ["Section_Character"] = "3. Персонаж и камера",
                    ["Section_UI"] = "4. Интерфейс и цвета",

                    // General
                    ["Name_EnableDebugDiagnostics"] = "Включить отладку",
                    ["Desc_EnableDebugDiagnostics"] = "Включает подробные логи для диагностики мода.",
                    ["Name_EnableVerboseLifecycleDiagnostics"] = "Подробные логи жизненного цикла",
                    ["Desc_EnableVerboseLifecycleDiagnostics"] = "Дополнительные отладочные сообщения координатора событий.",

                    // Text & Buttons
                    ["Name_EnableMenuButtonIcons"] = "Иконки кнопок меню",
                    ["Desc_EnableMenuButtonIcons"] = "Показывать или скрывать иконки на кнопках главного меню.",
                    ["Name_HoverLabelSlideDistance"] = "Сдвиг текста при наведении",
                    ["Desc_HoverLabelSlideDistance"] = "Горизонтальный сдвиг текста при наведении на кнопку.",
                    ["Name_HoverIconScaleMultiplier"] = "Увеличение иконки при наведении",
                    ["Desc_HoverIconScaleMultiplier"] = "Множитель размера иконки при наведении.",
                    ["Name_HoverIndicatorSizeMultiplier"] = "Размер индикатора наведения",
                    ["Desc_HoverIndicatorSizeMultiplier"] = "Множитель размера индикатора (стрелочки) относительно иконки.",
                    ["Name_HoverIndicatorSpacing"] = "Отступ индикатора наведения",
                    ["Desc_HoverIndicatorSpacing"] = "Расстояние между иконкой и индикатором наведения.",
                    ["Name_HoverAnimationDurationPreset"] = "Скорость анимации",
                    ["Desc_HoverAnimationDurationPreset"] = "Пресет длительности анимации наведения и покоя.",
                    ["Name_PositionPlayButtonHorizontal"] = "Позиция кнопки 'Побег'",
                    ["Desc_PositionPlayButtonHorizontal"] = "Горизонтальное смещение кнопки 'ПОБЕГ ИЗ ТАРКОВА'.",
                    ["Name_PositionCharacterButtonHorizontal"] = "Позиция кнопки 'Персонаж'",
                    ["Desc_PositionCharacterButtonHorizontal"] = "Горизонтальное смещение кнопки 'ПЕРСОНАЖ'.",
                    ["Name_PositionTradeButtonHorizontal"] = "Позиция кнопки 'Торговля'",
                    ["Desc_PositionTradeButtonHorizontal"] = "Горизонтальное смещение кнопки 'ТОРГОВЛЯ'.",
                    ["Name_PositionHideoutButtonHorizontal"] = "Позиция кнопки 'Убежище'",
                    ["Desc_PositionHideoutButtonHorizontal"] = "Горизонтальное смещение кнопки 'УБЕЖИЩЕ'.",
                    ["Name_PositionExitButtonHorizontal"] = "Позиция кнопки 'Выход'",
                    ["Desc_PositionExitButtonHorizontal"] = "Горизонтальное смещение кнопки 'ВЫХОД'.",
                    ["Name_ButtonVerticalSpacing"] = "Расстояние между кнопками",
                    ["Desc_ButtonVerticalSpacing"] = "Высота отступа между пунктами меню.",
                    ["Name_ButtonIconScale"] = "Размер иконок кнопок",
                    ["Desc_ButtonIconScale"] = "Масштаб иконок в главном меню.",
                    ["Name_ButtonIconPositionX"] = "Позиция иконок кнопок X",
                    ["Desc_ButtonIconPositionX"] = "Горизонтальный сдвиг иконок относительно текста.",
                    ["Name_ButtonIconPositionY"] = "Позиция иконок кнопок Y",
                    ["Desc_ButtonIconPositionY"] = "Вертикальный сдвиг иконок.",
                    ["Name_ButtonTextScale"] = "Размер текста кнопок",
                    ["Desc_ButtonTextScale"] = "Размер шрифта в главном меню.",
                    ["Name_ButtonTextPositionX"] = "Позиция текста кнопок X",
                    ["Desc_ButtonTextPositionX"] = "Горизонтальный сдвиг текста кнопок.",
                    ["Name_ButtonTextPositionY"] = "Позиция текста кнопок Y",
                    ["Desc_ButtonTextPositionY"] = "Вертикальный сдвиг текста кнопок.",

                    // Character & Camera
                    ["Name_EnableExtraShadows"] = "Дополнительные тени",
                    ["Desc_EnableExtraShadows"] = "Включает больше теней для детализации игрока в меню.",
                    ["Name_EnableLargerPlayerModel"] = "Увеличенная модель игрока",
                    ["Desc_EnableLargerPlayerModel"] = "Делает модель игрока больше и ближе в главном меню.",
                    ["Name_EnableHighQualityPlayerPreview"] = "Качественный рендер игрока",
                    ["Desc_EnableHighQualityPlayerPreview"] = "Более четкий рендер превью игрока (выше нагрузка на GPU).",
                    ["Name_EnableDefaultPlayerAnimation"] = "Стандартная анимация",
                    ["Desc_EnableDefaultPlayerAnimation"] = "Использовать стандартную анимацию превью игрока из EFT.",
                    ["Name_PositionPlayerModelHorizontal"] = "Позиция модели игрока",
                    ["Desc_PositionPlayerModelHorizontal"] = "Горизонтальное положение 3D-модели игрока.",
                    ["Name_RotationPlayerModelHorizontal"] = "Поворот модели игрока",
                    ["Desc_RotationPlayerModelHorizontal"] = "Горизонтальный угол поворота модели игрока.",
                    ["Name_PositionBottomFieldHorizontal"] = "Позиция инфо игрока X",
                    ["Desc_PositionBottomFieldHorizontal"] = "Горизонтальное положение текста с ником и уровнем.",
                    ["Name_PositionBottomFieldVertical"] = "Позиция инфо игрока Y",
                    ["Desc_PositionBottomFieldVertical"] = "Вертикальное положение текста с ником и уровнем.",
                    ["Name_CameraInventoryPositionX"] = "Позиция камеры X",
                    ["Name_CameraInventoryPositionY"] = "Позиция камеры Y",
                    ["Name_CameraInventoryPositionZ"] = "Позиция камеры Z",
                    ["Desc_CameraInventoryPosition"] = "Смещение камеры (Camera_inventory) для превью игрока.",
                    ["Name_CameraInventoryRotationX"] = "Поворот камеры X",
                    ["Name_CameraInventoryRotationY"] = "Поворот камеры Y",
                    ["Name_CameraInventoryRotationZ"] = "Поворот камеры Z",
                    ["Desc_CameraInventoryRotation"] = "Угол поворота камеры (Camera_inventory) для превью.",

                    // UI & Colors
                    ["Name_PositionBattlePassHorizontal"] = "Позиция БП X",
                    ["Desc_PositionBattlePassHorizontal"] = "Положение плашки боевого пропуска от левого края (0.0 - 1.0).",
                    ["Name_PositionBattlePassVertical"] = "Позиция БП Y",
                    ["Desc_PositionBattlePassVertical"] = "Положение плашки боевого пропуска от верхнего края (0.0 - 1.0).",
                    ["Name_AccentColorHex"] = "Акцентный цвет (RGBA)",
                    ["Desc_AccentColorHex"] = "Цвет акцента для текста и подсветки. Формат: R, G, B, A (от 0.0 до 1.0)."
                }
            };

        public static string T(string key)
        {
            if (_table.TryGetValue(_currentLang, out var lang) && lang.TryGetValue(key, out var value))
                return value;

            if (_table["En"].TryGetValue(key, out var fallback))
                return fallback;

            return key;
        }

        public static void SetLanguage(string lang)
        {
            var normalized = NormalizeLanguage(lang);
            if (!string.IsNullOrEmpty(normalized))
            {
                _currentLang = normalized;
            }
        }

        public static string CurrentLanguage => _currentLang;

        public static void DetectLanguage()
        {
            var detected = TryGetSptLanguage();
            if (string.IsNullOrEmpty(detected))
                detected = TryGetUnityLanguage();

            var normalized = NormalizeLanguage(detected);

            if (!string.IsNullOrEmpty(normalized))
            {
                _currentLang = normalized;
                return;
            }

            _currentLang = "En";
        }

        private static string TryGetSptLanguage()
        {
            try
            {
                Type helperType = null;
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var t = asm.GetType("EFT.LocalizedHelper", false)
                         ?? asm.GetType("LocalizedHelper", false);
                    if (t != null) { helperType = t; break; }
                }

                if (helperType == null) return null;

                const System.Reflection.BindingFlags flags =
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static;

                var prop = helperType.GetProperty("CurrentLanguage", flags)
                        ?? helperType.GetProperty("Language", flags);
                if (prop != null)
                {
                    var v = prop.GetValue(null);
                    if (v is string s && !string.IsNullOrEmpty(s))
                        return s;
                }

                var field = helperType.GetField("CurrentLanguage", flags)
                         ?? helperType.GetField("_currentLanguage", flags)
                         ?? helperType.GetField("Language", flags);
                if (field != null)
                {
                    var v = field.GetValue(null);
                    if (v is string s && !string.IsNullOrEmpty(s))
                        return s;

                    if (v != null) return v.ToString();
                }
            }
            catch { }

            return null;
        }

        private static string TryGetUnityLanguage()
        {
            try
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Russian: return "ru";
                    case SystemLanguage.English: return "en";
                }
            }
            catch { }
            return null;
        }

        private static string NormalizeLanguage(string lang)
        {
            if (string.IsNullOrEmpty(lang)) return null;

            var main = lang.Split('-', '_')[0].Trim().ToLowerInvariant();

            switch (main)
            {
                case "ru": return "Ru";
                case "en": return "En";
                default: return null;
            }
        }
    }
}