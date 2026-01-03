using System.Collections.Generic;
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Query/EditorRunSkillQueryListFromStack", -10, true)]
public class EditorRunSkillQueryListNodeFromStack : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private InputPort<EditorRunSkillQuery> Item;
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private InputPort<int> Stack;
    
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<List<EditorRunSkillQuery>> Value = new(self => (self as EditorRunSkillQueryListNodeFromStack).GetValue());

    private List<EditorRunSkillQuery> GetValue()
    {
        List<EditorRunSkillQuery> toRet = new();
        int stack = Stack.Value.ClampLower(0);
        for (int i = 0; i < stack; i++)
            toRet.Add(Item.Value);
        return toRet;
    }
}