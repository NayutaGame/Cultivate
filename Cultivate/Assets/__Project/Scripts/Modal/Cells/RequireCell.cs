
using System;
using System.Collections.Generic;
using CLLibrary;

public class RequireCell : Cell
{
    private string _titleText;
    private Func<ListModel<RequirementSlot>, string> _getDetailedText;
    private ListModel<RequirementSlot> _requirementSlotList;
    private Func<RequireCell, Cell> _submitOperation;

    #region Constructors

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((RequireCell)thisObject).GetGuideDescriptor() },
        { "Requirements",               thisObject => ((RequireCell)thisObject)._requirementSlotList },
    };
    public override object Get(string s) => Accessor[s](this);
    private RequireCell(
        string titleText,
        Func<ListModel<RequirementSlot>, string> getDetailedText,
        List<RunSkillQuery> requirements,
        Func<RequireCell, Cell> submitOperation)
    {
        _titleText = titleText;
        _getDetailedText = getDetailedText;
        _requirementSlotList = RequirementSlotListFromQueries(requirements);
        _submitOperation = submitOperation;
    }

    public static RequireCell FromLiteral(
        string titleText = null,
        Func<ListModel<RequirementSlot>, string> getDetailedText = null,
        List<RunSkillQuery> requirements = null,
        Func<RequireCell, Cell> submitOperation = null)
        => new(titleText ?? "选择", getDetailedText ?? (list => "请选择卡"), requirements ?? RunSkillQuery.AnySkill().Stack(1), submitOperation);
    
    public static RequireCell FromConstantDetailedText(
        string titleText = null,
        string detailedText = null,
        List<RunSkillQuery> requirements = null,
        Func<RequireCell, Cell> submitOperation = null)
        => new(titleText ?? "选择", list => detailedText ?? "请选择卡", requirements ?? RunSkillQuery.AnySkill().Stack(1), submitOperation);

    public static RequireCell GetTemplate()
    {
        RequireCell template = FromConstantDetailedText(
            titleText:          "选择",
            detailedText:       "请选择一张牌",
            requirements:       RunSkillQuery.AnySkill().Stack(1));
        
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

    public static RequireCell FromTianJiGe()
    {
        RequireCell cell = FromConstantDetailedText(
            titleText:          $"天机阁",
            detailedText:       $"选择1张牌，复制1次",
            requirements:            RunSkillQuery.AnySkill().Stack(1));

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

    public static RequireCell FromBaiCaoTang(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        JingJie nextJingJie = currJingJie + 1;
        
        RequireCell cell = FromConstantDetailedText(
            titleText:          $"百草堂",
            detailedText:       $"选择0~2张不高于{currJingJie.GetName()}牌，提升到{nextJingJie.GetName()}",
            requirements:            RunSkillQuery.FromJingJieBound(JingJie.LianQi, currJingJie).Stack(2));

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

    public static RequireCell FromTianJieShu(int ladder, Cell nextCell)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        
        RequireCell cell = FromConstantDetailedText(
            titleText:          $"天界树",
            detailedText:       $"选择至多5张牌，将被替换成新的牌。新的牌和原来的牌的五行有关。",
            requirements:            RunSkillQuery.FromJingJieBoundAndHasWuXing(JingJie.LianQi, JingJie.HuaShen).Stack(5));

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
                b.Draw(SkillEntryQuery.FromWuXingBaseJingJieBound(
                    wuXing: targetWuXing,
                    baseJingJieBound: new(JingJie.LianQi, targetJingJie)), targetJingJie, consume: true);
                slot.Skill = RunSkill.FromGainingSkill(b.GainingSkills[0]);
            });
                        
            cardPickerCell.WithdrawAll();
            return nextCell;
        });

        return cell;
    }

    public static RequireCell FromZhanDuanChenYuan(int ladder, Cell nextCell)
    {
        JingJie currJingJie = JingJie.FanXu;
            
        RequireCell cell = FromConstantDetailedText(
            titleText:          $"斩断尘缘",
            detailedText:       $"选择至多5张牌，将被替换成新的牌。无法再遇到被选择的牌。",
            requirements:            RunSkillQuery.AnySkill().Stack(5));

        cell.SetSubmitOperation(cardPickerCell =>
        {
            cardPickerCell.RequirementSlotList.Do(slot =>
            {
                RunSkill skillToDepopulate = slot.Skill;
                if (skillToDepopulate == null)
                    return;

                RunManager.Instance.Environment.SkillPool.Depopulate(pred: e => e == skillToDepopulate.GetEntry());
            });
            
            cardPickerCell.RequirementSlotList.Do(slot =>
            {
                if (slot.Skill == null)
                    return;
                
                JingJie targetJingJie = slot.Skill.GetJingJie();
                
                GainSkillBuilder b = new();
                SkillEntryQuery drawStrategy = SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, targetJingJie));
                b.Draw(drawStrategy, targetJingJie, consume: true);
                slot.Skill = RunSkill.FromGainingSkill(b.GainingSkills[0]);
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
    
    public RequireCell SetSubmitOperation(Func<RequireCell, Cell> submitOperation)
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
    
    private static ListModel<RequirementSlot> RequirementSlotListFromQueries(List<RunSkillQuery> queries)
    {
        ListModel<RequirementSlot> requirementSlotList = new();
        for (int i = 0; i < queries.Count; i++)
        {
            requirementSlotList.Add(new(i, queries[i]));
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
