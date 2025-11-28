
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Get/Get Gold", -10, true)]
public class GetGoldNode : Node
{
    [SerializeField]
    private OutputPort<int> Value = new(self => (self as GetGoldNode).GetValue());

    private int GetValue()
    {
        if (!Application.isPlaying)
            return 0;
        return RunManager.Instance.Environment.GetCurrGold();
    }
}