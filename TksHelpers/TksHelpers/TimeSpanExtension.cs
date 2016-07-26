using System;

namespace TksHelpers
{
    public static class TimeSpanExtension
    {
        public static string ToEasyString(this TimeSpan timeSpan)
        {
            var str = string.Empty;
            str += timeSpan.Hours > 0 ? timeSpan.Hours + ":" : string.Empty;
            str += timeSpan.Minutes + ":" + timeSpan.Seconds;
            return str;
        }
    }
}