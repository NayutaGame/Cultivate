
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("String/JingJieIndirect As String", -10, true)]
public class JingJieIndirectAsStringNode : Node
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<JingJieIndirect> Input;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, SpaceLine(-1)]
    private OutputPort<string> Output = new(x => (x as JingJieIndirectAsStringNode).GetValue());

    public string GetValue()
    {
        if (!Application.isPlaying)
            return "境界";
        return JingJie.FromIndirect(Input.Value).GetName();
    }
}