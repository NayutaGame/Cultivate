
public abstract class TechEventDescriptor
{
    private string _description;
    public string Description => _description;

    protected TechEventDescriptor(string description)
    {
        _description = description;
    }

    public abstract void Register(RunTech runTech);
    public abstract void Unregister(RunTech runTech);
}
