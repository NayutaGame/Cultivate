using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardPreview : ItemView
{
    public NoteCardView CardView;
    public TMP_Text AnnotationText;

    private RectTransform _rectTransform;

    private void Awake()
    {
    }

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);
        _rectTransform = GetComponent<RectTransform>();
        CardView.Configure(indexPath);
    }

    public override void Refresh()
    {
        base.Refresh();

        bool isNull = GetIndexPath() == null;

        gameObject.SetActive(!isNull);
        if (isNull)
            return;

        // ICardPreview
        StageNote note = StageManager.Get<StageNote>(GetIndexPath());
        StageWaiGong waiGong = note.WaiGong;
        if (waiGong == null)
        {
            AnnotationText.text = null;
        }
        else
        {
            AnnotationText.text = waiGong.GetAnnotationString();
        }
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }
}
