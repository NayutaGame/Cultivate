
using System;
using Cysharp.Threading.Tasks;

public class StageKernel
{
    private async UniTask<int> DefaultCommitProcedure(StageEnvironment env, int turn, int whosTurn, bool forced)
    {
        CommitDetails d = new CommitDetails(env.Entities[whosTurn]);
        
        await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

        if (forced)
        {
            d.Flag = env.Entities[0].Hp >= env.Entities[1].Hp ? 1 : 2;
        }
        else
        {
            if (d.Cancel)
                return 0;
        
            if (whosTurn == 0)
            {
                if (env.Entities[whosTurn].Hp <= 0)
                    d.Flag = 2;
                if (env.Entities[1 - whosTurn].Hp <= 0)
                    d.Flag = 1;
            }
            else
            {
                if (env.Entities[whosTurn].Hp <= 0)
                    d.Flag = 1;
                if (env.Entities[1 - whosTurn].Hp <= 0)
                    d.Flag = 2;
            }
        }
        
        await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

        if (d.Flag == 0)
            return d.Flag;
        
        env.Result.Flag = d.Flag;
        env.Result.HomeLeftHp = env.Entities[0].Hp;
        env.Result.AwayLeftHp = env.Entities[1].Hp;
        env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
        return d.Flag;
    }

    private Func<StageEnvironment, int, int, bool, UniTask<int>> _commitProcedure;

    public async UniTask<int> CommitProcedure(StageEnvironment env, int turn, int whosTurn, bool forced)
        => await _commitProcedure(env, turn, whosTurn, forced);
    
    public StageKernel(Func<StageEnvironment, int, int, bool, UniTask<int>> commitProcedure = null)
    {
        _commitProcedure = commitProcedure ?? DefaultCommitProcedure;
    }

    public static StageKernel Default() => new();
}
