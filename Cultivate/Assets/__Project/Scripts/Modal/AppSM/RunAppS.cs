
using System.Threading.Tasks;
using DG.Tweening;

public class RunAppS : AppS
{
    public override async Task Enter()
    {
        await base.Enter();
        RunManager.Instance.Enter();
        CanvasManager.Instance.RunCanvas.Configure();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.RunCanvas.NodeLayer.DisableCurrentPanel();
        CanvasManager.Instance.CurtainHide().SetAutoKill().Restart();
        CanvasManager.Instance.RunCanvas.MapPanel.SetShowing(true);
    }

    public override async Task Exit()
    {
        await base.Exit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        RunManager.Instance.Exit();
    }

    public override async Task CEnter()
    {
        await base.CEnter();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
    }

    public override async Task CExit()
    {
        await base.CExit();
        RunManager.Instance.CExit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.CurtainHide().SetAutoKill().Restart();
    }
}
