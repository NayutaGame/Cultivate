
using System;

public class CharacterEntry : Entry
{
    public string Description;
    public string AbilityDescription;

    public RunEventDescriptor[] _runEventDescriptors;
    public StageEventDescriptor[] _stageEventDescriptors;

    public CharacterEntry(string name, string description = null, string abilityDescription = null,
        RunEventDescriptor[] runEventDescriptors = null,
        StageEventDescriptor[] stageEventDescriptors = null) : base(name)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";

        _runEventDescriptors = runEventDescriptors ?? Array.Empty<RunEventDescriptor>();
        _stageEventDescriptors = stageEventDescriptors ?? Array.Empty<StageEventDescriptor>();
    }

    public static implicit operator CharacterEntry(string name) => Encyclopedia.CharacterCategory[name];
}
