
using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RunSkill : ISkill, ISerializationCallbackReceiver
{
    [SerializeField] private SkillSlot _skillSlot;
    public SkillSlot GetSkillSlot() => _skillSlot;
    public void SetSkillSlot(SkillSlot value) => _skillSlot = value;

    [SerializeField] private SkillEntry _entry;
    public SkillEntry GetEntry() => _entry;

    [SerializeField] private JingJie _jingJie;
    public JingJie JingJie
    {
        get => _jingJie;
        set => _jingJie = Mathf.Clamp(value, GetEntry().LowestJingJie, GetEntry().HighestJingJie);
    }

    [SerializeField] protected int _runUsedTimes;
    public int GetRunUsedTimes() => _runUsedTimes;
    public void SetRunUsedTimes(int value) => _runEquippedTimes = value;

    [SerializeField] protected int _runEquippedTimes;
    public int GetRunEquippedTimes() => _runEquippedTimes;
    public void SetRunEquippedTimes(int value) => _runEquippedTimes = value;
    
    [SerializeField] [OptionalField(VersionAdded = 4)] private bool _borrowed;
    public bool Borrowed { get => _borrowed; set => _borrowed = value; }

    public static RunSkill FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry, jingJie);

    public static RunSkill FromEntry(SkillEntry entry)
        => new(entry, entry.LowestJingJie);

    private RunSkill(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry;
        _jingJie = Mathf.Clamp(jingJie, _entry.LowestJingJie, _entry.HighestJingJie);
    }

    private RunSkill(RunSkill prototype)
    {
        _entry = prototype._entry;
        _jingJie = prototype._jingJie;
        _runUsedTimes = prototype._runUsedTimes;
        _runEquippedTimes = prototype._runEquippedTimes;
    }

    public RunSkill Clone()
        => new(this);

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.SkillCategory[_entry.GetId()];
    }

    public bool TryIncreaseJingJie(bool loop = true)
    {
        if (GetEntry().JingJieContains(JingJie + 1))
        {
            JingJie += 1;
            return true;
        }
        
        if (loop)
        {
            JingJie = GetEntry().LowestJingJie;
            return true;
        }

        return false;
    }

    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;

    public Sprite GetSprite()
        => _entry.GetSprite();

    public WuXing? GetWuXing()
        => _entry.WuXing;

    public string GetName()
        => _entry.GetName();

    public SkillTypeComposite GetSkillTypeComposite()
        => _entry.GetSkillTypeComposite();

    public string GetExplanation()
        => _entry.GetExplanation();

    public string GetTrivia()
        => _entry.GetTrivia();

    public JingJie GetJingJie()
        => _jingJie;

    public CostDescription GetCostDescription(JingJie showingJingJie)
        => _jingJie == showingJingJie
            ? GetEntry().GetCostDescription(showingJingJie, _skillSlot?.CostResult)
            : GetEntry().GetCostDescription(showingJingJie);

    public string GetHighlight(JingJie showingJingJie)
        => _jingJie == showingJingJie
            ? GetEntry().GetHighlight(showingJingJie, _skillSlot?.CostResult, _skillSlot?.CastResult)
            : GetEntry().GetHighlight(showingJingJie);

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => _entry.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => _entry.NextJingJie(showingJingJie);

    public override string ToString()
        => $"[{GetJingJie()}]{GetEntry().GetName()}";
}
