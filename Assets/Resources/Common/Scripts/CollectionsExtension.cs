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
        public static IEnumerable ForeachNonGeneric(this IEnumerable collection, System.Action<object> action)
        {
            foreach (object element in collection)
            {
                action?.Invoke(element);
            }
            return collection;
        }
        public static void Deconstruct<K, V>(this KeyValuePair<K, V> pair, out K key, out V value)
        {
            key = pair.Key;
            value = pair.Value;
        }
        public static void SafeAddToValueCollection<K, V, TCollection>(this IDictionary<K, TCollection> dictionary, K key, V value) where TCollection : ICollection<V>, new()
        {
            if (!dictionary.TryGetValue(key, out TCollection valueList))
            {
                valueList = dictionary[key] = new TCollection();
            }
            valueList.Add(value);
        }
    }
}