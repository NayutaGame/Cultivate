
public interface Addressable
{
    public object Get(string s);

    // private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    // {
    //     { "InventoryFromExpandedPack",  thisObject => ((AppManager)thisObject).InventoryFromExpandedPack },
    // };
    // public object Get(string s) => Accessor[s](this);
}
