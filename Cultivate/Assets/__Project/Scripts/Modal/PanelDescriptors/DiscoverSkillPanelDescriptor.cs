
public class DiscoverSkillPanelDescriptor : PanelDescriptor
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

    public DiscoverSkillPanelDescriptor(string titleText = null, string descriptionText = null, SkillEntryCollectionDescriptor descriptor = null, JingJie? preferredJingJie = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Skills",                   GetSkills },
        };

        _titleText = titleText ?? "灵感";
        _descriptionText = descriptionText ?? "请选择一张卡作为奖励";

        _descriptor = descriptor ?? new(jingJie: RunManager.Instance.Environment.JingJie, count: 3);
        _preferredJingJie = preferredJingJie ?? RunManager.Instance.Environment.JingJie;
        
        _skills = new();
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);
        DiscoverSkillDetails d = new DiscoverSkillDetails(_descriptor, _preferredJingJie);
        RunManager.Instance.Environment.DiscoverSkillProcedure(d);

        _skills.Clear();
        _skills.AddRange(d.Skills);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
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
}
