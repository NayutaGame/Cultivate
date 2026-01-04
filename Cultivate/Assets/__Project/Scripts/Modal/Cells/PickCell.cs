
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public class PickCell : Cell
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private Bound _pickCardCountRange;
    public Bound PickCardCountRange => _pickCardCountRange;
    public bool HasSpace(int occupied)
        => _pickCardCountRange.End > occupied;

    private List<SkillEntryQuery> _drawStrategies;
    public List<SkillEntryQuery> GetDrawStrategies() => _drawStrategies;

    private Func<ConfirmSkillsSignal, Cell> _confirmOperation;
    public PickCell SetConfirmOperation(Func<ConfirmSkillsSignal, Cell> confirmOperation)
    {
        _confirmOperation = confirmOperation;
        return this;
    }

    private ListModel<SkillGhost> _inventory;
    private ListModel<SkillGhost> GetInventory() => _inventory;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((PickCell)thisObject).GetGuideDescriptor() },
        { "Inventory",                  thisObject => ((PickCell)thisObject).GetInventory() },
    };
    public override object Get(string s) => Accessor[s](this);
    public PickCell(
        string titleText = null,
        string detailedText = null,
        Bound? pickCardCountRange = null,
        List<SkillEntryQuery> drawStrategies = null,
        Func<ConfirmSkillsSignal, Cell> confirmOperation = null)
    {
        _titleText = titleText ?? "选牌";
        _detailedText = detailedText ?? "请选择卡";
        _pickCardCountRange = pickCardCountRange ?? new Bound(1);
        _drawStrategies = drawStrategies ?? SkillEntryQuery.AnySkill().Stack(3);
        _confirmOperation = confirmOperation ?? DefaultConfirmOperation;
        
        _inventory = new ListModel<SkillGhost>();
    }

    public static PickCell FromJianChi(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        PickCell cell = new(
            titleText: $"剑池",
            detailedText: $"检索1张{currJingJie.GetName()}攻击牌",
            pickCardCountRange: new(0, 1),
            drawStrategies: SkillEntryQuery.FromBaseJingJieBoundTag(
                baseJingJieBound: new(JingJie.LianQi, RunManager.Instance.Environment.Map.JingJie),
                tagComposite: TagCategory.Attack).Stack(10)
        );
        
        return cell;
    }

    public static PickCell FromFengYuLou(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        PickCell cell = new(
            titleText: $"风雨楼",
            detailedText: $"检索1张{currJingJie.GetName()}防御牌",
            pickCardCountRange: new(0, 1),
            drawStrategies: SkillEntryQuery.FromBaseJingJieBoundTag(
                baseJingJieBound: new(JingJie.LianQi, RunManager.Instance.Environment.Map.JingJie),
                tagComposite: TagCategory.Defend).Stack(10)
        );
        
        return cell;
    }

    public static PickCell FromXingGong(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        PickCell cell = new(
            titleText: $"星宫",
            detailedText: $"检索1张{currJingJie.GetName()}灵气牌",
            pickCardCountRange: new(0, 1),
            drawStrategies: SkillEntryQuery.FromBaseJingJieBoundTag(
                baseJingJieBound: new(JingJie.LianQi, RunManager.Instance.Environment.Map.JingJie),
                tagComposite: TagCategory.Mana).Stack(10)
        );
        
        return cell;
    }

    public static PickCell FromBiYeJi(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound jingJieBound = new(JingJie.LianQi,
            (currJingJie - 2).ClampLower(JingJie.LianQi));

        PickCell cell = new(
            titleText: $"毕业季",
            detailedText: $"请选择至多两张牌",
            pickCardCountRange: new(0, 2),
            drawStrategies: SkillEntryQuery.FromBaseJingJieBound(jingJieBound).Stack(8)
        );

        cell.SetEnter(cell =>
        {
            PickCell pickCell = cell as PickCell;
            GainSkillBuilder b = new();
            b.Draw(pickCell.GetDrawStrategies(), JingJie.LianQi, filterEmpty: true, distinct: true, consume: false);
            b.GainingSkills.Do(g => pickCell.PopulateInventory(SkillGhost.FromGainingSkill(g)));
        });
        
        
        return cell;
    }

    public void PopulateInventory(SkillGhost skill)
    {
        _inventory.Add(skill);
    }

    public void PopulateInventory(List<SkillGhost> skills)
    {
        foreach(SkillGhost skill in skills)
            _inventory.Add(skill);
    }

    public override void DefaultEnter(Cell cell)
    {
        GainSkillBuilder b = new();
        b.Draw(_drawStrategies, RunManager.Instance.Environment.Map.JingJie, filterEmpty: true, distinct: true, consume: false);
        b.GainingSkills.Do(g => PopulateInventory(SkillGhost.FromGainingSkill(g)));
    }

    public Cell DefaultConfirmOperation(ConfirmSkillsSignal confirmSkillsSignal)
    {
        List<SkillGhost> skills = GetPickedSkillsFromPickedIndices(confirmSkillsSignal.PickedIndices);
        if (skills.Count <= 0)
            return null;
        RunManager.Instance.Environment.PickSkillsProcedure(skills);
        return null;
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ConfirmSkillsSignal selectedSkillsSignal && _confirmOperation != null)
        {
            return _confirmOperation(selectedSkillsSignal);
        }

        return this;
    }

    public List<SkillGhost> GetPickedSkillsFromPickedIndices(List<int> pickedIndices)
    {
        return pickedIndices.Map(index => _inventory[index]).ToList();
    }

    public List<SkillGhost> GetAbandonedSkillsFromPickedIndices(List<int> pickedIndices)
    {
        HashSet<int> pickedSet = new HashSet<int>(pickedIndices);
        List<SkillGhost> abandoned = new List<SkillGhost>();
        for (int i = 0; i < _inventory.Count(); i++)
        {
            if (!pickedSet.Contains(i))
            {
                abandoned.Add(_inventory[i]);
            }
        }
        return abandoned;
    }
}
