
public class FormationDetails : ClosureDetails
{
    public StageEntity Owner;
    public RunFormation _formation;
    public bool _recursive;

    public FormationDetails(StageEntity owner, RunFormation formation, bool recursive = true)
    {
        Owner = owner;
        _formation = formation;
        _recursive = recursive;
    }
}
