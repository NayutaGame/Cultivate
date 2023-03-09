using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipPreview : MonoBehaviour
{
    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public TMP_Text PowerText;
    public TMP_Text ManaCostText;
    public TMP_Text JingJieText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;

    private RectTransform _rectTransform;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Refresh()
    {
        if (IndexPath == null)
        {
            gameObject.SetActive(false);
            return;
        }

        object o = RunManager.Get<object>(IndexPath);
        if (o == null)
        {
            gameObject.SetActive(false);
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
            PowerText.text = acquiredRunChip.GetPowerString();
        }
        else if (o is HeroRunChip heroRunChip)
        {
            c = heroRunChip.RunChip;
            PowerText.text = heroRunChip.GetPowerString();
        }
        else
        {
            throw new Exception("undefined c");
        }

        gameObject.SetActive(true);

        NameText.text = c.GetName();
        DescriptionText.text = c.GetDescription();
        int manaCost = c.GetManaCost();
        ManaCostText.text = manaCost == 0 ? "" : manaCost.ToString();
        JingJieText.text = c.GetJingJie().ToString();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }
}
