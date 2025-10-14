
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
    [SerializeField] public Version _version;
    public static Version Version => Instance._version;
    
    private Thread _mainThread;
    public bool IsMainThread() => _mainThread.Equals(Thread.CurrentThread);
    
    public enum TargetAudience
    {
        Developer,
        Tester,
        Player,
        Streamer,
    }

    [SerializeField] private TargetAudience _targetAudience;
    public TargetAudience Audience => _targetAudience;
    public bool AudienceIsDeveloper() => _targetAudience == TargetAudience.Developer;
    public bool AudienceIsTester() => _targetAudience == TargetAudience.Tester;
    public bool AudienceIsPlayer() => _targetAudience == TargetAudience.Player;
    public bool AudienceIsStreamer() => _targetAudience == TargetAudience.Streamer;
    
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

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "App",                        thisObject => ((AppManager)thisObject) },
        { "Settings",                   thisObject => ((AppManager)thisObject).Settings },
        { "Encyclopedia",               thisObject => ((AppManager)thisObject).Encyclopedia },
        { "Editor",                     thisObject => ((AppManager)thisObject).EditorManager },
        { "Profile",                    thisObject => ((AppManager)thisObject).ProfileManager },
        { "Config",                     thisObject => ((AppManager)thisObject).ConfigManager },
        { "Run",                        thisObject => ((AppManager)thisObject).RunManager },
        { "Stage",                      thisObject => ((AppManager)thisObject).StageManager },
        { "Canvas",                     thisObject => CanvasManager.Instance },
        { "FormationInventory",         thisObject => ((AppManager)thisObject).FormationInventory },
        { "SkillInventory",             thisObject => ((AppManager)thisObject).SkillInventory },
    };
    public object Get(string s) => Accessor[s](this);
    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        _mainThread = Thread.CurrentThread;
        DOTween.SetTweensCapacity(500, 500);

        foreach (var kvp in Accessor)
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

        AppCanvas.gameObject.SetActive(true);

        RunManager.gameObject.SetActive(true);
        StageManager.CheckAwake();

        _appStateMachine = new();
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
