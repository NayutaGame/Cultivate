
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Procedure/Lose Gold", -10, true)]
public class LoseGoldProcedureNode : ProcedureNode
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Value = new(1);
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.LoseGoldProcedure(Value.Value);
    }
}