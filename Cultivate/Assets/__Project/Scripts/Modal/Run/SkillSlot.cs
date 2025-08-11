
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillSlot : ISerializationCallbackReceiver, AnnotatableSkill
{
    [NonSerialized] public Neuron ChangedNeuron = new();
    [NonSerialized] public PlacedSkill PlacedSkill;

    [NonSerialized] public CostDescription ActualCostDescription;
    [NonSerialized] public Description ActualDescription;
    
    [SerializeField] private int _index;
    [SerializeField] private bool _hidden;
    [SerializeReference] private RunSkill _skill;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Skill",                      thisObject => ((SkillSlot)thisObject)._skill },
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
    };
    public object Get(string s) => Accessor[s](this);
    public SkillSlot(int index)
    {
        _index = index;
        _hidden = true;
    }
    
    public int GetIndex() => _index;
    public bool Hidden
    {
        get => _hidden;
        set
        {
            _hidden = value;
            if (_hidden) Skill = null;
        }
    }
    public RunSkill Skill
    {
        get => _skill;
        set
        {
            _skill?.SetSkillSlot(null);
            _skill = value?.Clone();
            _skill?.SetSkillSlot(this);
            ChangedNeuron.Invoke();
        }
    }
    
    public bool IsOccupied()
        => !_hidden && _skill != null;

    public void ClearResults()
    {
        ActualCostDescription = null;
        ActualDescription = null;
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        ChangedNeuron = new();
    }
    
    public DeckIndex ToDeckIndex()
        => DeckIndex.FromField(_index);

    public bool CanShowAnnotation()
        => _skill != null;
    
    public JingJie GetJingJie() => _skill.GetJingJie();
    public JingJie GetLowestJingJie() => _skill.GetLowestJingJie();
    public JingJie GetHighestJingJie() => _skill.GetHighestJingJie();
    public Sprite GetSprite() => _skill.GetSprite();
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie) => _skill.GetLiteralCostDescription(showingJingJie);
    public string GetName() => _skill.GetName();
    public Description GetDescription(JingJie showingJingJie) => _skill.GetDescription(showingJingJie);
    public TagComposite GetTagComposite() => _skill?.GetTagComposite();
    public PackEntry GetPackEntry() => _skill?.GetPackEntry();

    public Sprite GetJingJieSprite(JingJie showingJingJie) => _skill.GetJingJieSprite(showingJingJie);
}
