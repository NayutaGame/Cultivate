
public class StageConfig
{
    public RunEntity Home;
    public RunEntity Away;
    public RunConfig RunConfig;
    public StageKernel Kernel;
    public bool Animated;
    public bool WillEffectResult;
    public bool WriteResult;
    public bool GenerateReport;
    public bool GenerateTimeline;

    private StageConfig(RunEntity home, RunEntity away, RunConfig runConfig, StageKernel kernel,
        bool animated,
        bool willEffectResult,
        bool writeResult,
        bool generateReport,
        bool generateTimeline)
    {
        Home = home;
        Away = away;
        RunConfig = runConfig;
        Kernel = kernel ?? StageKernel.Default();
        Animated = animated;
        WillEffectResult = willEffectResult;
        WriteResult = writeResult;
        GenerateReport = generateReport;
        GenerateTimeline = generateTimeline;
    }
    
    public static StageConfig ForCombatNormal(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, true, true, false, false, false);
    
    public static StageConfig ForCombatOnlyAnimation(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, true, false, false, false, false);

    public static StageConfig ForCombatOnlyResult(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, false, false, true, false, false);

    public static StageConfig ForSimulate(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, false, false, false, false, false);

    public static StageConfig ForEditor(RunEntity home, RunEntity away, RunConfig runConfig)
        => ForCombatOnlyAnimation(home, away, runConfig);

    public static StageConfig ForTimeline(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, false, false, false, false, true);

    public static StageConfig ForPuzzle(RunEntity home, RunEntity away, StageKernel kernel)
        => new(home, away, null, kernel, false, false, false, false, false);
}
