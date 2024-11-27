
using System;
using System.Collections.Generic;
using System.Threading;
using CLLibrary;
using SpriteShadersUltimate.Demo;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AppManager : Singleton<AppManager>, Addressable
{
    private Thread _mainThread;
    public bool IsMainThread() => _mainThread.Equals(Thread.CurrentThread);

    [SerializeField] private AppCanvas AppCanvas;

    private AppSM _sm;
    public Settings Settings;

    private Encyclopedia Encyclopedia;
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
            // Designer

            { "FormationInventory", () => FormationInventory },
            { "SkillInventory", () => SkillInventory },

            { "Profile", () => ProfileManager },
            { "Run", () => RunManager.Instance },
            { "Stage", () => StageManager.Instance },

            { "Canvas", () => CanvasManager.Instance },
        };

        foreach (var kvp in _accessors)
            Address.AddToRoot(kvp.Key, kvp.Value);

        Application.targetFrameRate = 120;

        Settings = new();
        Encyclopedia = new();

        FormationInventory = new();
        Encyclopedia.FormationCategory.Traversal.Do(e => FormationInventory.Add(e));

        SkillInventory = new();
        Encyclopedia.SkillCategory.Traversal.Map(e => RunSkill.FromEntryJingJie(e, e.LowestJingJie)).Do(s => SkillInventory.Add(s));

        AppCanvas.gameObject.SetActive(true);
        EditorManager.gameObject.SetActive(true);

        ProfileManager = new();
        RunManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(false);
        
        _sm = new AppSM();
    }

    private void Start()
    {
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
