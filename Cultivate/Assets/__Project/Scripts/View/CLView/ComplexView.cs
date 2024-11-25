
using UnityEngine;

public class ComplexView : XView
{
    [SerializeField] public SimpleView SimpleView;

    public override SimpleView GetView() => SimpleView.GetView();
    public override RectTransform GetViewTransform() => SimpleView.GetViewTransform();
    public override void SetViewTransform(RectTransform pivot) => SimpleView.SetViewTransform(pivot);

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
