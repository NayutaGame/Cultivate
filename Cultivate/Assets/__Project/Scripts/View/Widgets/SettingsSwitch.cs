
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSwitch : ItemView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button NextButton;

    private SwitchModel _model;

    private void Awake()
    {
        if (_model == null)
            SetAddress(null);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _model = GetAddress() == null ? SwitchModel.Default : Get<SwitchModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;

        PrevButton.onClick.RemoveAllListeners();
        PrevButton.onClick.AddListener(Prev);
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(Next);
    }

    public override void Refresh()
    {
        base.Refresh();
        ContentText.text = _model.GetContentText();
    }

    private void Prev()
    {
        _model.Prev();
        ContentText.text = _model.GetContentText();
    }

    private void Next()
    {
        _model.Next();
        ContentText.text = _model.GetContentText();;
    }
}
