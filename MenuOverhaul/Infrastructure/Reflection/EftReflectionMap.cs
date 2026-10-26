using EFT.UI;
using MenuOverhaul.Infrastructure.Diagnostics;
using MenuOverhaul.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace MenuOverhaul.Infrastructure.Reflection
{
    internal static class EftReflectionMap
    {
        public const string DefaultButtonIdleMethodKey = "DefaultUIButtonAnimation.DefaultButtonIdleMethod";
        public const string DefaultButtonHighlightedMethodKey = "DefaultUIButtonAnimation.DefaultButtonHighlightedMethod";
        public const string NormalIconColorFieldKey = "DefaultUIButtonAnimation.NormalIconColorField";
        public const string NormalLabelColorFieldKey = "DefaultUIButtonAnimation.NormalLabelColorField";
        public const string NormalImageColorFieldKey = "DefaultUIButtonAnimation.NormalImageColorField";
        public const string BackgroundNormalStateAlphaFieldKey = "DefaultUIButtonAnimation.BackgroundNormalStateAlphaField";
        public const string HighlightedIconColorFieldKey = "DefaultUIButtonAnimation.HighlightedIconColorField";
        public const string HighlightedImageColorFieldKey = "DefaultUIButtonAnimation.HighlightedImageColorField";
        public const string MenuScreenPlayButtonFieldKey = "MenuScreen.PlayButtonField";
        public const string MenuScreenWarningFieldKey = "MenuScreen.WarningField";

        public static MethodInfo DefaultButtonIdleMethod { get; private set; }
        public static MethodInfo DefaultButtonHighlightedMethod { get; private set; }

        public static MemberInfo NormalIconColorField { get; private set; }
        public static MemberInfo NormalLabelColorField { get; private set; }
        public static MemberInfo NormalImageColorField { get; private set; }
        public static MemberInfo BackgroundNormalStateAlphaField { get; private set; }
        public static MemberInfo HighlightedIconColorField { get; private set; }
        public static MemberInfo HighlightedImageColorField { get; private set; }
        public static MemberInfo MenuScreenPlayButtonField { get; private set; }
        public static MemberInfo MenuScreenWarningField { get; private set; }

        private static readonly HashSet<string> WarnedMissingMembers = new HashSet<string>();
        private static readonly List<string> MissingMembers = new List<string>();
        private static readonly List<string> ResolvedMembers = new List<string>();

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            if (IsInitialized) return;

            MissingMembers.Clear();
            ResolvedMembers.Clear();
            WarnedMissingMembers.Clear();

            DefaultButtonIdleMethod = ResolveMethod(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.DefaultButtonIdleMethod, DefaultButtonIdleMethodKey);
            DefaultButtonHighlightedMethod = ResolveMethod(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.DefaultButtonHighlightedMethod, DefaultButtonHighlightedMethodKey);

            NormalIconColorField = ResolveFieldOrProperty(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.NormalIconColorField, NormalIconColorFieldKey);
            NormalLabelColorField = ResolveFieldOrProperty(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.NormalLabelColorField, NormalLabelColorFieldKey);
            NormalImageColorField = ResolveFieldOrProperty(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.NormalImageColorField, NormalImageColorFieldKey);
            BackgroundNormalStateAlphaField = ResolveFieldOrProperty(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.BackgroundNormalStateAlphaField, BackgroundNormalStateAlphaFieldKey);
            HighlightedIconColorField = ResolveFieldOrProperty(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.HighlightedIconColorField, HighlightedIconColorFieldKey);
            HighlightedImageColorField = ResolveFieldOrProperty(typeof(DefaultUIButtonAnimation), MenuOverhaulConstants.Reflection.HighlightedImageColorField, HighlightedImageColorFieldKey);

            MenuScreenPlayButtonField = ResolveFieldOrProperty(typeof(MenuScreen), MenuOverhaulConstants.MenuScreen.PlayButtonField, MenuScreenPlayButtonFieldKey);
            MenuScreenWarningField = ResolveFieldOrProperty(typeof(MenuScreen), MenuOverhaulConstants.MenuScreen.WarningField, MenuScreenWarningFieldKey);

            IsInitialized = true;
        }

        public static object GetValue(MemberInfo member, object instance)
        {
            if (member is FieldInfo field) return field.GetValue(instance);
            if (member is PropertyInfo prop) return prop.GetValue(instance);
            return null;
        }

        public static void WarnIfMissing(MemberInfo member, string memberKey)
        {
            if (member != null) return;
            if (!WarnedMissingMembers.Add(memberKey)) return;
            MenuDiagnosticsLogger.Warning(LogSubsystem.Reflection, $"Отсутствует элемент, используемый во время выполнения: {memberKey}");
        }

        public static string GetStartupSummary() => $"Решенные {ResolvedMembers.Count} участники, потерянные {MissingMembers.Count}.";
        public static bool HasMissingMembers() => MissingMembers.Count > 0;
        public static IReadOnlyList<string> GetMissingMembers() => MissingMembers;

        public static bool TryGetMenuScreenField(string fieldName, out MemberInfo member, out string fieldKey)
        {
            switch (fieldName)
            {
                case MenuOverhaulConstants.MenuScreen.PlayButtonField:
                    member = MenuScreenPlayButtonField; fieldKey = MenuScreenPlayButtonFieldKey; return true;
                case MenuOverhaulConstants.MenuScreen.WarningField:
                    member = MenuScreenWarningField; fieldKey = MenuScreenWarningFieldKey; return true;
                default:
                    member = null; fieldKey = null; return false;
            }
        }

        private const BindingFlags MemberLookup = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static MethodInfo ResolveMethod(Type type, string methodName, string memberKey)
        {
            MethodInfo method = AccessTools.Method(type, methodName);
            TrackResolution(method, memberKey);
            return method;
        }

        private static MemberInfo ResolveFieldOrProperty(Type type, string memberName, string memberKey)
        {
            MemberInfo member = type.GetField(memberName, MemberLookup);
            if (member == null)
            {
                member = type.GetProperty(memberName, MemberLookup);
            }
            TrackResolution(member, memberKey);
            return member;
        }

        private static void TrackResolution(MemberInfo member, string memberKey)
        {
            if (member != null)
            {
                ResolvedMembers.Add(memberKey);
                return;
            }
            MissingMembers.Add(memberKey);
            WarnIfMissing(member, memberKey);
        }
    }
}