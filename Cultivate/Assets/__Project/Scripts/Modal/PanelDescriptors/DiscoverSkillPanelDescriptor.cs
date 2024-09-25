
public class DiscoverSkillPanelDescriptor : PanelDescriptor
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    public void SetTitleText(string value) => _titleText = value;

    private string _detailedText;
    public string GetDetailedText() => _detailedText;
    public void SetDetailedText(string value) => _detailedText = value;

    private ListModel<SkillEntryDescriptor> _skills;
    public ListModel<SkillEntryDescriptor> GetSkills() => _skills;
    public int GetSkillCount() => _skills.Count();
    public SkillEntryDescriptor GetSkill(int i) => _skills[i];
    public int GetIndexOfSkill(SkillEntryDescriptor skill) => _skills.IndexOf(skill);

    private SkillEntryCollectionDescriptor _descriptor;
    private JingJie _preferredJingJie;

    public DiscoverSkillPanelDescriptor(string titleText = null, string detailedText = null, SkillEntryCollectionDescriptor descriptor = null, JingJie? preferredJingJie = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Skills",                   GetSkills },
        };

        _titleText = titleText ?? "";
        _detailedText = detailedText ?? "请选择一张卡作为奖励";

        _descriptor = descriptor ?? new(jingJie: RunManager.Instance.Environment.JingJie, count: 3);
        _preferredJingJie = preferredJingJie ?? RunManager.Instance.Environment.JingJie;
        
        _skills = new();
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();
        DiscoverSkillDetails d = new DiscoverSkillDetails(_descriptor, _preferredJingJie);
        RunManager.Instance.Environment.DiscoverSkillProcedure(d);

        _skills.Clear();
        _skills.AddRange(d.Skills);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            SkillEntryDescriptor skill = _skills[selectedOptionSignal.Selected];
            RunManager.Instance.Environment.AddSkillProcedure(skill.Entry, skill.JingJie);
            return null;
        }

        return this;
    }
}
