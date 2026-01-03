
using System.Collections.Generic;
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Weighted Random", -10, true)]
public class WeightedRandomNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<int>[] Weights;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<int> Result = new(self => (self as WeightedRandomNode).GetResult());

    private int _randomIndex = -1;

    private int GetResult()
    {
        if (_randomIndex != -1)
            return _randomIndex;

        List<int> weightList = new();
        foreach (InputPort<int> port in Weights)
        {
            weightList.Add(port?.Value ?? 0);
        }

        WeightedRandom.SelectWeightedIndex(out _randomIndex, weightList);
        return _randomIndex;
    }
}
