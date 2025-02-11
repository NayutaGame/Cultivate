
public class DialogPanelDescriptor : PanelDescriptor
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
    public DialogPanelDescriptor SetReward(Reward reward)
    {
        _reward = reward;
        return this;
    }

    public DialogPanelDescriptor(string titleText, string detailedText, params DialogOption[] options)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };

        _titleText = titleText;
        _detailedText = detailedText;
        _options = options.Length > 0 ? options : new DialogOption[] { "确认" };
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);
        _reward?.Claim();
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
