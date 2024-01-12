
using UnityEngine;

public interface CLView
{
    public void Awake();
    public SimpleView GetSimpleView();
    public RectTransform GetDisplayTransform();
    public InteractBehaviour GetInteractBehaviour();
    public ItemBehaviour GetItemBehaviour();
    public SelectBehaviour GetSelectBehaviour();
    public StateBehaviour GetStateBehaviour();
}
