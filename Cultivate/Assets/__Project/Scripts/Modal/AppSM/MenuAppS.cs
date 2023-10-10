
public class MenuAppS : AppS
{
    public override void Enter()
    {
        base.Enter();
        CanvasManager.Instance.AppCanvas.SettingsPanel.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        CanvasManager.Instance.AppCanvas.SettingsPanel.gameObject.SetActive(false);
    }
}
