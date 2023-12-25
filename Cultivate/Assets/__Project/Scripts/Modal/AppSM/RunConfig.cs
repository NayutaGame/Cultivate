
public class RunConfig : Config
{
    public CharacterProfile CharacterProfile;
    public DifficultyProfile DifficultyProfile;

    public DesignerConfig DesignerConfig;

    public RunConfig(RunConfigForm runConfigForm, DesignerConfig designerConfig)
    {
        CharacterProfile = runConfigForm.CharacterProfile;
        DifficultyProfile = runConfigForm.DifficultyProfile;

        DesignerConfig = designerConfig;
    }
}
