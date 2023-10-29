
using System.Threading.Tasks;

public class TitleAppS : AppS
{
    public override async Task Enter()
    {
        await base.Enter();
        CanvasManager.Instance.AppCanvas.Configure();
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(true);
    }

    public override async Task CEnter()
    {
        await base.CEnter();
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(false);
    }

    public override async Task CExit()
    {
        await base.CExit();
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(true);
    }

    public override async Task Exit()
    {
        await base.Exit();
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(false);
    }
}
