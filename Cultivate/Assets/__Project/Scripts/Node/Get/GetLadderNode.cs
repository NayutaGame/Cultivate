
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Get/Get Ladder", -10, true)]
public class GetLadderNode : Node
{
    [SerializeField]
    private OutputPort<int> value = new(self => (self as GetLadderNode).GetValue());

    public int GetValue()
    {
        if (Graph.Blackboard == null)
            return default;

        string key = "Ladder";
        Graph.Blackboard.TryGetLocalValue(Graph, key, out int value);
        return value;
    }
}