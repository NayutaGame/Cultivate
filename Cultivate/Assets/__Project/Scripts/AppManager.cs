using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    private AppSM _sm;

    public override void DidAwake()
    {
        base.DidAwake();
        _sm = new AppSM();
        _sm.Push(new AppRunS());
    }

    public static void Push(AppS state)
    {
        Instance._sm.Push(state);
    }

    public static void Pop()
    {
        Instance._sm.Pop();
    }
}
