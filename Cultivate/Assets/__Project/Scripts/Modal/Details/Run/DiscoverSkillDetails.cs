
using System.Collections.Generic;

public class DiscoverSkillDetails : EventDetails
{
    public List<SkillDescriptor> Skills;
    public SkillCollectionDescriptor Descriptor;
    public JingJie PreferredJingJie;

    public DiscoverSkillDetails(SkillCollectionDescriptor descriptor, JingJie preferredJingJie)
    {
        Skills = new();
        Descriptor = descriptor;
        PreferredJingJie = preferredJingJie;
    }
}
