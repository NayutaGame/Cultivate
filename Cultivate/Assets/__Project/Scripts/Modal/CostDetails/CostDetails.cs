
public class CostDetails : StageClosureDetails
{
    public StageEnvironment Env;
    public StageEntity Entity;
    public StageSkill Skill;

    public ResultDict CostResult;
    public CostDescription CostDescription;
    
    public bool Blocking = false;
    public int Counter;

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
        StageSkill skill)
    {
        Env = env;
        Entity = entity;
        Skill = skill;
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
