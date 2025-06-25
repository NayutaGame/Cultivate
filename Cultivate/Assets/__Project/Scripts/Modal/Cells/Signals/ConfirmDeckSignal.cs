
using System.Collections.Generic;

public class ConfirmDeckSignal : Signal
{
    public List<DeckIndex> Indices;

    public ConfirmDeckSignal(List<DeckIndex> indices)
    {
        Indices = indices;
    }
}
