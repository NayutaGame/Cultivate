
using System.Collections.Generic;

public class DiscoverSkillDetails : RunClosureDetails
{
    public List<SkillGhost> Skills;
    public List<SkillEntryQuery> DrawStrategies;
    public JingJie PreferredJingJie;

    public int? MimicIndex;

    public DiscoverSkillDetails(List<SkillEntryQuery> drawStrategies, JingJie preferredJingJie)
    {
        Skills = new();
        DrawStrategies = drawStrategies;
        PreferredJingJie = preferredJingJie;
    }
}
