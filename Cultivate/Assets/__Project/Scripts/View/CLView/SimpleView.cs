
using UnityEngine;

public class SimpleView : CLView
{
    public override SimpleView GetSimpleView() => this;

    private RectTransform RectTransform;
    public override RectTransform GetDisplayTransform() => RectTransform;
    public override void SetDisplayTransform(RectTransform pivot)
    {
        RectTransform.position = pivot.position;
        RectTransform.localScale = pivot.localScale;
    }

    public void SetLocalPosition(Vector3 localPosition)
    {
        RectTransform.localPosition = localPosition;
    }

    private CanvasGroup CanvasGroup;
    public void SetVisible(bool value)
    {
        if (CanvasGroup != null)
            CanvasGroup.alpha = value ? 1 : 0;
    }

    public override void Awake()
    {
        RectTransform ??= GetComponent<RectTransform>();
        CanvasGroup ??= GetComponent<CanvasGroup>();

        base.Awake();
    }

    private Address _address;
    public override Address GetAddress() => _address;
    public override T Get<T>() => _address.Get<T>();

    public override void SetAddress(Address address)
    {
        _address = address;
    }

    public override void Refresh()
    {
    }
}
