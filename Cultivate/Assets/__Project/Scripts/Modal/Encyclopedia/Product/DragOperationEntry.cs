using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragOperationEntry : OperationEntry
{
    private Func<OperationProduct, Tile, bool> _canDrop;
    private Action<OperationProduct, Tile> _drop;

    public DragOperationEntry(string name, string description, int cost, Func<OperationProduct, Tile, bool> canDrop, Action<OperationProduct, Tile> drop, List<ILock> locks = null) : base(name, description, cost, locks)
    {
        _canDrop = canDrop;
        _drop = drop;
    }

    public override bool IsDrag => true;

    public bool CanDrop(OperationProduct product, Tile tile)
    {
        return _canDrop(product, tile);
    }

    public void Drop(OperationProduct product, Tile tile)
    {
        _drop(product, tile);
    }
}
