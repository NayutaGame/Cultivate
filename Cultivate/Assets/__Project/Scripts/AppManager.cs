
using System;
using System.Collections.Generic;
using System.Threading;
using CLLibrary;
using UnityEngine;

public class AppManager : Singleton<AppManager>, Addressable
{
    private Thread _mainThread;
    public bool IsMainThread() => _mainThread.Equals(Thread.CurrentThread);

    private AppSM _sm;
    private Encyclopedia Encyclopedia;
    public Settings Settings;

    [SerializeField] private AppCanvas AppCanvas;
    public EditorManager EditorManager;
    public RunManager RunManager;
    public StageManager StageManager;

    public FormationInventory FormationInventory;
    public SkillInventory SkillInventory;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _mainThread = Thread.CurrentThread;

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "App", () => Instance },
            { "Encyclopedia", () => Encyclopedia },
            { "Settings", () => Settings },

            { "Editor", () => EditorManager.Instance },
            { "Run", () => RunManager.Instance },
            { "Stage", () => StageManager.Instance },

            { "FormationInventory", () => FormationInventory },
            { "SkillInventory", () => SkillInventory },
        };

        Application.targetFrameRate = 120;

        Encyclopedia = new();
        Settings = new();

        FormationInventory = new();
        Encyclopedia.FormationCategory.Traversal.Do(e => FormationInventory.Add(e));

        SkillInventory = new();
        Encyclopedia.SkillCategory.Traversal.Map(e => RunSkill.From(e, e.JingJieRange.Start)).Do(s => SkillInventory.Add(s));

        AppCanvas.gameObject.SetActive(true);
        EditorManager.gameObject.SetActive(true);
        RunManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(false);

        _sm = new AppSM();
        Push(new TitleAppS());
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
