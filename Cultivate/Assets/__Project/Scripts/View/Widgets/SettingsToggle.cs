
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : LegacySimpleView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button FillRect;

    [SerializeField] private Transform FillRectTransform;
    [SerializeField] private Transform OnTransform;
    [SerializeField] private Transform OffTransform;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        FillRect.onClick.RemoveAllListeners();
        FillRect.onClick.AddListener(Toggle);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        ToggleModel model = Get<ToggleModel>();
        
        LabelText.text = model.Name;

        bool on = model.IsOn;
        FillRectTransform.position = on ? OnTransform.position : OffTransform.position;
    }

    private Tween _handle;

    private void Toggle()
    {
        ToggleModel model = Get<ToggleModel>();
        model.Toggle();

        bool on = model.IsOn;

        _handle?.Kill();
        _handle = FillRectTransform.DOMove(on ? OnTransform.position : OffTransform.position, 0.15f).SetEase(Ease.InOutQuad);
        _handle.SetAutoKill().Restart();
    }
}
