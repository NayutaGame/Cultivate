
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RunReport
{
    [SerializeReference]
    public RunConfig RunConfig;
    
    [SerializeReference]
    public List<TestReport> Reports;

    public TestReport GetCurrReport()
    {
        if (Reports.Count > 0)
            return Reports[^1];
        return null;
    }

    public RunReport(RunEnvironment env)
    {
        RunConfig = env.GetRunConfig();
        Reports = new();
    }

    public void AppendReport(TestReport report)
    {
        Reports.Add(report);
    }

    public void CopyRunReportToClipboard()
    {
        // finalize run report
        string json = JsonUtility.ToJson(this, false);
        GUIUtility.systemCopyBuffer = json;
    }
}
