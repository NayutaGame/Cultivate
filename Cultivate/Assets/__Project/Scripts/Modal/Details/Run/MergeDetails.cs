
using System;
using System.Collections.Generic;

public class MergeDetails : RunClosureDetails
{
    public enum MergeState
    {
        Continue,
        Cancel,
        Success,
    }
    
    public RunSkill Lhs;
    public RunSkill Rhs;

    public JingJie PlayerJingJie;
    public bool IsDryRun;
    public RunSkill Src;
    public RunSkill Tgt;

    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;

    public MergeState State;
    public MergePreresult MergeTarget;
    public List<Action> SideEffects;
    
    public MergeDetails(RunSkill lhs, RunSkill rhs)
    {
        Lhs = lhs;
        Rhs = rhs;

        FromDeckIndex = lhs.ToDeckIndex();
        ToDeckIndex = rhs.ToDeckIndex();

        State = MergeState.Continue;
        MergeTarget = null;
        SideEffects = new();
    }

    public void AddSideEffect(Action sideEffect)
    {
        if (IsDryRun)
            return;

        SideEffects.Add(sideEffect);
    }

    public void ExecuteSideEffects()
    {
        foreach (Action sideEffect in SideEffects)
            sideEffect();
        SideEffects.Clear();
    }
}
