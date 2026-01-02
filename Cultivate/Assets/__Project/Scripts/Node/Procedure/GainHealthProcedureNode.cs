
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Procedure/Gain Health", -10, true)]
public class GainHealthProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Value = new(0);
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.GainHealthProcedure(Value.Value);
    }
}