
using UnityEngine;

[RequireComponent(typeof(AddressBehaviour))]
public class SimpleItemBehaviour : ItemBehaviour
{
    [SerializeField] private AddressBehaviour AddressBehaviour;
    [SerializeField] private InteractBehaviour InteractBehaviour;
    [SerializeField] private SelectBehaviour SelectBehaviour;

    public override AddressBehaviour GetAddressBehaviour()
        => AddressBehaviour;

    public override SelectBehaviour GetSelectBehaviour()
        => SelectBehaviour;

    public override InteractBehaviour GetInteractBehaviour()
        => InteractBehaviour;

    public override void RefreshPivots() { }
}
