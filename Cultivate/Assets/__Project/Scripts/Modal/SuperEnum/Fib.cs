
public class Fib : SuperEnum<Fib>
{
    private readonly int _value;

    private Fib(int index, int value, string name) : base(index, name)
    {
        _value = value;
    }

    public static void Init()
    {
        _list = new Fib[]
        {
            new(0, 0, ""),
            new(1, 1, ""),
            new(2, 1, ""),
            new(3, 2, ""),
            new(4, 3, ""),
            new(5, 5, ""),
            new(6, 8, ""),
            new(7, 13, ""),
            new(8, 21, ""),
            new(9, 34, ""),
            new(10, 55, ""),
            new(11, 89, ""),
            new(12, 144, ""),
        };
    }

    public static int ToValue(int index)
        => _list[index]._value;
}
