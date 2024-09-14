
public class BattleRunNode : RunNode
{
    private RunEntity _entity;
    public RunEntity Entity => _entity;

    public BattleRunNode(RunEntity entity) : base("战斗")
    {
        _entity = entity;
    }

    public override string GetName()
        => _entity.GetEntry().GetName();
}
