
using System;

public class AnimatedItemView : ItemView
{
    public AddressBehaviour AddressPropagate;
    public InteractBehaviour InteractBehaviour;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        AddressPropagate.SetAddress(address);
    }

    public override void Refresh()
    {
        base.Refresh();
        AddressPropagate.Refresh();
    }

    private void OnEnable()
    {
        InteractBehaviour.SetEnabled(true);
    }

    private void OnDisable()
    {
        InteractBehaviour.SetEnabled(false);
    }
}
