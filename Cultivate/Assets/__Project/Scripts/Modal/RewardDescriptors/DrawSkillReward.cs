
public class DrawSkillReward : Reward
{
    private string _description;
    private SkillCollectionDescriptor _descriptor;

    public DrawSkillReward(string description, SkillCollectionDescriptor descriptor)
    {
        _description = description;
        _descriptor = descriptor;
    }

    public override void Claim()
        => RunManager.Instance.Environment.DrawSkillsProcedure(_descriptor);

    public override string GetDescription() => _description;
}
