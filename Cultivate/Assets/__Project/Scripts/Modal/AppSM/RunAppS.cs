
using System.Threading.Tasks;

public class RunAppS : AppS
{
    public override async Task Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);

        RunConfig runConfig = config as RunConfig;
        RunManager.Instance.SetEnvironmentFromConfig(runConfig);
        
        PanelS panelS;
        
        StepItem stepItem = RunManager.Instance.Environment.Map.CurrStepItem;
        NodeListModel nodes = stepItem._nodes;
        if (nodes.Count() == 1)
        {
            RunNode runNode = nodes[0];
            PanelDescriptor panelDescriptor = RunManager.Instance.Environment.MakeChoiceProcedure(runNode);
            panelS = PanelS.FromPanelDescriptorNullMeansMap(panelDescriptor);
        }
        else
        {
            panelS = PanelS.FromMap();
        }
        
        CanvasManager.Instance.RunCanvas.Configure();
        CanvasManager.Instance.RunCanvas.SetPanelS(panelS);
        CanvasManager.Instance.RunCanvas.TopBar.Refresh();
        await CanvasManager.Instance.Curtain.SetStateAsync(0);
    }

    public override async Task<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.Curtain.SetStateAsync(1);
        
        CanvasManager.Instance.RunCanvas.SetPanelS(PanelS.FromHide());
        
        RunResult result = RunManager.Instance.Environment.Result;
        RunManager.Instance.SetEnvironmentToNull();

        return result;
    }

    public override async Task<Config> CEnter(NavigateDetails d)
    {
        await base.CEnter(d);

        if (d.ToState is MenuAppS)
            return new();

        await CanvasManager.Instance.Curtain.SetStateAsync(1);
        CanvasManager.Instance.RunCanvas.gameObject.SetActive(false);
        return new();
    }

    public override async Task CExit(NavigateDetails d, Result result)
    {
        await base.CExit(d, result);

        if (d.FromState is MenuAppS)
            return;

        CanvasManager.Instance.RunCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.RunCanvas.Refresh();
        await CanvasManager.Instance.Curtain.SetStateAsync(0);
    }
}
