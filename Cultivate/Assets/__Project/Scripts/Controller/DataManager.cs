using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DataManager : Singleton<DataManager>, GDictionary
{
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "App", () => AppManager.Instance },
            { "Run", () => RunManager.Instance },
            { "Stage", () => StageManager.Instance },
            { "Encyclopedia", () => Encyclopedia.Instance },
        };
    }

    public static T Get<T>(IndexPath indexPath)
    {
        object curr = Instance;
        foreach (var clKey in indexPath.Values)
        {
            if (clKey is IntKey { Key: int i })
            {
                if (curr is IList l)
                {
                    if (l.Count <= i)
                        return default;
                    curr = l[i];
                }
                else
                {
                    Debug.Log($"{indexPath} @ {i}");
                }
            }
            else if (clKey is StringKey { Key: string s })
            {
                if (curr is GDictionary d)
                {
                    curr = d.Get(s);
                }
                else
                {
                    Debug.Log($"{indexPath} @ {s}");
                }
            }
        }

        if (curr is T ret)
            return ret;
        else
            return default;
    }
}
