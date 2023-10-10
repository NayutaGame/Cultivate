
public sealed class TutorialList : ListModel<TutorialDescriptor>
{
    public TutorialList()
    {
        Add(new(new RunConfig(env =>
        {
            env.Away = RunEntity.Default();
            env.Away.GetSlot(10).Skill = RunSkill.From("流沙", JingJie.LianQi);
            env.Away.GetSlot(11).Skill = RunSkill.From("流沙", JingJie.LianQi);
            env.Away.GetSlot(12).Skill = RunSkill.From("流沙", JingJie.LianQi);

            env.SkillInventory.Add(RunSkill.From("恋花", JingJie.LianQi));
            env.SkillInventory.Add(RunSkill.From("恋花", JingJie.LianQi));
            env.SkillInventory.Add(RunSkill.From("恋花", JingJie.LianQi));
        })));
        Add(new(new RunConfig(env =>
        {

        })));
        Add(new(new RunConfig(env =>
        {

        })));
        Add(new(new RunConfig(env =>
        {

        })));
    }
}
