using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatDetails
{
    public bool UseAnim;
    public bool FireSignal;
    public RunEntity Home;
    public RunEntity Away;

    public CombatDetails(bool useAnim, bool fireSignal, RunEntity home, RunEntity away)
    {
        UseAnim = useAnim;
        FireSignal = fireSignal;
        Home = home;
        Away = away;
    }
}
