
using System.Threading.Tasks;

public class TitleAppS : AppS
{
    public override async Task Enter()
    {
        await base.Enter();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(true);
    }

    public override async Task CEnter()
    {
        await base.CEnter();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(false);
    }

    public override async Task CExit()
    {
        await base.CExit();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(true);
    }

    public override async Task Exit()
    {
        await base.Exit();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(false);
    }
}
