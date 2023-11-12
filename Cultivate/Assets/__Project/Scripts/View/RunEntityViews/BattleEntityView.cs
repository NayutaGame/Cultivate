
public class BattleEntityView : ItemView
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

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(2,
            getId: view =>
            {
                if (view is FieldSlotView)
                    return 0;
                if (view is RunFormationIconView)
                    return 1;
                return null;
            });

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 0, (v, d) => ((FieldSlotView)v).HoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 0, (v, d) => ((FieldSlotView)v).UnhoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 0, (v, d) => ((FieldSlotView)v).PointerMove(d));

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 1, (v, d) => ((RunFormationIconView)v).PointerEnter(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 1, (v, d) => ((RunFormationIconView)v).PointerExit(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 1, (v, d) => ((RunFormationIconView)v).PointerMove(v, d));

        FieldView.SetDelegate(InteractDelegate);
        FormationListView.SetDelegate(InteractDelegate);
    }

    #endregion
}
