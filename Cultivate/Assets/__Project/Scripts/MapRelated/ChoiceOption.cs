
using System;
using System.Collections.Generic;

public class ChoiceOption : Addressable
{
    public int Index;
    public string Text;
    public Cell NextCell;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Skill",         thisObject => ((Commodity)thisObject).Skill },
    };
    public object Get(string s) => Accessor[s](this);
    private ChoiceOption(
        int index,
        string text,
        Cell nextCell)
    {
        Index = index;
        Text = text;
        NextCell = nextCell;
    }

    public static ChoiceOption FromIndex(int index)
        => new(index, "继续", null);

    public static ChoiceOption FromIndexText(int index, string text)
        => new(index, text, null);

    public void MakeChoice()
    {
        RunManager.Instance.Environment.ReceiveSignalProcedure(new SelectedChoiceSignal(Index));
    }
}