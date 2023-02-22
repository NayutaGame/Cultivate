using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipPreview : MonoBehaviour
{
    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public TMP_Text NameText;
    public TMP_Text DescriptionText;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh()
    {
        if (IndexPath == null)
        {
            gameObject.SetActive(false);
            return;
        }

        RunChip c = RunManager.Get<RunChip>(IndexPath);
        if (c == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        NameText.text = c.GetName();
        DescriptionText.text = c.GetDescription();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        rectTransform.pivot = pivot;
        rectTransform.position = pos;
    }
}
