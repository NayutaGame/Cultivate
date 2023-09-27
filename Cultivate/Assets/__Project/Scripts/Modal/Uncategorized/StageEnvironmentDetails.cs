
public class StageEnvironmentDetails
{
    public bool Animated;
    public bool WriteResult;
    public bool GenerateReport;
    public bool GenerateTimeline;
    public RunEntity Home;
    public RunEntity Away;

    public StageEnvironmentDetails(bool animated, bool writeResult, bool generateReport, bool generateTimeline, RunEntity home, RunEntity away)
    {
        Animated = animated;
        WriteResult = writeResult;
        GenerateReport = generateReport;
        GenerateTimeline = generateTimeline;
        Home = home;
        Away = away;
    }
}
