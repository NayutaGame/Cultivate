using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DataManager : Singleton<DataManager>, Addressable
{
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
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
}
