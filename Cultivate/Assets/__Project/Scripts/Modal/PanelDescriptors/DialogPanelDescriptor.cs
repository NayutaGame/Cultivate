using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;
    public string SetDetailedText(string value) => _detailedText = value;

    private DialogOption[] _options;
    public int GetOptionsCount() => _options.Length;
    public DialogOption GetOption(int i) => _options[i];

    public DialogOption this[int i] => _options[i];

    public RewardDescriptor _reward;

    public DialogPanelDescriptor(string detailedText, params DialogOption[] options)
    {
        _detailedText = detailedText;
        _options = options.Length > 0 ? options : new DialogOption[] { "确认" };
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();
        if (_reward != null)
            _reward.Claim();
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            int i = selectedOptionSignal.Selected;
            return _options[i].Select();
        }

        return this;
    }
}
