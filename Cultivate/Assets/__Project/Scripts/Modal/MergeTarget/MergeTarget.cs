
using System;
using System.Collections.Generic;

public abstract class MergeTarget
{
    public readonly string MergeType;
    public readonly bool Valid;
    public readonly string ErrorMessage;
    public readonly SkillEntry ResultEntry;
    public readonly JingJie ResultJingJie;
    public readonly WuXing ResultWuXing;
    public readonly List<SkillEntry> ResultMutators;
    public readonly Predicate<SkillEntry> Pred;

    public readonly CostDescription CostDescription;
    public readonly Description Description;

    public MergeTarget(
        string mergeType,
        bool valid,
        string errorMessage,
        SkillEntry resultEntry,
        JingJie resultJingJie,
        WuXing resultWuXing,
        List<SkillEntry> oldMutators,
        List<SkillEntry> newMutators,
        Predicate<SkillEntry> pred)
    {
        MergeType = mergeType;
        Valid = valid;
        ErrorMessage = errorMessage;
        ResultEntry = resultEntry;
        ResultJingJie = resultJingJie;
        ResultWuXing = resultWuXing;
        ResultMutators = CalcMutators(oldMutators, newMutators);
        Pred = pred;

        if (ResultEntry != null)
        {
            JingJie jingJie = ResultJingJie ?? ResultEntry.LowestJingJie;
            if (ResultMutators.Count == 0)
            {
                CostDescription = ResultEntry.GetLiteralCostDescription(jingJie);
                Description = ResultEntry.GetDescription(jingJie);
            }
            else
            {
                RunSkill runSkill = RunSkill.FromMutation(ResultEntry, jingJie, ResultMutators);
                CostDescription = runSkill.GetLiteralCostDescription(jingJie);
                Description = runSkill.GetDescription(jingJie);
            }
        }
        else
        {
            CostDescription = CostDescription.Empty;
            Description = "";
        }
    }

    private List<SkillEntry> CalcMutators(List<SkillEntry> oldMutators, List<SkillEntry> newMutators)
    {
        List<SkillEntry> mutators = new();
        
        if (oldMutators != null)
        {
            mutators.AddRange(oldMutators);
        }

        if (newMutators != null)
        {
            mutators.AddRange(newMutators);
        }
        
        return mutators;
    }

    public virtual void Execute(MergeDetails d, RunSkillListModel hand)
    {
        
    }
}
