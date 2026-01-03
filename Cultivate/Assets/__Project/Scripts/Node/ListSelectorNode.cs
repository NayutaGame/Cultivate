
using System;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("List Selector", -10, true)]
public class ListSelectorNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<int> Index;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<int>[] List;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<int> Result = new(self => (self as ListSelectorNode).GetResult());

    public int GetResult()
    {
        int index = Index.Value;
        if (0 <= index && index < List.Length)
            return List[index].Value;

        throw new Exception("CL:unexpected pathway");
    }
}