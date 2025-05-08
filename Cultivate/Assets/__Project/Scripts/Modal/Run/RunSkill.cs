
using System;
using System.Collections.Generic;
using Sirenix.Utilities;
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

    [SerializeField] private List<SkillEntry> _appliedMutators;
    [NonSerialized] private SkillDefinition _skillDefinition;
    
    public SkillEntry GetEntry() => _entry;
    public void SetEntry(SkillEntry entry) => _entry = entry;
    public SkillSlot GetSkillSlot() => _skillSlot;
    public void SetSkillSlot(SkillSlot value) => _skillSlot = value;
    public JingJie JingJie
    {
        get => _jingJie;
        set => _jingJie = Mathf.Clamp(value, GetEntry().LowestJingJie, GetEntry().HighestJingJie);
    }
    public int Dj
        => GetJingJie() - _entry.LowestJingJie;
    public int GetRunUsedTimes() => _runUsedTimes;
    public void SetRunUsedTimes(int value) => _runEquippedTimes = value;
    public int GetRunEquippedTimes() => _runEquippedTimes;
    public void SetRunEquippedTimes(int value) => _runEquippedTimes = value;
    public bool Borrowed
    {
        get => _borrowed;
        set => _borrowed = value;
    }

    private RunSkill(SkillEntry entry, JingJie jingJie, int runUsedTimes, int runEquippedTimes, List<SkillEntry> appliedMutators)
    {
        _entry = entry;
        _jingJie = jingJie;
        _runUsedTimes = runUsedTimes;
        _runEquippedTimes = runEquippedTimes;
        _appliedMutators = appliedMutators ?? new();
    }

    public static RunSkill FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry, Mathf.Clamp(jingJie, entry.LowestJingJie, entry.HighestJingJie), 0, 0, null);

    public static RunSkill FromEntry(SkillEntry entry)
        => FromEntryJingJie(entry, entry.LowestJingJie);

    public RunSkill Clone()
        => new(_entry, _jingJie, _runUsedTimes, _runEquippedTimes, _appliedMutators);

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

    public string GetCascadeAnnotated()
        => _entry.GetCascadeAnnotated();

    public string GetTrivia()
        => _entry.GetTrivia();

    public JingJie GetJingJie()
        => _jingJie;

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
    {
        CostDescription actualCostDescription = _skillSlot?.ActualCostDescription;
        if (actualCostDescription != null)
            return actualCostDescription;

        if (_jingJie == showingJingJie)
            return SkillDefinition.GetLiteralCostDescription();
        
        return GetEntry().GetLiteralCostDescription(showingJingJie);
    }

    public string GetHighlight(JingJie showingJingJie)
    {
        string actualDescription = _skillSlot?.ActualDescription;
        if (actualDescription != null)
            return actualDescription;

        if (_jingJie == showingJingJie)
            return SkillDefinition.GetLiteralDescriptionHighlighted();
        
        return GetEntry().GetHighlight(showingJingJie);
    }

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => _entry.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => _entry.NextJingJie(showingJingJie);

    public override string ToString()
        => $"[{GetJingJie()}]{GetEntry().GetName()}";

    public DeckIndex ToDeckIndex()
        => RunManager.Instance.Environment.DeckIndexFromSkill(this).Value;

    public bool CanMutate(RunSkill mutator)
    {
        MutateDefinition[] mutateDefinitions = mutator.GetEntry().GetMutateDefinitions();
        SkillDefinition skillDefinition = GetSkillDefinitionFromDj(Dj);
        return skillDefinition.CanMutate(mutateDefinitions);
    }

    public void Mutate(RunSkill mutator)
    {
        SkillEntry entry = mutator.GetEntry();
        _appliedMutators.Add(entry);
    }

    public SkillDefinition SkillDefinition
    {
        get
        {
            if (_skillDefinition != null)
                return _skillDefinition;

            if (!_appliedMutators.IsNullOrEmpty())
            {
                SkillDefinition skillDefinition = GetUnmutatedSkillDefinitionFromDj(Dj);
                _skillDefinition = SkillDefinition.FromMutate(skillDefinition, _appliedMutators);
                return _skillDefinition;
            }

            return GetEntry().GetSkillDefinitionFromDj(Dj);
        }
    }

    public SkillDefinition GetSkillDefinitionFromDj(int dj)
    {
        if (dj == Dj)
            return _skillDefinition ?? GetEntry().GetSkillDefinitionFromDj(dj);
        return GetEntry().GetSkillDefinitionFromDj(dj);
    }

    public SkillDefinition GetUnmutatedSkillDefinitionFromDj(int dj)
        => GetEntry().GetSkillDefinitionFromDj(dj);
}
