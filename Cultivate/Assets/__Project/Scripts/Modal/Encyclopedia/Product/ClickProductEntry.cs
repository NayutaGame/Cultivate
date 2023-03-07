using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickProductEntry : ProductEntry
{
    private Func<Product, bool> _canClick;
    private Action<Product> _click;

    public ClickProductEntry(string name, string description, int cost, Func<Product, bool> canClick = null, Action<Product> click = null) : base(name, description, cost)
    {
        _canClick = canClick ?? ((clicked) => true);
        _click = click ?? ((clicked) => { });
    }

    public override bool IsClick => true;

    public bool CanClick(Product product) => _canClick(product);

    public void Click(Product product) => _click(product);
}
