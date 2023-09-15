using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItemView : ItemView
{
    [SerializeField] private SliderView SliderView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SliderView.SetAddress(address);
    }

    public override void Refresh()
    {
        base.Refresh();
        SliderView.Refresh();
    }
}
