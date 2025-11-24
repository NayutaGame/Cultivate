
using System;

public class MenuOption
{
    public Description Description;
    public Action ClickAction;

    public MenuOption(Description description, Action clickAction)
    {
        Description = description;
        ClickAction = clickAction;
    }
}