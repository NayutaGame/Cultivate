
using UnityEngine;

[RequireComponent(typeof(AddressBehaviour))]
public class SimpleItemBehaviour : ItemBehaviour
{
    [SerializeField] private AddressBehaviour AddressBehaviour;

    public override AddressBehaviour GetAddressBehaviour()
        => AddressBehaviour;

    public override SelectBehaviour GetSelectBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public override InteractBehaviour GetInteractBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public override void RefreshPivots() { }
}
