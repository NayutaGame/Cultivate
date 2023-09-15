using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchItemView : ItemView
{
    [SerializeField] private SwitchView SwitchView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SwitchView.SetAddress(GetAddress());
    }

    public override void Refresh()
    {
        base.Refresh();
        SwitchView.Refresh();
    }
}
