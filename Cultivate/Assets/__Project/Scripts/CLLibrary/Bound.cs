
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CLLibrary
{
    [Serializable]
    public struct Bound
    {
        [SerializeField] public int Start;
        [SerializeField] public int End;

        public Bound(int value) : this(value, value) { }

        public Bound(int start, int end)
        {
            Start = start;
            End = end;
        }

        private static Bound From(int start, int end)
            => new(start, end);

        public bool Contains(int value)
            => Start <= value && value <= End;

        public int Length => End - Start + 1;

        public static implicit operator Bound(int i) => new(i);

        public override string ToString()
            => $"[{Start}, {End}]";
    }
}
