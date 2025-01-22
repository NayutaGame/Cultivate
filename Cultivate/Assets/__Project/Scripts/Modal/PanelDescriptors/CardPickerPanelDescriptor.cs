
using System;
using System.Collections.Generic;
using CLLibrary;

public class CardPickerPanelDescriptor : PanelDescriptor
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    
    private string _detailedText;
    public string GetDetailedText(int count)
        => $"{_detailedText}\n可以点击选择 {Bound.Start} ~ {Bound.End - 1} 张卡\n已选   {count}   张";

    private Bound _bound;
    public Bound Bound => _bound;
    public bool HasSpace(int occupied)
        => _bound.End - 1 > occupied;

    private Func<List<DeckIndex>, PanelDescriptor> _confirmOperation;
    public CardPickerPanelDescriptor SetConfirmOperation(Func<List<DeckIndex>, PanelDescriptor> select)
    {
        _confirmOperation = select;
        return this;
    }

    private RunSkillDescriptor _descriptor;

    public CardPickerPanelDescriptor(
        string titleText = null,
        string detailedText = null,
        Bound? bound = null,
        Func<List<DeckIndex>, PanelDescriptor> confirmOperation = null,
        RunSkillDescriptor descriptor = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };

        _titleText = titleText ?? "选择";
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
            return _confirmOperation(confirmDeckSignal.Indices);
        }

        return this;
    }

    public static CardPickerPanelDescriptor GetTemplate()
    {
        CardPickerPanelDescriptor template = new CardPickerPanelDescriptor(
            titleText:          "选择",
            detailedText:       "请选择一张牌",
            bound:              new Bound(0, 2),
            descriptor:         new RunSkillDescriptor(skillTypeComposite: SkillType.Swift));
        
        DialogPanelDescriptor win = new(
            titleText: "成功",
            detailedText: "成功对话。");
        DialogPanelDescriptor lose = new(
            titleText: "失败",
            detailedText: "失败对话。");

        template.SetConfirmOperation(indices =>
        {
            if (indices.Count == 0)
                return lose;

            indices.Do(RunManager.Instance.Environment.RemoveSkillProcedure);
            return win;
        });
        
        return template;
    }
}
