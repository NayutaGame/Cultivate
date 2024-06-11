
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
        await TitlePanel.SetStateAsync(0);
        await RunConfigPanel.SetStateAsync(1);
    }

    public async void CloseRunConfigPanel()
    {
        await RunConfigPanel.SetStateAsync(0);
        await TitlePanel.SetStateAsync(1);
    }
}
