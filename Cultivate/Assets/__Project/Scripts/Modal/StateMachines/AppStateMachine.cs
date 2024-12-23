
using System;
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;

public class AppStateMachine
{
    public static readonly int GENERIC = -1;
    
    public static readonly int APP = 0;
    public static readonly int TITLE = 1;
    public static readonly int MENU = 2;
    public static readonly int RUN = 3;
    public static readonly int STAGE = 4;

    public static readonly int COUNT = 5;
    
    private Table<Func<bool, object, UniTask>> _table;
    private Stack<int> _stack;
    
    public Func<bool, object, UniTask> this[int from, int to]
    {
        get => _table[from, to];
        set => _table[from, to] = value;
    }

    private async UniTask SetState(bool isAwait, int fromState, int toState, object args)
    {
        if (this[fromState, -1] is { } first)
            await first.Invoke(isAwait, args);

        if (this[fromState, toState] is { } second)
            await second.Invoke(isAwait, args);

        if (this[-1, toState] is { } third)
            await third.Invoke(isAwait, args);
    }

    public async UniTask Push(bool isAwait, int state, object args)
    {
        int fromState = _stack.Peek();
        await SetState(isAwait, fromState, state, args);
        _stack.Push(state);
    }

    public async UniTask Pop(bool isAwait, object args)
    {
        int fromState = _stack.Pop();
        int toState = _stack.Peek();
        await SetState(isAwait, fromState, toState, args);
    }

    public async UniTask Clear(bool isAwait)
    {
        while(_stack.Count > 0)
            await Pop(isAwait, null);
    }

    public AppStateMachine()
    {
        _stack = new();
        _stack.Push(0);
        
        _table = new(COUNT);
        this[APP, TITLE] = FromAppToTitle;
        this[TITLE, APP] = FromTitleToApp;
        this[TITLE, MENU] = FromTitleToMenu;
        this[MENU, TITLE] = FromMenuToTitle;
        this[TITLE, RUN] = FromTitleToRun;
        this[RUN, TITLE] = FromRunToTitle;
        this[RUN, MENU] = FromRunToMenu;
        this[MENU, RUN] = FromMenuToRun;
        this[RUN, STAGE] = FromRunToStage;
        this[STAGE, RUN] = FromStageToRun;
        this[GENERIC, TITLE] = FromGenericToTitle;
    }

    private async UniTask FromGenericToTitle(bool isAwait, object args)
    {
        CanvasManager.Instance.AppCanvas.TitlePanel.Refresh();
    }

    private async UniTask FromAppToTitle(bool isAwait, object args)
    {
        AudioManager.Play("BGMTitle");
        CanvasManager.Instance.Curtain.GetAnimator().SetState(1);
        await UniTask.Delay(1000);
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(1);
    }

    private async UniTask FromTitleToApp(bool isAwait, object args)
    {
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(0);
    }

    private async UniTask FromTitleToMenu(bool isAwait, object args)
    {
        CanvasManager.Instance.AppCanvas.SettingsPanel.HideExitButtons();

        CanvasManager.Instance.AppCanvas.SettingsPanel.GetAnimator().SetState(0);
        await CanvasManager.Instance.AppCanvas.SettingsPanel.GetAnimator().SetStateAsync(1);
    }
    
    private async UniTask FromMenuToTitle(bool isAwait, object args)
    {
        await CanvasManager.Instance.AppCanvas.SettingsPanel.GetAnimator().SetStateAsync(0);
    }

    private async UniTask FromTitleToRun(bool isAwait, object args)
    {
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(0);
        
        if (args is RunConfigForm form)
        {
            RunConfig runConfig = new(form);
            RunManager.Instance.SetEnvironmentFromConfig(runConfig);
        }
        else if (args is RunEnvironment env)
        {
            RunManager.Instance.SetEnvironmentFromSaved(env);
        }
        else
        {
            throw new Exception("Shouldn't be here");
        }
        
        RunManager.Instance.SetBackgroundFromJingJie(JingJie.LianQi);
        StageManager.Instance.SetHomeFromCharacterProfile(RunManager.Instance.Environment.GetRunConfig().CharacterProfile);

        RunCanvas runCanvas = CanvasManager.Instance.RunCanvas;
        runCanvas.AwakeFunction();
        runCanvas.LegacySetPanelS(PanelS.FromPanelDescriptor(RunManager.Instance.Environment.Panel));
        runCanvas.TopBar.Refresh();
        CanvasManager.Instance.Curtain.GetAnimator().SetState(1);
        await UniTask.WaitForSeconds(0.1f);
        runCanvas.LayoutRebuild();
        CanvasManager.Instance.AppCanvas.RunConfigPanel.GetAnimator().SetState(0);
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(0);
        runCanvas.Refresh();
    }
    
    private async UniTask FromRunToTitle(bool isAwait, object args)
    {
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(1);
        
        RunEnvironment runEnv = RunManager.Instance.Environment;
        RunCanvas runCanvas = CanvasManager.Instance.RunCanvas;
        
        runCanvas.LegacySetPanelS(PanelS.FromHide());
        
        RunResult result = runEnv.GetResult();
        RunManager.Instance.SetEnvironmentToNull();
        
        
        
        
        AudioManager.Play("BGMTitle");
        
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(1);
    }

    private async UniTask FromRunToMenu(bool isAwait, object args)
    {
        CanvasManager.Instance.AppCanvas.SettingsPanel.ShowExitButtons();

        CanvasManager.Instance.AppCanvas.SettingsPanel.GetAnimator().SetState(0);
        await CanvasManager.Instance.AppCanvas.SettingsPanel.GetAnimator().SetStateAsync(1);
    }
    
    private async UniTask FromMenuToRun(bool isAwait, object args)
    {
        await CanvasManager.Instance.AppCanvas.SettingsPanel.GetAnimator().SetStateAsync(0);
    }
    
    private async UniTask FromRunToStage(bool isAwait, object args)
    {
        StageConfig stageConfig = args as StageConfig;
        
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(1);
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        
        
        
        
        

        AppManager.Instance.StageManager.gameObject.SetActive(true);
        
        StageManager.Instance.SetEnvironmentFromConfig(stageConfig);
        StageManager.Instance.SetAwayFromRunEntity(stageConfig.Away);
        
        CanvasManager.Instance.StageCanvas.Configure();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.StageCanvas.InitialSetup();
        StageManager.Instance.Enter();
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(0);
    }
    
    private async UniTask FromStageToRun(bool isAwait, object args)
    {
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(1);
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(false);
        AppManager.Instance.StageManager.gameObject.SetActive(false);
        await StageManager.Instance.Exit();
        
        
        
        
        
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.RunCanvas.Refresh();
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(0);
    }
}
