
public class StageConfig
{
    public bool Animated;
    public bool WriteResult;
    public bool GenerateReport;
    public bool GenerateTimeline;
    public RunEntity Home;
    public RunEntity Away;
    public RunConfig RunConfig;

    private StageConfig(bool animated, bool writeResult, bool generateReport, bool generateTimeline, RunEntity home, RunEntity away, RunConfig runConfig)
    {
        Animated = animated;
        WriteResult = writeResult;
        GenerateReport = generateReport;
        GenerateTimeline = generateTimeline;
        Home = home;
        Away = away;
        RunConfig = runConfig;
    }

    public static StageConfig ForCombat(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(true, true, false, false, home, away, runConfig);

    public static StageConfig ForEditor(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(true, false, false, false, home, away, runConfig);

    public static StageConfig ForSimulate(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(false, false, false, false, home, away, runConfig);

    public static StageConfig ForTimeline(RunEntity home, RunEntity away, RunConfig runConfig)
        => new(false, false, false, true, home, away, runConfig);
}
