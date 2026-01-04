
using UnityEngine;
using UnityEngine.EventSystems;

public class ScribblePanel : Panel
{
    [SerializeField] private FaceCanvas FaceCanvas;
    [SerializeField] private CLButton ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        
        FaceCanvas.CheckAwake();
        ExitButton.LeftClickNeuron.Join(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        ScribbleCell cell = cellAdapter.AsCell() as ScribbleCell;
        CharacterEntry characterEntry = cell.GetCharacterEntry();
        FaceCanvas.SetPrefabEntry(characterEntry.GetScribblePrefabEntry());
    }

    private void Exit(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.Map.ReceiveSignalProcedure(new ExitScribbleSignal());
    }
}