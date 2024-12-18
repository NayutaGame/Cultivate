
using UnityEngine;

public class AppCanvas : MonoBehaviour
{
    [SerializeField] public TitlePanel TitlePanel;
    [SerializeField] public RunConfigPanel RunConfigPanel;
    [SerializeField] public SettingsPanel SettingsPanel;

    public void Configure()
    {
        TitlePanel.CheckAwake();
        RunConfigPanel.CheckAwake();
        SettingsPanel.CheckAwake();
    }
}
