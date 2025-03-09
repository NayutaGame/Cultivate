
using System;
using UnityEngine;

[Serializable]
public class BattleReport : TestReport
{
    [SerializeReference]
    public int StageResultFlag;

    [SerializeReference]
    public RunEntity Enemy;
    
    public BattleReport(RunEntity enemy)
    {
        StageResultFlag = -1;
        Enemy = enemy.Clone();
    }

    public static BattleReport FromEnvironment(RunEnvironment env, RunEntity enemy)
    {
        return new(enemy);
    }

    public override void OnEnter(RunEnvironment env)
    {
        env.CommitBattleNeuron.Add(WriteStageResultFlag);
        base.OnEnter(env);
    }

    public override void OnExit(RunEnvironment env)
    {
        env.CommitBattleNeuron.Remove(WriteStageResultFlag);
        base.OnExit(env);
    }

    private void WriteStageResultFlag(bool isWin)
    {
        StageResultFlag = isWin ? 1 : 2;
    }
}
