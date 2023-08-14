
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using UnityEngine;

[Serializable]
public class MechComposite : EmulatedSkill, ISkillModel
{
    public static readonly int MAX_CHAIN = 3;
    private static Dictionary<int, SkillEntry> _composeDict = new()
    {
        { 0, null },

        { MechType.Xiang._hash, "醒神香" },
        { MechType.Ren._hash, "飞镖" },
        { MechType.Xia._hash, "铁匣" },
        { MechType.Lun._hash, "滑索" },

        { MechType.Xiang._hash + MechType.Xiang._hash, "还魂香" },
        { MechType.Xiang._hash + MechType.Ren._hash, "净魂刀" },
        { MechType.Xiang._hash + MechType.Xia._hash, "防护罩" },
        { MechType.Xiang._hash + MechType.Lun._hash, "能量饮料" },
        { MechType.Ren._hash + MechType.Ren._hash, "炎铳" },
        { MechType.Ren._hash + MechType.Xia._hash, "机关人偶" },
        { MechType.Ren._hash + MechType.Lun._hash, "铁陀螺" },
        { MechType.Xia._hash + MechType.Xia._hash, "防壁" },
        { MechType.Xia._hash + MechType.Lun._hash, "不倒翁" },
        { MechType.Lun._hash + MechType.Lun._hash, "助推器" },

        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Xiang._hash, "反应堆" },
        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Ren._hash, "烟花" },
        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Xia._hash, "长明灯" },
        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Lun._hash, "大往生香" },
        { MechType.Lun._hash + MechType.Xiang._hash + MechType.Ren._hash, "地府通讯器" },

        { MechType.Ren._hash + MechType.Ren._hash + MechType.Ren._hash, "无人机阵列" },
        { MechType.Ren._hash + MechType.Ren._hash + MechType.Xiang._hash, "弩炮" },
        { MechType.Ren._hash + MechType.Ren._hash + MechType.Xia._hash, "尖刺陷阱" },
        { MechType.Ren._hash + MechType.Ren._hash + MechType.Lun._hash, "暴雨梨花针" },
        { MechType.Xiang._hash + MechType.Ren._hash + MechType.Xia._hash, "炼丹炉" },

        { MechType.Xia._hash + MechType.Xia._hash + MechType.Xia._hash, "浮空艇" },
        { MechType.Xia._hash + MechType.Xia._hash + MechType.Xiang._hash, "动量中和器" },
        { MechType.Xia._hash + MechType.Xia._hash + MechType.Ren._hash, "机关伞" },
        { MechType.Xia._hash + MechType.Xia._hash + MechType.Lun._hash, "一轮马" },
        { MechType.Ren._hash + MechType.Xia._hash + MechType.Lun._hash, "外骨骼" },

        { MechType.Lun._hash + MechType.Lun._hash + MechType.Lun._hash, "永动机" },
        { MechType.Lun._hash + MechType.Lun._hash + MechType.Xiang._hash, "火箭靴" },
        { MechType.Lun._hash + MechType.Lun._hash + MechType.Ren._hash, "定龙桩" },
        { MechType.Lun._hash + MechType.Lun._hash + MechType.Xia._hash, "飞行器" },
        { MechType.Xia._hash + MechType.Lun._hash + MechType.Xiang._hash, "时光机" },
    };

    [SerializeField] private SkillSlot _skillSlot;
    public SkillSlot GetSkillSlot() => _skillSlot;
    public void SetSkillSlot(SkillSlot value) => _skillSlot = value;

    [SerializeField]
    private List<MechType> _mechTypes;
    public List<MechType> MechTypes => _mechTypes;

    public MechComposite()
    {
        _mechTypes = new();
    }

    public MechComposite(MechType mechType) : this()
    {
        _mechTypes.Add(mechType);
    }

    public SkillEntry GetEntry()
        => _composeDict[_mechTypes.Map(t => t?._hash ?? 0).Sum()];

    public JingJie GetJingJie()
        => GetEntry().JingJieRange.Start;

    public Sprite GetSprite()
        => GetEntry().Sprite;

    public int GetManaCost()
        => GetEntry().GetManaCost(GetJingJie(), GetJingJie() - GetEntry().JingJieRange.Start, GetSkillSlot()?.IsJiaShi ?? false);

    public int GetChannelTime()
        => GetEntry().GetChannelTime(GetJingJie(), GetJingJie() - GetEntry().JingJieRange.Start, GetSkillSlot()?.IsJiaShi ?? false);

    public string GetName()
        => GetEntry().Name;

    public string GetAnnotatedDescription(string evaluated = null)
        => GetEntry().GetAnnotatedDescription(evaluated ?? GetDescription());

    public SkillTypeComposite GetSkillTypeComposite()
        => GetEntry().SkillTypeComposite;

    public Color GetColor()
        => CanvasManager.Instance.JingJieColors[GetJingJie()];

    public Sprite GetCardFace()
        => GetEntry().CardFace;

    public Sprite GetJingJieSprite()
        => CanvasManager.Instance.JingJieSprites[GetJingJie()];

    public Sprite GetWuXingSprite()
        => CanvasManager.Instance.GetWuXingSprite(GetEntry().WuXing);

    public string GetDescription()
        => GetEntry().Evaluate(GetJingJie(), GetJingJie() - GetEntry().JingJieRange.Start);

    public string GetAnnotationText()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in GetEntry().GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>  {annotation.GetAnnotatedDescription()}\n");

        return sb.ToString();
    }

    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;

    public int GetRunUsedTimes() => 0;
    public void SetRunUsedTimes(int value) { }
    public int GetRunEquippedTimes() => 0;
    public void SetRunEquippedTimes(int value) { }
}
