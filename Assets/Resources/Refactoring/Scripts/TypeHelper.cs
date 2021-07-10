using Biosearcher.Common;
using Biosearcher.Refactoring.FileInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if BIOSEARCHER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.Refactoring
{
    public static class TypeHelper
    {
        internal static IEnumerable<SearchedTypeInfo> GetSearchedTypeInfos()
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetSearchedTypeInfos));
#endif

            IEnumerable<SearchedTypeInfo> searchedTypeInfos = ReflectionHelper.GetAllTypes()
                .Select(GetSearchedTypeInfoOrNull)
                .Where(info => info != null)
                .Cast<SearchedTypeInfo>();

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return searchedTypeInfos;
        }
        private static SearchedTypeInfo? GetSearchedTypeInfoOrNull(Type type)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetSearchedTypeInfoOrNull));
#endif

            if (!type.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
#if BIOSEARCHER_PROFILING
                Profiler.EndSample();
#endif

                return null;
            }

            List<SearchedMemberInfo> searchedMemberInfos = new List<SearchedMemberInfo>(
                type.GetMembers(ReflectionHelper.AllMembersFlags)
                .Select(GetSearchedMemberInfoOrNull)
                .Where(info => info != null)
                .Cast<SearchedMemberInfo>());

            var searchedTypeInfo = new SearchedTypeInfo
            {
                titleInfo = new SearchedTitleInfo { type = type, attribute = attribute },
                memberInfos = searchedMemberInfos
            };

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return searchedTypeInfo;
        }
        private static SearchedMemberInfo? GetSearchedMemberInfoOrNull(MemberInfo member)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetSearchedMemberInfoOrNull));
#endif

            if (!member.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
#if BIOSEARCHER_PROFILING
                Profiler.EndSample();
#endif

                return null;
            }

            var searchedTypeMemberInfo = new SearchedMemberInfo
            {
                member = member,
                attribute = attribute
            };

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return searchedTypeMemberInfo;
        }
    }
}