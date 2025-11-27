
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Procedure/Pick Skill", -10, true)]
public class PickSkillProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> SkillName;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorJingJie> PreferredJingJie = new(EditorJingJie.任意);
    
    public override void Procedure()
    {
        SkillEntry skillEntry = Encyclopedia.SkillCategory.FromName(SkillName.Value);

        EditorJingJie preferredJingJieType = PreferredJingJie.Value;
        JingJie preferredJingJie = JingJie.FromEditor(preferredJingJieType);
        
        RunManager.Instance.Environment.PickSkillProcedure(skillEntry, preferredJingJie);
    }
}