
public class PlacementDetails : EventDetails
{
    public RunEntity Owner;
    public SkillEntry OverridingSkillEntry;
    public JingJie OverridingJingJie;

    public PlacementDetails(RunEntity owner)
    {
        Owner = owner;
        OverridingSkillEntry = "0001";
        OverridingJingJie = JingJie.LianQi;
    }
}
