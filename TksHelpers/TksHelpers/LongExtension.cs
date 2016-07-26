using System;

namespace TksHelpers
{
    public static class LongExtension
    {
        public static string ToEasyString(this long lg)
        {
            var p = lg + string.Empty;
            var s = string.Empty;
            var l = p.Length - 1;
            for (var i = p.Length - 1; i >= 0; i--)
                s = p[i] + ((l - i) % 3 == 0 ? " " : string.Empty) + s;
            return s.Remove(s.Length - 1);
        }

        public static DateTime ToDateTime(this long lg)
        {
            var dt = new DateTime(lg);
            return dt;
        }
    }
}