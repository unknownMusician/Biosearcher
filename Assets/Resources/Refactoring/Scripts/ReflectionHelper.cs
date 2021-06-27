//#define REFLECTION_HELPER_PROFILING

using Biosearcher.Refactoring.FileInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if REFLECTION_HELPER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.Refactoring
{
    public static class ReflectionHelper
    {
        private const BindingFlags MembersFlags =
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        internal static bool TryGetCustomAttribute<TAttribute>(this MemberInfo element, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = element.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }
        private static IEnumerable<Type> GetAllTypes()
        {
#if REFLECTION_HELPER_PROFILING
            Profiler.BeginSample(nameof(GetAllTypes));
#endif

            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains(nameof(Biosearcher)));

#if REFLECTION_HELPER_PROFILING
            Profiler.EndSample();
#endif

            return types;
        }
        internal static IEnumerable<SearchedTypeInfo> GetSearchedTypeInfos()
        {
#if REFLECTION_HELPER_PROFILING
            Profiler.BeginSample(nameof(GetSearchedTypeInfos));
#endif

            IEnumerable<SearchedTypeInfo> searchedTypeInfos = GetAllTypes()
                .Select(GetSearchedTypeInfoOrNull)
                .Where(info => info != null)
                .Cast<SearchedTypeInfo>();

#if REFLECTION_HELPER_PROFILING
            Profiler.EndSample();
#endif

            return searchedTypeInfos;
        }
        private static SearchedTypeInfo? GetSearchedTypeInfoOrNull(Type type)
        {
#if REFLECTION_HELPER_PROFILING
            Profiler.BeginSample(nameof(GetSearchedTypeInfoOrNull));
#endif

            if (!type.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
#if REFLECTION_HELPER_PROFILING
                Profiler.EndSample();
#endif

                return null;
            }

            List<SearchedMemberInfo> searchedMemberInfos = new List<SearchedMemberInfo>(
                type.GetMembers(MembersFlags)
                .Select(GetSearchedMemberInfoOrNull)
                .Where(info => info != null)
                .Cast<SearchedMemberInfo>());

            var searchedTypeInfo = new SearchedTypeInfo
            {
                titleInfo = new SearchedTitleInfo { type = type, attribute = attribute },
                memberInfos = searchedMemberInfos
            };

#if REFLECTION_HELPER_PROFILING
            Profiler.EndSample();
#endif

            return searchedTypeInfo;
        }
        private static SearchedMemberInfo? GetSearchedMemberInfoOrNull(MemberInfo member)
        {
#if REFLECTION_HELPER_PROFILING
            Profiler.BeginSample(nameof(GetSearchedMemberInfoOrNull));
#endif

            if (!member.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
#if REFLECTION_HELPER_PROFILING
                Profiler.EndSample();
#endif

                return null;
            }

            var searchedTypeMemberInfo = new SearchedMemberInfo
            {
                member = member,
                attribute = attribute
            };

#if REFLECTION_HELPER_PROFILING
            Profiler.EndSample();
#endif

            return searchedTypeMemberInfo;
        }
    }
}