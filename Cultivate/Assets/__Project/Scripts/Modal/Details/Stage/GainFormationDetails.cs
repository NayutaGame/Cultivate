
public class GainFormationDetails : ClosureDetails
{
    public StageEntity Owner;
    public RunFormation _formation;
    public bool _recursive;

    public GainFormationDetails(StageEntity owner, RunFormation formation, bool recursive = true)
    {
        Owner = owner;
        _formation = formation;
        _recursive = recursive;
    }

    public GainFormationDetails Clone() => new(Owner, _formation, _recursive);
}
