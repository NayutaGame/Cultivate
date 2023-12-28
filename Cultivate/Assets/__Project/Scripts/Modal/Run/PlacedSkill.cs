
public class PlacedSkill
{
    public SkillEntry Entry;
    public JingJie JingJie;

    public static PlacedSkill FromEntry(SkillEntry entry)
        => new(entry, entry.LowestJingJie);

    public static PlacedSkill FromEntryAndJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry, jingJie);

    private PlacedSkill(SkillEntry entry, JingJie jingJie)
    {
        Entry = entry;
        JingJie = jingJie;
    }
}
