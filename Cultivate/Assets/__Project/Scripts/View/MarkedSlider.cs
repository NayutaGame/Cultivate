
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class MarkedSlider : XView
{
    [SerializeField] public ListView MarkList;
    [SerializeField] private Slider _slider;
    [SerializeField] private RectTransform FillRect;
    [SerializeField] private MarkCursorView MarkCursorView;

    private float XFromMark(float mark)
    {
        float t = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, mark);
        return Mathf.Lerp(FillRect.rect.xMin, FillRect.rect.xMax, t);
    }

    private void SetPositionForMarkView(MarkView markView, int mark)
    {
        Vector3 oldPosition = markView.RectTransform.localPosition;
        markView.RectTransform.localPosition = new Vector3(XFromMark(mark), oldPosition.y, oldPosition.z);
    }

    private void SetPositionForMarkCursor(float mark)
    {
        Vector3 oldPosition = MarkCursorView.GetLocalPosition();
        MarkCursorView.SetLocalPosition(new Vector3(XFromMark(mark), oldPosition.y, oldPosition.z));
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        IMarkedSliderModel model = Get<IMarkedSliderModel>();
        Address markListModelAddress = model.GetMarkListModelAddress(address);
        MarkList.SetAddress(markListModelAddress);
    }

    public override void Refresh()
    {
        base.Refresh();

        IMarkedSliderModel model = Get<IMarkedSliderModel>();
        _slider.minValue = model.GetMin();
        _slider.maxValue = model.GetMax();

        MarkList.Refresh();
        MarkList.TraversalActive().Do(itemBehaviour =>
        {
            MarkView markView = itemBehaviour.GetView() as MarkView;
            MarkModel markModel = markView.Get<MarkModel>();
            SetPositionForMarkView(markView, markModel._mark);
        });

        SetValue(model.GetValue());
    }

    private void SetValue(int? value)
    {
        MarkCursorView.gameObject.SetActive(value.HasValue);

        if (!value.HasValue)
            return;

        _slider.value = value.Value;
        SetPositionForMarkCursor(_slider.value);
        MarkCursorView.SetText(_slider.value.ToString());
    }
}
