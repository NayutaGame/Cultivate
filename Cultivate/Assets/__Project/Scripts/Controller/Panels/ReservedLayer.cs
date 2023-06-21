using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReservedLayer : MonoBehaviour
{
    public Button TechButton;
    public Button SimulateButton;
    public Button ArenaButton;
    public Button LibraryButton;

    public TechTreePanel TechTreePanel;
    public SimulatePanel SimulatePanel;
    public ArenaPanel ArenaPanel;
    public LibraryPanel LibraryPanel;

    private Panel _currentPanel;

    public void Configure()
    {
        TechButton.onClick.AddListener(OpenTechTreePanel);
        SimulateButton.onClick.AddListener(OpenSimulatePanel);
        ArenaButton.onClick.AddListener(OpenArenaPanel);
        LibraryButton.onClick.AddListener(OpenLibraryPanel);
    }

    public void Refresh()
    {
        if(_currentPanel != null)
            _currentPanel.Refresh();
    }

    public void OpenTechTreePanel() => ChangePanel(TechTreePanel);
    public void OpenSimulatePanel() => ChangePanel(SimulatePanel);
    public void OpenArenaPanel() => ChangePanel(ArenaPanel);
    public void OpenLibraryPanel() => ChangePanel(LibraryPanel);

    private void ChangePanel(Panel next)
    {
        if (_currentPanel != null)
            _currentPanel.gameObject.SetActive(false);
        _currentPanel = next;
        _currentPanel.gameObject.SetActive(true);
        _currentPanel.Refresh();
    }
}
