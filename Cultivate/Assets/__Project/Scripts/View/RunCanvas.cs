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

    public ChipPreview ChipPreview;
    public GhostProduct GhostProduct;
    public GhostChip GhostChip;

    public Button CharacterButton;
    public Button TechButton;
    public Button SimulateButton;
    public Button ArenaButton;
    public Button MapButton;
    public Button NodeButton;

    public CharacterPanel CharacterPanel;
    public TechTreePanel TechTreePanel;
    public SimulatePanel SimulatePanel;
    public ArenaPanel ArenaPanel;
    public MapPanel MapPanel;
    public NodePanel NodePanel;

    private Panel _currentPanel;

    public TMP_Text TurnText;
    public Button TurnButton;
    public TMP_Text XiuWeiText;
    public TMP_Text TurnXiuWeiText;
    public Button XiuWeiButton;
    public TMP_Text ChanNengText;
    public TMP_Text TurnChanNengText;
    public Button ChanNengButton;

    public override void DidAwake()
    {
        base.DidAwake();

        CharacterButton.onClick.AddListener(OpenCharacterPanel);
        TechButton.onClick.AddListener(OpenTechTreePanel);
        SimulateButton.onClick.AddListener(OpenSimulatePanel);
        ArenaButton.onClick.AddListener(OpenArenaPanel);
        MapButton.onClick.AddListener(OpenMapPanel);
        NodeButton.onClick.AddListener(OpenNodePanel);

        TurnButton.onClick.AddListener(AddTurn);
        XiuWeiButton.onClick.AddListener(AddXiuWei);
        ChanNengButton.onClick.AddListener(AddChanNeng);

        Refresh();
    }

    private void OpenCharacterPanel() => OpenPanel(CharacterPanel);
    private void OpenTechTreePanel() => OpenPanel(TechTreePanel);
    private void OpenSimulatePanel() => OpenPanel(SimulatePanel);
    private void OpenArenaPanel() => OpenPanel(ArenaPanel);
    private void OpenMapPanel() => OpenPanel(MapPanel);
    private void OpenNodePanel() => OpenPanel(NodePanel);
    private void OpenPanel(Panel panel)
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

        TurnText.text = RunManager.Instance.Turn.ToString();
        XiuWeiText.text = RunManager.Instance.XiuWei.ToString();
        TurnXiuWeiText.text = $"+{RunManager.Instance.TurnXiuWei}";
        ChanNengText.text = RunManager.Instance.ChanNeng.ToString();
        TurnChanNengText.text = $"+{RunManager.Instance.TurnChanNeng}";
    }

    public void AddTurn()
    {
        RunManager.Instance.AddTurn();
        Refresh();
    }

    public void AddXiuWei()
    {
        RunManager.Instance.AddXiuWei();
        Refresh();
    }

    public void AddChanNeng()
    {
        RunManager.Instance.AddChanNeng();
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }
}
