
public class SettingsTabListModel : ListModel<SettingsTab>
{
    public SettingsTab GetSoundTab()
        => First(tab => tab.Name == "声音");
}
