using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckboxItemView : ItemView
{
    [SerializeField] private CheckboxView CheckboxView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        CheckboxView.SetAddress(address);
    }

    public override void Refresh()
    {
        base.Refresh();
        CheckboxView.Refresh();
    }
}
