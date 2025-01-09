
using System;

namespace CLLibrary
{
    public static class Util
    {
        public static string FormatTime(TimeSpan time)
            => $"{time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}";
    }
}
