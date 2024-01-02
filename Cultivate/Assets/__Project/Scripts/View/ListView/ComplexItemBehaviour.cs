
using UnityEngine;

[RequireComponent(typeof(ComplexView))]
public class ComplexItemBehaviour : ItemBehaviour
{
    [SerializeField] private ComplexView ComplexView;

    public override AddressBehaviour GetAddressBehaviour()
        => ComplexView.AddressBehaviour;

    public override SelectBehaviour GetSelectBehaviour()
        => ComplexView.GetSelectBehaviour();

    public override InteractBehaviour GetInteractBehaviour()
        => ComplexView.GetInteractBehaviour();

    public override void RefreshPivots()
        => ComplexView.RefreshPivots();
}
