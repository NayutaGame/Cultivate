
using System;
using System.Collections.Generic;
using System.Threading;
using CLLibrary;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AppManager : Singleton<AppManager>, Addressable
{
    public static string Version = "20250109";
    
    private Thread _mainThread;
    public bool IsMainThread() => _mainThread.Equals(Thread.CurrentThread);

    [SerializeField] private bool _isDeveloperMode;
    public bool IsDeveloperMode
    {
        get
        {
            #if UNITY_EDITOR
            return _isDeveloperMode;
            #else
            return _isDeveloperMode;
            // return false;
            #endif
        }
    }

    [SerializeField] private AppCanvas AppCanvas;

    private AppStateMachine _appStateMachine;
    public Settings Settings;

    private Encyclopedia Encyclopedia;
    public EditorManager EditorManager;
    public ProfileManager ProfileManager;
    public ConfigManager ConfigManager;
    public RunManager RunManager;
    public StageManager StageManager;

    [NonSerialized] public FormationInventory FormationInventory;
    [NonSerialized] public SkillInventory SkillInventory;
    [NonSerialized] public InventoryFromExpandedPack InventoryFromExpandedPack;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _mainThread = Thread.CurrentThread;
        DOTween.SetTweensCapacity(500, 500);

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "App", () => Instance },

            { "Settings", () => Settings },
            // Designer
            { "Encyclopedia", () => Encyclopedia },
            { "Editor", () => EditorManager.Instance },
            { "Profile", () => ProfileManager },
            { "Config", () => ConfigManager },
            { "Run", () => RunManager.Instance },
            { "Stage", () => StageManager.Instance },

            { "Canvas", () => CanvasManager.Instance },

            // Browser
            { "FormationInventory", () => FormationInventory },
            { "SkillInventory", () => SkillInventory },
            { "InventoryFromExpandedPack", () => InventoryFromExpandedPack },
        };

        foreach (var kvp in _accessors)
            Address.AddToRoot(kvp.Key, kvp.Value);

        Application.targetFrameRate = 60;

        Settings = new();
        Encyclopedia = new();

        SkillInventory = new();
        Encyclopedia.SkillCategory.Traversal.Map(e => RunSkill.FromEntryJingJie(e, e.LowestJingJie)).Do(s => SkillInventory.Add(s));

        EditorManager.gameObject.SetActive(true);
        ProfileManager = new();
        ConfigManager = new();

        FormationInventory = new();
        Encyclopedia.FormationCategory.Traversal.Do(e => FormationInventory.Add(e));

        InventoryFromExpandedPack = new();

        AppCanvas.gameObject.SetActive(true);

        RunManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(true);
        StageManager.gameObject.SetActive(false);

        _appStateMachine = new();
    }

    public void SetExpandedPack(PackEntry entry)
    {
        InventoryFromExpandedPack.Clear();
        
        if (entry != null)
        {
            entry.Cards.Do(skillEntry =>
            {
                InventoryFromExpandedPack.Add(skillEntry);
            });
        }
        
        CanvasManager.Instance.PackPreview.Sync();
        CanvasManager.Instance.PackPreview.gameObject.SetActive(true);
    }

    private void Start()
    {
        Push(AppStateMachine.TITLE);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitGame();
    }

    public void Push(int state, object args = null) => _appStateMachine.Push(true, state, args);

    public void Pop(int times = 1)
    {
        for(int i = 0; i < times; i++)
            _appStateMachine.Pop(true, null);
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
