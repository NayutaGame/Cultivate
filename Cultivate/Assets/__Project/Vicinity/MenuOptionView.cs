
using UnityEngine;

public class MenuOptionView : XView
{
    [SerializeField] private ColorButton Button;

    public override void Refresh()
    {
        base.Refresh();

        string menuOption = Get<string>();
        Button.Text.text = menuOption;
    }
}