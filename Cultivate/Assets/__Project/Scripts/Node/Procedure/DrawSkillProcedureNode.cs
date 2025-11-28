
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Draw Skill", -10, true)]
public class DrawSkillProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorSkillEntryQuery> DrawStrategy;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<JingJieIndirect> PreferredJingJie = new();
    
    public override void Procedure()
    {
        JingJie preferredJingJie = JingJie.FromIndirect(PreferredJingJie.Value);

        SkillEntryQuery query = SkillEntryQuery.FromEditorQuery(DrawStrategy.Value);
        
        RunManager.Instance.Environment.DrawSkillProcedure(query, preferredJingJie);
    }
}