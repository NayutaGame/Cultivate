
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Cell/Get String From Int Node", -10, true)]
public class GetStringFromIntNode : Node
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> Input;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited), SerializeField, SpaceLine(-1)]
    private OutputPort<string> Output = new(x => (x as GetStringFromIntNode).GetValue());

    public string GetValue()
    {
        return Input.Value.ToString();
    }
}