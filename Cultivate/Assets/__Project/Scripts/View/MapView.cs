using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class MapView : MonoBehaviour
{
    private static readonly int HEIGHT = 3;
    private static readonly int WIDTH = 10;

    public Transform Container;

    private IInventory _inventory;
    private NodeView[] _views;

    public virtual void Configure(IInventory inventory)
    {
        _inventory = inventory;
        _views = new NodeView[HEIGHT * WIDTH];
        PopulateList();
    }

    public virtual void Refresh()
    {
        foreach(NodeView view in _views) view.Refresh();
    }

    private void PopulateList()
    {
        for (int x = 0; x < Container.childCount; x++)
        {
            Transform levelTransform = Container.GetChild(x);

            for (int y = 0; y < levelTransform.childCount; y++)
            {
                _views[x * HEIGHT + y] = levelTransform.GetChild(y).GetComponent<NodeView>();
                _views[x * HEIGHT + y].Configure(new IndexPath(_inventory.GetIndexPathString(), x, y));
            }
        }
    }
}
