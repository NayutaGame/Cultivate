
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SkillView : ItemView
{
    [SerializeField] private Image CardImage;
    [SerializeField] private GameObject ManaCostView;
    [SerializeField] private TMP_Text ManaCostText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject[] TypeViews;
    [SerializeField] private TMP_Text[] TypeTexts;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image WuXingImage;

    public bool IsSelected() => false;
    public void SetSelected(bool selected) { }

    public override void Refresh()
    {
        base.Refresh();
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        ISkillModel skill = Get<ISkillModel>();
        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        SetCardImage(skill.GetSprite());
        SetManaCost(skill.GetManaCost());
        SetName(skill.GetName());
        SetDescription(skill.GetAnnotatedDescription());
        SetSkillTypeComposite(skill.GetSkillTypeComposite());
        SetJingJieSprite(skill.GetJingJieSprite());
        SetWuXingSprite(skill.GetWuXingSprite());
        // SetColor(skill.GetColor());
        // SetCardFace(skill.GetCardFace());
        // SetManaCostColor();
        // SetCounter(skill.GetCurrCounter(), skill.GetMaxCounter());
        // SetAnnotationText(skill.GetAnnotationText());
    }

    protected virtual void SetCardImage(Sprite sprite)
    {
        CardImage.sprite = sprite;
    }

    protected virtual void SetManaCost(int manaCost)
    {
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
    }

    public virtual void SetManaCostState(ManaIndicator.ManaCostState state)
    {
        Color color = Color.white;
        switch (state)
        {
            case ManaIndicator.ManaCostState.Unwritten:
            case ManaIndicator.ManaCostState.Normal:
                break;
            case ManaIndicator.ManaCostState.Reduced:
                color = Color.green;
                break;
            case ManaIndicator.ManaCostState.Shortage:
                color = Color.red;
                break;
        }

        ManaCostText.color = color;
    }

    protected virtual void SetName(string name)
    {
        NameText.text = name;
    }

    protected virtual void SetDescription(string description)
    {
        DescriptionText.text = description;
    }

    protected virtual void SetSkillTypeComposite(SkillTypeComposite skillTypeComposite)
    {
        List<SkillType> skillTypes = skillTypeComposite.ContainedSkillTypes.FirstN(TypeViews.Length).ToList();

        for (int i = 0; i < skillTypes.Count; i++)
        {
            TypeViews[i].SetActive(true);
            TypeTexts[i].text = skillTypes[i].ToString();
        }

        for (int i = skillTypes.Count; i < TypeViews.Length; i++)
        {
            TypeViews[i].SetActive(false);
        }
    }

    protected virtual void SetJingJieSprite(Sprite jingJieSprite)
    {
        JingJieImage.sprite = jingJieSprite;
    }

    protected virtual void SetWuXingSprite(Sprite wuXingSprite)
    {
        if (wuXingSprite != null)
        {
            WuXingImage.sprite = wuXingSprite;
            WuXingImage.enabled = true;
        }
        else
        {
            WuXingImage.enabled = false;
        }
    }

    // [SerializeField] protected RectTransform _rectTransform;
    // [SerializeField] protected Image _image;
    // [SerializeField] protected CanvasGroup _canvasGroup;

    // [SerializeField] private TMP_Text AnnotationText;

    // [SerializeField] private Image CounterImage;
    // [SerializeField] private Image SelectionImage;
    //
    // private bool _selected;
    // public virtual bool IsSelected() => _selected;
    // public virtual void SetSelected(bool selected)
    // {
    //     _selected = selected;
    //     if (SelectionImage != null)
    //         SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    // }

    // public virtual void SetCounter(int currCounter, int maxCounter)
    // {
    //     if (CounterImage == null)
    //         return;
    //
    //     CounterImage.fillAmount = (float)currCounter / maxCounter;
    // }

    // public virtual void SetAnnotationText(string annotationText)
    // {
    //     if (AnnotationText == null)
    //         return;
    //
    //     AnnotationText.text = annotationText;
    // }

    // public virtual void SetColor(Color color)
    // {
    //     // _image.color = color;
    // }
    //
    // public virtual void SetCardFace(Sprite cardFace)
    // {
    //     // _image.sprite = cardFace;
    // }
}
