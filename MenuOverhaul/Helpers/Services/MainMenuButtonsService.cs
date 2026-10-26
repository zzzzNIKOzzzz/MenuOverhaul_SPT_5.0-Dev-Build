using EFT.UI;
using DG.Tweening;
using UnityEngine;

namespace MenuOverhaul.Helpers.Services
{
    internal static class MainMenuButtonsService
    {
        public static void ApplyMainMenuButtons(MenuScreen menuScreen)
        {
            if (menuScreen == null) return;

            var anims = menuScreen.GetComponentsInChildren<DefaultUIButtonAnimation>(true);
            foreach (var anim in anims)
            {
                if (anim == null) continue;
                if (anim.Label != null) anim.Label.transform.DOKill();
                if (anim.Icon != null) anim.Icon.transform.DOKill();
                anim.Stop();
            }

            MainMenuButtonRuntime.SetupButtonIcons(menuScreen);
            MainMenuButtonRuntime.ProcessButtons(menuScreen);
            MainMenuButtonRuntime.RefreshButtonIdleState(menuScreen);
        }

        public static void UpdateMenuIconVisibility()
        {
            MainMenuButtonRuntime.UpdateMenuButtonIconVisibility();
        }

        public static void UpdateMenuButtonPositions()
        {
            MainMenuButtonRuntime.UpdateMenuButtonGroupPositions();
        }
    }
}