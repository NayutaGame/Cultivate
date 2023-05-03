using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteCardView : ItemView,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private RectTransform _rectTransform;

    public GameObject ManaCostView;
    public TMP_Text ManaCostText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public GameObject[] TypeViews;
    public TMP_Text[] TypeTexts;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        Refresh();
    }

    public override void Refresh()
    {
        base.Refresh();

        if (GetIndexPath() == null)
            return;

        StageNote note = StageManager.Get<StageNote>(GetIndexPath());

        StageWaiGong waiGong = note.WaiGong;

        if (waiGong == null)
        {
            ManaCostText.text = "";
            ManaCostView.SetActive(false);
            WaiGongEntry juQiShu = Encyclopedia.ChipCategory["聚气术"] as WaiGongEntry;
            NameText.text = juQiShu.Name;
            DescriptionText.text = juQiShu.Evaluate(0, 0);
            foreach(var v in TypeViews)
            {
                v.SetActive(false);
            }
            return;
        }

        int manaCost = waiGong.GetManaCost();
        if (manaCost == 0)
        {
            ManaCostText.text = "";
            ManaCostView.SetActive(false);
        }
        else
        {
            ManaCostText.text = manaCost.ToString();
            ManaCostView.SetActive(true);
        }

        NameText.text = waiGong.GetName();
        DescriptionText.text = waiGong.GetDescription();

        SkillTypeCollection skillTypeCollection = waiGong.GetSkillTypeCollection();
        List<SkillType> skillTypes = skillTypeCollection.ContainedTags.FirstN(TypeViews.Length).ToList();

        for (int i = 0; i < skillTypes.Count; i++)
        {
            TypeViews[i].SetActive(true);
            TypeTexts[i].text = skillTypes[i].ToString();
        }

        for (int i = skillTypes.Count; i < TypeViews.Length; i++)
        {
            TypeViews[i].SetActive(false);
        }

        // _image.color = CanvasManager.Instance.JingJieColors[waiGong.GetJingJie()];
        // _image.sprite = waiGong.Entry.CardFace;
    }

    public Tween GetExpandTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOScale(1, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    public Tween GetShrinkTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOScale(0.5f, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.SetIndexPathForPreview(GetIndexPath());
        StageManager.Instance.Pause();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.SetIndexPathForPreview(null);
        StageManager.Instance.Resume();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        StageCanvas.Instance.UpdateMousePosForPreview(eventData.position);
    }
}
