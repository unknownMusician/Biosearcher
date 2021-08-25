using Biosearcher.Refactoring;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Biosearcher.Common
{
    public static class CollectionsExtensions
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
        //public static T[] For<T>(this T[] array, System.Action<T> action)
        //{
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        action?.Invoke(array[i]);
        //    }
        //    return array;
        //}

        public static T[] For<T>(this T[] array, Action<T> action) => ForArray<T, T[]>(array, action);
        public static T[,] For<T>(this T[,] array, Action<T> action) => ForArray<T, T[,]>(array, action);
        public static T[,,] For<T>(this T[,,] array, Action<T> action) => ForArray<T, T[,,]>(array, action);
        public static T[,,,] For<T>(this T[,,,] array, Action<T> action) => ForArray<T, T[,,,]>(array, action);
        public static T[,,,,] For<T>(this T[,,,,] array, Action<T> action) => ForArray<T, T[,,,,]>(array, action);

        private static TArray ForArray<TElement, TArray>(Array array, Action<TElement> action) where TArray : class
        {
            For(array, new int[array.Rank], obj => action?.Invoke((TElement)obj));
            return array as TArray;
        }

        [NeedsRefactor(Needs.Check)]
        private static void For(Array array, int[] indices, Action<object> action, int dimension = 0)
        {
            Action nextAction = dimension == indices.Length - 1 ?
                () => action?.Invoke(array.GetValue(indices)) :
                (Action)(() => For(array, indices, action, dimension + 1));

            for (int i = 0; i < array.GetLength(dimension); i++)
            {
                indices[dimension] = i;
                nextAction();
            }
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