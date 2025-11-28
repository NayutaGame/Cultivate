
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Procedure/Lose MingYuan", -10, true)]
public class LoseMingYuanProcedureNode : ProcedureNode
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Value = new(1);
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.LoseMingYuanProcedure(Value.Value);
    }
}