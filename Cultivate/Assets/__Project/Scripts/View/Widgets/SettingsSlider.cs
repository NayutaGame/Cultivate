
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : XView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text NumberText;
    [SerializeField] private Slider Slider;

    private SliderModel _model;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        if (_model == null)
            SetAddress(null);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _model = GetAddress() == null ? SliderModel.Default : Get<SliderModel>();

        if (LabelText != null)
            LabelText.text = _model.Name;
        Slider.minValue = _model.MinValue;
        Slider.maxValue = _model.MaxValue;
        Slider.wholeNumbers = _model.WholeNumbers;
        
        Slider.onValueChanged.RemoveAllListeners();
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    public override void Refresh()
    {
        base.Refresh();
        Slider.SetValueWithoutNotify(_model.Value);
        NumberText.text = ((int)Slider.value).ToString();
    }

    private void OnValueChanged(float value)
    {
        NumberText.text = ((int)value).ToString();

        SliderModel model = Get<SliderModel>();
        if (model == null)
            return;

        model.Value = value;
    }
}
