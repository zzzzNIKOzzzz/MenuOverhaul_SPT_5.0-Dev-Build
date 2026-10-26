using UnityEngine;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Utils;

namespace MenuOverhaul.Helpers
{
    internal static class MenuScreenVisibilityPolicy
    {
        public static bool IsMainMenuActive { get; private set; }

        public static void MarkMainMenuActive()
        {
            IsMainMenuActive = true;
        }

        public static void HideCustomElements()
        {
            IsMainMenuActive = false;
            if (PlayerProfileViewService.ClonedPlayerModelView != null)
            {
                PlayerProfileViewService.ClonedPlayerModelView.SetActive(false);
            }
        }

        public static void ShowCustomElements()
        {
            IsMainMenuActive = true;
            if (PlayerProfileViewService.ClonedPlayerModelView != null)
            {
                PlayerProfileViewService.ClonedPlayerModelView.SetActive(true);
            }
        }
    }
}