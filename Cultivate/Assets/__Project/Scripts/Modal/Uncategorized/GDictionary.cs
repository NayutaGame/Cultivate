using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GDictionary
{
    Dictionary<string, Func<object>> GetAccessors();

    /*
    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public static T Get<T>(IndexPath indexPath)
    {
        object curr = Instance;
        foreach (string key in indexPath.Values)
        {
            if (int.TryParse(key, out int i))
            {
                IList l = curr as IList;
                if (l.Count <= i)
                    return default;
                curr = l[i];
            }
            else
            {
                curr = (curr as GDictionary).GetAccessors()[key]();
            }
        }

        if (curr is T ret)
            return ret;
        else
            return default;
    }

    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "DanTian", () => DanTian },
        };
    }
    */
}
