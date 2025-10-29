
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Shop Cell", -8, true)]
public class ShopCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Title = new("收藏家");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> ContentText = new("可以花钱购买卡牌");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<float> PriceMultiplier = new(2f);
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> SpriteName = new("收藏家");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> AcceptGold = new(true);
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> AcceptHealth = new(false);
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<SkillEntryQuery>> DrawStrategies = new();
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> _next = new(self => self as ILogicNode);
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => _next;
    
    public override IEnumerable<ILogicNode> Prevs => prevs.Values;
    
    public override ILogicNode Next
    {
        get
        {
            var node = NextPort?.Connection?.Node as ILogicNode;
            while (node != null && node is ILogicConnector)
                node = node.Next;
            return node;
        }
    }
    
    public override void Execute()
    {
    }
    
    protected override Cell CreateInternalCell()
    {
        if (!Application.isPlaying)
            return null;
            
        // int ladder = 8; // From blackboard - TODO: get from actual blackboard
        // float priceMultiplier = PriceMultiplier.Value;
        // string title = Title.Value;
        // string contentText = ContentText.Value;
        // string spriteName = SpriteName.Value;
        // bool acceptGold = AcceptGold.Value;
        // bool acceptHealth = AcceptHealth.Value;
        //
        // SpriteEntry spriteEntry = Encyclopedia.SpriteCategory.FromName(spriteName);
        //
        // SkillEntryCollectionDescriptor drawStrategy = DrawStrategy.Value;
        // if (drawStrategy == null)
        // {
        //     drawStrategy = SkillEntryCollectionDescriptor.FromEverything(
        //         baseJingJieRange: new(JingJie.LianQi, RoomDefinition.GetJingJieFromLadder(ladder)),
        //         count: 8,
        //         consume: false);
        // }
        //
        // return ShopCell.FromEverything(ladder, priceMultiplier, title, contentText, spriteEntry, acceptGold, acceptHealth, drawStrategy);
        return null;
    }
    
    public override void ReceiveSignal(Signal signal)
    {
        ExitShopSignal exitShopSignal = signal as ExitShopSignal;
        if (exitShopSignal == null)
            return;
    }
}
