
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Gain Skill From First Step And WuXing", -10, true)]
public class GainSkillFromFirstStepAndWuXingProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorWuXing> EditorWuXing;
    
    public override void Procedure()
    {
        if (!Application.isPlaying)
            return;
        RunManager.Instance.Environment.GainSkillFromFirstStepAndWuXingProcedure(WuXing.FromEditor(EditorWuXing.Value));
    }
}