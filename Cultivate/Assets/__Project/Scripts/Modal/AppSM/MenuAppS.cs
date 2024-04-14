
using System.Threading.Tasks;

public class MenuAppS : AppS
{
    public override async Task Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);

        if (d.FromState is TitleAppS)
        {
            CanvasManager.Instance.AppCanvas.SettingsPanel.HideExitButtons();
        }
        else
        {
            CanvasManager.Instance.AppCanvas.SettingsPanel.ShowExitButtons();
        }

        CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowingNoTween(false);
        await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(true);
    }

    public override async Task<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);

        await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(false);
        return new();
    }
}
