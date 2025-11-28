using System.Collections.Generic;
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Query/EditorSkillEntryQueryListFromStack", -10, true)]
public class EditorSkillEntryQueryListNodeFromStack : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private InputPort<EditorSkillEntryQuery> Item;
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private InputPort<int> Stack;
    
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<List<EditorSkillEntryQuery>> Value = new(self => (self as EditorSkillEntryQueryListNodeFromStack).GetValue());

    private List<EditorSkillEntryQuery> GetValue()
    {
        List<EditorSkillEntryQuery> toRet = new();
        int stack = Stack.Value.ClampLower(0);
        for (int i = 0; i < stack; i++)
            toRet.Add(Item.Value);
        return toRet;
    }
}