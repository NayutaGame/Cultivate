
public class DrawSkillReward : Reward
{
    private string _description;
    private SkillEntryCollectionDescriptor _descriptor;

    public DrawSkillReward(string description, SkillEntryCollectionDescriptor descriptor)
    {
        _description = description;
        _descriptor = descriptor;
    }

    public override void Claim()
        => RunManager.Instance.Environment.LegacyDrawSkillsProcedure(_descriptor);

    public override string GetDescription() => _description;
}
