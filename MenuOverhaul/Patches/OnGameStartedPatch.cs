using EFT;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;
using SPTushonka.Reflection.Patching;
using System.Reflection;

namespace MenuOverhaul.Patches
{
    internal class OnGameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        private static void PatchPostfix()
        {
            GameStateUtility.SetGameStarted(true);

            if (PlayerProfileViewService.ClonedPlayerModelView != null)
            {
                PlayerProfileViewService.ClonedPlayerModelView.SetActive(false);
            }

            GameStateUtility.ConfigureDecalPlane(false);
        }
    }
}