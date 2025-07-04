
using System;
using System.Collections.Generic;
using System.Threading;
using CLLibrary;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

public class AppManager : Singleton<AppManager>, Addressable
{
    public static string Version = "20250703";
    
    private Thread _mainThread;
    public bool IsMainThread() => _mainThread.Equals(Thread.CurrentThread);
    
    public enum TargetAudience
    {
        Developer,
        Tester,
        Player,
    }

    [SerializeField] private TargetAudience _targetAudience;
    public TargetAudience Audience => _targetAudience;
    public bool AudienceIsDeveloper() => _targetAudience == TargetAudience.Developer;
    public bool AudienceIsTester() => _targetAudience == TargetAudience.Tester;
    public bool AudienceIsPlayer() => _targetAudience == TargetAudience.Player;
    
    public enum TargetPackage
    {
        Official,
        Demo,
    }

    [SerializeField] private TargetPackage _targetPackage;
    public TargetPackage Package => _targetPackage;
    public bool PackageIsOfficial() => _targetPackage == TargetPackage.Official;
    public bool PackageIsDemo() => _targetPackage == TargetPackage.Demo;

    [SerializeField] private AppCanvas AppCanvas;

    private AppStateMachine _appStateMachine;
    public Settings Settings;

    private Encyclopedia Encyclopedia;
    public RandomManager RandomManager;
    public AudioManager AudioManager;
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

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        _mainThread = Thread.CurrentThread;
        DOTween.SetTweensCapacity(500, 500);

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "App", () => Instance },

            { "Settings", () => Settings },
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
            { "AchievementList", () => ProfileManager.GetCurrProfile().AchievementProfileList },
            { "InventoryFromExpandedPack", () => InventoryFromExpandedPack },
        };

        foreach (var kvp in _accessors)
            Address.AddToRoot(kvp.Key, kvp.Value);

        Application.targetFrameRate = 60;
        
        RandomManager.CheckAwake();
        AudioManager.CheckAwake();

        Settings = new();
        Encyclopedia = new();

        SkillInventory = new();
        Encyclopedia.SkillCategory.Map(e => RunSkill.FromEntryJingJie(e, e.LowestJingJie)).Do(s => SkillInventory.Add(s));

        EditorManager.gameObject.SetActive(true);
        ProfileManager = new();
        ConfigManager = new();

        FormationInventory = new();
        Encyclopedia.FormationCategory.Do(e => FormationInventory.Add(e));

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

    public Stack<Action> EscFuncStack = new();
    public void ClearEscStack() => EscFuncStack.Clear();
    public void PushEscFunc(Action func) => EscFuncStack.Push(func);
    public void PopEscFunc()
    {
        CanvasManager.Instance.CloseAnnotation();
        EscFuncStack.Pop();
    }

    public void InvokeEscFunc()
    {
        if (EscFuncStack.Count > 0)
            EscFuncStack.Peek()?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            InvokeEscFunc();
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
