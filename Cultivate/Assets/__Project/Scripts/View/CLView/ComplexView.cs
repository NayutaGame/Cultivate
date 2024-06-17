
using UnityEngine;

public class ComplexView : CLView
{
    [SerializeField] public SimpleView SimpleView;

    public override SimpleView GetSimpleView() => SimpleView.GetSimpleView();
    public override RectTransform GetDisplayTransform() => SimpleView.GetDisplayTransform();
    public override void SetDisplayTransform(RectTransform pivot) => SimpleView.SetDisplayTransform(pivot);

    public override Address GetAddress() => SimpleView.GetAddress();
    public override T Get<T>() => SimpleView.Get<T>();
    public override void SetAddress(Address address) => SimpleView.SetAddress(address);
    public override void Refresh() => SimpleView.Refresh();

    public override void AwakeFunction()
    {
        SimpleView.AwakeFunction();

        base.AwakeFunction();
    }
}
