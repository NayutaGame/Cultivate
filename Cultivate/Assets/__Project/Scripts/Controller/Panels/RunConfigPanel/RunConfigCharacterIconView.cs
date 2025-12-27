
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunConfigCharacterIconView : XView
{
    [SerializeField] public RectTransform Scale;
    [SerializeField] public ToggleButton ToggleButton;
    [SerializeField] public Image Image;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        ToggleButton.LeftClickNeuron.Join(Select);
    }

    public override void Refresh()
    {
        base.Refresh();

        CharacterProfile characterProfile = Get<CharacterProfile>();
        Image.sprite = characterProfile.GetEntry().GetRunConfigIcon();
    }

    private void OnEnable()
    {
        ApplyTabButtonDown();
    }
    
    private void ApplyTabButtonDown()
    {
        bool selected = Get<CharacterProfile>() == AppManager.Instance.ConfigManager.CharacterTabControl.GetSelectedCharacterProfile();
        SetSelected(selected);
    }

    private void Select(InteractBehaviour ib, PointerEventData d)
    {
        CharacterProfile profile = Get<CharacterProfile>();
        AppManager.Instance.ConfigManager.CharacterTabControl.SelectCharacterProcedure(new CharacterSelectDetails(profile));
    }

    private Tween _handle;

    public void SetSelected(bool selected)
    {
        ToggleButton.IsDown = selected;
        _handle?.Kill();
        _handle = Scale.DOScale(selected ? 1 : 0.66f, 0.15f).SetEase(Ease.OutQuad);
        _handle.SetAutoKill().Restart();
    }
}