
using CLLibrary;
using UnityEngine;

public class AppCanvas : Singleton<AppCanvas>
{
    [SerializeField] public TitlePanel TitlePanel;
    [SerializeField] public SettingsPanel SettingsPanel;

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
    }

    public void Refresh()
    {
        TitlePanel.Refresh();
        // SettingsPanel.Refresh();
    }
}
