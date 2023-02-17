
using System;
using System.Collections.Generic;

namespace CLLibrary
{
    public static class Linq
    {
        private static void Min<T>(this IEnumerable<T> enumerable, Func<T, float> eval, out int? outIdx, out float? outVal, out T outObj)
        {
            int? minIdx = null;
            float? minVal = null;
            T minObj = default;

            int i = 0;
            foreach (T obj in enumerable)
            {
                float currVal = eval(obj);
                if (!minVal.HasValue || currVal < minVal.Value)
                {
                    minIdx = i;
                    minVal = currVal;
                    minObj = obj;
                }

                i++;
            }

            outIdx = minIdx;
            outVal = minVal;
            outObj = minObj;
        }

        public static int? MinIdx<T>(this IEnumerable<T> enumerable, Func<T, float> eval)
        {
            Min(enumerable, eval, out int? outIdx, out float? outVal, out T outObj);
            return outIdx;
        }

        public static float? MinVal<T>(this IEnumerable<T> enumerable, Func<T, float> eval)
        {
            Min(enumerable, eval, out int? outIdx, out float? outVal, out T outObj);
            return outVal;
        }

        public static T MinObj<T>(this IEnumerable<T> enumerable, Func<T, float> eval)
        {
            Min(enumerable, eval, out int? outIdx, out float? outVal, out T outObj);
            return outObj;
        }

        private static void First<T>(this IEnumerable<T> enumerable, Predicate<T> pred, out int? outIdx, out T outObj)
        {
            int i = 0;
            foreach (T obj in enumerable)
            {
                if (pred(obj))
                {
                    outIdx = i;
                    outObj = obj;
                    return;
                }

                i++;
            }

            outIdx = null;
            outObj = default;
        }

        public static int? FirstIdx<T>(this IEnumerable<T> enumerable, Predicate<T> pred)
        {
            First(enumerable, pred, out int? outIdx, out T outObj);
            return outIdx;
        }

        public static T FirstObj<T>(this IEnumerable<T> enumerable, Predicate<T> pred)
        {
            First(enumerable, pred, out int? outIdx, out T outObj);
            return outObj;
        }

        public static IEnumerable<int> FilterIdx<T>(this IEnumerable<T> enumerable, Predicate<T> pred)
        {
            int i = 0;

            foreach (T obj in enumerable)
            {
                if (pred(obj))
                {
                    yield return i;
                }

                i++;
            }
        }

        public static IEnumerable<T> FilterObj<T>(this IEnumerable<T> enumerable, Predicate<T> pred)
        {
            foreach (T obj in enumerable)
            {
                if (pred(obj))
                {
                    yield return obj;
                }
            }
        }

        public static void Do<T>(this IEnumerable<T> enumerable, Action<T> func)
        {
            foreach (T e in enumerable) func(e);
        }
    }
}
