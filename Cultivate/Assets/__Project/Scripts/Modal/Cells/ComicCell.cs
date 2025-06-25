
public class ComicCell : Cell
{
    public PrefabEntry _prefabEntry;
    public Cell Next;

    public ComicCell(string prefabName)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };

        _prefabEntry = prefabName;
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is FinishedComicSignal finishedComicSignal)
            return Next;

        return this;
    }
}
