using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelStateDragProductCell : CharacterPanelState
{
    private ProductCellView _item;
    public ProductCellView Item => _item;

    public CharacterPanelStateDragProductCell(ProductCellView item)
    {
        _item = item;
    }
}
