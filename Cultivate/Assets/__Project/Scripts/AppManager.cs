using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    private AppSM _sm;
    public RunManager RunManager;
    public StageManager StageManager;

    public override void DidAwake()
    {
        base.DidAwake();

        Application.targetFrameRate = 120;

        _sm = new AppSM();
        _sm.Push(new AppRunS());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
