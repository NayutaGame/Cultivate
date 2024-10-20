
public class SettingsContentModel
{
    public string Name;
    public WidgetListModel Widgets;

    public SettingsContentModel(string name, WidgetListModel widgets)
    {
        Name = name;
        Widgets = widgets;
    }
}
