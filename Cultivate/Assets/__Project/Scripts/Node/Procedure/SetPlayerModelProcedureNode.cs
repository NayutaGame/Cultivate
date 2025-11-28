using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Set Player Model", -10, true)]
public class SetPlayerModelProcedureNode : ProcedureNode
{
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<string> ModelName;
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.SetPlayerModelProcedure(ModelName.Value);
    }
}