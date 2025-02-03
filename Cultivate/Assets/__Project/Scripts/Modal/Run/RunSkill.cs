
using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RunSkill : ISkill, ISerializationCallbackReceiver
{
    [SerializeField] private SkillEntry _entry;
    [SerializeReference] private SkillSlot _skillSlot;
    [SerializeField] private JingJie _jingJie;
    [SerializeField] protected int _runUsedTimes;
    [SerializeField] protected int _runEquippedTimes;
    [SerializeField] private bool _borrowed;
    
    public SkillEntry GetEntry() => _entry;
    public void SetEntry(SkillEntry entry) => _entry = entry;
    public SkillSlot GetSkillSlot() => _skillSlot;
    public void SetSkillSlot(SkillSlot value) => _skillSlot = value;
    public JingJie JingJie
    {
        get => _jingJie;
        set => _jingJie = Mathf.Clamp(value, GetEntry().LowestJingJie, GetEntry().HighestJingJie);
    }
    public int GetRunUsedTimes() => _runUsedTimes;
    public void SetRunUsedTimes(int value) => _runEquippedTimes = value;
    public int GetRunEquippedTimes() => _runEquippedTimes;
    public void SetRunEquippedTimes(int value) => _runEquippedTimes = value;
    public bool Borrowed
    {
        get => _borrowed;
        set => _borrowed = value;
    }

    private RunSkill(SkillEntry entry, JingJie jingJie, int runUsedTimes, int runEquippedTimes)
    {
        _entry = entry;
        _jingJie = jingJie;
        _runUsedTimes = runUsedTimes;
        _runEquippedTimes = runEquippedTimes;
    }

    public static RunSkill FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry, Mathf.Clamp(jingJie, entry.LowestJingJie, entry.HighestJingJie), 0, 0);

    public static RunSkill FromEntry(SkillEntry entry)
        => FromEntryJingJie(entry, entry.LowestJingJie);

    public RunSkill Clone()
        => new(_entry, _jingJie, _runUsedTimes, _runEquippedTimes);

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

    public DeckIndex ToDeckIndex()
        => RunManager.Instance.Environment.GetDeckIndexOfSkill(this).Value;
}
