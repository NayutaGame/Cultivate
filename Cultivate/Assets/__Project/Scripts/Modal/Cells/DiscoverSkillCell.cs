
using System;
using System.Collections.Generic;
using CLLibrary;

public class DiscoverSkillCell : Cell
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    public void SetTitleText(string value) => _titleText = value;

    private string _descriptionText;
    public string GetDescriptionText() => _descriptionText;
    public void SetDescriptionText(string value) => _descriptionText = value;

    private ListModel<SkillEntryDescriptor> _skills;
    public ListModel<SkillEntryDescriptor> GetSkills() => _skills;
    public int GetSkillCount() => _skills.Count();
    public SkillEntryDescriptor GetSkill(int i) => _skills[i];
    public int GetIndexOfSkill(SkillEntryDescriptor skill) => _skills.IndexOf(skill);

    private SkillEntryCollectionDescriptor _descriptor;
    private JingJie _preferredJingJie;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((DiscoverSkillCell)thisObject).GetGuideDescriptor() },
        { "Skills",                     thisObject => ((DiscoverSkillCell)thisObject).GetSkills() },
    };
    public override object Get(string s) => Accessor[s](this);
    private DiscoverSkillCell(
        string titleText = null,
        string descriptionText = null,
        SkillEntryCollectionDescriptor descriptor = null,
        JingJie? preferredJingJie = null)
    {
        _titleText = titleText ?? "灵感";
        _descriptionText = descriptionText ?? "请选择一张卡作为奖励";

        _descriptor = descriptor ?? new(jingJie: RunManager.Instance.Environment.JingJie, count: 3);
        _preferredJingJie = preferredJingJie ?? RunManager.Instance.Environment.JingJie;
        
        _skills = new();
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);
        DiscoverSkillDetails d = new DiscoverSkillDetails(_descriptor, _preferredJingJie);
        RunManager.Instance.Environment.DiscoverSkillProcedure(d);

        _skills.Clear();
        _skills.AddRange(d.Skills);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is PickDiscoveredSkillSignal pickDiscoveredSkillSignal)
        {
            int pickedIndex = pickDiscoveredSkillSignal.Selected;
            SkillEntryDescriptor skill = _skills[pickedIndex];
            // RunManager.Instance.Environment.PickDiscoveredSkillProcedure(pickedIndex, skill);
            return null;
        }

        return this;
    }

    public static DiscoverSkillCell FromDefault(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        return new(
            titleText: "灵感",
            descriptionText: "请选择一张卡作为奖励",
            descriptor: new(jingJie: currJingJie, count: 3),
            preferredJingJie: currJingJie
        );
    }

    public static DiscoverSkillCell FromLingYunFeng(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound jingJieBound = new(JingJie.LianQi, currJingJie + 1);

        DiscoverSkillCell d = new(
            titleText: $"凌云峰",
            descriptionText: $"选择1张{currJingJie}金牌",
            descriptor: new(wuXing: WuXing.Jin, pred: e => jingJieBound.Contains(e.LowestJingJie), count: 3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverSkillCell FromXiaoYaoHai(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound jingJieBound = new(JingJie.LianQi, currJingJie + 1);

        DiscoverSkillCell d = new(
            titleText: $"逍遥海",
            descriptionText: $"选择1张{currJingJie}水牌",
            descriptor: new(wuXing: WuXing.Shui, pred: e => jingJieBound.Contains(e.LowestJingJie), count: 3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverSkillCell FromTaohuaGong(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound jingJieBound = new(JingJie.LianQi, currJingJie + 1);
        
        DiscoverSkillCell d = new(
            titleText: $"桃花宫",
            descriptionText: $"选择1张{currJingJie}木牌",
            descriptor: new(wuXing: WuXing.Mu, pred: e => jingJieBound.Contains(e.LowestJingJie), count: 3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverSkillCell FromChangMingDian(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound jingJieBound = new(JingJie.LianQi, currJingJie + 1);
        
        DiscoverSkillCell d = new(
            titleText: $"长明殿",
            descriptionText: $"选择1张{currJingJie}火牌",
            descriptor: new(wuXing: WuXing.Huo, pred: e => jingJieBound.Contains(e.LowestJingJie), count: 3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverSkillCell FromHuanYueLing(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound jingJieBound = new(JingJie.LianQi, currJingJie + 1);
        
        DiscoverSkillCell d = new(
            titleText: $"环岳岭",
            descriptionText: $"选择1张{currJingJie}土牌",
            descriptor: new(wuXing: WuXing.Tu, pred: e => jingJieBound.Contains(e.LowestJingJie), count: 3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverSkillCell FromSanXiu(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        DiscoverSkillCell d = new(
            titleText: $"散修",
            descriptionText: $"选择1张基础境界是{currJingJie}期的牌",
            descriptor: new(pred: e => e.LowestJingJie == currJingJie, count: 3),
            preferredJingJie: currJingJie
        );
        return d;
    }
}
