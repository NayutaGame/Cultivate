
using CLLibrary;
using UnityEngine.Serialization;

public class CharacterPanel : Panel
{
    public CharacterPanelState _state;

    public StatusView StatusView;
    public DanTianView DanTianView;
    public ChipInventoryView ChipInventoryView;
    // public InspectorView InspectorView;

    public override void Configure()
    {
        StatusView.Configure();
        DanTianView.Configure();
        ChipInventoryView.Configure(RunManager.Instance.ChipInventory);
        // InspectorView.Configure();

        _state = new CharacterPanelStateNormal();
    }

    public override void Refresh()
    {
        StatusView.Refresh();
        DanTianView.Refresh();
        ChipInventoryView.Refresh();
        // InspectorView.Refresh();
    }
}
