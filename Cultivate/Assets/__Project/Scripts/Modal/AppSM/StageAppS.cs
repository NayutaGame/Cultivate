
using System.Threading.Tasks;
using DG.Tweening;

public class StageAppS : AppS
{
    public override async Task Enter(NavigateDetails d)
    {
        await base.Enter(d);
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        CanvasManager.Instance.StageCanvas.Configure();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(true);
        StageManager.Instance.Enter();
        CanvasManager.Instance.CurtainHide().SetAutoKill().Restart();
    }

    public override async Task Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await StageManager.Instance.Exit();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(false);
        AppManager.Instance.StageManager.gameObject.SetActive(false);
        CanvasManager.Instance.CurtainShow().SetAutoKill().Restart();
    }
}
