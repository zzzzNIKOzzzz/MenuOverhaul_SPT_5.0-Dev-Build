using EFT;
using SPTushonka.Reflection.Patching;
using System.Reflection;
using MenuOverhaul.Helpers;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;

namespace MenuOverhaul.Patches
{
    internal class OnGameEndedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod(nameof(Player.OnGameSessionEnd), BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void PatchPostfix()
        {
            GameStateUtility.SetGameStarted(false);

            if (PlayerProfileViewService.ClonedPlayerModelView != null)
            {
                PlayerProfileViewService.ClonedPlayerModelView.SetActive(true);
                MainMenuLightingService.SetupLights(PlayerProfileViewService.ClonedPlayerModelView);
            }
        }
    }
}