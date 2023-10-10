
using CLLibrary;
using UnityEngine;

public class AppCanvas : Singleton<AppCanvas>
{
    [SerializeField] public TitlePanel TitlePanel;
    [SerializeField] public SettingsPanel SettingsPanel;
    [SerializeField] public TutorialPanel TutorialPanel;

    public override void DidAwake()
    {
        base.DidAwake();
        Configure();
        Refresh();
    }

    public void Configure()
    {
        TitlePanel.Configure();
        // SettingsPanel.Configure();
        TutorialPanel.Configure();
    }

    public void Refresh()
    {
        TitlePanel.Refresh();
        // SettingsPanel.Refresh();
        TutorialPanel.Refresh();
    }
}
