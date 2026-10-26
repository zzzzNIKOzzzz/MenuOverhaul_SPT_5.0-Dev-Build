using EFT;
using EFT.UI;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Infrastructure.Diagnostics;
using SPTushonka.Reflection.Patching;
using System;
using System.Reflection;

namespace MenuOverhaul.Patches
{
    internal class PlayerProfileViewPatchAdapter : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return PlayerProfileViewService.GetMenuScreenShowTargetMethod();
        }

        [PatchPostfix]
        private static void Postfix(MenuScreen __instance, MenuScreen.MainMenuBaseScreenController controller)
        {
            try
            {
                if (!MenuGuardHelpers.IsMainMenuShowContext(__instance, controller, "Profile"))
                {
                    return;
                }

                // Вызываем новый синхронный метод
                PlayerProfileViewService.ApplyMainMenuProfileView();
            }
            catch (Exception e)
            {
                MenuDiagnosticsLogger.Error(LogSubsystem.Profile, e.ToString());
            }
        }
    }
}