
using System;
using System.Collections.Generic;
using CLLibrary;

public class CardPickerCell : Cell
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

    private Func<List<DeckIndex>, Cell> _confirmOperation;
    public CardPickerCell SetConfirmOperation(Func<List<DeckIndex>, Cell> select)
    {
        _confirmOperation = select;
        return this;
    }

    private RunSkillDescriptor _descriptor;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((CardPickerCell)thisObject).GetGuideDescriptor() },
    };
    public override object Get(string s) => Accessor[s](this);
    public CardPickerCell(
        string titleText = null,
        string detailedText = null,
        Bound? bound = null,
        Func<List<DeckIndex>, Cell> confirmOperation = null,
        RunSkillDescriptor descriptor = null)
    {
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

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ConfirmDeckSignal confirmDeckSignal && _confirmOperation != null)
        {
            return _confirmOperation(confirmDeckSignal.Indices);
        }

        return this;
    }

    public static CardPickerCell GetTemplate()
    {
        CardPickerCell template = new CardPickerCell(
            titleText:          "选择",
            detailedText:       "请选择一张牌",
            bound:              new Bound(0, 2),
            descriptor:         new RunSkillDescriptor(tagComposite: TagCategory.Swift));
        
        DialogCell win = new(
            titleText: "成功",
            detailedText: "成功对话。");
        DialogCell lose = new(
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
