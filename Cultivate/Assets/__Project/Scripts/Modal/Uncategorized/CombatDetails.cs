using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatDetails
{
    public bool Animated;
    public bool WriteResult;
    public RunEntity Home;
    public RunEntity Away;

    public StageReport Report;

    public CombatDetails(bool animated, bool writeResult, RunEntity home, RunEntity away, StageReport report)
    {
        Animated = animated;
        WriteResult = writeResult;
        Home = home;
        Away = away;

        Report = report;
    }
}
