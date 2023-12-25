
using System;
using System.Threading.Tasks;

public class TitleAppS : AppS
{
    public override async Task Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);
        CanvasManager.Instance.AppCanvas.Configure();
        AudioManager.Play("BGMTitle");
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(true);
    }

    public override async Task<Config> CEnter(NavigateDetails d)
    {
        await base.CEnter(d);

        if (d.ToState is MenuAppS)
            return new();

        if (d.ToState is RunAppS)
        {
            RunConfigForm form = AppManager.Instance.ProfileManager.RunConfigForm;
            await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(false);
            await CanvasManager.Instance.AppCanvas.RunConfigPanel.SetShowing(false);
            return new RunConfig(form, DesignerEnvironment.GetDesignerConfig());
        }

        throw new Exception($"Undefined navigation {GetType()} -> {d.ToState.GetType()}");
    }

    public override async Task CExit(NavigateDetails d, Result result)
    {
        await base.CExit(d, result);

        if (d.FromState is MenuAppS)
            return;

        AudioManager.Play("BGMTitle");
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(true);
    }

    public override async Task<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(false);
        return new();
    }
}
