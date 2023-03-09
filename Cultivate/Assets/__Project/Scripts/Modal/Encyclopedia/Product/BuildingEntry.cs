using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEntry : DragProductEntry, ILock
{
    public BuildingEntry(string name, string description, int cost, Func<Product, Tile, bool> canDrop = null, Action<Product, Tile> drop = null) : base(name, description, cost,
            canDrop: (product, tile) =>
            {
                // if (tile.Building != null)
                //     return false;
                bool? flag = canDrop?.Invoke(product, tile);
                return !flag.HasValue || flag.Value;
            },
            drop: (product, tile) =>
            {
                // tile.Building = new RunBuilding(name);
                drop?.Invoke(product, tile);
            })
    {
    }
}
