
using UnityEngine;

public class DelegatingView : XView
{
    [SerializeField] private XView _delegatedView;
    public XView GetDelegatedView() => _delegatedView;

    private AnimatedListView _parentListView;
    public AnimatedListView GetParentListView() => _parentListView;
    public void SetParentListView(AnimatedListView parentListView) => _parentListView = parentListView;

    public override Address GetAddress() => _delegatedView.GetAddress();
    public override T Get<T>() => _delegatedView.Get<T>();
    public override void SetAddress(Address address) => _delegatedView.SetAddress(address);
    public override void Refresh() => _delegatedView.Refresh();

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _delegatedView.CheckAwake();
    }

    public void SetMoveFromRectToIdle(RectTransform rect)
    {
        GetDelegatedView().GetRect().position = rect.position;
        GetDelegatedView().GetRect().localScale = rect.localScale;
        GetAnimator().SetStateAsync(1);
    }
}
