using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProduct : MonoBehaviour
{
    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    private RectTransform _rectTransform;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Refresh()
    {
        gameObject.SetActive(_indexPath != null);

        if (IndexPath == null)
            return;
    }

    public void UpdateMousePos(Vector2 pos)
    {
        _rectTransform.position = pos;
    }
}
