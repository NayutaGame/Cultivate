
public class CharacterSelectDetails
{
    public CharacterProfile FromCharacter;
    public CharacterProfile ToCharacter;

    public int FromIndex;
    public int ToIndex;

    public CharacterSelectDetails(CharacterProfile toCharacter)
    {
        ToCharacter = toCharacter;
    }
}
