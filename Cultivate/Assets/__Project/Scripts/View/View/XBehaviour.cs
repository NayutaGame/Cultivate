
using UnityEngine;

[RequireComponent(typeof(XView))]
public abstract class XBehaviour : MonoBehaviour
{
    protected XView View;
    public XView GetView() => View;

    public InteractBehaviour GetInteractBehaviour() => View.GetInteractBehaviour();
    public void SetInteractBehaviour(InteractBehaviour ib) => View.SetInteractBehaviour(ib);
    
    public ItemBehaviour GetItemBehaviour() => View.GetBehaviour<ItemBehaviour>();
    public SelectBehaviour GetSelectBehaviour() => View.GetBehaviour<SelectBehaviour>();
    public T GetBehaviour<T>() where T : XBehaviour => View.GetBehaviour<T>();
    
    public Address GetAddress() => View.GetAddress();
    public T Get<T>() where T : class => View.Get<T>();
    public virtual void SetAddress(Address address) => View.SetAddress(address);
    public void Refresh() => View.Refresh();

    public virtual void AwakeFunction(XView view)
    {
        View = view;
    }
}
