
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
        await TitlePanel.AsyncSetState(0);
        await RunConfigPanel.AsyncSetState(1);
    }

    public async void CloseRunConfigPanel()
    {
        await RunConfigPanel.AsyncSetState(0);
        await TitlePanel.AsyncSetState(1);
    }
}
