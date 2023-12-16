
public class DifficultyEntry : Entry
{
    public string Description;

    public DifficultyEntry(string name, string description = null) : base(name)
    {
        Description = description ?? "没有描述";
    }
}
