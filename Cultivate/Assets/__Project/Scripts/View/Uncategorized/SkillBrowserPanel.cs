
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

    private InteractHandler _interactHandler;
    public InteractHandler GetDelegate() => _interactHandler;
    private void ConfigureInteractDelegate()
    {
        _interactHandler = new(1,
            getId: view =>
            {
                if (view.GetComponent<BrowserSkillInteractBehaviour>() != null)
                    return 0;
                return null;
            });

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 0, (v, d) => ((BrowserSkillInteractBehaviour)v).PointerEnter(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 0, (v, d) => ((BrowserSkillInteractBehaviour)v).PointerExit(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 0, (v, d) => ((BrowserSkillInteractBehaviour)v).PointerMove(v, d));

        SkillInventoryView.SetHandler(_interactHandler);
    }

    #endregion

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
    }
}
