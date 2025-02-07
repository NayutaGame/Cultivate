
using System;
using Cysharp.Threading.Tasks;

public class StageKernel
{
    private async UniTask<int> DefaultCommitProcedure(StageCommitDetails d)
    {
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

        if (d.Forced)
        {
            d.Flag = (d.Env.Home.Hp >= d.Env.Away.Hp) ? 1 : 2;
        }
        else
        {
            if (d.Cancel)
                return 0;

            bool homeDead = d.Env.Home.Hp <= 0;
            bool awayDead = d.Env.Away.Hp <= 0;
            
            if (awayDead)
            {
                d.Flag = 1;
            }
            else if (homeDead)
            {
                d.Flag = 2;
            }
        }
        
        await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

        if (d.Flag == 0)
            return d.Flag;
        
        d.Env.RecordResult(d.Flag);
        
        return d.Flag;
    }

    private Func<StageCommitDetails, UniTask<int>> _commitProcedure;

    public async UniTask<int> CommitProcedure(StageEnvironment env, int turn, int whosTurn, bool forced)
        => await _commitProcedure(new(env, turn, whosTurn, forced));
    
    public StageKernel(Func<StageCommitDetails, UniTask<int>> commitProcedure = null)
    {
        _commitProcedure = commitProcedure ?? DefaultCommitProcedure;
    }

    public static StageKernel Default() => new();
}
