
public class CostDetails : StageClosureDetails
{
    public StageEntity Entity;
    public StageSkill Skill;
    public CostDescription CostDescription;

    public bool Blocking = false;
    public int Counter;
    public ResultDict CostResult;

    public int Value
    {
        get => CostDescription.Value;
        set => CostDescription.Value = value;
    }

    public CostState State
    {
        get => CostDescription.State;
        set => CostDescription.State = value;
    }

    public CostDetails(
        StageEnvironment env,
        StageEntity entity,
        StageSkill skill,
        CostDescription costDescription) : base(env)
    {
        Entity = entity;
        Skill = skill;
        CostDescription = costDescription;
        
        CostResult = new();
    }

    public int J => Skill.GetJingJie();
    public int Dj => Skill.Dj;
    public int Cc => Skill.TotalStageCastedCount;

    public void Clear()
    {
        CostResult.Clear();
    }
}
