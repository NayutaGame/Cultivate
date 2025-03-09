
public class EngageEnemyDetails : RunClosureDetails
{
    public bool IsDummy;
    public RunEntity Entity;

    public EngageEnemyDetails(bool isDummy, RunEntity entity)
    {
        IsDummy = isDummy;
        Entity = IsDummy ? null : RunEntity.FromTemplate(entity);
    }
}
