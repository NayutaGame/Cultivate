
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
        b.Pick(_entry);
        b.Create(_jingJie);
        b.Add();
        b.Invoke();
    }

    public override string GetDescription() => _description;
}
