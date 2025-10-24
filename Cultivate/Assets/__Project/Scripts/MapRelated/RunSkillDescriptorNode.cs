
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/RunSkillDescriptor", -9, true)]
public class RunSkillDescriptorNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] 
    [SerializeField]
    private OutputPort<RunSkillDescriptor> value = new(self => (self as RunSkillDescriptorNode).GetDescriptor());
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<WuXingType> WuXing = new(WuXingType.Any);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<Bound> JingJieBound = new(new Bound(0, 1));
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<TagType> Tag = new(TagType.None);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<string> Description = new("请提交卡牌");
    
    public RunSkillDescriptor GetDescriptor()
    {
        if (!Application.isPlaying)
            return null;

        WuXingType wuXingType = WuXing.Value;
        WuXing wuXing;
        
        if (wuXingType == WuXingType.Any)
        {
            wuXing = null;
        }
        else
        {
            wuXing = global::WuXing.FromIndex((int)wuXingType);
        }
        
        Bound jingJieBound = JingJieBound.Value;
        
        TagType tagType = Tag.Value;
        TagComposite tagComposite = (TagComposite)((int)tagType << 6);
        
        string description = Description.Value;
        
        return RunSkillDescriptor.FromEverything(wuXing, jingJieBound, tagComposite, description);
    }
}
