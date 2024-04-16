
using System;
using System.Collections.Generic;
using Range = CLLibrary.Range;

public class CardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private Range _range;
    public Range Range => _range;

    private Func<List<object>, PanelDescriptor> _select;
    public CardPickerPanelDescriptor SetSelect(Func<List<object>, PanelDescriptor> select)
    {
        _select = select;
        return this;
    }

    public SkillDescriptor _skillDescriptor;

    public CardPickerPanelDescriptor(string detailedText = null, Range range = null, Func<List<object>, PanelDescriptor> select = null,
        SkillDescriptor skillDescriptor = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };
        
        _detailedText = detailedText ?? "请选择卡";
        _range = range ?? new Range(1);
        _select = select;
        _skillDescriptor = skillDescriptor;
    }

    public bool CanSelect(EmulatedSkill skill)
    {
        if (_skillDescriptor != null && _skillDescriptor.Pred(skill.GetEntry()))
            return false;
        return skill is RunSkill;
    }

    public bool CanSelect(SkillSlot slot)
        => slot.Skill != null && CanSelect(slot.Skill);

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedIRunSkillsSignal selectedIRunSkillsSignal && _select != null)
        {
            return _select(selectedIRunSkillsSignal.Selected);
        }

        return null;
    }
}
