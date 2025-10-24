using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;
using CLLibrary;

[NodeWidth(400)]
[CreateNodeMenu("Variable/RunSkillDescriptorListModel", -9, true)]
public class RunSkillDescriptorListModelNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] 
    [SerializeField]
    private OutputPort<RunSkillDescriptorListModel> value = new(self => (self as RunSkillDescriptorListModelNode).GetDescriptorList());
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<RunSkillDescriptor>[] Descriptor = new InputPort<RunSkillDescriptor>[1];
    
    public RunSkillDescriptorListModel GetDescriptorList()
    {
        if (!Application.isPlaying)
            return null;

        if (Descriptor == null || Descriptor.Length == 0)
            return RunSkillDescriptorListModel.Default();

        // 收集所有非空的描述符
        var descriptors = new System.Collections.Generic.List<RunSkillDescriptor>();
        foreach (var descriptorPort in Descriptor)
        {
            if (descriptorPort?.Value != null)
            {
                descriptors.Add(descriptorPort.Value);
            }
        }

        if (descriptors.Count == 0)
            return RunSkillDescriptorListModel.Default();

        // 如果只有一个描述符，使用现有的方法
        if (descriptors.Count == 1)
        {
            return RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(descriptors[0], 1);
        }

        // 多个描述符的情况，需要创建包含多个描述符的模型
        return RunSkillDescriptorListModel.FromDescriptors(descriptors.ToArray());
    }
}
