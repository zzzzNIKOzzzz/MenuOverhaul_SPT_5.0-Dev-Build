using System;
using UnityEngine;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;

namespace MenuOverhaul.Helpers
{
    internal static class MainMenuLightingService
    {
        private static Light mainLightComponent;
        private static Light hairLightComponent;
        private static Light mainLightAccentComponent;

        public static void SetupLights(GameObject clonedPlayerModelView)
        {
            if (clonedPlayerModelView == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Layout, "SetupLights_NullModel", "SetupLights — clonedPlayerModelView имеет значение null.");
                return;
            }
            mainLightComponent = FindAndSetupLight(clonedPlayerModelView, MenuOverhaulConstants.PlayerModel.MainLightPath, ConfigureMainLight);
            mainLightAccentComponent = FindAndSetupLight(clonedPlayerModelView, MenuOverhaulConstants.PlayerModel.MainAccentLightPath, ConfigureMainLightAccent);
            hairLightComponent = FindAndSetupLight(clonedPlayerModelView, MenuOverhaulConstants.PlayerModel.HairLightPath, ConfigureHairLight);
        }

        private static Light FindAndSetupLight(GameObject parent, string path, Action<Light> configureAction)
        {
            if (parent == null || string.IsNullOrEmpty(path) || configureAction == null)
            {
                MenuDiagnosticsLogger.Error(LogSubsystem.Layout, "FindAndSetupLight — недопустимые аргументы.");
                return null;
            }

            Transform lightTransform = parent.transform.Find(path);
            if (lightTransform == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Layout, $"LightTransformMissing_{path}_{parent.name}", $"Объект источника света GameObject не найден по указанному пути: {path} в {parent.name}.");
                return null;
            }

            Light lightComponent = lightTransform.GetComponent<Light>();
            if (lightComponent == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.Layout, $"LightComponentMissing_{path}_{parent.name}", $"Компонент источника света не найден в игровом объекте GameObject по указанному пути: {path} в {parent.name}.");
                return null;
            }

            configureAction(lightComponent);
            return lightComponent;
        }

        private static void ConfigureMainLight(Light light)
        {
            if (light == null) return;
            light.shadows = Settings.EnableExtraShadows.Value ? LightShadows.Soft : LightShadows.None;
        }

        private static void ConfigureHairLight(Light light)
        {
            if (light == null) return;
            light.shadows = Settings.EnableExtraShadows.Value ? LightShadows.Soft : LightShadows.None;
        }

        private static void ConfigureMainLightAccent(Light light)
        {
            if (light == null) return;
            // ИЗМЕНЕНО: Убрали .Value
            light.color = Settings.AccentColor;
        }

        public static void UpdateLights()
        {
            UpdateSingleLightShadows(mainLightComponent);
            UpdateSingleLightShadows(hairLightComponent);
            UpdateAccentLightColor();
        }

        public static void UpdateAccentLightColor()
        {
            if (mainLightAccentComponent != null)
            {
                // ИЗМЕНЕНО: Убрали .Value
                mainLightAccentComponent.color = Settings.AccentColor;
            }
        }

        private static void UpdateSingleLightShadows(Light lightComponent)
        {
            if (lightComponent != null)
            {
                lightComponent.shadows = Settings.EnableExtraShadows.Value ? LightShadows.Soft : LightShadows.None;
            }
        }

        public static void Cleanup()
        {
            mainLightComponent = null;
            hairLightComponent = null;
            mainLightAccentComponent = null;
        }
    }
}