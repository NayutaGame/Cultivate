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

        RunChip chip = RunManager.Get<RunChip>(_indexPath);
        if(chip == null)
        {
            InfoText.text = "空";
            return;
        }
        else
        {
            InfoText.text = $"{chip.GetName()}[{chip.Level}]";
        }
    }

    public void UpdateMousePos(Vector2 pos)
    {
        _rectTransform.position = pos;
    }
}
