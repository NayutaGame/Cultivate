
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderView : MonoBehaviour, IIndexPath
{
    [SerializeField] private Slider Slider;
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text NumberText;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private SliderModel _model;

    private void Awake()
    {
        if (_model == null)
            Configure(null);
    }

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _model = _indexPath == null ? SliderModel.Default : DataManager.Get<SliderModel>(_indexPath);

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
