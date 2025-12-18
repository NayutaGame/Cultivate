
using System;
using UnityEngine;

public class MenuOptionView : XView
{
    [SerializeField] private ColorButton Button;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        MenuOption menuOption = Get<MenuOption>();
        Button.LeftClickNeuron.ClearAction();
        Button.LeftClickNeuron.Add((ib, d) => CanvasManager.Instance.MenuManager.HideMenu());
        Button.LeftClickNeuron.Add((ib, d) => menuOption.ClickAction());
    }

    private void OnDisable()
    {
        Button.LeftClickNeuron.ClearAction();
    }

    public override void Refresh()
    {
        base.Refresh();

        MenuOption menuOption = Get<MenuOption>();
        Button.Text.text = menuOption.Description.GetHighlightedString();
    }
}