
using System;
using System.Collections.Generic;
using CLLibrary;

public class CardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private Bound _bound;
    public Bound Bound => _bound;

    private Func<List<object>, PanelDescriptor> _confirmOperation;
    public CardPickerPanelDescriptor SetConfirmOperation(Func<List<object>, PanelDescriptor> select)
    {
        _confirmOperation = select;
        return this;
    }

    private RunSkillDescriptor _descriptor;

    public CardPickerPanelDescriptor(string detailedText = null, Bound bound = null,
        Func<List<object>, PanelDescriptor> confirmOperation = null,
        RunSkillDescriptor descriptor = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };
        
        _detailedText = detailedText ?? "请选择卡";
        _bound = bound ?? new Bound(1);
        _confirmOperation = confirmOperation;
        _descriptor = descriptor;
    }

    public bool CanSelect(RunSkill skill)
        => _descriptor?.Contains(skill) ?? skill != null;

    public bool CanSelect(SkillSlot slot)
        => slot.Skill != null && CanSelect(slot.Skill);

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is ConfirmDeckSignal confirmDeckSignal && _confirmOperation != null)
        {
            return _confirmOperation(confirmDeckSignal.Selected);
        }

        return this;
    }

    public static CardPickerPanelDescriptor GetTemplate()
    {
        CardPickerPanelDescriptor template = new CardPickerPanelDescriptor(
            detailedText:       "请提交一张二动牌",
            bound:              new Bound(0, 2),
            descriptor:         new RunSkillDescriptor(skillTypeComposite: SkillType.ErDong));
        
        DialogPanelDescriptor win = new("成功对话。");
        DialogPanelDescriptor lose = new("失败对话。");

        template.SetConfirmOperation(iRunSkillList =>
        {
            if (iRunSkillList.Count == 0)
                return lose;

            foreach (object iSkill in iRunSkillList)
            {
                if (iSkill is RunSkill skill)
                {
                    RunManager.Instance.Environment.Hand.Remove(skill);
                }
                else if (iSkill is SkillSlot slot)
                {
                    slot.Skill = null;
                }
            }

            return win;
        });
        
        return template;
    }
}
