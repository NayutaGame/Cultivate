
using System.Threading.Tasks;

public class TitleAppS : AppS
{
    public override async Task Enter(NavigateDetails d)
    {
        await base.Enter(d);
        CanvasManager.Instance.AppCanvas.Configure();
        AudioManager.Play("BGMTitle");
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(true);
    }

    public override async Task CEnter(NavigateDetails d)
    {
        await base.CEnter(d);

        if (d.ToState is MenuAppS)
            return;

        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(false);
    }

    public override async Task CExit(NavigateDetails d)
    {
        await base.CExit(d);

        if (d.FromState is MenuAppS)
            return;

        AudioManager.Play("BGMTitle");
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(true);
    }

    public override async Task Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.AppCanvas.TitlePanel.SetShowing(false);
    }
}
