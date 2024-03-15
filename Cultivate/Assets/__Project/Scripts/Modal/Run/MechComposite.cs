
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using UnityEngine;

[Serializable]
public class MechComposite : EmulatedSkill
{
    public static readonly int MAX_CHAIN = 3;
    private static Dictionary<int, SkillEntry> _composeDict = new()
    {
        { 0, null },

        { MechType.Xiang._hash, "0700" },
        { MechType.Ren._hash, "0701" },
        { MechType.Xia._hash, "0702" },
        { MechType.Lun._hash, "0703" },

        { MechType.Xiang._hash + MechType.Xiang._hash, "0704" },
        { MechType.Xiang._hash + MechType.Ren._hash, "0705" },
        { MechType.Xiang._hash + MechType.Xia._hash, "0706" },
        { MechType.Xiang._hash + MechType.Lun._hash, "0707" },
        { MechType.Ren._hash + MechType.Ren._hash, "0708" },
        { MechType.Ren._hash + MechType.Xia._hash, "0709" },
        { MechType.Ren._hash + MechType.Lun._hash, "0710" },
        { MechType.Xia._hash + MechType.Xia._hash, "0711" },
        { MechType.Xia._hash + MechType.Lun._hash, "0712" },
        { MechType.Lun._hash + MechType.Lun._hash, "0713" },

        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Xiang._hash, "0714" },
        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Ren._hash, "0715" },
        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Xia._hash, "0716" },
        { MechType.Xiang._hash + MechType.Xiang._hash + MechType.Lun._hash, "0717" },
        { MechType.Lun._hash + MechType.Xiang._hash + MechType.Ren._hash, "0718" },

        { MechType.Ren._hash + MechType.Ren._hash + MechType.Ren._hash, "0719" },
        { MechType.Ren._hash + MechType.Ren._hash + MechType.Xiang._hash, "0720" },
        { MechType.Ren._hash + MechType.Ren._hash + MechType.Xia._hash, "0721" },
        { MechType.Ren._hash + MechType.Ren._hash + MechType.Lun._hash, "0722" },
        { MechType.Xiang._hash + MechType.Ren._hash + MechType.Xia._hash, "0723" },

        { MechType.Xia._hash + MechType.Xia._hash + MechType.Xia._hash, "0724" },
        { MechType.Xia._hash + MechType.Xia._hash + MechType.Xiang._hash, "0725" },
        { MechType.Xia._hash + MechType.Xia._hash + MechType.Ren._hash, "0726" },
        { MechType.Xia._hash + MechType.Xia._hash + MechType.Lun._hash, "0727" },
        { MechType.Ren._hash + MechType.Xia._hash + MechType.Lun._hash, "0728" },

        { MechType.Lun._hash + MechType.Lun._hash + MechType.Lun._hash, "0729" },
        { MechType.Lun._hash + MechType.Lun._hash + MechType.Xiang._hash, "0730" },
        { MechType.Lun._hash + MechType.Lun._hash + MechType.Ren._hash, "0731" },
        { MechType.Lun._hash + MechType.Lun._hash + MechType.Xia._hash, "0732" },
        { MechType.Xia._hash + MechType.Lun._hash + MechType.Xiang._hash, "0733" },
    };

    [SerializeReference] private SkillSlot _skillSlot;
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
        => GetEntry().LowestJingJie;

    public Sprite GetSprite()
        => GetEntry().Sprite;

    public CostDescription GetCostDescription()
        => GetEntry().GetCostDescription(GetJingJie(), _skillSlot?.CostResult);

    public string GetName()
        => GetEntry().GetName();

    public string GetHighlight()
        => GetEntry().GetHighlight(GetJingJie(), _skillSlot?.CostResult, _skillSlot?.CastResult);

    public string GetExplanation()
        => GetEntry().GetExplanation();

    public string GetTrivia()
        => GetEntry().GetTrivia();

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
        => GetEntry().GetDescription(GetJingJie(), _skillSlot?.CostResult, _skillSlot?.CastResult);

    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;

    public int GetRunUsedTimes() => 0;
    public void SetRunUsedTimes(int value) { }
    public int GetRunEquippedTimes() => 0;
    public void SetRunEquippedTimes(int value) { }
}
