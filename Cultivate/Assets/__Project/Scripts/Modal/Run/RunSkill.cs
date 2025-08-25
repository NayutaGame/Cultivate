
using System;
using System.Collections.Generic;
using CLLibrary;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class RunSkill : ISerializationCallbackReceiver, AnnotatableSkill, RunClosureListener
{
    [Obsolete] [SerializeReference] private SkillSlot _skillSlot;
    [Obsolete] [SerializeField] private bool _borrowed;
    [Obsolete] [SerializeField] protected int _runUsedTimes;
    [Obsolete] [SerializeField] protected int _runEquippedTimes;
    
    [SerializeField] private SkillEntry _entry;
    [SerializeField] private JingJie _jingJie;
    [SerializeField] private List<SkillEntry> _appliedMutators;
    
    [NonSerialized] private SkillDefinition _skillDefinition;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
    };
    public object Get(string s) => Accessor[s](this);
    private RunSkill(SkillEntry entry, JingJie jingJie, List<SkillEntry> appliedMutators)
    {
        _entry = entry;
        _jingJie = Mathf.Clamp(jingJie, entry.LowestJingJie, entry.HighestJingJie);
        _appliedMutators = appliedMutators ?? new();
    }

    public static RunSkill FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry, jingJie, null);

    public static RunSkill FromEntryId(string id)
        => FromEntry(Encyclopedia.SkillCategory.FromId(id));

    public static RunSkill FromEntry(SkillEntry entry)
        => FromEntryJingJie(entry, entry.LowestJingJie);

    public static RunSkill FromChangeJingJie(RunSkill original, JingJie jingJie)
        => new(original._entry, jingJie, original._appliedMutators);

    public static RunSkill FromMutation(SkillEntry entry, JingJie jingJie, List<SkillEntry> mutators)
        => new(entry, jingJie, mutators);

    public RunSkill Clone()
    {
        List<SkillEntry> appliedMutators = new();
        foreach (SkillEntry mutator in _appliedMutators)
            appliedMutators.Add(mutator);
        return new(_entry, _jingJie, appliedMutators);
    }
    
    public SkillEntry GetEntry() => _entry;
    public int Dj => GetJingJie() - _entry.LowestJingJie;
    public Sprite GetSprite() => _entry.GetSprite();
    public WuXing GetWuXing() => _entry.WuXing;
    public string GetName() => _entry.GetName();
    public TagComposite GetTagComposite() => _entry.GetTagComposite();
    public PackEntry GetPackEntry() => _entry.GetPackEntry();
    public string GetTrivia() => _entry.GetTrivia();
    public JingJie GetJingJie() => _jingJie;
    public JingJie GetLowestJingJie() => _entry.LowestJingJie;
    public JingJie GetHighestJingJie() => _entry.HighestJingJie;
    public Sprite GetJingJieSprite(JingJie showingJingJie) => _entry.GetJingJieSprite(showingJingJie);
    public JingJie NextJingJie(JingJie showingJingJie) => _entry.NextJingJie(showingJingJie);
    public override string ToString() => $"[{GetJingJie()}]{GetEntry().GetName()}";
    public bool CanShowAnnotation() => true;
    public List<SkillEntry> GetMutators() => _appliedMutators;

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
    {
        if (_jingJie != showingJingJie)
            return GetEntry().GetLiteralCostDescription(showingJingJie);
            
        return SkillDefinition.GetLiteralCostDescription();
    }

    public Description GetDescription(JingJie showingJingJie)
    {
        if (_jingJie != showingJingJie)
            return GetEntry().GetDescription(showingJingJie);

        return SkillDefinition.GetLiteralDescription();
    }

    public DeckIndex ToDeckIndex() => RunManager.Instance.Environment.DeckIndexFromSkill(this).Value;

    public bool CanMutate(RunSkill mutator)
    {
        MutateDefinition[] mutateDefinitions = mutator.GetEntry().GetMutateDefinitions();
        SkillDefinition skillDefinition = GetSkillDefinitionFromDj(Dj);
        return skillDefinition.CanMutate(mutateDefinitions);
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
}
