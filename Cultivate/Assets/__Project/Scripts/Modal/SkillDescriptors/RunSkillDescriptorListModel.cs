
public class RunSkillDescriptorListModel : ListModel<RunSkillDescriptor>
{
    public static RunSkillDescriptorListModel FromRunSkillDescriptorAndCount(RunSkillDescriptor descriptor, int count)
    {
        RunSkillDescriptorListModel list = new();
        for (int i = 0; i < count; i++)
        {
            list.Add(descriptor.Clone());
        }
        return list;
    }
    
    public static RunSkillDescriptorListModel FromDescriptors(RunSkillDescriptor[] descriptors)
    {
        RunSkillDescriptorListModel list = new();
        foreach (var descriptor in descriptors)
        {
            if (descriptor != null)
            {
                list.Add(descriptor.Clone());
            }
        }
        return list;
    }
    
    public static RunSkillDescriptorListModel Default()
        => FromCount(1);
    
    public static RunSkillDescriptorListModel FromCount(int count)
        => FromRunSkillDescriptorAndCount(RunSkillDescriptor.AnySkill(), count);
}