
using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class RunSkill : ISerializationCallbackReceiver, AnnotatableSkill
{
    [SerializeReference] private SkillSlot _skillSlot;
    [SerializeField] private SkillEntry _entry;
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
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
    };
    public object Get(string s) => Accessor[s](this);
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

    public static RunSkill FromEntryId(string id)
        => FromEntry(Encyclopedia.SkillCategory.FromId(id));

    public static RunSkill FromEntry(SkillEntry entry)
        => FromEntryJingJie(entry, entry.LowestJingJie);

    public RunSkill Clone()
        => new(_entry, _jingJie, _runUsedTimes, _runEquippedTimes, _appliedMutators);

    public Sprite GetSprite()
        => _entry.GetSprite();

    public WuXing GetWuXing()
        => _entry.WuXing;

    public string GetName()
        => _entry.GetName();

    public TagComposite GetTagComposite()
        => _entry.GetTagComposite();

    public PackEntry GetPackEntry()
        => _entry.GetPackEntry();

    public string GetTrivia()
        => _entry.GetTrivia();

    public JingJie GetJingJie()
        => _jingJie;

    public JingJie GetLowestJingJie()
        => _entry.LowestJingJie;

    public JingJie GetHighestJingJie()
        => _entry.HighestJingJie;

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
    {
        if (_jingJie != showingJingJie)
            return GetEntry().GetLiteralCostDescription(showingJingJie);
        
        CostDescription actualCostDescription = _skillSlot?.ActualCostDescription;
        if (actualCostDescription != null)
            return actualCostDescription;
            
        return SkillDefinition.GetLiteralCostDescription();
    }

    public Description GetDescription(JingJie showingJingJie)
    {
        if (GetEntry().GetName() == "金刃")
            ;
        
        if (_jingJie != showingJingJie)
            return GetEntry().GetDescription(showingJingJie);
        
        Description actualDescription = _skillSlot?.ActualDescription;
        if (actualDescription != null)
            return actualDescription;

        return SkillDefinition.GetLiteralDescription();
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
        _skillDefinition = null;
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
        if (dj != Dj)
            return GetEntry().GetSkillDefinitionFromDj(dj);
        return SkillDefinition;
        
    }

    public SkillDefinition GetUnmutatedSkillDefinitionFromDj(int dj)
        => GetEntry().GetSkillDefinitionFromDj(dj);

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.SkillCategory.FromId(_entry.GetId());
        _jingJie = string.IsNullOrEmpty(_jingJie.GetId()) ? null : Encyclopedia.JingJieCategory.FromId(_jingJie.GetId());

        if (_appliedMutators != null)
        {
            for (int i = 0; i < _appliedMutators.Count; i++)
            {
                _appliedMutators[i] = string.IsNullOrEmpty(_appliedMutators[i].GetId()) ? null : Encyclopedia.SkillCategory.FromId(_appliedMutators[i].GetId());
            }
        }
    }

    public bool CanShowAnnotation()
        => true;
}
