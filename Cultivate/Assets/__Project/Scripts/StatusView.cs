using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    public TMP_Text StatusText;

    private IndexPath _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        Refresh();
    }

    public void Refresh()
    {
        StatusText.text = RunManager.Get<string>(_indexPath);
    }
}
