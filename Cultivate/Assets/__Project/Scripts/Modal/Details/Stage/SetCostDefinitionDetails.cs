
public class SetCostDefinitionDetails : StageClosureDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public CostDefinition CostDefinition;
    
    public SetCostDefinitionDetails(StageEnvironment env, StageEntity caster, StageSkill skill) : base(env)
    {
        Caster = caster;
        Skill = skill;
        CostDefinition = skill.GetSkillDefinition().GetCostDefinition();
    }
}