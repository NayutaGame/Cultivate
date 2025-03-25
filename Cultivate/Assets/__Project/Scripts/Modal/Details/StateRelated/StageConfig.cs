
public class StageConfig
{
    public bool Animated;
    public bool WriteResult;
    public bool GenerateReport;
    public bool GenerateTimeline;
    public RunEntity Home;
    public RunEntity Away;
    public RunConfig RunConfig;
    public StageKernel Kernel;
    public bool EffectAchievements;

    private StageConfig(bool animated, bool writeResult, bool generateReport, bool generateTimeline, RunEntity home, RunEntity away, RunConfig runConfig, bool effectAchievements, StageKernel kernel)
    {
        Animated = animated;
        WriteResult = writeResult;
        GenerateReport = generateReport;
        GenerateTimeline = generateTimeline;
        Home = home;
        Away = away;
        RunConfig = runConfig;
        EffectAchievements = effectAchievements;
        Kernel = kernel ?? StageKernel.Default();
    }

    public static StageConfig ForCombat(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(true, true, false, false, home, away, runConfig, true, null);

    public static StageConfig ForSkipCombat(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(false, true, false, false, home, away, runConfig, true, null);

    public static StageConfig ForEditor(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(true, false, false, false, home, away, runConfig, false, null);

    public static StageConfig ForSimulate(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(false, false, false, false, home, away, runConfig, false, null);

    public static StageConfig ForTimeline(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(false, false, false, true, home, away, runConfig, false, null);

    public static StageConfig ForPuzzle(RunEntity home, RunEntity away, StageKernel kernel)
        => new(false, false, false, false, home, away, null, false, kernel);
}
