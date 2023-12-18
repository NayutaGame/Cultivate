
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

        public static implicit operator Range(int i) => new(i);
    }
}
