using System.Collections;
using System.Collections.Generic;
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
    }
}
