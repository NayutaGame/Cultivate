
public class RunCharacter
{
    public CharacterEntry CharacterEntry;
    
    private RunEntity _build;
    public RunEntity Build
    {
        get => _build;
        set
        {
            // _build?.ChangedNeuron.Remove(RunManager.Instance.Environment.ResimulateNeuron);
            _build = value;
            // _build?.ChangedNeuron.Add(RunManager.Instance.Environment.ResimulateNeuron);
        }
    }

    public RunCharacter(CharacterEntry characterEntry)
    {
        CharacterEntry = characterEntry;
    }
}