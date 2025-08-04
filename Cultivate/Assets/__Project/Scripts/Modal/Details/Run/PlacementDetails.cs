
public class PlacementDetails : RunClosureDetails
{
    public RunEntity Owner;
    public SkillEntry OverridingSkillEntry;
    public JingJie OverridingJingJie;

    public PlacementDetails(RunEntity owner)
    {
        Owner = owner;
        OverridingSkillEntry = Encyclopedia.SkillCategory.FromName("聚气术");
        OverridingJingJie = JingJie.LianQi;
    }
}
