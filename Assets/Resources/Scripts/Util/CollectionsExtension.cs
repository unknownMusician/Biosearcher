using System.Collections;
using System.Collections.Generic;

namespace Biosearcher.Util
{
    public static class CollectionsExtension
    {
        public static void Foreach<T>(this IEnumerable<T> collection, System.Action<T> action)
        {
            foreach (T element in collection)
            {
                action?.Invoke(element);
            }
        }
        public static void Foreach(this IEnumerable collection, System.Action<object> action)
        {
            foreach (object element in collection)
            {
                action?.Invoke(element);
            }
        }
    }
}