
using UnityEngine;

public class IndirectDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public StageSkill SrcSkill;
    public WuXing? WuXing;
    public bool Recursive;
    public CastResult CastResult;

    /// <summary>
    /// 一次间接攻击行为的细节，例如锋锐，灼烧，会结算目标的护甲，不会继承攻击词条
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="srcSkill">技能来源</param>
    /// <param name="wuXing">攻击特效的五行</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="castResult">结果描述</param>
    /// <param name="induced">是否是间接行为</param>
    public IndirectDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        StageSkill srcSkill,
        WuXing? wuxing,
        bool recursive,
        CastResult castResult,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        SrcSkill = srcSkill;
        WuXing = wuxing;
        Recursive = recursive;
        CastResult = castResult;
        Induced = induced;
    }

    public IndirectDetails Clone() => new(Src, Tgt, _value, SrcSkill, WuXing, Recursive, CastResult, Induced);
}
