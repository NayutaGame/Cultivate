
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchView : MonoBehaviour, IAddress
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button NextButton;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private SwitchModel _model;

    private void Awake()
    {
        if (_model == null)
            SetAddress(null);
    }

    public void SetAddress(Address address)
    {
        _address = address;
        _model = _address == null ? SwitchModel.Default : Get<SwitchModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;

        PrevButton.onClick.RemoveAllListeners();
        PrevButton.onClick.AddListener(Prev);
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(Next);
    }

    public void Refresh()
    {
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
