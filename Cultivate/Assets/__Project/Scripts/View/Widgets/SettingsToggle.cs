
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : LegacyAddressBehaviour
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button Button;

    [SerializeField] private CanvasGroup OpenSign;
    [SerializeField] private CanvasGroup ClosedSign;

    private ToggleModel _model;

    private void Awake()
    {
        if (_model == null)
            SetAddress(null);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _model = GetAddress() == null ? ToggleModel.Default : Get<ToggleModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Toggle);
    }

    private Tween _handle;

    private void Toggle()
    {
        _model.Toggle();

        float on = _model.Value ? 1 : 0;

        if (_handle != null)
            _handle.Kill();
        _handle = DOTween.Sequence().SetAutoKill()
            .Append(OpenSign.DOFade(on, 0.15f))
            .Append(ClosedSign.DOFade(1 - on, 0.15f));
        _handle.Restart();
    }
}
