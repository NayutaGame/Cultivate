
using System.Threading.Tasks;

public class MenuAppS : AppS
{
    public override async Task Enter()
    {
        await base.Enter();
        await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(true);
    }

    public override async Task Exit()
    {
        await base.Exit();
        await CanvasManager.Instance.AppCanvas.SettingsPanel.SetShowing(false);
    }
}
