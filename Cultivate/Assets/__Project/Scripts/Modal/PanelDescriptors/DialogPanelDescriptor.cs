using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private string[] _options;
    public int GetOptionsCount() => _options.Length;
    public string GetOption(int i) => _options[i];

    public DialogPanelDescriptor(string detailedText, params string[] options)
    {
        _detailedText = detailedText;
        _options = options.Length > 0 ? options : new[] { "确认" };
    }
}
