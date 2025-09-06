
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillSlot : ISerializationCallbackReceiver, AnnotatableSkill
{
    [NonSerialized] public Neuron ChangedNeuron = new();

    [NonSerialized] private PlacedSkill _placeSkill;
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
    
    public PlacedSkill PlacedSkill
    {
        get => _placeSkill;
        set => _placeSkill = value;
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
            _skill = value?.Clone();
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
    public Sprite GetCardIllustration() => _skill.GetCardIllustration();
    public Sprite GetBarIllustration() => _skill.GetBarIllustration();
    
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
    {
        if (_skill.GetJingJie() != showingJingJie)
            return _skill.GetEntry().GetLiteralCostDescription(showingJingJie);

        if (ActualCostDescription != null)
            return ActualCostDescription;

        return _skill.SkillDefinition.GetLiteralCostDescription();
    }
    
    public Description GetDescription(JingJie showingJingJie)
    {
        if (_skill.GetJingJie() != showingJingJie)
            return _skill.GetEntry().GetDescription(showingJingJie);

        if (ActualDescription != null)
            return ActualDescription;

        return _skill.SkillDefinition.GetLiteralDescription();
    }

    public string GetName() => _skill.GetName();
    public string GetTrivia() => _skill.GetTrivia();

    public TagComposite GetTagComposite() => _skill?.GetTagComposite();
    public PackEntry GetPackEntry() => _skill?.GetPackEntry();

    public Sprite GetJingJieSprite(JingJie showingJingJie) => _skill.GetJingJieSprite(showingJingJie);
}
