
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderView : MonoBehaviour, IAddress
{
    [SerializeField] private Slider Slider;
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text NumberText;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private SliderModel _model;

    private void Awake()
    {
        if (_model == null)
            SetAddress(null);
    }

    public void SetAddress(Address address)
    {
        _address = address;
        _model = _address == null ? SliderModel.Default : Get<SliderModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;
        Slider.minValue = _model.MinValue;
        Slider.maxValue = _model.MaxValue;
        Slider.wholeNumbers = _model.WholeNumbers;

        Slider.onValueChanged.RemoveAllListeners();
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    public void Refresh()
    {
    }

    private void OnValueChanged(float value)
    {
        NumberText.text = ((int)value).ToString();
    }
}
