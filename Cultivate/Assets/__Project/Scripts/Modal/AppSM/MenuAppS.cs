
using System.Threading.Tasks;
using DG.Tweening;

public class MenuAppS : AppS
{
    public override async Task Enter()
    {
        await base.Enter();

        CanvasManager.Instance.AppCanvas.SettingsPanel.gameObject.SetActive(true);

        Tween handle = CanvasManager.Instance.CurtainHide().SetAutoKill();
        handle.Restart();
        await handle.AsyncWaitForCompletion();
        // await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(true);
    }

    public override async Task Exit()
    {
        await base.Exit();

        Tween handle = CanvasManager.Instance.CurtainShow().SetAutoKill();
        handle.Restart();
        await handle.AsyncWaitForCompletion();

        CanvasManager.Instance.AppCanvas.SettingsPanel.gameObject.SetActive(false);
        // await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(false);
    }
}
