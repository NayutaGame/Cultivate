using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffView : MonoBehaviour
{
    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public virtual void Refresh()
    {

    }
}
