
using System.Threading.Tasks;

public class RunAppS : AppS
{
    public override async Task Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);

        RunConfig runConfig = config as RunConfig;
        RunManager.Instance.SetEnvironment(runConfig);

        CanvasManager.Instance.RunCanvas.Configure();
        CanvasManager.Instance.RunCanvas.TopBar.Refresh();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);

        Address address = new Address("Run.Environment.Map.StepItem");
        StepItem stepItem = address.Get<StepItem>();
        NodeListModel nodes = stepItem._nodes;
        if (nodes.Count() == 1)
        {
            RunNode runNode = nodes[0];

            PanelDescriptor panelDescriptor = RunManager.Instance.Environment.MakeChoiceProcedure(runNode);
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

    public override async Task<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.Curtain.PlayShowAnimation();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        CanvasManager.Instance.RunCanvas.RunPanelCollection.DisableCurrentPanel();
        RunResult result = RunManager.Instance.Environment.Result;
        RunManager.Instance.SetEnvironmentToNull();

        return result;
    }

    public override async Task<Config> CEnter(NavigateDetails d)
    {
        await base.CEnter(d);

        if (d.ToState is MenuAppS)
            return new();

        await CanvasManager.Instance.Curtain.PlayShowAnimation();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        return new();
    }

    public override async Task CExit(NavigateDetails d, Result result)
    {
        await base.CExit(d, result);

        if (d.FromState is MenuAppS)
            return;

        CanvasManager.Instance.RunCanvas.Refresh();
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        await CanvasManager.Instance.Curtain.PlayHideAnimation();
    }
}
