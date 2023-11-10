
using System.Threading.Tasks;
using DG.Tweening;

public class MenuAppS : AppS
{
    public override async Task Enter(NavigateDetails d)
    {
        await base.Enter(d);

        if (d.FromState is TitleAppS)
        {
            CanvasManager.Instance.AppCanvas.SettingsPanel.HideExitButtons();
        }
        else
        {
            CanvasManager.Instance.AppCanvas.SettingsPanel.ShowExitButtons();
        }

        CanvasManager.Instance.AppCanvas.SettingsPanel.SetHideState();
        await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(true);

        // Tween handle = CanvasManager.Instance.CurtainHide().SetAutoKill();
        // handle.Restart();
        // await handle.AsyncWaitForCompletion();
    }

    public override async Task Exit(NavigateDetails d)
    {
        await base.Exit(d);

        await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(false);

        // Tween handle = CanvasManager.Instance.CurtainShow().SetAutoKill();
        // handle.Restart();
        // await handle.AsyncWaitForCompletion();
    }
}
