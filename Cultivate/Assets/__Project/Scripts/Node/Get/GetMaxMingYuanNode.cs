
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Get/Get Max MingYuan", -10, true)]
public class GetMaxMingYuanNode : Node
{
    [SerializeField]
    private OutputPort<int> Value = new(self => (self as GetMaxMingYuanNode).GetValue());

    private int GetValue()
    {
        if (!Application.isPlaying)
            return 0;
        return RunManager.Instance.Environment.GetMaxMingYuan();
    }
}