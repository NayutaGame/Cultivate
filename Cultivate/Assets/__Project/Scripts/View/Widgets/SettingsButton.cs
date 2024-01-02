
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : AddressBehaviour
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button Button;

    private ButtonModel _model;

    private void Awake()
    {
        if (_model == null)
            SetAddress(null);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _model = GetAddress() == null ? ButtonModel.Default : Get<ButtonModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Click);
    }

    public void Click()
    {
        _model.Click();
    }
}
