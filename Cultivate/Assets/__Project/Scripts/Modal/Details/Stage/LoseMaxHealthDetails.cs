
public class LoseMaxHealthDetails : NestedStageClosureDetails
{
    public StageEntity Entity;
    public int Value;

    public LoseMaxHealthDetails(StageEntity entity, int value, StageClosureListener listener, ResultDict castResult, StageClosure[] closures, bool induced)
    {
        Entity = entity;
        Value = value;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
        Induced = induced;
    }
    
    public LoseMaxHealthDetails ShallowClone() => new(Entity, Value, Listener, CastResult, Closures, Induced);
}