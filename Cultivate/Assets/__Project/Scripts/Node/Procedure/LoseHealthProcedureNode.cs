
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Procedure/Lose Health", -10, true)]
public class LoseHealthProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Value = new(0);
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.LoseHealthProcedure(Value.Value);
    }
}