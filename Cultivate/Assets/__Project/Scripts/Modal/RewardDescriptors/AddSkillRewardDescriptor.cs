
public class AddSkillRewardDescriptor : RewardDescriptor
{
    private SkillEntry _entry;
    private JingJie _jingJie;

    private string _description;

    public AddSkillRewardDescriptor(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry;
        _jingJie = jingJie;

        _description = $"获得《{_entry.GetName()} @ {_jingJie}》";
    }

    public override void Claim()
    {
        RunManager.Instance.Battle.ForceAddSkill(new AddSkillDetails(_entry, _jingJie));
    }

    public override string GetDescription() => _description;
}
