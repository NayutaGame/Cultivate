using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteView : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Image _image;

    public CanvasGroup MiniGroup;
    public TMP_Text MiniNameText;

    public CanvasGroup NormalGroup;
    public TMP_Text ManaCostText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text SkillTypeText;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        StageNote note = StageManager.Get<StageNote>(GetIndexPath());
        StageWaiGong waiGong = note.WaiGong;
        MiniNameText.text = waiGong.GetName();
        ManaCostText.text = waiGong.GetManaCostString();
        NameText.text = waiGong.GetName();
        DescriptionText.text = waiGong.GetDescription();
        SkillTypeText.text = waiGong.GetSkillTypeString();
        _image.color = CanvasManager.Instance.JingJieColors[waiGong.GetJingJie()];
        _image.sprite = waiGong.Entry.CardFace;
    }

    public Tween Expand()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOSizeDelta(new Vector2(200, 300), 0.6f).SetEase(Ease.InOutQuad));
        seq.Join(MiniGroup.DOFade(0, 0.6f).SetEase(Ease.OutQuad));
        seq.Join(NormalGroup.DOFade(1, 0.6f).SetEase(Ease.InQuad));
        return seq;
    }

    public Tween Shrink()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOSizeDelta(new Vector2(80, 80), 0.6f).SetEase(Ease.InOutQuad));
        seq.Join(NormalGroup.DOFade(0, 0.6f).SetEase(Ease.OutQuad));
        seq.Join(MiniGroup.DOFade(1, 0.6f).SetEase(Ease.InQuad));
        return seq;
    }
}
