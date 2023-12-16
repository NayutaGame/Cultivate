
public class CharacterEntry : Entry
{
    public string Description;
    public string AbilityDescription;

    public CharacterEntry(string name, string description = null, string abilityDescription = null) : base(name)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";
    }

    public static implicit operator CharacterEntry(string name) => Encyclopedia.CharacterCategory[name];
}
