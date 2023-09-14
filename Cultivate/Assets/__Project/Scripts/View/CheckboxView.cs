using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxView : MonoBehaviour, IAddress
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button Button;
    [SerializeField] private Image ButtonImage;

    [SerializeField] private Sprite OnImage;
    [SerializeField] private Sprite OffImage;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private CheckboxModel _model;

    private void Awake()
    {
        if (_model == null)
            SetAddress(null);
    }

    public void SetAddress(Address address)
    {
        _address = address;
        _model = _address == null ? CheckboxModel.Default : Get<CheckboxModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Toggle);
    }

    public void Refresh()
    {
    }

    public void Toggle()
    {
        _model.Toggle();
        ButtonImage.sprite = _model.Value ? OnImage : OffImage;
    }
}
