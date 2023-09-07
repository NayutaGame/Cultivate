using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchModel
{
    public string Label;
    public List<string> Options;
    public int Index;

    public SwitchModel(string label, List<string> options, int index)
    {
        Label = label;
        Options = options;
        Index = index;
    }

    public string GetContentText()
        => Options[Index];

    public void Prev()
    {
        Index = (Index + Options.Count - 1) % Options.Count;
    }

    public void Next()
    {
        Index = (Index + 1) % Options.Count;
    }

    public static SwitchModel Default
        => new("默认Switch", new List<string>() { "苹果", "香蕉", "橙子" }, 0);
}
