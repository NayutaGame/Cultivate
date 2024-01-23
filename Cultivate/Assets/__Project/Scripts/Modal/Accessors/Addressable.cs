
public interface Addressable
{
    public object Get(string s);

    // private Dictionary<string, Func<object>> _accessors;
    // public object Get(string s) => _accessors[s]();
    // public Constructor()
    // {
    //     _accessors = new()
    //     {
    //         { "Key",         () => Value },
    //     };
    // }
}
