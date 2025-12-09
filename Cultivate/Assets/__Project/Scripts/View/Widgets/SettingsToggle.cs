
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsToggle : XView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private CLButtonPatternA Handle;

    [SerializeField] private RectTransform HandleRect;
    [SerializeField] private RectTransform OnPivot;
    [SerializeField] private RectTransform OffPivot;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        Handle.LeftClickNeuron.Join(Toggle);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        ToggleModel model = Get<ToggleModel>();
        
        LabelText.text = model.Name;

        bool on = model.IsOn;
        HandleRect.anchoredPosition = on ? OnPivot.anchoredPosition : OffPivot.anchoredPosition;
    }

    private Tween _handle;

    private void Toggle(InteractBehaviour ib, PointerEventData d)
        => Toggle();
    
    private void Toggle()
    {
        ToggleModel model = Get<ToggleModel>();
        model.Toggle();

        bool on = model.IsOn;

        _handle?.Kill();
        _handle = HandleRect.DOAnchorPos(on ? OnPivot.anchoredPosition : OffPivot.anchoredPosition, 0.15f).SetEase(Ease.InOutQuad);
        _handle.SetAutoKill().Restart();
    }
}
