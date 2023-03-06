using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationCategory : Category<OperationEntry>
{
    public OperationCategory()
    {
        List = new()
        {
            new DragOperationEntry("改良", "改良", 2,
                canDrop: (runProduct, tile) =>
                {
                    return tile.XiuWei > 0 || tile.ChanNeng > 0;
                },
                drop: (runProduct, tile) =>
                {
                    if (tile.XiuWei > 0)
                        tile.Terrain.XiuWei += 1;

                    if (tile.ChanNeng > 0)
                        tile.Terrain.ChanNeng += 1;
                }),
            new DragOperationEntry("收获", "收获", 2,
                canDrop: (runProduct, tile) =>
                {
                    return tile.XiuWei > 0 || tile.ChanNeng > 0;
                },
                drop: (runProduct, tile) =>
                {
                    if (tile.XiuWei > 0)
                        tile.Terrain.XiuWei -= 1;

                    if (tile.ChanNeng > 0)
                        tile.Terrain.ChanNeng -= 1;
                }),
        };
    }
}
