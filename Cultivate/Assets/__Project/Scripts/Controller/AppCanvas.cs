
using UnityEngine;

public class AppCanvas : MonoBehaviour
{
    [SerializeField] public TitlePanel TitlePanel;
    [SerializeField] public SettingsPanel SettingsPanel;

    public void Configure()
    {
        SettingsPanel.Configure();
        TitlePanel.Configure();
    }
}
