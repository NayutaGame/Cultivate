
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("String/Int As String", -10, true)]
public class IntAsStringNode : Node
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> Input;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, SpaceLine(-1)]
    private OutputPort<string> Output = new(x => (x as IntAsStringNode).GetValue());

    public string GetValue()
    {
        return Input.Value.ToString();
    }
}