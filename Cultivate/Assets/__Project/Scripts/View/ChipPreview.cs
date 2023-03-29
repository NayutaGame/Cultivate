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
        int manaCost;
        if (o is RunChip runChip)
        {
            c = runChip;
            DescriptionText.text = runChip.GetDescription();
            PowerText.text = "";
            manaCost = c.GetManaCost();
        }
        else if (o is AcquiredRunChip acquiredRunChip)
        {
            c = acquiredRunChip.Chip;
            DescriptionText.text = acquiredRunChip.GetDescription();
            PowerText.text = acquiredRunChip.GetPowerString();
            manaCost = acquiredRunChip.GetManaCost();
        }
        else if (o is HeroChipSlot heroChipSlot)
        {
            c = heroChipSlot.RunChip;
            DescriptionText.text = heroChipSlot.GetDescription();
            PowerText.text = heroChipSlot.GetPowerString();
            manaCost = heroChipSlot.GetManaCost();
        }
        else if (o is EnemyChipSlot enemyChipSlot)
        {
            c = enemyChipSlot.Chip;
            DescriptionText.text = enemyChipSlot.GetDescription();
            PowerText.text = enemyChipSlot.GetPowerString();
            manaCost = enemyChipSlot.GetManaCost();
        }
        else
        {
            throw new Exception($"undefined, o.type = {o.GetType()}");
        }

        if (c == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        NameText.text = c.GetName();
        ManaCostText.text = manaCost == 0 ? "" : manaCost.ToString();
        JingJieText.text = c.JingJie.ToString();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }
}
