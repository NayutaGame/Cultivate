using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragDrop
{
    DragDropDelegate GetDragDropDelegate();
    void SetDragDropDelegate(DragDropDelegate dragDrop);
}
