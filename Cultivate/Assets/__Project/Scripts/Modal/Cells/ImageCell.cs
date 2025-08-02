
using System;
using System.Collections.Generic;
using UnityEngine;

public class ImageCell : Cell
{
    private SpriteEntry _spriteEntry;
    public Sprite GetSprite() => _spriteEntry.Sprite;
    public Cell Next;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((ImageCell)thisObject).GetGuideDescriptor() },
    };
    public override object Get(string s) => Accessor[s](this);
    public ImageCell(string spriteName)
    {
        _spriteEntry = Encyclopedia.SpriteCategory.FromName(spriteName);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ClickedSignal clickedSignal)
            return Next;

        return this;
    }
}
