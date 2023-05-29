
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillInventory : Inventory<RunSkill>
{
    public void AddSkill(RunSkill skill)
    {
        RunSkill clone = skill.Clone();
        Add(clone);
    }

    public void AddSkills(List<RunSkill> skills)
    {
        foreach(RunSkill skill in skills)
            AddSkill(skill);
    }

    public void RemoveSkill(RunSkill skill)
    {
        Remove(skill);
    }

    public void ReplaceSkill(RunSkill from, RunSkill to)
    {
        Replace(from, to);
    }

    public void RefreshChip()
    {
        Clear();
        foreach (var chip in Encyclopedia.SkillCategory.Traversal)
            AddSkill(new RunSkill(chip, chip.JingJieRange.Start));
    }

    public bool PickSkill(out RunSkill picked, SkillEntry e, JingJie? jingJie)
    {
        picked = new RunSkill(e, jingJie ?? e.JingJieRange.Start);
        AddSkill(picked);
        return true;
    }

    public bool TryDrawSkill(out RunSkill skill, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
    {
        bool success = RunManager.Instance.SkillPool.TryDrawSkill(out skill, pred, wuXing, jingJie);
        if (!success)
            return false;

        AddSkill(skill);
        return true;
    }

    private static Comparison<RunSkill> JingJieComparison =
        (lhs, rhs) => lhs.JingJie - rhs.JingJie;

    private static Comparison<RunSkill> WuXingComparison =
        (lhs, rhs) => (lhs.Entry.WuXing.HasValue ? lhs.Entry.WuXing.Value._index : -1) -
                      (rhs.Entry.WuXing.HasValue ? rhs.Entry.WuXing.Value._index : -1);

    private static Comparison<RunSkill> IndexComparison =
        (lhs, rhs) => Encyclopedia.SkillCategory.IndexOf(lhs.Entry) - Encyclopedia.SkillCategory.IndexOf(rhs.Entry);

    private static Comparison<RunSkill> TypeComparison =
        (lhs, rhs) => lhs.GetSkillTypeCollection().Value - rhs.GetSkillTypeCollection().Value;

    private static Comparison<RunSkill> InventoryComparison = (lhs, rhs) =>
    {
        int jingJieResult = JingJieComparison(lhs, rhs);
        if (jingJieResult != 0)
            return -jingJieResult;

        int wuXingResult = WuXingComparison(lhs, rhs);
        if (wuXingResult != 0)
            return wuXingResult;

        return IndexComparison(lhs, rhs);
    };

    private static Comparison<RunSkill> WuXingIndexComparison = (lhs, rhs) =>
    {
        int wuXingResult = WuXingComparison(lhs, rhs);
        if (wuXingResult != 0)
            return wuXingResult;

        return IndexComparison(lhs, rhs);
    };

    private static Comparison<RunSkill> TypeIndexComparison = (lhs, rhs) =>
    {
        int typeResult = TypeComparison(lhs, rhs);
        if (typeResult != 0)
            return typeResult;

        return IndexComparison(lhs, rhs);
    };

    private static Comparison<RunSkill>[] Comparisons = new Comparison<RunSkill>[]
    {
        InventoryComparison,
        IndexComparison,
        WuXingIndexComparison,
        TypeIndexComparison,
    };

    public void SortByComparisonId(int i)
    {
        Sort(Comparisons[i]);
    }
}
