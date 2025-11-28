
using PuppyDragon.uNody;
using UnityEngine;

public class GetGoldValueFromLadderNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Ladder;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None)] [SerializeField] [SpaceLine(-1)]
    private OutputPort<int> Value = new(self => (self as GetGoldValueFromLadderNode).GetValue());

    private int GetValue()
    {
        if (!Application.isPlaying)
            return 0;
        return RoomDefinition.GetGoldRewardFromLadder(Ladder.Value);
    }
}