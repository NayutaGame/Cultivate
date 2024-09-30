
public class RunConfig : Config
{
    public CharacterProfile CharacterProfile;
    public DifficultyProfile DifficultyProfile;

    public MapEntry MapEntry;

    public RunConfig(RunConfigForm runConfigForm)
    {
        CharacterProfile = runConfigForm.CharacterProfile;
        DifficultyProfile = runConfigForm.DifficultyProfile;

        MapEntry = "标准";
    }
}
