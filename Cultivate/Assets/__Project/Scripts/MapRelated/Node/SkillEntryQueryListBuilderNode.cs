using System.Collections.Generic;
using CLLibrary;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(500)]
[CreateNodeMenu("Variable/SkillEntryQueryListBuilder", -7, true)]
public class SkillEntryQueryListBuilderNode : Node
{
    // [SerializeField]
    // private List<EditorSkillEntryQuery> _queryList = new List<EditorSkillEntryQuery>();
    
    // [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] 
    // [SerializeField]
    // private OutputPort<List<EditorSkillEntryQuery>> value = new(self => (self as SkillEntryQueryListBuilderNode).GetQueryList());
    
    // public List<SkillEntryQuery> GetQueryList()
    // {
    //     if (!Application.isPlaying)
    //         return null;
    //         
    //     List<SkillEntryQuery> result = new List<SkillEntryQuery>();
    //     foreach (var data in _queryList)
    //     {
    //         var query = data.ToQuery();
    //         if (query != null)
    //         {
    //             result.Add(query);
    //         }
    //     }
    //     return result;
    // }
    //
    // public List<EditorSkillEntryQuery> QueryList => _queryList;
}
