
using System.Threading.Tasks;

public class RunAppS : AppS
{
    public override async Task Enter(NavigateDetails d)
    {
        await base.Enter(d);
        RunManager.Instance.Enter();
        CanvasManager.Instance.RunCanvas.Configure();
        CanvasManager.Instance.RunCanvas.TopBar.Refresh();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);

        Address address = new Address("Run.Environment.Map.StepItems");
        StepItemListModel stepItems = address.Get<StepItemListModel>();
        NodeListModel nodes = stepItems[0]._nodes;
        if (nodes.Count() == 1)
        {
            RunNode runNode = nodes[0];

            PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.SelectedNode(runNode);
            CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        }
        else
        {
            CanvasManager.Instance.RunCanvas.MapPanel.SetShowing(true);
        }

        CanvasManager.Instance.RunCanvas.MapPanel.SetHideState();
        CanvasManager.Instance.RunCanvas.DeckPanel.SetHideState();
        CanvasManager.Instance.RunCanvas.DeckPanel.DeckOpenZone.gameObject.SetActive(true);
        CanvasManager.Instance.RunCanvas.DeckPanel.DeckCloseZone.gameObject.SetActive(false);

        await CanvasManager.Instance.Curtain.PlayHideAnimation();
    }

    public override async Task Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.Curtain.PlayShowAnimation();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        CanvasManager.Instance.RunCanvas.RunPanelCollection.DisableCurrentPanel();
        RunManager.Instance.Exit();
    }

    public override async Task CEnter(NavigateDetails d)
    {
        await base.CEnter(d);

        if (d.ToState is MenuAppS)
            return;

        await CanvasManager.Instance.Curtain.PlayShowAnimation();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
    }

    public override async Task CExit(NavigateDetails d)
    {
        await base.CExit(d);

        if (d.FromState is MenuAppS)
            return;

        RunManager.Instance.CExit();
        CanvasManager.Instance.RunCanvas.TopBar.Refresh();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        await CanvasManager.Instance.Curtain.PlayHideAnimation();
    }
}
