
using System.Collections.Generic;

public class CharacterEntry : Entry
{
    public string Description;
    public string AbilityDescription;

    public Dictionary<int, CLEventDescriptor> _eventDescriptorDict;

    public CharacterEntry(string name, string description = null, string abilityDescription = null,
        params CLEventDescriptor[] eventDescriptors) : base(name)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";

        _eventDescriptorDict = new Dictionary<int, CLEventDescriptor>();
        foreach (var eventDescriptor in eventDescriptors)
            _eventDescriptorDict[eventDescriptor.EventId] = eventDescriptor;
    }

    public static implicit operator CharacterEntry(string name) => Encyclopedia.CharacterCategory[name];
}
