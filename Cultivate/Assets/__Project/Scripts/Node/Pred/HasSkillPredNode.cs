
using PuppyDragon.uNody;
using UnityEngine;

[CreateNodeMenu("Pred/Has Skill", -10, true)]
public class HasSkillPredNode : PredNode
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorRunSkillQuery> Query;

    public override bool GetResult()
    {
        return RunManager.Instance.Environment.DeckIndexFromQuery(out _, RunSkillQuery.FromEditorQuery(Query.Value));
    }
}