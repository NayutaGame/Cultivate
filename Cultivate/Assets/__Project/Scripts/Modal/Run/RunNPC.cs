
public class RunNPC
{
    private CharacterEntry _characterEntry;
    private RunEntity _build;

    private int _relation;
    private int _power;

    public RunNPC(CharacterEntry characterEntry)
    {
        _characterEntry = characterEntry;
    }

    public CharacterEntry CharacterEntry
        => _characterEntry;

    public int Relation => _relation;

    public int Power => _power;
}