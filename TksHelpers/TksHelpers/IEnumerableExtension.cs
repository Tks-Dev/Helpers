using System;
using System.Collections.Generic;
using System.Linq;

namespace TksHelpers
{
    public static class IEnumerableExtension
    {
        public static void Shuffle<T>(this List<T> list)
        {
            var l = new List<T>();
            var r = new Random(DateTime.Now.ToInt());
            while (list.Count > 0)
            {
                var i = r.Next(list.Count);
                l.Add(list[i]);
                list.RemoveAt(i);
            }
            list.AddRange(l);
        }

        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            var r = new Random(DateTime.Now.ToInt());
            return enumerable.ElementAt(r.Next(enumerable.Count()));
        }

        public static T GetRandomElement<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var l = enumerable.Where(predicate);
            return l.GetRandomElement();
        }
    }
}