
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/RunSkillQuery", -9, true)]
public class RunSkillQueryNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] 
    [SerializeField]
    private OutputPort<RunSkillQuery> value = new(self => (self as RunSkillQueryNode).GetQuery());
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<WuXingType> WuXing = new(WuXingType.Any);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<Bound> JingJieBound = new(JingJie.LianQiOnly);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<TagType> Tag = new(TagType.None);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
    [SerializeField]
    private InputPort<string> Description = new("请提交卡牌");
    
    public RunSkillQuery GetQuery()
    {
        if (!Application.isPlaying)
            return null;

        WuXingType wuXingType = WuXing.Value;
        WuXing wuXing;
        
        wuXing = global::WuXing.FromWuXingType(wuXingType);
        
        Bound jingJieBound = JingJieBound.Value;
        
        TagType tagType = Tag.Value;
        TagComposite tagComposite = (TagComposite)((int)tagType << 6);
        
        string description = Description.Value;
        
        return RunSkillQuery.FromEverything(wuXing, jingJieBound, tagComposite, description);
    }
}
