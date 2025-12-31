
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Procedure/Commit Run", -10, true)]
public class CommitRunProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> IsWin;
    
    public override void Procedure()
    {
        RunManager.Instance.Environment.CommitRunProcedure(IsWin.Value
            ? RunResult.RunOutcome.Victorious
            : RunResult.RunOutcome.Defeated);
    }
}