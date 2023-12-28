
public class PlacementDetails : EventDetails
{
    public RunEntity Owner;
    public SkillEntry OverridingSkillEntry;
    public JingJie OverridingJingJie;

    public PlacementDetails(RunEntity owner)
    {
        Owner = owner;
        OverridingSkillEntry = "聚气术";
        OverridingJingJie = JingJie.LianQi;
    }
}
