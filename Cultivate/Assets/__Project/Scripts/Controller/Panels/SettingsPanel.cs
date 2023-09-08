
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    private IndexPath _indexPath;

    [SerializeField] private TMP_Text CurrLabel;
    [SerializeField] private TMP_Text[] OtherLabels;

    [SerializeField] private Button ToTitleButton;
    [SerializeField] private Button ToDesktopButton;
    [SerializeField] private Button ResumeButton;

    public void Configure()
    {
        _indexPath = new IndexPath("App.Settings");
        Settings settings = DataManager.Get<Settings>(_indexPath);

        CurrLabel.text = $"<rotate=90>{settings.GetCurrentContentModel().Name}";

        for (int i = 0; i < settings.GetOtherContentCount(); i++)
        {
            SettingsContentModel content = settings.GetOtherContent(i);
            OtherLabels[i].text = content.Name;
        }

        ResumeButton.onClick.RemoveAllListeners();
        ResumeButton.onClick.AddListener(Resume);
    }

    public void Refresh()
    {
    }

    private void Resume()
    {
        AppManager.Pop();
    }
}
