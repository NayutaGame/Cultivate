
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("GetNpc", -10, true)]
public class GetNPCNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<string> CharacterName;
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField]
    private OutputPort<int> Power = new(self => (self as GetNPCNode).GetPower());
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField]
    private OutputPort<int> Relation = new(self => (self as GetNPCNode).GetRelation());

    private int GetPower()
    {
        if (!Application.isPlaying)
            return default;

        CharacterEntry characterEntry = Encyclopedia.CharacterCategory.FromName(CharacterName.Value);
        if (characterEntry == null)
            return default;
        
        RunNPC npc = RunManager.Instance.Environment.Map.GetNpc(characterEntry);
        if (npc == null)
            return default;
        
        return npc.Power;
    }

    private int GetRelation()
    {
        if (!Application.isPlaying)
            return default;

        CharacterEntry characterEntry = Encyclopedia.CharacterCategory.FromName(CharacterName.Value);
        if (characterEntry == null)
            return default;
        
        RunNPC npc = RunManager.Instance.Environment.Map.GetNpc(characterEntry);
        if (npc == null)
            return default;
        
        return npc.Relation;
    }
}