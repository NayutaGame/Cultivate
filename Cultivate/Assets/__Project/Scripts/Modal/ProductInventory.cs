using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ProductInventory
{
    private List<Product> _list;

    public int Count => _list.Count;

    public Product this[int i]
    {
        get
        {
            if (i >= _list.Count)
                return null;
            return _list[i];
        }
    }

    public ProductInventory()
    {
        _list = new List<Product>();
        Encyclopedia.OperationCategory.Traversal.Do(item => _list.Add(new OperationProduct(item)));
        Encyclopedia.BuildingCategory.Traversal.Do(item => _list.Add(new BuildingProduct(item)));
    }

    public bool CanDrop(Product product, Tile tile)
    {
        return product.CanDrop(tile);
    }

    public void Drop(Product product, Tile tile)
    {
        product.Drop(tile);
    }

    public bool CanClick(Product product)
    {
        return product.CanClick();
    }

    public void Click(Product product)
    {
        product.Click();
    }
}
