using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItemView : ItemView
{
    [SerializeField] private ButtonView ButtonView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        ButtonView.SetAddress(address);
    }

    public override void Refresh()
    {
        base.Refresh();
        ButtonView.Refresh();
    }
}
