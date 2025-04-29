
using System.Collections.Generic;

public class CastResult
{
    public Dictionary<string, string> SSDictionary;

    public CastResult()
    {
        SSDictionary = new();
    }

    public string this[string key]
    {
        get => SSDictionary[key];
        set => SSDictionary[key] = value;
    }

    public bool ContainsKey(string key)
        => SSDictionary.ContainsKey(key);

    public void Remove(string key)
        => SSDictionary.Remove(key);
    
    private static CastResult _default;

    public static CastResult Default
    {
        get
        {
            if (_default != null)
                return _default;

            _default = new();
            _default["doubleEnd"] = "Hide";
            return _default;
        }
    }
}
