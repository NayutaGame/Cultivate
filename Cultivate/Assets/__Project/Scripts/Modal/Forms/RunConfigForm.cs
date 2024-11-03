
public class RunConfigForm
{
    public CharacterProfile CharacterProfile;
    public DifficultyProfile DifficultyProfile;

    public RunConfigForm(CharacterProfile characterProfile, DifficultyProfile difficultyProfile)
    {
        CharacterProfile = characterProfile;
        DifficultyProfile = difficultyProfile;
    }

    public static RunConfigForm FirstTime()
    {
        Profile profile = AppManager.Instance.ProfileManager.ProfileList[0];
        return new(profile.CharacterProfileList[0], profile.DifficultyProfileList[0]);
    }
}
