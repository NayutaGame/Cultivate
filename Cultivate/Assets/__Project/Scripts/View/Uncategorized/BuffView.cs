using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffView : MonoBehaviour, IAddress
{
    public TMP_Text NameText;
    public TMP_Text StackText;

    private Address _address;
    public Address GetIndexPath() => _address;
    public T Get<T>() => _address.Get<T>();

    public void Configure(Address address)
    {
        _address = address;
    }

    public virtual void Refresh()
    {
        // TryGetHeroBuff
        Buff b = Get<Buff>();

        gameObject.SetActive(b != null);
        if (b == null) return;

        NameText.text = $"{b.GetName()}";
        StackText.text = $"{b.Stack}";
    }
}
