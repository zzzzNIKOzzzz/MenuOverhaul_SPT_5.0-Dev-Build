using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Infrastructure.Diagnostics;
using SPTushonka.Reflection.Patching;
using System;
using System.Reflection;
using MenuOverhaul.Helpers;
using UnityEngine.UI;
using UnityEngine;

namespace MenuOverhaul.Patches
{
    internal class MainMenuLayoutPatchAdapter : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return MenuGuardHelpers.GetMenuScreenShowTargetMethod();
        }

        [PatchPostfix]
        private static void Postfix(MenuScreen __instance, MenuScreen.MainMenuBaseScreenController controller)
        {
            try
            {
                if (!MenuGuardHelpers.IsMainMenuShowContext(__instance, controller, "Layout"))
                {
                    return;
                }

                MainMenuLayoutService.ApplyMainMenuLayout(__instance);
                MainMenuButtonsService.ApplyMainMenuButtons(__instance);

                Canvas.ForceUpdateCanvases();
                if (__instance.GetComponent<RectTransform>() != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(__instance.GetComponent<RectTransform>());
                }

                MainMenuButtonRuntime.RefreshButtonIdleState(__instance);
            }
            catch (Exception e)
            {
                MenuDiagnosticsLogger.Error(LogSubsystem.Layout, e.ToString());
            }
        }
    }
}