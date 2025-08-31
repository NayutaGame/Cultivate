
public class PlacedSkill
{
    public RunSkill RunSkill;
    public SkillEntry Entry;
    public JingJie JingJie;

    private PlacedSkill(RunSkill runSkill, SkillEntry entry, JingJie jingJie)
    {
        RunSkill = runSkill;
        Entry = entry;
        JingJie = jingJie;
    }

    public static PlacedSkill FromRunSkill(RunSkill runSkill)
        => new(runSkill, runSkill.GetEntry(), runSkill.GetJingJie());

    public static PlacedSkill FromEntry(SkillEntry entry)
        => new(null, entry, entry.LowestJingJie);

    public static PlacedSkill FromEntryAndJingJie(SkillEntry entry, JingJie jingJie)
        => new(null, entry, jingJie);

    public SkillDefinition GetSkillDefinitionFromDj(int dj)
    {
        if (RunSkill != null)
            return RunSkill.GetSkillDefinitionFromDj(dj);
        return Entry.GetSkillDefinitionFromDj(dj);
    }
}
