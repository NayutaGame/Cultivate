
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunConfigDifficultyIconView : XView
{
    [SerializeField] public RectTransform Scale;
    [SerializeField] public ToggleButton ToggleButton;
    [SerializeField] public TMP_Text Text;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        ToggleButton.LeftClickNeuron.Join(Select);
    }

    public override void Refresh()
    {
        base.Refresh();

        DifficultyProfile difficultyProfile = Get<DifficultyProfile>();
        Text.text = difficultyProfile.GetEntry().GetName();
    }

    private void OnEnable()
    {
        ApplyTabButtonDown();
    }
    
    private void ApplyTabButtonDown()
    {
        bool selected = Get<DifficultyProfile>() == AppManager.Instance.ConfigManager.DifficultyTabControl.GetSelectedDifficultyProfile();
        SetSelected(selected);
    }

    private void Select(InteractBehaviour ib, PointerEventData d)
    {
        DifficultyProfile difficultyProfile = Get<DifficultyProfile>();
        AppManager.Instance.ConfigManager.DifficultyTabControl.SelectDifficultyProcedure(new DifficultySelectDetails(difficultyProfile));
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