
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : XView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text NumberText;
    [SerializeField] private Slider Slider;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        Slider.onValueChanged.RemoveAllListeners();
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        SliderModel model = Get<SliderModel>(); 
        LabelText.text = model.Name;
        Slider.minValue = model.MinValue;
        Slider.maxValue = model.MaxValue;
    }

    public override void Refresh()
    {
        base.Refresh();
        
        SliderModel model = Get<SliderModel>(); 
        Slider.SetValueWithoutNotify(model.Value);
        NumberText.text = ((int)Slider.value).ToString();
    }

    private void OnValueChanged(float value)
    {
        SliderModel model = Get<SliderModel>();
        int intValue = (int)value;
        model.Value = intValue;
        NumberText.text = intValue.ToString();
    }
}
