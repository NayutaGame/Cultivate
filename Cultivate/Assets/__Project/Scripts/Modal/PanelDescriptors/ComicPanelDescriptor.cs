
public class ComicPanelDescriptor : PanelDescriptor
{
    public PrefabEntry _prefabEntry;
    public PanelDescriptor Next;

    public ComicPanelDescriptor(string prefabName)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };

        _prefabEntry = prefabName;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is FinishedComicSignal finishedComicSignal)
            return Next;

        return this;
    }
}
