
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class AppManager : Singleton<AppManager>, Addressable
{
    private AppSM _sm;
    public RunManager RunManager;
    public StageManager StageManager;

    public Settings Settings;
    public FormationInventory FormationInventory;
    public SkillInventory SkillInventory;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "Settings", () => Settings },
            { "FormationInventory", () => FormationInventory },
            { "SkillInventory", () => SkillInventory },
        };

        Application.targetFrameRate = 120;

        Settings = new();

        FormationInventory = new();
        Encyclopedia.FormationCategory.Traversal.Do(e => FormationInventory.Add(e));

        SkillInventory = new();
        Encyclopedia.SkillCategory.Traversal.Map(e => RunSkill.From(e, e.JingJieRange.Start)).Do(e => SkillInventory.AddSkill(e));

        StageManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(false);

        _sm = new AppSM();
        _sm.Push(new RunAppS());
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
