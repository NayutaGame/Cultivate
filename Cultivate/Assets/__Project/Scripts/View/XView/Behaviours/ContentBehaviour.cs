
public class ContentBehaviour : XBehaviour
{
    private SlotView _slotView;

    public SlotView Slot
    {
        get => _slotView;
        set
        {
            _slotView = value;
        }
    }
}