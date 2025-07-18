
using System;
using System.Collections.Generic;

public class DialogCell : Cell
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    
    private string _detailedText;
    public string GetDetailedText() => _detailedText;
    public string SetDetailedText(string value) => _detailedText = value;

    private DialogOption[] _options;
    public int GetOptionsCount() => _options.Length;
    public DialogOption GetOption(int i) => _options[i];

    public DialogOption this[int i] => _options[i];

    private Reward _reward;
    public DialogCell SetReward(Reward reward)
    {
        _reward = reward;
        return this;
    }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((DialogCell)thisObject).GetGuideDescriptor() },
    };
    public override object Get(string s) => Accessor[s](this);
    public DialogCell(string titleText, string detailedText, params DialogOption[] options)
    {
        _titleText = titleText;
        _detailedText = detailedText;
        _options = options.Length > 0 ? options : new DialogOption[] { DialogOption.FromText("确认") };
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);
        _reward?.Claim();
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            int i = selectedOptionSignal.Selected;
            return _options[i].Select();
        }

        return this;
    }
}
