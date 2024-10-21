
using System;

public class ButtonModel : WidgetModel
{
    private Action _click;
    
    public ButtonModel(string name, Action click = null) : base(name)
    {
        _click = click;
    }

    public void Click()
    {
        _click?.Invoke();
    }

    public static ButtonModel Default
        => new("默认Button");
}
