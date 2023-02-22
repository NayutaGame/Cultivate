
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    public GameObject RunCanvas;
    public StageCanvas StageCanvas;

    public RectTransform GhostHolder;
    public ChipPreview ChipPreview;
    public Button CharacterButton;
    public Button BattleButton;

    public CharacterPanel CharacterPanel;
    public BattlePanel BattlePanel;

    private Panel _currentPanel;

    public override void DidAwake()
    {
        base.DidAwake();

        CharacterButton.onClick.AddListener(OpenCharacterPanel);
        BattleButton.onClick.AddListener(OpenBattlePanel);
    }

    private void OpenCharacterPanel()
    {
        if(_currentPanel != null)
            _currentPanel.gameObject.SetActive(false);
        _currentPanel = CharacterPanel;
        _currentPanel.gameObject.SetActive(true);
        _currentPanel.Refresh();
    }

    private void OpenBattlePanel()
    {
        if(_currentPanel != null)
            _currentPanel.gameObject.SetActive(false);
        _currentPanel = BattlePanel;
        _currentPanel.gameObject.SetActive(true);
        _currentPanel.Refresh();
    }

    public void Configure()
    {
        _currentPanel.Configure();
    }

    public void Refresh()
    {
        _currentPanel.Refresh();
    }
}
