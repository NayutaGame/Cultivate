
using PuppyDragon.uNody;
using UnityEngine;

public class GetHealthValueFromLadderNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Ladder;
    
    [SpaceLine(-1)]
    [SerializeField]
    private OutputPort<int> Value = new(self => (self as GetHealthValueFromLadderNode).GetValue());

    private int GetValue()
    {
        if (!Application.isPlaying)
            return 0;
        return RoomDefinition.GetHealthRewardFromLadder(Ladder.Value);
    }
}