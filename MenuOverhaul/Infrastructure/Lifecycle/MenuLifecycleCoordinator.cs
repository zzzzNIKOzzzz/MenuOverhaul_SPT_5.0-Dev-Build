using EFT.UI.Screens;
using MenuOverhaul.Helpers;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;
using System;

namespace MenuOverhaul.Infrastructure.Lifecycle
{
    internal static class MenuLifecycleCoordinator
    {
        private enum ScreenPolicy
        {
            MainMenu,
            HideAll
        }

        private static bool _screenSubscribed;
        private static bool _layoutSettingsSubscribed;
        private static bool _profileSettingsSubscribed;
        private static bool _experienceSubscribed;

        private static bool _handlingScreenChange;
        private static bool _hasQueuedScreenChange;
        private static EEftScreenType _queuedScreenType;
        private static readonly object ScreenChangeLock = new object();
        private static bool _isShuttingDown;

        public static void EnsureScreenSubscription()
        {
            if (_screenSubscribed) return;

            EftScreenManager singleton = EftScreenManager.Instance;
            if (singleton == null)
            {
                MenuDiagnosticsLogger.Warning(LogSubsystem.Lifecycle, "Подписка на экран отложена; EftScreenManager.Instance имеет значение null.");
                return;
            }

            singleton.OnScreenChanged += new Action<EEftScreenType>(OnScreenChanged);
            _screenSubscribed = true;
        }

        public static void EnsureLayoutSettingsSubscription()
        {
            if (_layoutSettingsSubscribed) return;
            MainMenuLayoutService.SubscribeToLayoutSettingsChanges();
            _layoutSettingsSubscribed = true;
        }

        public static void EnsureProfileSubscriptions()
        {
            if (!_profileSettingsSubscribed)
            {
                PlayerProfileViewService.SubscribeToProfileSettingsChanges();
                _profileSettingsSubscribed = true;
            }

            if (!_experienceSubscribed)
            {
                PlayerProfileViewService.SubscribeToCharacterLevelUpEvent();
                _experienceSubscribed = true;
            }
        }

        public static void CleanupOnUnload()
        {
            _isShuttingDown = true;

            lock (ScreenChangeLock)
            {
                _hasQueuedScreenChange = false;
                _handlingScreenChange = false;
            }

            if (_screenSubscribed)
            {
                EftScreenManager singleton = EftScreenManager.Instance;
                if (singleton != null)
                {
                    singleton.OnScreenChanged -= new Action<EEftScreenType>(OnScreenChanged);
                }
                _screenSubscribed = false;
            }

            if (_layoutSettingsSubscribed)
            {
                MainMenuLayoutService.UnsubscribeFromLayoutSettingsChanges();
                _layoutSettingsSubscribed = false;
            }

            if (_profileSettingsSubscribed)
            {
                PlayerProfileViewService.UnsubscribeFromProfileSettingsChanges();
                _profileSettingsSubscribed = false;
            }

            if (_experienceSubscribed)
            {
                PlayerProfileViewService.UnsubscribeFromCharacterLevelUpEvent();
                _experienceSubscribed = false;
            }

            MenuScreenVisibilityPolicy.HideCustomElements();
        }

        private static void OnScreenChanged(EEftScreenType screenType)
        {
            if (_isShuttingDown) return;
            if (GameStateUtility.IsInGame()) return;

            lock (ScreenChangeLock)
            {
                if (_handlingScreenChange)
                {
                    _queuedScreenType = screenType;
                    _hasQueuedScreenChange = true;
                    return;
                }
                _handlingScreenChange = true;
            }

            EEftScreenType currentScreenType = screenType;

            try
            {
                while (true)
                {
                    ApplyScreenPolicy(currentScreenType);

                    lock (ScreenChangeLock)
                    {
                        if (_hasQueuedScreenChange)
                        {
                            currentScreenType = _queuedScreenType;
                            _hasQueuedScreenChange = false;
                            continue;
                        }
                        _handlingScreenChange = false;
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                lock (ScreenChangeLock)
                {
                    _handlingScreenChange = false;
                }
                MenuDiagnosticsLogger.Error(LogSubsystem.Lifecycle, $"Ошибка обработчика смены экрана: {ex}");
            }
        }

        private static void ApplyScreenPolicy(EEftScreenType screenType)
        {
            if (GameStateUtility.IsInGame())
            {
                MenuScreenVisibilityPolicy.HideCustomElements();
                return;
            }

            ScreenPolicy policy = ResolvePolicy(screenType);
            switch (policy)
            {
                case ScreenPolicy.MainMenu:
                    MenuScreenVisibilityPolicy.ShowCustomElements();
                    break;
                default:
                    MenuScreenVisibilityPolicy.HideCustomElements();
                    break;
            }
        }

        private static ScreenPolicy ResolvePolicy(EEftScreenType screenType)
        {
            switch (screenType)
            {
                case EEftScreenType.MainMenu:
                    return ScreenPolicy.MainMenu;
                default:
                    return ScreenPolicy.HideAll;
            }
        }
    }
}