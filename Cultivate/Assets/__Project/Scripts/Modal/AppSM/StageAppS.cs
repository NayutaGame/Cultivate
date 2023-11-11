
using System.Threading.Tasks;

public class StageAppS : AppS
{
    public override async Task Enter(NavigateDetails d)
    {
        await base.Enter(d);
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        CanvasManager.Instance.StageCanvas.Configure();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(true);
        StageManager.Instance.Enter();
        await CanvasManager.Instance.Curtain.PlayHideAnimation();
    }

    public override async Task Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.Curtain.PlayShowAnimation();
        await StageManager.Instance.Exit();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(false);
        AppManager.Instance.StageManager.gameObject.SetActive(false);
    }
}
