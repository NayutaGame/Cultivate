
public class GainFormationDetails : StageClosureDetails
{
    public StageEntity Owner;
    public RunFormation _formation;
    public bool _recursive;

    public GainFormationDetails(StageEnvironment env, StageEntity owner, RunFormation formation, bool recursive = true) : base(env)
    {
        Owner = owner;
        _formation = formation;
        _recursive = recursive;
    }

    public GainFormationDetails Clone() => new(Env, Owner, _formation, _recursive);
}
