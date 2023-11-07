
using System.Collections.Generic;

public class SwitchModel : WidgetModel
{
    public List<string> Options;
    public int Index;

    public SwitchModel(string name, List<string> options, int index = 0) : base(name)
    {
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
