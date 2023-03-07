using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragProductEntry : ProductEntry
{
    private Func<Product, Tile, bool> _canDrop;
    private Action<Product, Tile> _drop;

    public DragProductEntry(string name, string description, int cost, Func<Product, Tile, bool> canDrop = null, Action<Product, Tile> drop = null) : base(name, description, cost)
    {
        _canDrop = canDrop ?? ((product, tile) => true);
        _drop = drop ?? ((product, tile) => { });
    }

    public override bool IsDrag => true;

    public bool CanDrop(Product product, Tile tile)
    {
        return _canDrop(product, tile);
    }

    public void Drop(Product product, Tile tile)
    {
        _drop(product, tile);
    }
}
