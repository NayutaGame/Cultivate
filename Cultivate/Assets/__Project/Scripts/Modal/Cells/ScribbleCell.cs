
using System;
using System.Collections.Generic;

public class ScribbleCell : Cell
{
    private CharacterEntry _characterEntry;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Guide",                      thisObject => ((DialogCell)thisObject).GetGuideDescriptor() },
    };
    public override object Get(string s) => Accessor[s](this);
    public ScribbleCell(CharacterEntry characterEntry)
    {
        // _characterEntry = Encyclopedia.CharacterCategory.FromName("彼此卿");
        _characterEntry = characterEntry;
    }

    public CharacterEntry GetCharacterEntry()
        => _characterEntry;

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitScribbleSignal)
        {
            return null;
        }

        return this;
    }
}