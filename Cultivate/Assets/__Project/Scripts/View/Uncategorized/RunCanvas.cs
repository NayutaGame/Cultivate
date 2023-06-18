using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RunCanvas : Singleton<RunCanvas>
{
    public Color TechColorGreen;
    public Color TechColorYellow;
    public Color TechColorRed;

    [SerializeField] private RunSkillPreview RunSkillPreview;
    public SkillGhost SkillGhost;

    public TopBar TopBar;
    public Button ConsoleToggle;
    public DeckView DeckView;

    public Button TechButton;
    public Button SimulateButton;
    public Button ArenaButton;
    public Button LibraryButton;
    public Button MapButton;
    public Button NodeButton;

    public ConsolePanel ConsolePanel;

    public TechTreePanel TechTreePanel;
    public SimulatePanel SimulatePanel;
    public ArenaPanel ArenaPanel;
    public LibraryPanel LibraryPanel;
    public MapPanel MapPanel;
    public NodePanel NodePanel;

    private Panel _currentPanel;

    public override void DidAwake()
    {
        base.DidAwake();

        TopBar.Configure();
        ConsoleToggle.onClick.AddListener(ConsolePanel.ToggleShowing);
        DeckView.Configure();

        TechButton.onClick.AddListener(OpenTechTreePanel);
        SimulateButton.onClick.AddListener(OpenSimulatePanel);
        ArenaButton.onClick.AddListener(OpenArenaPanel);
        LibraryButton.onClick.AddListener(OpenLibraryPanel);
        MapButton.onClick.AddListener(OpenMapPanel);
        NodeButton.onClick.AddListener(OpenNodePanel);

        Refresh();
    }

    public void OpenTechTreePanel() => ChangePanel(TechTreePanel);
    public void OpenSimulatePanel() => ChangePanel(SimulatePanel);
    public void OpenArenaPanel() => ChangePanel(ArenaPanel);
    public void OpenLibraryPanel() => ChangePanel(LibraryPanel);
    public void OpenMapPanel() => ChangePanel(MapPanel);
    public void OpenNodePanel() => ChangePanel(NodePanel);
    private void ChangePanel(Panel panel)
    {
        if(_currentPanel != null)
            _currentPanel.gameObject.SetActive(false);
        _currentPanel = panel;
        _currentPanel.gameObject.SetActive(true);
        _currentPanel.Refresh();
    }

    public void Configure()
    {
        _currentPanel.Configure();
    }

    public void Refresh()
    {
        if(_currentPanel != null)
            _currentPanel.Refresh();
        TopBar.Refresh();
        ConsolePanel.Refresh();
        DeckView.Refresh();
    }

    public void SetIndexPathForPreview(IndexPath indexPath)
    {
        RunSkillPreview.Configure(indexPath);
        RunSkillPreview.Refresh();
    }

    public void UpdateMousePosForPreview(Vector2 pos)
    {
        RunSkillPreview.UpdateMousePos(pos);
        RunSkillPreview.Refresh();
    }
}
