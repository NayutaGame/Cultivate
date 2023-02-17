
using CLLibrary;

public class CharacterPanel : Panel
{
    public StatusView StatusView;
    public DanTianView DanTianView;
    public InventoryView InventoryView;

    public override void Configure()
    {
        StatusView.Configure(new IndexPath("GetStatusString"));
        DanTianView.Configure();
        InventoryView.Configure();
    }

    public override void Refresh()
    {
        StatusView.Refresh();
        DanTianView.Refresh();
        InventoryView.Refresh();
    }
}
