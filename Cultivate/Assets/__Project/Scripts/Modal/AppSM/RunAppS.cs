
using System.Threading.Tasks;

public class RunAppS : AppS
{
    public override async Task Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);

        RunConfig runConfig = config as RunConfig;
        RunManager.Instance.SetEnvironmentFromConfig(runConfig);

        RunEnvironment runEnv = RunManager.Instance.Environment;
        RunCanvas runCanvas = CanvasManager.Instance.RunCanvas;
        
        PanelDescriptor panelDescriptor = runEnv.Map.Panel;
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        
        runCanvas.Configure();
        runCanvas.SetPanelS(panelS);
        runCanvas.TopBar.Refresh();
        await CanvasManager.Instance.Curtain.SetStateAsync(0);
    }

    public override async Task<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.Curtain.SetStateAsync(1);
        
        RunEnvironment runEnv = RunManager.Instance.Environment;
        RunCanvas runCanvas = CanvasManager.Instance.RunCanvas;
        
        runCanvas.SetPanelS(PanelS.FromHide());
        
        RunResult result = runEnv.Result;
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
