using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatDetails
{
    public bool UseAnim;
    public RunEntity Home;
    public RunEntity Away;

    public CombatDetails(bool useAnim, RunEntity home, RunEntity away)
    {
        UseAnim = useAnim;
        Home = home;
        Away = away;
    }
}
