
public class SkillBrowserPanel : Panel
{
    public SkillInventoryView SkillInventoryView;

    public override void Configure()
    {
        base.Configure();
        SkillInventoryView.SetAddress(new Address("App.SkillInventory"));

        ConfigureInteractDelegate();
    }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(1,
            getId: view =>
            {
                if (view is BrowserSkillView)
                    return 0;
                return null;
            });

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 0, (v, d) => ((BrowserSkillView)v).PointerEnter(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 0, (v, d) => ((BrowserSkillView)v).PointerExit(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 0, (v, d) => ((BrowserSkillView)v).PointerMove(v, d));

        SkillInventoryView.SetDelegate(InteractDelegate);
    }

    #endregion

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
    }
}
