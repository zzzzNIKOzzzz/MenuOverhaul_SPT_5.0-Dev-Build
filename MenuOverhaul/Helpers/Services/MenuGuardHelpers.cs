using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;
using System.Reflection;
using HarmonyLib;

namespace MenuOverhaul.Helpers.Services
{
    internal static class MenuGuardHelpers
    {
        public static MethodBase GetMenuScreenShowTargetMethod()
        {
            return AccessTools.Method(typeof(MenuScreen), "Show", new System.Type[]
            {
                typeof(MenuScreen.MainMenuBaseScreenController)
            }, null);
        }

        public static bool IsMainMenuShowContext(MenuScreen menuScreen, MenuScreen.MainMenuBaseScreenController controller, string scope)
        {
            if (menuScreen == null)
            {
                MenuDiagnosticsLogger.WarningOnce(LogSubsystem.General, $"MenuScreenNull_{scope}", $"{scope}: MenuScreen instance is null.");
                return false;
            }

            if (GameStateUtility.IsInGame())
            {
                return false;
            }

            return true;
        }

        public static bool CanApplyButtonAnimation(DefaultUIButtonAnimation buttonAnimation)
        {
            if (buttonAnimation == null)
            {
                return false;
            }

            if (!MenuScreenVisibilityPolicy.IsMainMenuActive)
            {
                return false;
            }

            return MainMenuLayoutRuntime.IsPartOfMenuScreen(buttonAnimation);
        }
    }
}