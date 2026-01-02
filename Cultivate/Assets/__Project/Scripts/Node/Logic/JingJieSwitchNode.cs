using System;
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Logic/JingJie Switch", -10, true)]
public class JingJieSwitchNode : Node, ILogicNode, ILogicConnector
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<JingJieIndirect> Indicator;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> 练气;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> 筑基;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> 金丹;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> 元婴;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> 化神;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> 返虚;

    public NodePort PrevPort => prevs;
    public NodePort NextPort
    {
        get
        {
            JingJieIndirect jingJieIndirect = Indicator.Value;

            if (Application.isPlaying)
            {
                int jingJie = JingJie.FromIndirect(jingJieIndirect);
                switch (jingJie)
                {
                    case 0:
                        return 练气;
                    case 1:
                        return 筑基;
                    case 2:
                        return 金丹;
                    case 3:
                        return 元婴;
                    case 4:
                        return 化神;
                    case 5:
                        return 返虚;
                    default:
                        return 练气;
                }
            }
            else
            {
                switch (jingJieIndirect)
                {
                    case JingJieIndirect.练气:
                        return 练气;
                    case JingJieIndirect.筑基:
                        return 筑基;
                    case JingJieIndirect.金丹:
                        return 金丹;
                    case JingJieIndirect.元婴:
                        return 元婴;
                    case JingJieIndirect.化神:
                        return 化神;
                    case JingJieIndirect.返虚:
                        return 返虚;
                    case JingJieIndirect.当前:
                        return 练气;
                    case JingJieIndirect.上一境界:
                        return 练气;
                    case JingJieIndirect.下一境界:
                        return 练气;
                }
            }
            
            throw new Exception("CL:Unexpected pathway");
        }
    }

    public IEnumerable<ILogicNode> Prevs => prevs.Values;
    public ILogicNode Next
    {
        get
        {
            var node = NextPort.Connection?.Node as ILogicNode;
            while (node != null && node is ILogicConnector)
                node = node.Next;
            return node;
        }
    }

    public void Execute()
    {
    }

    public int GetLadder()
    {
        if (Graph.Blackboard == null)
            return default;

        string key = "Ladder";
        Graph.Blackboard.TryGetLocalValue(Graph, key, out int value);
        return value;
    }
}