
using PuppyDragon.uNody;
using UnityEngine;

public class GetJingJieFromLadderNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Ladder;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None)] [SerializeField] [SpaceLine(-1)]
    private OutputPort<JingJieIndirect> Value = new(self => (self as GetJingJieFromLadderNode).GetValue());

    private JingJieIndirect GetValue()
    {
        if (!Application.isPlaying)
            return JingJieIndirect.练气;
        JingJie jingJie = RoomDefinition.GetJingJieFromLadder(Ladder.Value);
        return JingJie.ToIndirect(jingJie);
    }
}