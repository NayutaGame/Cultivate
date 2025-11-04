
using System;
using System.Collections.Generic;

public class ChoiceCell : Cell
{
    private ListModel<ChoiceOption> _choiceListModel;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "ChoiceList",                     thisObject => ((ChoiceCell)thisObject)._choiceListModel },
    };
    public override object Get(string s) => Accessor[s](this);
    public ChoiceCell(List<ChoiceOption> choices)
    {
        _choiceListModel = new ListModel<ChoiceOption>(choices.ToArray());
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedChoiceSignal selectedChoiceSignal)
        {
            int i = selectedChoiceSignal.Selected;
            return _choiceListModel[i].NextCell;
        }

        return this;
    }
}