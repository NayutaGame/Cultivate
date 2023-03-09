using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GhostChip : MonoBehaviour
{
    public TMP_Text InfoText;

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

        if (_indexPath == null)
            return;

        object o = RunManager.Get<object>(IndexPath);
        if (o == null)
        {
            InfoText.text = "空";
            return;
        }

        RunChip c;
        if (o is RunChip runChip)
        {
            c = runChip;
        }
        else if (o is AcquiredRunChip acquiredRunChip)
        {
            c = acquiredRunChip.Chip;
        }
        else
        {
            throw new Exception("undefined c");
        }

        InfoText.text = $"{c.GetName()}";
    }

    public void UpdateMousePos(Vector2 pos)
    {
        _rectTransform.position = pos;
    }
}
