using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Get/Get Curr JingJie", -10, true)]
public class GetCurrJingJieNode : Node
{
    [SerializeField]
    private OutputPort<JingJieIndirect> Value = new(self => (self as GetCurrJingJieNode).GetValue());

    public JingJieIndirect GetValue()
    {
        if (!Application.isPlaying)
            return JingJieIndirect.练气;
        return JingJie.ToIndirect(RunManager.Instance.Environment.JingJie);
    }
}