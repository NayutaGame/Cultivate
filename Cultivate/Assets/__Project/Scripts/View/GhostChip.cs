using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GhostChip : MonoBehaviour
{
    protected Image _image;

    public TMP_Text InfoText;

    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

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
        else if (o is HeroChipSlot heroRunChip)
        {
            c = heroRunChip.RunChip;
        }
        else
        {
            throw new Exception($"undefined, o.type = {o.GetType()}");
        }

        if (c == null)
        {
            InfoText.text = "空";
            SetColorFromJingJie(JingJie.LianQi);
            return;
        }

        InfoText.text = $"{c.GetName()}";
        SetColorFromJingJie(c.GetJingJie());
    }

    protected void SetColorFromJingJie(JingJie jingJie)
    {
        _image.color = CanvasManager.Instance.JingJieColors[jingJie];
    }

    public void UpdateMousePos(Vector2 pos)
    {
        _rectTransform.position = pos;
    }
}
