using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLLibrary
{
    public class Range
    {
        public int Start;
        public int End;

        public Range(int value) : this(value, value + 1) { }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(int value)
            => Start <= value && value < End;
    }
}
