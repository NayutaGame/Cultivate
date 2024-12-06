
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RunAppS : AppS
{
    public override async UniTask Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);

        RunConfig runConfig = config as RunConfig;
        RunManager.Instance.SetEnvironmentFromConfig(runConfig);
        
        RunManager.Instance.SetBackgroundFromJingJie(JingJie.LianQi);
        StageManager.Instance.SetHomeFromCharacterProfile(runConfig.CharacterProfile);

        RunEnvironment runEnv = RunManager.Instance.Environment;
        RunCanvas runCanvas = CanvasManager.Instance.RunCanvas;
        
        PanelDescriptor panelDescriptor = runEnv.Map.Panel;
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        
        runCanvas.AwakeFunction();
        runCanvas.LegacySetPanelS(panelS);
        runCanvas.TopBar.Refresh();
        CanvasManager.Instance.Curtain.GetAnimator().SetState(1);
        await UniTask.WaitForSeconds(0.1f);
        runCanvas.LayoutRebuild();
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(0);
        runCanvas.Refresh();
    }

    public override async UniTask<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(1);
        
        RunEnvironment runEnv = RunManager.Instance.Environment;
        RunCanvas runCanvas = CanvasManager.Instance.RunCanvas;
        
        runCanvas.LegacySetPanelS(PanelS.FromHide());
        
        RunResult result = runEnv.Result;
        RunManager.Instance.SetEnvironmentToNull();

        return result;
    }

    public override async UniTask<Config> CEnter(NavigateDetails d)
    {
        await base.CEnter(d);

        if (d.ToState is MenuAppS)
            return new();

        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(1);
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        return new();
    }

    public override async UniTask CExit(NavigateDetails d, Result result)
    {
        await base.CExit(d, result);

        if (d.FromState is MenuAppS)
            return;

        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.RunCanvas.Refresh();
        await CanvasManager.Instance.Curtain.GetAnimator().SetStateAsync(0);
    }
}
