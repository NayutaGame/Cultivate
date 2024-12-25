
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

        public static int Clamp(this int value, int min, int max)
            => Mathf.Clamp(value, min, max);

        public static int ClampUpper(this int value, int bound)
            => Mathf.Min(value, bound);

        public static int ClampLower(this int value, int bound)
            => Mathf.Max(value, bound);

        public static float Remap(this float value, float a0, float a1, float b0, float b1)
        {
            return Mathf.Lerp(b0, b1, Mathf.InverseLerp(a0, a1, value));
        }

        public static int[] GetCombination(int n, int k)
        {
            Pool<int> pool = new Pool<int>();
            n.Do(i => pool.Populate(i));
            pool.Shuffle();
            int[] popped = new int[k];
            popped.Length.Do(i => pool.TryPopItem(out popped[i]));
            return popped;
        }
    }
}
