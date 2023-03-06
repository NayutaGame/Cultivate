using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Product
{
    // List<Provider>

    public abstract string GetName();
    public abstract int GetCost();

    public abstract bool IsDrag();
    public abstract bool CanDrop(Tile tile);
    public abstract void Drop(Tile tile);

    public abstract bool IsClick();
    public abstract bool CanClick();
    public abstract void Click();
}
