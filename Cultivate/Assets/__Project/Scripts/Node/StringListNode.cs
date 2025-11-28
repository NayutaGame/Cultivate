using System.Collections.Generic;
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("String List", -9, true)]
public class StringListNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private InputPort<string> String;
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private InputPort<int> Stack;
    
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<List<string>> Value = new(self => (self as StringListNode).GetValue());

    private List<string> GetValue()
    {
        List<string> toRet = new();
        int stack = Stack.Value.ClampLower(0);
        for (int i = 0; i < stack; i++)
            toRet.Add(String.Value);
        return toRet;
    }
}