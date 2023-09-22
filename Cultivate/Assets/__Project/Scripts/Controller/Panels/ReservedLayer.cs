using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReservedLayer : MonoBehaviour
{
    public Button TechButton;
    public Button EntityEditorButton;
    public Button ArenaButton;
    public Button SkillBrowserButton;
    public Button FormationBrowserButton;
    public Button CloseButton;

    public TechTreePanel TechTreePanel;
    public EntityEditorPanel EntityEditorPanel;
    public ArenaPanel ArenaPanel;
    public SkillBrowserPanel SkillBrowserPanel;
    public FormationBrowserPanel FormationBrowserPanel;

    private Panel _currentPanel;

    public void Configure()
    {
        TechButton.onClick.RemoveAllListeners();
        TechButton.onClick.AddListener(OpenTechTreePanel);

        EntityEditorButton.onClick.RemoveAllListeners();
        EntityEditorButton.onClick.AddListener(OpenEntityEditorPanel);

        ArenaButton.onClick.RemoveAllListeners();
        ArenaButton.onClick.AddListener(OpenArenaPanel);

        SkillBrowserButton.onClick.RemoveAllListeners();
        SkillBrowserButton.onClick.AddListener(OpenSkillBrowserPanel);

        FormationBrowserButton.onClick.RemoveAllListeners();
        FormationBrowserButton.onClick.AddListener(OpenFormationBrowserPanel);

        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(ClosePanel);
    }

    public void Refresh()
    {
        if(_currentPanel != null)
            _currentPanel.Refresh();
    }

    public void OpenTechTreePanel() => ChangePanel(TechTreePanel);
    public void OpenEntityEditorPanel() => ChangePanel(EntityEditorPanel);
    public void OpenArenaPanel() => ChangePanel(ArenaPanel);
    public void OpenSkillBrowserPanel() => ChangePanel(SkillBrowserPanel);
    public void OpenFormationBrowserPanel() => ChangePanel(FormationBrowserPanel);
    public void ClosePanel() => ChangePanel(null);

    private void ChangePanel(Panel next)
    {
        if (_currentPanel != null)
            _currentPanel.gameObject.SetActive(false);
        _currentPanel = next;
        if (_currentPanel != null)
        {
            _currentPanel.gameObject.SetActive(true);
            _currentPanel.Refresh();
        }
    }
}
