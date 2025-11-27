
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Gain Gold", -10, true)]
public class GainGoldProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Value = new(0);
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.GainGoldProcedure(Value.Value);
    }
}