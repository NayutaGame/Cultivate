using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class AppManager : Singleton<AppManager>, GDictionary
{
    private AppSM _sm;
    public RunManager RunManager;
    public StageManager StageManager;

    public FormationInventory FormationInventory;
    public SkillInventory SkillInventory;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "FormationInventory", () => FormationInventory },
            { "SkillInventory", () => SkillInventory },
        };

        Application.targetFrameRate = 120;

        FormationInventory = new();
        Encyclopedia.FormationCategory.Traversal.Do(e => FormationInventory.Add(e));

        SkillInventory = new();
        Encyclopedia.SkillCategory.Traversal.Map(e => new RunSkill(e, e.JingJieRange.Start)).Do(e => SkillInventory.AddSkill(e));

        StageManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(false);

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
