
public class AddSkillReward : Reward
{
    private SkillEntry _entry;
    private JingJie _jingJie;

    private string _description;

    public AddSkillReward(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry ?? Encyclopedia.SkillCategory.Default();
        _jingJie = jingJie;

        _description = $"获得《{_entry.GetName()} @ {_jingJie}》";
    }

    public override void Claim()
    {
        GainSkillBuilder b = new();
        b.Pick(SkillGhost.FromEntryJingJie(_entry, _jingJie));
        RunManager.Instance.Environment.GainSkillProcedure(b);
    }

    public override string GetDescription() => _description;
}
