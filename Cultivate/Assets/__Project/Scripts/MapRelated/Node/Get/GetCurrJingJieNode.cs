
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Get/Curr JingJie", -10, true)]
public class GetCurrJingJieNode : Node
{
    [SerializeField]
    private OutputPort<int> Value = new(self => (self as GetCurrJingJieNode).GetValue());
    
    [SerializeField]
    private OutputPort<EditorJingJie> JingJie = new(self => (self as GetCurrJingJieNode).GetJingJie());

    private int GetValue()
    {
        if (!Application.isPlaying)
            return 0;
        return RunManager.Instance.Environment.JingJie;
    }

    private EditorJingJie GetJingJie()
    {
        return EditorJingJie.当前;
    }
}