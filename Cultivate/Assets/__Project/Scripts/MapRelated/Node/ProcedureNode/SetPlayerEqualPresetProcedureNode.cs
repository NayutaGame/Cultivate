
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Set Player Equal Preset", -9, true)]
public class SetPlayerEqualPresetProcedureNode : ProcedureNode
{
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<string> TemplateName;
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<bool> ToField;
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<bool> Overwrite;
    
    public override void Procedure()
    {
        if (!Application.isPlaying)
            return;
        RunManager.Instance.Environment.SetPlayerEqualPresetProcedure(TemplateName.Value, ToField.Value, Overwrite.Value);
    }
}