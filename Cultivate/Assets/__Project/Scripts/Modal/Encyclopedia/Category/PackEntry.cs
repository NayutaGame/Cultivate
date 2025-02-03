
public class PackEntry : Entry, IPack
{
    public string Name;
    public WuXing? WuXing;
    public SkillEntry[] Cards;
    public string Trivia;
    
    public PackEntry(string id,
        string name,
        WuXing? wuXing,
        SkillEntry[] cards,
        string trivia = null
    ) : base(id)
    {
        Name = name;
        WuXing = wuXing;
        Cards = cards;
        Trivia = trivia;
    }
}
