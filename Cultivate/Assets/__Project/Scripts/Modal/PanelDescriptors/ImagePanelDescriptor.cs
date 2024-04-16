
using UnityEngine;

public class ImagePanelDescriptor : PanelDescriptor
{
    private SpriteEntry _spriteEntry;
    public Sprite GetSprite() => _spriteEntry.Sprite;
    public PanelDescriptor Next;

    public ImagePanelDescriptor(string spriteName)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };

        _spriteEntry = spriteName;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is ClickedSignal clickedSignal)
            return Next;

        return this;
    }
}
