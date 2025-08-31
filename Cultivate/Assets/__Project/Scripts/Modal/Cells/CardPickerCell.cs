
using System;
using System.Collections.Generic;
using CLLibrary;

public class CardPickerCell : Cell
{
    private string _titleText;
    private Func<ListModel<RequirementSlot>, string> _getDetailedText;
    private ListModel<RequirementSlot> _requirementSlotList;
    private Func<CardPickerCell, Cell> _submitOperation;

    #region Constructors

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((CardPickerCell)thisObject).GetGuideDescriptor() },
        { "Requirements",               thisObject => ((CardPickerCell)thisObject)._requirementSlotList },
    };
    public override object Get(string s) => Accessor[s](this);
    public CardPickerCell(
        string titleText,
        Func<ListModel<RequirementSlot>, string> getDetailedText,
        RunSkillDescriptorListModel descriptor,
        Func<CardPickerCell, Cell> submitOperation)
    {
        _titleText = titleText;
        _getDetailedText = getDetailedText;
        _requirementSlotList = RequirementSlotListFromRunSkillDescriptorListModel(descriptor);
        _submitOperation = submitOperation;
    }

    public static CardPickerCell FromLiteral(
        string titleText = null,
        Func<ListModel<RequirementSlot>, string> getDetailedText = null,
        RunSkillDescriptorListModel descriptor = null,
        Func<CardPickerCell, Cell> submitOperation = null)
        => new(titleText ?? "选择", getDetailedText ?? (list => "请选择卡"), descriptor ?? RunSkillDescriptorListModel.Default(), submitOperation);
    
    public static CardPickerCell FromConstantDetailedText(
        string titleText = null,
        string detailedText = null,
        RunSkillDescriptorListModel descriptor = null,
        Func<CardPickerCell, Cell> submitOperation = null)
        => new(titleText ?? "选择", list => detailedText ?? "请选择卡", descriptor ?? RunSkillDescriptorListModel.Default(), submitOperation);

    public static CardPickerCell GetTemplate()
    {
        CardPickerCell template = FromConstantDetailedText(
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

    public static CardPickerCell FromTianJiGe()
    {
        CardPickerCell cell = FromConstantDetailedText(
            titleText:          $"天机阁",
            detailedText:       $"选择1张牌，复制1次",
            descriptor:         RunSkillDescriptorListModel.FromCount(1));

        cell.SetSubmitOperation(cardPickerCell =>
        {
            bool fulfilled = cardPickerCell.AllFulfilled();
            if (!fulfilled)
            {
                cardPickerCell.WithdrawAll();
                return null;
            }

            int count = cardPickerCell.RequirementSlotList.Count();
            RequirementSlot copyingSlot = cardPickerCell.RequirementSlotList[RandomManager.Range(0, count)];
            RunSkill copyingSkill = copyingSlot.Skill;
            
            RunManager.Instance.Environment.PickSkillProcedure(copyingSkill.GetEntry(), copyingSkill.GetJingJie());
                        
            cardPickerCell.WithdrawAll();
            return null;
        });

        return cell;
    }

    public static CardPickerCell FromBaiCaoTang(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        JingJie nextJingJie = currJingJie + 1;

        RunSkillDescriptor singleDescriptor = RunSkillDescriptor.FromJingJieBound(JingJie.LianQi, nextJingJie);
        
        CardPickerCell cell = FromConstantDetailedText(
            titleText:          $"百草堂",
            detailedText:       $"选择0~2张不高于{currJingJie}牌，提升到{nextJingJie.GetName()}",
            descriptor:         RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(singleDescriptor, 2));

        cell.SetSubmitOperation(cardPickerCell =>
        {
            cardPickerCell.RequirementSlotList.Do(slot =>
            {
                if (slot.Skill == null)
                    return;
                
                slot.Skill = RunSkill.FromChangeJingJie(slot.Skill, nextJingJie);
            });
                        
            cardPickerCell.WithdrawAll();
            return null;
        });

        return cell;
    }

    public static CardPickerCell FromTianJieShu(int ladder, Cell nextCell)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        RunSkillDescriptorListModel descriptorList = RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(
                RunSkillDescriptor.FromJingJieBoundAndHasWuXing(JingJie.LianQi, JingJie.HuaShen + 1), 
                5);
            
        CardPickerCell cell = FromConstantDetailedText(
            titleText:          $"天界树",
            detailedText:       $"选择至多5张牌，将被替换成新的牌。新的牌和原来的牌的五行有关。",
            descriptor:         descriptorList);

        cell.SetSubmitOperation(cardPickerCell =>
        {
            cardPickerCell.RequirementSlotList.Do(slot =>
            {
                if (slot.Skill == null)
                    return;

                if (slot.Skill.GetWuXing() == WuXing.Wu)
                    return;

                WuXing targetWuXing = slot.Skill.GetWuXing().Next;
                JingJie targetJingJie = slot.Skill.GetJingJie();
                
                GainSkillBuilder b = new();
                SkillEntryCollectionDescriptor descriptor = new(
                    wuXing: targetWuXing,
                    jingJie: targetJingJie,
                    count: 1,
                    consume: true);
                
                b.Draw(descriptor);
                SkillEntry newSkill = b.DrawnSkillEntries[0];
                
                slot.Skill = RunSkill.FromEntryJingJie(newSkill, targetJingJie);
            });
                        
            cardPickerCell.WithdrawAll();
            return nextCell;
        });

        return cell;
    }

    #endregion
    
    public string GetTitleText() => _titleText;
    public string GetDetailedText() => _getDetailedText(_requirementSlotList);
    public ListModel<RequirementSlot> RequirementSlotList => _requirementSlotList;
    
    public CardPickerCell SetSubmitOperation(Func<CardPickerCell, Cell> submitOperation)
    {
        _submitOperation = submitOperation;
        return this;
    }

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
