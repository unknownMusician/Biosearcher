using System;
using System.Reflection;

namespace Biosearcher.Common
{
    public static class AttributesExtensions
    {
        public static bool TryGetCustomAttribute<TAttribute>(this MemberInfo element, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = element.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }
    }
}