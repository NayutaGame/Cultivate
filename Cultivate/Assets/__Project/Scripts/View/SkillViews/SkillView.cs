
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SkillView : ItemView
{
    [SerializeField] private Image Image;
    // [SerializeField] private GameObject ManaCostView;
    [SerializeField] private TMP_Text ManaCostText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image WuXingImage;

    #region Select

    [SerializeField] private Image SelectionImage;

    private Tween _selectHandle;

    private bool _selected;
    public bool IsSelected() => _selected;
    public void SetSelected(bool selected)
    {
        _selected = selected;

        _selectHandle?.Kill();
        _selectHandle = SelectionImage.DOFade(_selected ? 1 : 0, 0.15f);
        _selectHandle.Restart();
    }

    #endregion

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
    }

    protected virtual void SetCardImage(Sprite sprite)
    {
        Image.sprite = sprite;
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
