using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NoteCardView : ItemView
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

        StageNote note = StageManager.Get<StageNote>(GetIndexPath());

        StageWaiGong waiGong = note.WaiGong;

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
            TypeTexts[i].text = skillTypes.ToString();
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
}
