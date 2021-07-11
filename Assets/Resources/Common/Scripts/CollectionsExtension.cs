using System.Collections;
using System.Collections.Generic;

namespace Biosearcher.Common
{
    public static class CollectionsExtension
    {
        public static IDictionary<K, V> Foreach<K, V>(this IDictionary<K, V> dictionary, System.Action<K, V> action)
        {
            foreach ((K key, V value) in dictionary)
            {
                action?.Invoke(key, value);
            }
            return dictionary;
        }
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> collection, System.Action<T> action)
        {
            foreach (T element in collection)
            {
                action?.Invoke(element);
            }
            return collection;
        }

        public static T[] For<T>(this T[] array, System.Action<int> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action?.Invoke(i);
            }
            return array;
        }
        public static T[] For<T>(this T[] array, System.Action<int, T> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action?.Invoke(i, array[i]);
            }
            return array;
        }

        public static TEnumerable ForeachNonGeneric<TEnumerable>(this TEnumerable collection, System.Action<object> action) where TEnumerable : IEnumerable
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