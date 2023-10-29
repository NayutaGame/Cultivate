
using UnityEngine;

public class AppCanvas : MonoBehaviour
{
    [SerializeField] public TitlePanel TitlePanel;
    [SerializeField] public SettingsPanel SettingsPanel;

    public void Configure()
    {
        TitlePanel.Configure();
        SettingsPanel.Configure();
    }

    public void Refresh()
    {
        TitlePanel.Refresh();
        SettingsPanel.Refresh();
    }
}
