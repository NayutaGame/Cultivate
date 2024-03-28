
public class AddSkillDetails
{
    public SkillEntry _entry;
    public JingJie _jingJie;

    public AddSkillDetails(SkillEntry entry, JingJie? jingJie = null)
    {
        _entry = entry;
        _jingJie = jingJie ?? entry.LowestJingJie;
    }
}
