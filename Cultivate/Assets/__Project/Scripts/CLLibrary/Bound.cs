
namespace CLLibrary
{
    public struct Bound
    {
        public int Start;
        public int End;

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
