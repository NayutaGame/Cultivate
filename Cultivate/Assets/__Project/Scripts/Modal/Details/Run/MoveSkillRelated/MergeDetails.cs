
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
    public MergeTarget MergeTarget;
    public List<Action> SideEffects;
    
    private MergeDetails(RunSkill lhs, RunSkill rhs, JingJie playerJingJie, bool isDryRun)
    {
        Lhs = lhs;
        Rhs = rhs;
        PlayerJingJie = playerJingJie;
        IsDryRun = isDryRun;

        FromDeckIndex = lhs.ToDeckIndex();
        ToDeckIndex = rhs.ToDeckIndex();

        State = MergeState.Continue;
        MergeTarget = null;
        SideEffects = new();
    }

    public static MergeDetails ForDryRun(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(lhs, rhs, playerJingJie, true);

    public static MergeDetails ForActualRun(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(lhs, rhs, playerJingJie, false);

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

    public void CalcMergeTarget()
    {
        ProcessOverridingRules();
        ProcessDefaultRules();
    }

    private void ProcessOverridingRules()
    {
        MergeRule lhsRule = Lhs.GetEntry().OverridingMergeRule;
        MergeRule rhsRule = Rhs.GetEntry().OverridingMergeRule;

        if (lhsRule.Order <= rhsRule.Order)
        {
            Src = Lhs;
            Tgt = Rhs;
            ProcessMergeRule(lhsRule);
            if (State != MergeState.Continue)
                return;
            
            Src = Rhs;
            Tgt = Lhs;
            ProcessMergeRule(rhsRule);
        }
        else
        {
            Src = Rhs;
            Tgt = Lhs;
            ProcessMergeRule(rhsRule);
            if (State != MergeState.Continue)
                return;
                
            Src = Lhs;
            Tgt = Rhs;
            ProcessMergeRule(lhsRule);
        }
    }

    private void ProcessDefaultRules()
    {
        foreach (MergeRule mergeRule in RunManager.Instance.Environment.GetRunConfig().GetDefaultMergeRules())
        {
            if (State != MergeState.Continue)
                break;
            ProcessMergeRule(mergeRule);
        }
    }

    private void ProcessMergeRule(MergeRule mergeRule)
    {
        mergeRule.ProcessMerge(this);
    }
}
