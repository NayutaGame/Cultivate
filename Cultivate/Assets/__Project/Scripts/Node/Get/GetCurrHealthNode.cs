
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Get/Get Curr Health", -10, true)]
public class GetCurrHealthNode : Node
{
    [SerializeField]
    private OutputPort<int> Value = new(self => (self as GetCurrHealthNode).GetValue());

    private int GetValue()
    {
        if (!Application.isPlaying)
            return 0;
        return RunManager.Instance.Environment.Home.GetHealth();
    }
}