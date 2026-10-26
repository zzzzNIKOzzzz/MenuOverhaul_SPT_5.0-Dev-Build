using BepInEx.Logging;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using MenuOverhaul.Patches;
using MenuOverhaul.Utils;
using MenuOverhaul.Helpers;
using MenuOverhaul.Infrastructure.Reflection;
using MenuOverhaul.Infrastructure.Lifecycle;
using MenuOverhaul.Infrastructure.Diagnostics;
using System.Collections.Generic;
using System;
using SPTushonka.Reflection.Patching;
using UnityEngine;

namespace MenuOverhaul
{
    [BepInPlugin(MenuOverhaulConstants.Plugin.Guid, MenuOverhaulConstants.Plugin.Name, MenuOverhaulConstants.Plugin.Version)]
    public class Plugin : BasePlugin 
    {
        public static ManualLogSource LogSource { get; private set; }

        private readonly List<ModulePatch> _patches = new List<ModulePatch>();

        public override void Load()
        {
            LogSource = Log;
            Settings.Init(Config);

            ClassInjector.RegisterTypeInIl2Cpp<PlayerModelDragRotator>();
            ClassInjector.RegisterTypeInIl2Cpp<MenuOverhaulUpdater>();

            var updaterObj = new GameObject("MenuOverhaulUpdater");
            updaterObj.AddComponent<MenuOverhaulUpdater>();
            UnityEngine.Object.DontDestroyOnLoad(updaterObj);

            EftReflectionMap.Initialize();
            MenuDiagnosticsLogger.Info(LogSubsystem.Reflection, EftReflectionMap.GetStartupSummary());
            
            if (EftReflectionMap.HasMissingMembers())
            {
                foreach (string missingMember in EftReflectionMap.GetMissingMembers())
                {
                    MenuDiagnosticsLogger.WarningOnce(
                        LogSubsystem.Reflection,
                        "Reflection.StartupMissing." + missingMember,
                        "При запуске отсутствует элемент отражения: " + missingMember);
                }
            }
            else
            {
                MenuDiagnosticsLogger.Info(LogSubsystem.Reflection, "Проверка целостности пройдена: все сопоставленные элементы разрешены.");
            }

            InitializeAndEnablePatches();

            LogSource.LogInfo("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
            LogSource.LogInfo($"Плагин {MenuOverhaulConstants.Plugin.Name} версия {MenuOverhaulConstants.Plugin.Version} загружен.");
            LogSource.LogInfo("zzzNIKOzzz: Ля какая красота, глаз не оторвать!");
            LogSource.LogInfo("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");

        }

        private void InitializeAndEnablePatches()
        {
            _patches.Add(new MainMenuLayoutPatchAdapter());
            _patches.Add(new PlayerProfileViewPatchAdapter());
            _patches.Add(new DefaultUIButtonIdlePatchAdapter());
            _patches.Add(new DefaultUIButtonHoverPatchAdapter());
            _patches.Add(new OnGameStartedPatch());
            _patches.Add(new OnGameEndedPatch());

            foreach (var patch in _patches)
            {
                try { patch.Enable(); }
                catch (Exception ex) { LogSource.LogError($"Не удалось включить патч {patch.GetType().Name}: {ex}"); }
            }
        }
        
        private void OnDestroy() => CleanupResources();
        
        private void CleanupResources()
        {
            try
            {
                MenuLifecycleCoordinator.CleanupOnUnload();
                MainMenuLayoutRuntime.DisposeResources();
                GameStateUtility.ResetGameState();
            }
            catch (Exception ex)
            {
                LogSource.LogError($"Ошибка при очистке плагина: {ex}");
            }
        }
    }
}