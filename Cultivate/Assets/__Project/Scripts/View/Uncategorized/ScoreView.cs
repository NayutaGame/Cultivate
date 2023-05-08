using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh() { }
}
