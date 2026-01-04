using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Get/Get Curr JingJie", -10, true)]
public class GetCurrJingJieNode : Node
{
    [SerializeField]
    private OutputPort<JingJieIndirect> Value = new(self => (self as GetCurrJingJieNode).GetValue());
    
    [SerializeField]
    private OutputPort<int> Ordinal = new(self => (self as GetCurrJingJieNode).GetOrdinal());

    private JingJieIndirect GetValue()
    {
        if (!Application.isPlaying)
            return JingJieIndirect.练气;
        return JingJie.ToIndirect(RunManager.Instance.Environment.Map.JingJie);
    }

    private int GetOrdinal()
    {
        if (!Application.isPlaying)
            return 0;
        return RunManager.Instance.Environment.Map.JingJie.GetIndex();
    }
}