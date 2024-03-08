
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardView : SimpleView
{
    [SerializeField] private Image CardImage;
    // [SerializeField] private GameObject ManaCostView;
    [SerializeField] private TMP_Text ManaCostText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image WuXingImage;

    public override void Refresh()
    {
        base.Refresh();

        ISkillModel skill = Get<ISkillModel>();

        SetCardImage(skill.GetSprite());
        SetManaCost(skill.GetManaCost());
        SetName(skill.GetName());
        SetDescription(skill.GetHighlight());
        SetSkillTypeComposite(skill.GetSkillTypeComposite());
        SetJingJieSprite(skill.GetJingJieSprite());
        SetWuXingSprite(skill.GetWuXingSprite());
    }

    protected virtual void SetCardImage(Sprite sprite)
    {
        CardImage.sprite = sprite != null ? sprite : Encyclopedia.SpriteCategory["Default"].Sprite;
    }

    protected virtual void SetManaCost(int manaCost)
    {
        if (manaCost == 0)
        {
            ManaCostText.text = "";
            // ManaCostView.SetActive(false);
        }
        else
        {
            ManaCostText.text = manaCost.ToString();
            // ManaCostView.SetActive(true);
        }
    }

    public virtual void SetManaCostState(ManaIndicator.ManaCostState state)
    {
        Color color = CanvasManager.Instance.ManaCostColors[0];
        switch (state)
        {
            case ManaIndicator.ManaCostState.Unwritten:
                color = CanvasManager.Instance.ManaCostColors[0];
                break;
            case ManaIndicator.ManaCostState.Normal:
                color = CanvasManager.Instance.ManaCostColors[1];
                break;
            case ManaIndicator.ManaCostState.Reduced:
                color = CanvasManager.Instance.ManaCostColors[2];
                break;
            case ManaIndicator.ManaCostState.Shortage:
                color = CanvasManager.Instance.ManaCostColors[3];
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
        // List<SkillType> skillTypes = skillTypeComposite.ContainedSkillTypes.FirstN(TypeViews.Length).ToList();
        //
        // for (int i = 0; i < skillTypes.Count; i++)
        // {
        //     TypeViews[i].SetActive(true);
        //     TypeTexts[i].text = skillTypes[i].ToString();
        // }
        //
        // for (int i = skillTypes.Count; i < TypeViews.Length; i++)
        // {
        //     TypeViews[i].SetActive(false);
        // }
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
}
