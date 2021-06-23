using System.Collections;
using System.Collections.Generic;

namespace Biosearcher.Common
{
    public static class CollectionsExtension
    {
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> collection, System.Action<T> action)
        {
            foreach (T element in collection)
            {
                action?.Invoke(element);
            }
            return collection;
        }
        public static IEnumerable Foreach(this IEnumerable collection, System.Action<object> action)
        {
            foreach (object element in collection)
            {
                action?.Invoke(element);
            }
            return collection;
        }
    }
}