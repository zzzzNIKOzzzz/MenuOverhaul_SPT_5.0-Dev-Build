using System.Globalization;
using System;
using EFT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;

namespace MenuOverhaul.Helpers
{
    internal static class PlayerProfileStatsController
    {
        public static void UpdateTextColors(GameObject clonedPlayerModelView)
        {
            Transform bottomFieldTransform = PlayerProfileTransformController.GetBottomFieldTransform(clonedPlayerModelView);
            if (bottomFieldTransform == null)
            {
                return;
            }

            Color accentColor = Settings.AccentColor;

            Transform nicknameTransform = bottomFieldTransform.Find(MenuOverhaulConstants.BottomFieldUi.NicknameText);
            TextMeshProUGUI nicknameTMP = nicknameTransform != null ? nicknameTransform.GetComponent<TextMeshProUGUI>() : null;
            if (nicknameTMP != null)
            {
                nicknameTMP.color = accentColor;
            }

            Transform experienceRow = bottomFieldTransform.Find(MenuOverhaulConstants.BottomFieldUi.ExperienceRow);
            if (experienceRow != null)
            {
                Transform expValueTransform = experienceRow.Find(MenuOverhaulConstants.BottomFieldUi.ExpValue);
                TextMeshProUGUI expValueTMP = expValueTransform != null ? expValueTransform.GetComponent<TextMeshProUGUI>() : null;
                if (expValueTMP != null)
                {
                    expValueTMP.color = accentColor;
                }
            }
        }

        public static void UpdatePlayerStats(GameObject clonedPlayerModelView, Profile profile, Action<Image, int> setLevelIcon)
        {
            Transform bottomFieldTransform = PlayerProfileTransformController.GetBottomFieldTransform(clonedPlayerModelView);
            if (bottomFieldTransform == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdateNicknameDisplay_MissingNickname", "UpdateNicknameDisplay — компонент NicknameText (типа TMP) не найден в BottomField.");
                return;
            }

            if (profile == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdateExperienceDisplay_MissingRow", "UpdateExperienceDisplay — ExperienceRow не найден в BottomField.");
                return;
            }

            UpdateNicknameDisplay(bottomFieldTransform, profile);
            UpdateExperienceDisplay(bottomFieldTransform, profile);
            UpdateLevelDisplay(bottomFieldTransform, profile, setLevelIcon);
            UpdateTextColors(clonedPlayerModelView);
        }

        private static void UpdateNicknameDisplay(Transform bottomField, Profile profile)
        {
            Transform nicknameTransform = bottomField.Find(MenuOverhaulConstants.BottomFieldUi.NicknameText);
            TextMeshProUGUI nicknameTMP = nicknameTransform != null ? nicknameTransform.GetComponent<TextMeshProUGUI>() : null;
            if (nicknameTMP != null)
            {
                nicknameTMP.text = profile.Nickname;
            }
            else
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdateExperienceDisplay_MissingExpValue", "UpdateExperienceDisplay — в ExperienceRow не найден компонент TMP для ExpValue.");
            }

            Transform originalNicknameAndKarma = bottomField.Find(MenuOverhaulConstants.BottomFieldUi.NicknameAndKarma);
            if (originalNicknameAndKarma != null)
            {
                originalNicknameAndKarma.gameObject.SetActive(false);
            }
        }

        private static void UpdateExperienceDisplay(Transform bottomField, Profile profile)
        {
            Transform experienceRow = bottomField.Find(MenuOverhaulConstants.BottomFieldUi.ExperienceRow);
            if (experienceRow == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdateLevelDisplay_MissingLevelInfoRow", "UpdateLevelDisplay — в BottomField не найден transform для LevelInfoRow.");
                return;
            }

            Transform expValueTransform = experienceRow.Find(MenuOverhaulConstants.BottomFieldUi.ExpValue);
            TextMeshProUGUI experienceTMP = expValueTransform != null ? expValueTransform.GetComponent<TextMeshProUGUI>() : null;
            if (experienceTMP != null)
            {
                var numberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };
                experienceTMP.text = profile.Experience.ToString("N", numberFormat);
            }
            else
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdateLevelDisplay_NullProfileInfo", "UpdateLevelDisplay — Profile.Info имеет значение null.");
            }
        }

        private static void UpdateLevelDisplay(Transform bottomField, Profile profile, Action<Image, int> setLevelIcon)
        {
            Transform levelInfoRow = bottomField.Find(MenuOverhaulConstants.BottomFieldUi.LevelInfoRow);
            if (levelInfoRow == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdateLevelDisplay_MissingLevelTMP", "UpdateLevelDisplay — компонент TMP для уровня не найден в LevelInfoRow.");
                return;
            }

            if (profile.Info == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdateLevelDisplay_MissingLevelIcon", "UpdateLevelDisplay — в LevelInfoRow не найден компонент Transform для значка уровня.");
                return;
            }

            Transform levelTransform = levelInfoRow.Find(MenuOverhaulConstants.BottomFieldUi.Level);
            TextMeshProUGUI levelTMP = levelTransform != null ? levelTransform.GetComponent<TextMeshProUGUI>() : null;
            if (levelTMP != null)
            {
                levelTMP.alignment = TextAlignmentOptions.Right;
                levelTMP.margin = Vector4.zero;
                levelTMP.lineSpacing = 0;
                levelTMP.fontSize = 54f;
                levelTMP.enableWordWrapping = false;
                levelTMP.text = profile.Info.Level.ToString();
            }
            else
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdatePlayerStats_NullBottomField", "UpdatePlayerStats — bottomFieldTransform имеет значение null.");
            }

            Transform iconTransform = levelInfoRow.Find(MenuOverhaulConstants.BottomFieldUi.LevelIcon);
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null)
                {
                    if (setLevelIcon != null)
                    {
                        setLevelIcon.Invoke(iconImage, profile.Info.Level);
                    }
                    iconImage.preserveAspect = true;
                }

                RectTransform iconRect = iconTransform.GetComponent<RectTransform>();
                if (iconRect != null)
                {
                    iconRect.sizeDelta = new Vector2(56f, 56f);
                    iconRect.offsetMax = new Vector2(-5f, iconRect.offsetMax.y);
                }

                LayoutElement iconLe = iconTransform.GetComponent<LayoutElement>();
                if (iconLe != null)
                {
                    iconLe.minWidth = 56f;
                    iconLe.minHeight = 56f;
                    iconLe.preferredWidth = 56f;
                    iconLe.preferredHeight = 56f;
                }
            }
            else
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Profile, "UpdatePlayerStats_NullProfile", "UpdatePlayerStats — профиль отсутствует (null). Невозможно обновить статистику.");
            }

            HorizontalLayoutGroup hlg = levelInfoRow.GetComponent<HorizontalLayoutGroup>();
            if (hlg != null)
            {
                if (!hlg.reverseArrangement)
                {
                    hlg.reverseArrangement = true;
                }

                hlg.spacing = 0f;
            }
        }
    }
}