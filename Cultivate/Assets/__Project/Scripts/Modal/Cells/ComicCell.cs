
using System;
using System.Collections.Generic;

public class ComicCell : Cell
{
    public PrefabEntry _prefabEntry;
    public Cell Next;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((ComicCell)thisObject).GetGuideDescriptor() },
    };
    public override object Get(string s) => Accessor[s](this);
    public ComicCell(string prefabName)
    {
        _prefabEntry = Encyclopedia.PrefabCategory.FromName(prefabName);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is FinishedComicSignal finishedComicSignal)
            return Next;

        return this;
    }
}
