
public class StageConfig
{
    public RunEntity Home;
    public RunEntity Away;
    public RunConfig RunConfig;
    public StageKernel Kernel;
    public bool Animated;
    public bool WriteResult;
    public bool GenerateReport;
    public bool GenerateTimeline;
    public bool EffectAchievements;

    private StageConfig(RunEntity home, RunEntity away, RunConfig runConfig, StageKernel kernel,
        bool animated,
        bool writeResult,
        bool generateReport,
        bool generateTimeline,
        bool effectAchievements)
    {
        Home = home;
        Away = away;
        RunConfig = runConfig;
        Kernel = kernel ?? StageKernel.Default();
        Animated = animated;
        WriteResult = writeResult;
        GenerateReport = generateReport;
        GenerateTimeline = generateTimeline;
        EffectAchievements = effectAchievements;
    }

    public static StageConfig ForCombat(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, true, true, false, false, true);

    public static StageConfig ForSkipCombat(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, false, true, false, false, true);

    public static StageConfig ForEditor(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, true, false, false, false, false);

    public static StageConfig ForSimulate(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, false, false, false, false, false);

    public static StageConfig ForTimeline(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(home, away, runConfig, null, false, false, false, true, false);

    public static StageConfig ForPuzzle(RunEntity home, RunEntity away, StageKernel kernel)
        => new(home, away, null, kernel, false, false, false, false, false);
}
