
using System;
using Cysharp.Threading.Tasks;

public class TitleAppS : AppS
{
    public override async UniTask Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);
        
        AudioManager.Play("BGMTitle");
        CanvasManager.Instance.Curtain.Animator.SetState(1);
        await UniTask.Delay(1000);
        await CanvasManager.Instance.AppCanvas.TitlePanel.Animator.SetStateAsync(1);
    }

    public override async UniTask<Config> CEnter(NavigateDetails d)
    {
        await base.CEnter(d);

        if (d.ToState is MenuAppS)
            return new();

        if (d.ToState is RunAppS)
        {
            RunConfigForm form = AppManager.Instance.ProfileManager.RunConfigForm;
            await CanvasManager.Instance.AppCanvas.TitlePanel.Animator.SetStateAsync(0);
            return new RunConfig(form);
        }

        throw new Exception($"Undefined navigation {GetType()} -> {d.ToState.GetType()}");
    }

    public override async UniTask CExit(NavigateDetails d, Result result)
    {
        await base.CExit(d, result);

        if (d.FromState is MenuAppS)
            return;

        AudioManager.Play("BGMTitle");
        
        await CanvasManager.Instance.AppCanvas.TitlePanel.Animator.SetStateAsync(1);
    }

    public override async UniTask<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.AppCanvas.TitlePanel.Animator.SetStateAsync(0);
        return new();
    }
}
