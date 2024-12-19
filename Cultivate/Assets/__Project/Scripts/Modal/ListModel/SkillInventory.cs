
using System;

[Serializable]
public class SkillInventory : ListModel<RunSkill>
{
    public override void Add(RunSkill item)
    {
        RunSkill clone = item.Clone();
        base.Add(clone);
    }

    private static Comparison<RunSkill> JingJieComparison =
        (lhs, rhs) => lhs.JingJie - rhs.JingJie;

    private static Comparison<RunSkill> WuXingComparison =
        (lhs, rhs) => (lhs.GetEntry().WuXing.HasValue ? lhs.GetEntry().WuXing.Value._index : -1) -
                      (rhs.GetEntry().WuXing.HasValue ? rhs.GetEntry().WuXing.Value._index : -1);

    private static Comparison<RunSkill> IndexComparison =
        (lhs, rhs) => Encyclopedia.SkillCategory.IndexOf(lhs.GetEntry()) - Encyclopedia.SkillCategory.IndexOf(rhs.GetEntry());

    private static Comparison<RunSkill> TypeComparison =
        (lhs, rhs) => lhs.GetSkillTypeComposite().Value - rhs.GetSkillTypeComposite().Value;

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
