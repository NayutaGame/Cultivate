
using UnityEngine;

public class AppCanvas : MonoBehaviour
{
    [SerializeField] public TitlePanel TitlePanel;
    [SerializeField] public RunConfigPanel RunConfigPanel;
    [SerializeField] public EntityEditorPanel EntityEditorPanel;
    [SerializeField] public SkillBrowserPanel SkillBrowserPanel;
    
    [SerializeField] public SettingsPanel SettingsPanel;

    public void Configure()
    {
        TitlePanel.CheckAwake();
        RunConfigPanel.CheckAwake();
        EntityEditorPanel.CheckAwake();
        SkillBrowserPanel.CheckAwake();
        
        SettingsPanel.CheckAwake();
    }
}
