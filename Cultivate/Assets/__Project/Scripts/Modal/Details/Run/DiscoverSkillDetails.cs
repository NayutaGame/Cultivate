
using System.Collections.Generic;

public class DiscoverSkillDetails : EventDetails
{
    public List<SkillEntryDescriptor> Skills;
    public SkillEntryCollectionDescriptor Descriptor;
    public JingJie PreferredJingJie;

    public DiscoverSkillDetails(SkillEntryCollectionDescriptor descriptor, JingJie preferredJingJie)
    {
        Skills = new();
        Descriptor = descriptor;
        PreferredJingJie = preferredJingJie;
    }
}
