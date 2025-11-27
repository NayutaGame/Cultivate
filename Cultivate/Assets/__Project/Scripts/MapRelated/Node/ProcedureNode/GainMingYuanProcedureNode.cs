
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Gain MingYuan", -10, true)]
public class GainMingYuanProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Value = new(1);
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.GainMingYuanProcedure(Value.Value);
    }
}