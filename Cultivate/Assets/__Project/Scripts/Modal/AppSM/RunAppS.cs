
using System.Threading.Tasks;
using DG.Tweening;

public class RunAppS : AppS
{
    public override async Task Enter(NavigateDetails d)
    {
        await base.Enter(d);
        RunManager.Instance.Enter();
        CanvasManager.Instance.RunCanvas.Configure();
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
    }

    public override async Task Exit(NavigateDetails d)
    {
        await base.Exit(d);
        CanvasManager.Instance.CurtainShow().SetAutoKill().Restart();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        CanvasManager.Instance.RunCanvas.NodeLayer.DisableCurrentPanel();
        RunManager.Instance.Exit();
    }

    public override async Task CEnter(NavigateDetails d)
    {
        await base.CEnter(d);
        CanvasManager.Instance.CurtainShow().SetAutoKill().Restart();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
    }

    public override async Task CExit(NavigateDetails d)
    {
        await base.CExit(d);
        RunManager.Instance.CExit();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.CurtainHide().SetAutoKill().Restart();
    }
}
