
public class ButtonModel : WidgetModel
{
    public ButtonModel(string name) : base(name) { }

    public void Click()
    {

    }

    public static ButtonModel Default
        => new("默认Button");
}
