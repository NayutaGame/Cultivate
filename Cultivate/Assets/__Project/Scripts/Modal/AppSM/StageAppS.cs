
using Cysharp.Threading.Tasks;

public class StageAppS : AppS
{
    private StageConfig _config;

    public StageAppS(StageConfig config)
    {
        _config = config;
    }

    public override async UniTask Enter(NavigateDetails d, Config config)
    {
        await base.Enter(d, config);

        AppManager.Instance.StageManager.gameObject.SetActive(true);
        
        StageManager.Instance.SetEnvironmentFromConfig(_config);
        StageManager.Instance.SetAwayFromRunEntity(_config.Away);
        
        CanvasManager.Instance.StageCanvas.Configure();
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(true);
        CanvasManager.Instance.StageCanvas.InitialSetup();
        StageManager.Instance.Enter();
        await CanvasManager.Instance.Curtain.Animator.SetStateAsync(0);
    }

    public override async UniTask<Result> Exit(NavigateDetails d)
    {
        await base.Exit(d);
        await CanvasManager.Instance.Curtain.Animator.SetStateAsync(1);
        CanvasManager.Instance.StageCanvas.gameObject.SetActive(false);
        AppManager.Instance.StageManager.gameObject.SetActive(false);
        await StageManager.Instance.Exit();
        return new();
    }
}
