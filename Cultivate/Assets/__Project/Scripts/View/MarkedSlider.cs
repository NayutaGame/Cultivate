
using UnityEngine;
using UnityEngine.UI;

public class MarkedSlider : SimpleView
{
    [SerializeField] private ListView MarkList;
    [SerializeField] private Slider _slider;
    [SerializeField] private MonoBehaviour CurrMark;

    public void hehe()
    {
        int curr = 7;
        int halfCount = 6;

        float x = Mathf.Lerp(_slider.handleRect.rect.xMin, _slider.handleRect.rect.xMax, (float)(curr - halfCount) / halfCount);
    }

    public void haha()
    {
        // 1. min and max
        // 2. mark objects
        // 3. curr value, which is progress, can be null
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        IMarkedSliderModel model = Get<IMarkedSliderModel>();
        // MarkList.SetAddress(model.GetMarkListModelAddressSuffix());
    }

    public override void Refresh()
    {
        base.Refresh();

        IMarkedSliderModel model = Get<IMarkedSliderModel>();
        _slider.minValue = model.GetMin();
        _slider.maxValue = model.GetMax();
        _slider.value = model.GetValue();

        // MarkList.Refresh();
    }
}
