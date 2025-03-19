
using System;

public class ButtonModel : WidgetModel
{
    private Action _click;
    
    public ButtonModel(string name, Action click) : base(name, null)
    {
        _click = click;
    }

    public void Click()
    {
        _click();
    }
}
