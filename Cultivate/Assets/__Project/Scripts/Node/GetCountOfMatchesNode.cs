
using System.Linq;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[NodeHeaderTint(typeof(int))]
[CreateNodeMenu("Get/Count Matching Skills", -10, true)]
public class GetCountOfMatchesNode : Node
{
    [PortSettings(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<int> Result = new(self =>
    {
        if (!Application.isPlaying)
            return 0;
        return (self as GetCountOfMatchesNode).GetResult();
    });
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorRunSkillQuery> Query;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> ExcludingField;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> ExcludingHand;

    public int GetResult()
    {
        return RunManager.Instance.Environment.TraversalMatchingSkills(
            RunSkillQuery.FromEditorQuery(Query.Value),
            excludingField: ExcludingField.Value,
            excludingHand: ExcludingHand.Value).Count();
    }
}