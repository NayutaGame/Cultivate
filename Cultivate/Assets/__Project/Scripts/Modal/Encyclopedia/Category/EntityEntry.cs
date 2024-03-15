
using System;

[Serializable]
public class EntityEntry : Entry
{
    public string GetName() => GetId();
    
    private string _description;
    public string Description => _description;

    public EntityEntry(string id, string description) : base(id)
    {
        _description = description;
    }

    public static implicit operator EntityEntry(string id) => Encyclopedia.EntityCategory[id];
}
