
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

    private ListModel<SkillEntryDescriptor> _inventory;
    private ListModel<SkillEntryDescriptor> GetInventory() => _inventory;

    private Bound _bound;
    public Bound Bound => _bound;
    public bool HasSpace(int occupied)
        => _bound.End - 1 > occupied;

    private Func<List<SkillEntryDescriptor>, Cell> _confirmOperation;
    public PickCell SetConfirmOperation(Func<List<SkillEntryDescriptor>, Cell> confirmOperation)
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
        Func<List<SkillEntryDescriptor>, Cell> confirmOperation = null)
    {
        _titleText = titleText ?? "选牌";
        _detailedText = detailedText ?? "请选择卡";
        _bound = bound ?? new Bound(1);
        _confirmOperation = confirmOperation;
        
        _inventory = new ListModel<SkillEntryDescriptor>();
    }

    public void PopulateInventory(List<SkillEntryDescriptor> skills)
    {
        foreach(SkillEntryDescriptor skill in skills)
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
            bound: new Bound(0, 2)
        );
        
        SkillEntryCollectionDescriptor descriptor = new(jingJie: RunManager.Instance.Environment.JingJie,
            tagComposite: TagCategory.Attack,
            count: 10,
            consume: false);
        
        GainSkillBuilder b = new();
        b.Draw(descriptor);
        List<SkillEntryDescriptor> list = b.DrawnSkillEntries
            .FilterObj(e => e != Encyclopedia.SkillCategory.Default())    
            .Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie))
            .ToList();
        cell.PopulateInventory(list);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Entry));
            skills.Do(item => b.SingleCreate(item.JingJie));
            b.Add();
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
            bound: new Bound(0, 2)
        );
        
        SkillEntryCollectionDescriptor descriptor = new(jingJie: RunManager.Instance.Environment.JingJie,
            tagComposite: TagCategory.Defend,
            count: 10,
            consume: false);
        
        GainSkillBuilder b = new();
        b.Draw(descriptor);
        List<SkillEntryDescriptor> list = b.DrawnSkillEntries
            .FilterObj(e => e != Encyclopedia.SkillCategory.Default())    
            .Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie))
            .ToList();
        cell.PopulateInventory(list);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Entry));
            skills.Do(item => b.SingleCreate(item.JingJie));
            b.Add();
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
            bound: new Bound(0, 2)
        );
        
        SkillEntryCollectionDescriptor descriptor = new(jingJie: RunManager.Instance.Environment.JingJie,
            tagComposite: TagCategory.Mana,
            count: 10,
            consume: false);
        
        GainSkillBuilder b = new();
        b.Draw(descriptor);
        List<SkillEntryDescriptor> list = b.DrawnSkillEntries
            .FilterObj(e => e != Encyclopedia.SkillCategory.Default())    
            .Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie))
            .ToList();
        cell.PopulateInventory(list);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Entry));
            skills.Do(item => b.SingleCreate(item.JingJie));
            b.Add();
            b.Invoke();
            return null;
        });
        
        return cell;
    }

    public static PickCell FromBiYeJi(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound jingJieBound = new Bound(JingJie.LianQi,
            (currJingJie - 2).ClampLower(JingJie.LianQi) + 1);

        PickCell cell = new(
            titleText: $"毕业季",
            detailedText: $"请选择至多两张牌",
            bound: new Bound(0, 3)
        );
        
        SkillEntryCollectionDescriptor descriptor = new(pred: e => jingJieBound.Contains(e.LowestJingJie),
            count: 10,
            consume: false);
        
        GainSkillBuilder b = new();
        b.Draw(descriptor);
        List<SkillEntryDescriptor> list = b.DrawnSkillEntries
            .FilterObj(e => e != Encyclopedia.SkillCategory.Default())    
            .Map(e => SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie))
            .ToList();
        cell.PopulateInventory(list);
        cell.SetConfirmOperation(skills =>
        {
            GainSkillBuilder b = new();
            skills.Do(item => b.Pick(item.Entry));
            skills.Do(item => b.SingleCreate(item.JingJie));
            b.Add();
            b.Invoke();
            return null;
        });
        
        return cell;
    }
}
