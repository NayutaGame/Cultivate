
using CLLibrary;
using UnityEngine;

public class AppCanvas : Singleton<AppCanvas>
{
    [SerializeField] private SettingsPanel SettingsPanel;

    public override void DidAwake()
    {
        base.DidAwake();
        Configure();
        Refresh();
    }

    public void Configure()
    {
        SettingsPanel.Configure();
    }

    public void Refresh()
    {
        SettingsPanel.Refresh();
    }
}
