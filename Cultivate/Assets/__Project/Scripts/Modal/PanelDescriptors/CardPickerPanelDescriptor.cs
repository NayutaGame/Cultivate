
using System;
using System.Collections.Generic;
using Range = CLLibrary.Range;

public class CardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private Range _range;
    public Range Range => _range;

    public Func<List<object>, PanelDescriptor> _select;

    public DrawSkillDetails _drawSkillDetails;

    public CardPickerPanelDescriptor(string detailedText = null, Range range = null, Func<List<object>, PanelDescriptor> select = null,
        DrawSkillDetails drawSkillDetails = null)
    {
        _detailedText = detailedText ?? "请选择卡";
        _range = range ?? new Range(1);
        _select = select;
        _drawSkillDetails = drawSkillDetails;
    }

    public bool CanSelect(EmulatedSkill skill)
    {
        if (_drawSkillDetails != null && !_drawSkillDetails.CanDraw(skill.GetEntry()))
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
