
using System.Collections.Generic;
using PuppyDragon.uNody;
using Sirenix.OdinInspector;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Procedure/Draw Skills", -10, true)]
public class DrawSkillsProcedureNode : ProcedureNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<EditorSkillEntryQuery>> DrawStrategies;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<JingJieIndirect> PreferredJingJie = new();
    
    public override void Procedure()
    {
        JingJie preferredJingJie = JingJie.FromIndirect(PreferredJingJie.Value);

        List<SkillEntryQuery> queries = SkillEntryQuery.FromEditorQueries(DrawStrategies.Value, preferredJingJie);
        
        RunManager.Instance.Environment.DrawSkillsProcedure(queries, preferredJingJie);
    }
}