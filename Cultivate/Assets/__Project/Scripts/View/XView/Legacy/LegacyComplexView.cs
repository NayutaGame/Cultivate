
using UnityEngine;

public class LegacyComplexView : LegacyView
{
    [SerializeField] public LegacySimpleView SimpleView;

    public override LegacySimpleView GetView() => SimpleView.GetView();
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
