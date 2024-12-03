
public class AddSkillReward : Reward
{
    private SkillEntry _entry;
    private JingJie _jingJie;

    private string _description;

    public AddSkillReward(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry;
        _jingJie = jingJie;

        _description = $"获得《{_entry.GetName()} @ {_jingJie}》";
    }

    public override void Claim()
    {
        RunManager.Instance.Environment.LegacyAddSkillProcedure(_entry, _jingJie);
    }

    public override string GetDescription() => _description;
}
