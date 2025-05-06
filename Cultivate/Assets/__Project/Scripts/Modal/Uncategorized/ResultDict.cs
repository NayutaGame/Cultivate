
using System.Collections.Generic;

public class ResultDict
{
    public Dictionary<string, string> SSDictionary;
    public Dictionary<object, string> ObjectDictionary;

    public ResultDict()
    {
        SSDictionary = new();
        ObjectDictionary = new();
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

    public string this[object key]
    {
        get => ObjectDictionary[key];
        set => ObjectDictionary[key] = value;
    }

    public bool ContainsKey(object key)
        => ObjectDictionary.ContainsKey(key);

    public void Remove(object key)
        => ObjectDictionary.Remove(key);

    public void Clear()
    {
        SSDictionary.Clear();
        ObjectDictionary.Clear();
    }
    
    private static ResultDict _default;

    public static ResultDict Default
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
