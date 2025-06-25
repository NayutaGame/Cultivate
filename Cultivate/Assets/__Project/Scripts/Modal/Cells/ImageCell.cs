
using UnityEngine;

public class ImageCell : Cell
{
    private SpriteEntry _spriteEntry;
    public Sprite GetSprite() => _spriteEntry.Sprite;
    public Cell Next;

    public ImageCell(string spriteName)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };

        _spriteEntry = spriteName;
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ClickedSignal clickedSignal)
            return Next;

        return this;
    }
}
