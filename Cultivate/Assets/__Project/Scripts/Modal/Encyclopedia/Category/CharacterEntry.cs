
using System.Collections.Generic;

public class CharacterEntry : Entry
{
    public string Description;
    public string AbilityDescription;

    public Dictionary<int, RunEventDescriptor> _runEventDescriptorDict;
    public Dictionary<int, StageEventDescriptor> _stageEventDescriptorDict;

    public CharacterEntry(string name, string description = null, string abilityDescription = null,
        RunEventDescriptor[] runEventDescriptors = null,
        StageEventDescriptor[] stageEventDescriptors = null) : base(name)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";

        _runEventDescriptorDict = new();
        if (runEventDescriptors != null)
            foreach (var d in runEventDescriptors)
                _runEventDescriptorDict[d.EventId] = d;

        _stageEventDescriptorDict = new();
        if (stageEventDescriptors != null)
            foreach (var d in stageEventDescriptors)
                _stageEventDescriptorDict[d.EventId] = d;
    }

    public static implicit operator CharacterEntry(string name) => Encyclopedia.CharacterCategory[name];
}
