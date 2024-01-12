
using UnityEngine;

public interface CLView
{
    public void Awake();
    public SimpleView GetSimpleView();
    public RectTransform GetDisplayTransform();
    public void SetDisplayTransform(RectTransform pivot);
    public InteractBehaviour GetInteractBehaviour();
    public ItemBehaviour GetItemBehaviour();
    public SelectBehaviour GetSelectBehaviour();
    public StateBehaviour GetStateBehaviour();
}
