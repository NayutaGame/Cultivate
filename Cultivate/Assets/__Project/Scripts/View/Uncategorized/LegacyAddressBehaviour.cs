
using UnityEngine;

public class LegacyAddressBehaviour : MonoBehaviour
{
    public RectTransform RectTransform;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public virtual void SetAddress(Address address)
    {
        _address = address;
    }

    public virtual void Refresh()
    {
    }
}
