
public class TitleAppS : AppS
{
    public override void Enter()
    {
        base.Enter();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(true);
    }

    public override void CEnter()
    {
        base.CEnter();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(false);
    }

    public override void CExit()
    {
        base.CExit();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        CanvasManager.Instance.AppCanvas.TitlePanel.gameObject.SetActive(false);
    }
}
