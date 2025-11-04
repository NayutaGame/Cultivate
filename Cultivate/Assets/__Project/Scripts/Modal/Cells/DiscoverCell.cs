
using System;
using System.Collections.Generic;
using CLLibrary;

public class DiscoverCell : Cell
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    public void SetTitleText(string value) => _titleText = value;

    private string _descriptionText;
    public string GetDescriptionText() => _descriptionText;
    public void SetDescriptionText(string value) => _descriptionText = value;

    private ListModel<SkillGhost> _skills;
    public ListModel<SkillGhost> GetSkills() => _skills;

    private List<SkillEntryQuery> _drawStrategies;
    public DiscoverCell SetDrawStrategy(List<SkillEntryQuery> drawStrategies)
    {
        _drawStrategies = drawStrategies;
        return this;
    }

    private JingJie _preferredJingJie;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((DiscoverCell)thisObject).GetGuideDescriptor() },
        { "Skills",                     thisObject => ((DiscoverCell)thisObject).GetSkills() },
    };
    public override object Get(string s) => Accessor[s](this);
    private DiscoverCell(
        string titleText = null,
        string descriptionText = null,
        List<SkillEntryQuery> drawStrategies = null,
        JingJie preferredJingJie = null)
    {
        _titleText = titleText ?? "灵感";
        _descriptionText = descriptionText ?? "请选择一张卡作为奖励";

        _drawStrategies = drawStrategies ?? SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, RunManager.Instance.Environment.JingJie)).Stack(3);
        _preferredJingJie = preferredJingJie ?? RunManager.Instance.Environment.JingJie;
        
        _skills = new();
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);
        DiscoverSkillDetails d = new DiscoverSkillDetails(_drawStrategies, _preferredJingJie);
        RunManager.Instance.Environment.DiscoverSkillProcedure(d);

        _skills.Clear();
        _skills.AddRange(d.Skills);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is PickDiscoveredSkillSignal pickDiscoveredSkillSignal)
        {
            int pickedIndex = pickDiscoveredSkillSignal.Selected;
            SkillGhost skill = _skills[pickedIndex];
            // RunManager.Instance.Environment.PickDiscoveredSkillProcedure(pickedIndex, skill);
            return null;
        }

        return this;
    }

    public static DiscoverCell FromDefault(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        return new(
            titleText: "灵感",
            descriptionText: "请选择一张卡作为奖励",
            drawStrategies: SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, currJingJie)).Stack(3),
            preferredJingJie: currJingJie
        );
    }

    public static DiscoverCell FromTitleDescription(int ladder, string title, string description)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        
        return new(
            titleText: "灵感",
            descriptionText: "请选择一张卡作为奖励",
            preferredJingJie: currJingJie
        );
    }

    public static DiscoverCell FromLingYunFeng(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        DiscoverCell d = new(
            titleText: $"凌云峰",
            descriptionText: $"选择1张{currJingJie.GetName()}金牌",
            drawStrategies: SkillEntryQuery.FromWuXingBaseJingJieBound(
                wuXing: WuXing.Jin,
                baseJingJieBound: new(JingJie.LianQi, currJingJie)
            ).Stack(3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverCell FromXiaoYaoHai(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        DiscoverCell d = new(
            titleText: $"逍遥海",
            descriptionText: $"选择1张{currJingJie.GetName()}水牌",
            drawStrategies: SkillEntryQuery.FromWuXingBaseJingJieBound(
                wuXing: WuXing.Shui,
                baseJingJieBound: new(JingJie.LianQi, currJingJie)
            ).Stack(3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverCell FromTaoHuaGong(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        
        DiscoverCell d = new(
            titleText: $"桃花宫",
            descriptionText: $"选择1张{currJingJie.GetName()}木牌",
            drawStrategies: SkillEntryQuery.FromWuXingBaseJingJieBound(
                wuXing: WuXing.Mu,
                baseJingJieBound: new(JingJie.LianQi, currJingJie)
            ).Stack(3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverCell FromChangMingDian(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        
        DiscoverCell d = new(
            titleText: $"长明殿",
            descriptionText: $"选择1张{currJingJie.GetName()}火牌",
            drawStrategies: SkillEntryQuery.FromWuXingBaseJingJieBound(
                wuXing: WuXing.Huo,
                baseJingJieBound: new(JingJie.LianQi, currJingJie)
            ).Stack(3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverCell FromHuanYueLing(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        
        DiscoverCell d = new(
            titleText: $"环岳岭",
            descriptionText: $"选择1张{currJingJie.GetName()}土牌",
            drawStrategies: SkillEntryQuery.FromWuXingBaseJingJieBound(
                wuXing: WuXing.Tu,
                baseJingJieBound: new(JingJie.LianQi, currJingJie)
            ).Stack(3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverCell FromSanXiu(int ladder)
    {
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);

        DiscoverCell d = new(
            titleText: $"散修",
            descriptionText: $"选择1张基础境界是{currJingJie.GetName()}期的牌",
            drawStrategies: SkillEntryQuery.FromBaseJingJieBound(new(currJingJie, currJingJie)).Stack(3),
            preferredJingJie: currJingJie
        );
        return d;
    }

    public static DiscoverCell FromEverything(
        string titleText,
        string descriptionText,
        List<SkillEntryQuery> drawStrategies,
        JingJie preferredJingJie)
    {
        return new(
            titleText: titleText,
            descriptionText: descriptionText,
            drawStrategies: drawStrategies,
            preferredJingJie: preferredJingJie
        );
    }
}
