
using System;
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Logic/Random Switch", -10, true)]
public class RandomSwitchNode : Node, ILogicNode, ILogicConnector
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> PrevCell;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<int>[] Weights;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode>[] Paths;

    private int? _pathIndex;

    private int PathIndex
    {
        get
        {
            if (_pathIndex.HasValue)
                return _pathIndex.Value;
            
            _pathIndex = CalcPathIndex();
            return _pathIndex.Value;
        }
    }

    private int CalcPathIndex()
    {
        // sum up weights
        int totalWeight = 0;
        int pathCount = Mathf.Min(Weights?.Length ?? 0, Paths?.Length ?? 0);
        
        if (pathCount == 0)
            return 0;
        
        for (int i = 0; i < pathCount; i++)
        {
            int weight = Weights[i]?.Value ?? 0;
            if (weight > 0)
                totalWeight += weight;
        }
        
        if (totalWeight <= 0)
            return 0;
        
        // generate random value
        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        
        // find the path based on cumulative weights
        int cumulativeWeight = 0;
        for (int i = 0; i < pathCount; i++)
        {
            int weight = Weights[i]?.Value ?? 0;
            if (weight > 0)
            {
                cumulativeWeight += weight;
                if (randomValue < cumulativeWeight)
                    return i;
            }
        }
        
        // fallback to first path
        return 0;
    }

    public NodePort PrevPort => PrevCell;
    public NodePort NextPort 
    {
        get
        {
            if (PathIndex < 0 || PathIndex >= Paths.Length)
            {
                throw new Exception("CL:Unexpected pathway");
                return null;
            }
            return Paths[PathIndex];
        }
    }

    public IEnumerable<ILogicNode> Prevs => PrevCell.Values;
    public ILogicNode Next
    {
        get
        {
            var node = NextPort.Connection?.Node as ILogicNode;
            while (node != null && node is ILogicConnector)
                node = node.Next;

            _pathIndex = null;
            
            return node;
        }
    }

    public void Execute()
    {
    }
}