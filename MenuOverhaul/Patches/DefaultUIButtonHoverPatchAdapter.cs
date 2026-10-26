using EFT.UI;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Infrastructure.Reflection;
using SPTushonka.Reflection.Patching;
using System.Reflection;

namespace MenuOverhaul.Patches
{
    internal class DefaultUIButtonHoverPatchAdapter : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            if (!EftReflectionMap.IsInitialized)
            {
                EftReflectionMap.Initialize();
            }

            var targetMethod = EftReflectionMap.DefaultButtonHighlightedMethod;
            if (targetMethod == null)
            {
                EftReflectionMap.WarnIfMissing(EftReflectionMap.DefaultButtonHighlightedMethod, EftReflectionMap.DefaultButtonHighlightedMethodKey);
            }

            return targetMethod;
        }

        [PatchPostfix]
        private static void Postfix(DefaultUIButtonAnimation __instance, bool animated)
        {
            MainMenuButtonAnimationService.ApplyHover(__instance, animated);
        }
    }
}
