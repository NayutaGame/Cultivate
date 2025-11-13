
using UnityEngine;

public class ScribblePanel : Panel
{
    [SerializeField] private FaceCanvas FaceCanvas;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        
        FaceCanvas.CheckAwake();
    }

    public override void Refresh()
    {
        base.Refresh();
        
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        ScribbleCell cell = cellAdapter.AsCell() as ScribbleCell;
        CharacterEntry characterEntry = cell.GetCharacterEntry();
        FaceCanvas.SetPrefabEntry(characterEntry.GetScribblePrefabEntry());
    }
}