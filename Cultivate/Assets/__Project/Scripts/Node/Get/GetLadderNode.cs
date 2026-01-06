
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Get/Get Ladder", -10, true)]
public class GetLadderNode : Node
{
    [SerializeField]
    private OutputPort<int> value = new(self => (self as GetLadderNode).GetValue());
    
    [SerializeField]
    private OutputPort<bool> IsOverHalf = new(self => (self as GetLadderNode).IsOverHalfValue());

    public int GetValue()
    {
        if (!Application.isPlaying)
            return 0;

        return RunManager.Instance.Environment.Map._room.GetLadder();
    }

    public bool IsOverHalfValue()
    {
        if (!Application.isPlaying)
            return false;

        return RunManager.Instance.Environment.Map._room.IsOverHalf();
    }
}