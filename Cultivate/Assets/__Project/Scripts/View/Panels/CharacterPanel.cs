
using CLLibrary;
using UnityEngine.Serialization;

public class CharacterPanel : Panel
{
    public CharacterPanelState _state;

    public StatusView StatusView;

    public override void Configure()
    {
        StatusView.Configure();
        _state = new CharacterPanelStateNormal();
    }

    public override void Refresh()
    {
        StatusView.Refresh();
    }
}
