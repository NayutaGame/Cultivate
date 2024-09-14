
public class RunConfig : Config
{
    public CharacterProfile CharacterProfile;
    public DifficultyProfile DifficultyProfile;

    public MapEntry MapEntry;

    public DesignerConfig DesignerConfig;

    public RunConfig(RunConfigForm runConfigForm, DesignerConfig designerConfig)
    {
        CharacterProfile = runConfigForm.CharacterProfile;
        DifficultyProfile = runConfigForm.DifficultyProfile;

        MapEntry = "标准";

        DesignerConfig = designerConfig;
    }
}
