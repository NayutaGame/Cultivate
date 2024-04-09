
public interface EmulatedSkill : ISkillModel
{
    SkillSlot GetSkillSlot();
    void SetSkillSlot(SkillSlot value);

    SkillEntry GetEntry();

    int GetRunUsedTimes();
    void SetRunUsedTimes(int value);

    int GetRunEquippedTimes();
    void SetRunEquippedTimes(int value);
}
