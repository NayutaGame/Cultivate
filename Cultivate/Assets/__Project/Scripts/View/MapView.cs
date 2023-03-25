using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class MapView : MonoBehaviour, IIndexPath
{
    private static readonly int HEIGHT = 3;
    private static readonly int WIDTH = 10;

    public Transform Container;

    private NodeView[] _views;
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
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
                int i = x * HEIGHT + y;
                _views[i] = levelTransform.GetChild(y).GetComponent<NodeView>();
                _views[i].Configure(new IndexPath($"{_indexPath}.Nodes#{i}"));
            }
        }
    }
}
