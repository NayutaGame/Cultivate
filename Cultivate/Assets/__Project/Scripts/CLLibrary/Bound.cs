
using System;
using UnityEngine;

namespace CLLibrary
{
    [Serializable]
    public struct Bound
    {
        [SerializeField] public int Start;
        [SerializeField] public int End;

        public Bound(int value) : this(value, value + 1) { }

        public Bound(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(int value)
            => Start <= value && value < End;

        public static implicit operator Bound(int i) => new(i);
    }
}
