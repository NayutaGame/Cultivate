
public class BattleEntityView : AddressBehaviour
{
    public ListView FieldView;
    public ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FormationListView.SetAddress(GetAddress().Append(".ActivatedSubFormations"));

        ConfigureInteractDelegate();
    }

    public override void Refresh()
    {
        base.Refresh();
        FieldView.Refresh();
        FormationListView.Refresh();
    }

    #region IInteractable

    private InteractHandler _interactHandler;
    public InteractHandler GetDelegate() => _interactHandler;
    private void ConfigureInteractDelegate()
    {
        _interactHandler = new(2,
            getId: view =>
            {
                InteractBehaviour d = view.GetComponent<InteractBehaviour>();
                if (d is FieldSlotInteractBehaviour)
                    return 0;
                if (d is RunFormationIconInteractBehaviour)
                    return 1;
                return null;
            });

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 0, (v, d) => ((FieldSlotInteractBehaviour)v).HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 0, (v, d) => ((FieldSlotInteractBehaviour)v).UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 0, (v, d) => ((FieldSlotInteractBehaviour)v).PointerMove(d));

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 1, (v, d) => ((RunFormationIconInteractBehaviour)v).PointerEnter(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 1, (v, d) => ((RunFormationIconInteractBehaviour)v).PointerExit(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 1, (v, d) => ((RunFormationIconInteractBehaviour)v).PointerMove(v, d));

        FieldView.SetHandler(_interactHandler);
        FormationListView.SetHandler(_interactHandler);
    }

    #endregion
}
