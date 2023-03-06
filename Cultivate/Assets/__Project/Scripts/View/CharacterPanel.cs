
using CLLibrary;

public class CharacterPanel : Panel
{
    public CharacterPanelState _state;

    public StatusView StatusView;
    public DanTianView DanTianView;
    // public InventoryView InventoryView;
    public InspectorView InspectorView;

    public override void Configure()
    {
        StatusView.Configure();
        DanTianView.Configure();
        // InventoryView.Configure();
        InspectorView.Configure();

        _state = new CharacterPanelStateNormal();
    }

    public override void Refresh()
    {
        StatusView.Refresh();
        DanTianView.Refresh();
        // InventoryView.Refresh();
        InspectorView.Refresh();
    }
}
