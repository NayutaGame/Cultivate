
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

    private ToggleModel _model;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        if (_model == null)
            SetAddress(null);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _model = GetAddress() == null ? ToggleModel.Default : Get<ToggleModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;

        bool on = _model.Value;
        FillRectTransform.position = on ? OnTransform.position : OffTransform.position;
        
        FillRect.onClick.RemoveAllListeners();
        FillRect.onClick.AddListener(Toggle);
    }

    private Tween _handle;

    private void Toggle()
    {
        _model.Toggle();

        bool on = _model.Value;

        _handle?.Kill();
        _handle = FillRectTransform.DOMove(on ? OnTransform.position : OffTransform.position, 0.15f).SetEase(Ease.InOutQuad);
        _handle.SetAutoKill().Restart();
    }
}
