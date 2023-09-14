using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour, IAddress
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button Button;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private ButtonModel _model;

    private void Awake()
    {
        if (_model == null)
            Configure(null);
    }

    public void Configure(Address address)
    {
        _address = address;
        _model = _address == null ? ButtonModel.Default : Get<ButtonModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Click);
    }

    public void Refresh()
    {
    }

    public void Click()
    {
        _model.Click();
    }
}
