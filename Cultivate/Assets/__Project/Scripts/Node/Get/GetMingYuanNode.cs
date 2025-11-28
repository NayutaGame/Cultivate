
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Mini)]
[CreateNodeMenu("Get/Get MingYuan", -10, true)]
public class GetMingYuanNode : Node
{
    [SerializeField]
    private OutputPort<int> Value = new(self => (self as GetMingYuanNode).GetValue());

    private int GetValue()
    {
        if (!Application.isPlaying)
            return 0;
        return RunManager.Instance.Environment.GetCurrMingYuan();
    }
}