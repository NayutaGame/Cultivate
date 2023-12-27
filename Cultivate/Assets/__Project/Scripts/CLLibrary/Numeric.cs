
using UnityEngine;

namespace CLLibrary
{
    public static class Numeric
    {
        public static (int, int) Negate(int i0, int i1)
        {
            int min = Mathf.Min(i0, i1);
            i0 -= min;
            i1 -= min;
            return (i0, i1);
        }

        public static int ClampUpper(this int value, int bound)
            => Mathf.Min(value, bound);

        public static int ClampLower(this int value, int bound)
            => Mathf.Max(value, bound);
    }
}
