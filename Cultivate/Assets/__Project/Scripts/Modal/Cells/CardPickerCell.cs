
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public class CardPickerCell : Cell
{
    private string _titleText;
    private string _detailedText;
    private ListModel<RequirementSlot> _requirementSlotList;
    
    private Func<List<DeckIndex>, Cell> _confirmOperation;
    public CardPickerCell SetConfirmOperation(Func<List<DeckIndex>, Cell> select)
    {
        _confirmOperation = select;
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
        Func<List<DeckIndex>, Cell> confirmOperation = null)
    {
        _titleText = titleText ?? "选择";
        _detailedText = detailedText ?? "请选择卡";
        _confirmOperation = confirmOperation;
        _requirementSlotList = RequirementSlotListFromRunSkillDescriptorListModel(descriptor ?? RunSkillDescriptorListModel.Default());
    }
    
    public string GetTitleText() => _titleText;
    public ListModel<RequirementSlot> RequirementSlotList => _requirementSlotList;

    // public bool CanSelect(RunSkill skill)
    //     => _descriptor?.Contains(skill) ?? skill != null;

    // public bool CanSelect(SkillSlot slot)
    //     => slot.Skill != null && CanSelect(slot.Skill);

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
            descriptor:         RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(RunSkillDescriptor.FromTagComposite(TagCategory.Swift), 1));
        
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

    private static ListModel<RequirementSlot> RequirementSlotListFromRunSkillDescriptorListModel(RunSkillDescriptorListModel descriptors)
    {
        ListModel<RequirementSlot> requirementSlotList = new();
        for (int i = 0; i < descriptors.Count(); i++)
        {
            requirementSlotList.Add(new(i, descriptors[i]));
        }
        return requirementSlotList;
    }
}
