using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductCategory : Category<ProductEntry>
{
    public ProductCategory()
    {
        List = new()
        {
            new DragProductEntry("改良", "改良", 2,
                canDrop: (product, tile) =>
                {
                    return tile.XiuWei > 0 || tile.ChanNeng > 0;
                },
                drop: (product, tile) =>
                {
                    if (tile.XiuWei > 0)
                        tile.Terrain.XiuWei += 1;

                    if (tile.ChanNeng > 0)
                        tile.Terrain.ChanNeng += 1;
                }),
            new DragProductEntry("收获", "收获", 2,
                canDrop: (product, tile) =>
                {
                    return tile.XiuWei > 0 || tile.ChanNeng > 0;
                },
                drop: (product, tile) =>
                {
                    if (tile.XiuWei > 0)
                        tile.Terrain.XiuWei -= 1;

                    if (tile.ChanNeng > 0)
                        tile.Terrain.ChanNeng -= 1;
                }),

            new BuildingEntry("金建筑", "金建筑", 2),
            new ClickProductEntry("抽卡", "抽卡", 2,
                click: runProduct =>
                {
                    RunManager.Instance.DrawChip();
                }),
        };
    }
}
