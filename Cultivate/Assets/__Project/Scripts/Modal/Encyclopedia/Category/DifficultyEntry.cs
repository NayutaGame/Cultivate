
public class DifficultyEntry : Entry
{
    public string Description;

    public DifficultyEntry(string name, string description = null) : base(name)
    {
        Description = description ?? "没有描述";
    }

    public static implicit operator DifficultyEntry(string name) => Encyclopedia.DifficultyCategory[name];
}
