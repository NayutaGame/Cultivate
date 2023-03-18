using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product
{
    // List<Provider>

    public ProductEntry _entry;

    public Product(string entryName) : this(Encyclopedia.ProductCategory[entryName]) { }
    public Product(ProductEntry entry)
    {
        _entry = entry;
    }

    public string GetName() => _entry.Name;
    public int GetCost()
    {
        return _entry.Cost;
        // return _entry.Cost(RunUsedTimes);
    }

    public virtual bool IsDrag() => _entry.IsDrag;
    public virtual bool CanDrop(Tile tile)
    {
        if (!_entry.IsDrag)
            return false;

        var dragProductEntry = _entry as DragProductEntry;
        return dragProductEntry.CanDrop(this, tile);
    }

    public virtual void Drop(Tile tile)
    {
        var dragProductEntry = _entry as DragProductEntry;
        dragProductEntry.Drop(this, tile);
    }

    public bool IsClick() => _entry.IsClick;
    public bool CanClick()
    {
        if (!_entry.IsClick)
            return false;

        var clickProductEntry = _entry as ClickProductEntry;
        return clickProductEntry.CanClick(this);
    }

    public void Click()
    {
        var clickProductEntry = _entry as ClickProductEntry;
        clickProductEntry.Click(this);
    }
}
