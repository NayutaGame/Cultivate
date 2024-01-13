
using UnityEngine;

public class ComplexView : CLView
{
    [SerializeField] public SimpleView SimpleView;

    public override SimpleView GetSimpleView() => SimpleView.GetSimpleView();
    public override RectTransform GetDisplayTransform() => SimpleView.GetDisplayTransform();
    public override void SetDisplayTransform(RectTransform pivot) => SimpleView.SetDisplayTransform(pivot);

    public Address GetAddress() => SimpleView.GetAddress();
    public T Get<T>() => SimpleView.Get<T>();
    public void SetAddress(Address address) => SimpleView.SetAddress(address);
    public void Refresh() => SimpleView.Refresh();

    public override void Awake()
    {
        SimpleView.Awake();

        base.Awake();
    }
}
