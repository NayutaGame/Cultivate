
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/RunSkillQuery", -9, true)]
public class RunSkillQueryNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<RunSkillQuery> value = new(self => (self as RunSkillQueryNode).GetQuery());
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<string> EntryName = new();
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<WuXingType> WuXing = new(WuXingType.Any);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<JingJieType> JingJie = new();
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<Bound> BaseJingJieBound = new(global::JingJie.LianQi2HuaShen);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<TagType> Tag = new(TagType.None);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<string> Description = new("请提交卡牌");
    
    public RunSkillQuery GetQuery()
    {
        if (!Application.isPlaying)
            return null;

        SkillEntry skillEntry = Encyclopedia.SkillCategory.FromName(EntryName.Value);
        WuXing wuXing = global::WuXing.FromWuXingType(WuXing.Value);
        JingJie jingJie = global::JingJie.FromJingJieType(JingJie.Value);
        Bound jingJieBound = BaseJingJieBound.Value;
        TagComposite tagComposite = (((int)(Tag.Value)) << 6);
        string description = Description.Value;
        
        return RunSkillQuery.FromEverything(skillEntry, wuXing, jingJie, jingJieBound, tagComposite, description);
    }
}
