
using System.Collections.Generic;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Procedure/Pick Skills", -10, true)]
public class PickSkillsProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<string>> SkillNames;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<JingJieIndirect> PreferredJingJie = new();
    
    public override void Procedure()
    {
        JingJie preferredJingJie = JingJie.FromIndirect(PreferredJingJie.Value);
        List<SkillGhost> skillGhosts = new();
        foreach (string skillName in SkillNames.Value)
        {
            SkillEntry skillEntry = Encyclopedia.SkillCategory.FromName(skillName);
            if (skillEntry == null)
                continue;
            skillGhosts.Add(SkillGhost.FromEntryJingJie(skillEntry, preferredJingJie ?? skillEntry.LowestJingJie));
        }
        
        RunManager.Instance.Environment.PickSkillsProcedure(skillGhosts);
    }
}