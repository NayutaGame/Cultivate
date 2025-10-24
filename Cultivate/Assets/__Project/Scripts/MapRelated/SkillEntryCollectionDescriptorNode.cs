
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/SkillEntryCollectionDescriptor", -9, true)]
public class SkillEntryCollectionDescriptorNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] 
    [SerializeField]
    private OutputPort<SkillEntryCollectionDescriptor> value = new(self => (self as SkillEntryCollectionDescriptorNode).GetDescriptor());
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<WuXingType> WuXing = new(WuXingType.Wu);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<JingJieType> JingJie = new(JingJieType.LianQi);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<TagType> Tag = new(TagType.None);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<int> Count = new InputPort<int>(3);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<bool> Distinct = new InputPort<bool>(true);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<bool> Consume = new InputPort<bool>(true);
    
    public SkillEntryCollectionDescriptor GetDescriptor()
    {
        if (!Application.isPlaying)
            return null;
        
        WuXingType wuXingType = WuXing.Value;
        WuXing wuXing = global::WuXing.FromIndex((int)wuXingType);
        
        JingJieType jingJieType = JingJie.Value;
        JingJie jingJie = (int)jingJieType;
        TagType tagType = Tag.Value;
        TagComposite tagComposite = (TagComposite)((int)tagType << 6);
        int count = Count.Value;
        bool distinct = Distinct.Value;
        bool consume = Consume.Value;
        
        return new SkillEntryCollectionDescriptor(
            pred: null,
            wuXing: wuXing,
            jingJie: jingJie,
            tagComposite: tagComposite,
            count: count,
            distinct: distinct,
            consume: consume
        );
    }
}
