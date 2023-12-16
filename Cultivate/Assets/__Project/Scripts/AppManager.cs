
using System;
using System.Collections.Generic;
using System.Threading;
using CLLibrary;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AppManager : Singleton<AppManager>, Addressable
{
    private Thread _mainThread;
    public bool IsMainThread() => _mainThread.Equals(Thread.CurrentThread);

    private AppSM _sm;
    public Settings Settings;
    private Encyclopedia Encyclopedia;

    [SerializeField] private AppCanvas AppCanvas;
    public EditorManager EditorManager;
    public ProfileManager ProfileManager;
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
            { "Settings", () => Settings },
            { "Encyclopedia", () => Encyclopedia },

            { "Editor", () => EditorManager.Instance },
            { "Profile", () => ProfileManager },
            { "Run", () => RunManager.Instance },
            { "Stage", () => StageManager.Instance },

            { "FormationInventory", () => FormationInventory },
            { "SkillInventory", () => SkillInventory },
        };

        Application.targetFrameRate = 120;

        Settings = new();
        Encyclopedia = new();

        FormationInventory = new();
        Encyclopedia.FormationCategory.Traversal.Do(e => FormationInventory.Add(e));

        SkillInventory = new();
        Encyclopedia.SkillCategory.Traversal.Map(e => RunSkill.From(e, e.JingJieRange.Start)).Do(s => SkillInventory.Add(s));

        AppCanvas.gameObject.SetActive(true);
        EditorManager.gameObject.SetActive(true);

        ProfileManager = new();

        RunManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(false);

        _sm = new AppSM();
        Push(new TitleAppS());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitGame();
    }

    public static void Push(AppS state)
    {
        Instance._sm.Push(state);
    }

    public static void Pop(int times = 1)
    {
        Instance._sm.Pop(times);
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
