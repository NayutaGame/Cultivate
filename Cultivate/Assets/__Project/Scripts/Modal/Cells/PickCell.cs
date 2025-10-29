
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

    private ListModel<SkillReference> _inventory;
    private ListModel<SkillReference> GetInventory() => _inventory;

    private Bound _bound;
    public Bound Bound => _bound;
    public bool HasSpace(int occupied)
        => _bound.End > occupied;

    private Func<List<SkillReference>, Cell> _confirmOperation;
    public PickCell SetConfirmOperation(Func<List<SkillReference>, Cell> confirmOperation)
    {
        _confirmOperation = confirmOperation;
        return this;
    }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((PickCell)thisObject).GetGuideDescriptor() },
        { "Inventory",                  thisObject => ((PickCell)thisObject).GetInventory() },
    };
    public override object Get(string s) => Accessor[s](this);
    public PickCell(
        string titleText = null,
        string detailedText = null,
        Bound? bound = null,
        Func<List<SkillReference>, Cell> confirmOperation = null)
    {
        _titleText = titleText ?? "选牌";
        _detailedText = detailedText ?? "请选择卡";
        _bound = bound ?? new Bound(1);
        _confirmOperation = confirmOperation;
        
        _inventory = new ListModel<SkillReference>();
    }

    public void PopulateInventory(List<SkillReference> skills)
    {
        foreach(SkillReference skill in skills)
            _inventory.Add(skill);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ConfirmSkillsSignal selectedSkillsSignal && _confirmOperation != null)
        {
            return _confirmOperation(selectedSkillsSignal.Selected);
        }

        return this;
    }

    public static PickCell FromJianChi(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        PickCell cell = new(
            titleText: $"剑池",
            detailedText: $"检索1张{currJingJie.GetName()}攻击牌",
            bound: new(0, 1)
        );
        
        List<SkillEntryQuery> drawStrategies = SkillEntryQuery.FromBaseJingJieBoundTag(
            baseJingJieBound: new(JingJie.LianQi, RunManager.Instance.Environment.JingJie),
            tagComposite: TagCategory.Attack).Stack(10);
        
        GainSkillBuilder b = new();
        b.Draw(drawStrategies, RunManager.Instance.Environment.JingJie, filterEmpty: true, distinct: true, consume: false);
        cell.PopulateInventory(b.DrawnSkills);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Clone()));
            b.Execute();
            b.Invoke();
            return null;
        });
        
        return cell;
    }

    public static PickCell FromFengYuLou(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        PickCell cell = new(
            titleText: $"风雨楼",
            detailedText: $"检索1张{currJingJie.GetName()}防御牌",
            bound: new(0, 1)
        );
        
        List<SkillEntryQuery> drawStrategies = SkillEntryQuery.FromBaseJingJieBoundTag(
            baseJingJieBound: new(JingJie.LianQi, RunManager.Instance.Environment.JingJie),
            tagComposite: TagCategory.Defend).Stack(10);
        
        GainSkillBuilder b = new();
        b.Draw(drawStrategies, RunManager.Instance.Environment.JingJie, filterEmpty: true, distinct: true, consume: false);
        cell.PopulateInventory(b.DrawnSkills);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Clone()));
            b.Execute();
            b.Invoke();
            return null;
        });
        
        return cell;
    }

    public static PickCell FromXingGong(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        PickCell cell = new(
            titleText: $"星宫",
            detailedText: $"检索1张{currJingJie.GetName()}灵气牌",
            bound: new(0, 1)
        );
        
        List<SkillEntryQuery> drawStrategies = SkillEntryQuery.FromBaseJingJieBoundTag(
            baseJingJieBound: new(JingJie.LianQi, RunManager.Instance.Environment.JingJie),
            tagComposite: TagCategory.Mana).Stack(10);
        
        GainSkillBuilder b = new();
        b.Draw(drawStrategies, RunManager.Instance.Environment.JingJie, filterEmpty: true, distinct: true, consume: false);
        cell.PopulateInventory(b.DrawnSkills);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Clone()));
            b.Execute();
            b.Invoke();
            return null;
        });
        
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
            bound: new(0, 2)
        );
        
        List<SkillEntryQuery> drawStrategies = SkillEntryQuery.FromBaseJingJieBound(jingJieBound).Stack(10);
        
        GainSkillBuilder b = new();
        b.Draw(drawStrategies, RunManager.Instance.Environment.JingJie, filterEmpty: true, distinct: true, consume: false);
        cell.PopulateInventory(b.DrawnSkills);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Clone()));
            b.Execute();
            b.Invoke();
            return null;
        });
        
        return cell;
    }
}
