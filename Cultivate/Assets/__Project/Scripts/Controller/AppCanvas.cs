
using UnityEngine;

public class AppCanvas : MonoBehaviour
{
    [SerializeField] public TitlePanel TitlePanel;
    [SerializeField] public SettingsPanel SettingsPanel;
    [SerializeField] public RunConfigPanel RunConfigPanel;

    public void Configure()
    {
        TitlePanel.Configure();
        SettingsPanel.Configure();
        RunConfigPanel.Configure();
    }

    public async void OpenRunConfigPanel()
    {
        await TitlePanel.SetShowing(false);
        await RunConfigPanel.SetShowing(true);
    }

    public async void CloseRunConfigPanel()
    {
        await RunConfigPanel.SetShowing(false);
        await TitlePanel.SetShowing(true);
    }
}
