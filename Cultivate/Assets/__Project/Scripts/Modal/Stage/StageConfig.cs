
public class StageConfig
{
    public bool Animated;
    public bool WriteResult;
    public bool GenerateReport;
    public bool GenerateTimeline;
    public RunEntity Home;
    public RunEntity Away;
    public RunConfig RunConfig;

    public StageConfig(bool animated, bool writeResult, bool generateReport, bool generateTimeline, RunEntity home, RunEntity away, RunConfig runConfig)
    {
        Animated = animated;
        WriteResult = writeResult;
        GenerateReport = generateReport;
        GenerateTimeline = generateTimeline;
        Home = home;
        Away = away;
        RunConfig = runConfig;
    }
}
