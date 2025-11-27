
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Draw Skill", -10, true)]
public class DrawSkillProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorSkillEntryQuery> DrawStrategy;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorJingJie> PreferredJingJie = new(EditorJingJie.任意);
    
    public override void Procedure()
    {
        EditorJingJie preferredEditorJingJie = PreferredJingJie.Value;
        JingJie preferredJingJie = JingJie.FromEditor(preferredEditorJingJie);

        SkillEntryQuery query = SkillEntryQuery.FromEditorQuery(DrawStrategy.Value);
        
        RunManager.Instance.Environment.DrawSkillProcedure(query, preferredJingJie);
    }
}