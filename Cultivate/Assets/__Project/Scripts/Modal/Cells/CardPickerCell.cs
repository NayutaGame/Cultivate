
using System;
using System.Collections.Generic;
using CLLibrary;

public class CardPickerCell : Cell
{
    private string _titleText;
    private string _detailedText;
    private ListModel<RequirementSlot> _requirementSlotList;
    
    private Func<CardPickerCell, Cell> _submitOperation;
    public CardPickerCell SetSubmitOperation(Func<CardPickerCell, Cell> submitOperation)
    {
        _submitOperation = submitOperation;
        return this;
    }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((CardPickerCell)thisObject).GetGuideDescriptor() },
        { "Requirements",               thisObject => ((CardPickerCell)thisObject)._requirementSlotList },
    };
    public override object Get(string s) => Accessor[s](this);
    public CardPickerCell(
        string titleText = null,
        string detailedText = null,
        RunSkillDescriptorListModel descriptor = null,
        Func<CardPickerCell, Cell> submitOperation = null)
    {
        _titleText = titleText ?? "选择";
        _detailedText = detailedText ?? "请选择卡";
        _submitOperation = submitOperation;
        _requirementSlotList = RequirementSlotListFromRunSkillDescriptorListModel(descriptor ?? RunSkillDescriptorListModel.Default());
    }
    
    public string GetTitleText() => _titleText;
    public string GetDetailedText() => _detailedText;
    public ListModel<RequirementSlot> RequirementSlotList => _requirementSlotList;

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ConfirmDeckSignal confirmDeckSignal)
        {
            return _submitOperation(this);
        }

        return this;
    }

    public override void DefaultExit(Cell cell)
    {
        base.DefaultExit(cell);

        _requirementSlotList = null;
    }

    public static CardPickerCell GetTemplate()
    {
        CardPickerCell template = new CardPickerCell(
            titleText:          "选择",
            detailedText:       "请选择一张牌",
            descriptor:         RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(RunSkillDescriptor.FromTagComposite(TagCategory.Swift), 1));
        
        DialogCell win = new(
            titleText: "成功",
            detailedText: "成功对话。");
        DialogCell lose = new(
            titleText: "失败",
            detailedText: "失败对话。");

        template.SetSubmitOperation(cardPickerCell =>
        {
            bool fulfilled = cardPickerCell.AllFulfilled();
            if (!fulfilled)
            {
                cardPickerCell.WithdrawAll();
                return lose;
            }

            return win;
        });
        
        return template;
    }

    private static ListModel<RequirementSlot> RequirementSlotListFromRunSkillDescriptorListModel(RunSkillDescriptorListModel descriptors)
    {
        ListModel<RequirementSlot> requirementSlotList = new();
        for (int i = 0; i < descriptors.Count(); i++)
        {
            requirementSlotList.Add(new(i, descriptors[i]));
        }
        return requirementSlotList;
    }

    public bool AllFulfilled()
        => null == RequirementSlotList.First(requirementSlot => !requirementSlot.IsFulfilled());
    
    public bool AnyFulfilled()
        => null != RequirementSlotList.First(requirementSlot => requirementSlot.IsFulfilled());

    public void WithdrawAll()
    {
        RequirementSlotList.Do(requirementSlot =>
        {
            if (requirementSlot.Skill != null)
            {
                RunManager.Instance.Environment.WithdrawToHandProcedure(WithdrawToHandDetails.FromSlot(requirementSlot));
            }
        });
    }
}
