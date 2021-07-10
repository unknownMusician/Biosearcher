using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if BIOSEARCHER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.Common
{
    internal static class ReflectionHelper
    {
        public const BindingFlags AllMembersFlags =
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static IEnumerable<Type> GetAllTypes()
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetAllTypes));
#endif

            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains(nameof(Biosearcher)));

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return types;
        }

        public static IEnumerable<Type> GetAllTypes<TAttribute>() where TAttribute : Attribute
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetAllTypes));
#endif

            IEnumerable<Type> types = GetAllTypes().Where(type => type.TryGetCustomAttribute<TAttribute>(out _));

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return types;
        }
        
        public static IEnumerable<MethodInfo> GetAllMethods<TAttribute>(BindingFlags flags = AllMembersFlags) where TAttribute : Attribute
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetAllTypes));
#endif

            IEnumerable<MethodInfo> methods = GetAllTypes().SelectMany(type => type.GetMethods(flags)).Where(method => method.TryGetCustomAttribute<TAttribute>(out _));

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return methods;
        }


    }
}