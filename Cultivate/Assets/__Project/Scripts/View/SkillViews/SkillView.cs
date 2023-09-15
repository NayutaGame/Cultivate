
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public abstract class SkillView : ItemView, IInteractable,
    IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected RectTransform _rectTransform;
    protected Image _image;

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    [SerializeField] private Image CardImage;
    [SerializeField] private GameObject ManaCostView;
    [SerializeField] private TMP_Text ManaCostText;
    [SerializeField] protected TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject[] TypeViews;
    [SerializeField] private TMP_Text[] TypeTexts;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image WuXingImage;
    [SerializeField] private TMP_Text AnnotationText;
    [SerializeField] private Image CounterImage;
    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public virtual bool IsSelected() => _selected;
    public virtual void SetSelected(bool selected)
    {
        _selected = selected;
        if (SelectionImage != null)
            SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    }

    public void ClearIsManaShortage() => IsManaShortageDelegate = null;
    public event Func<bool> IsManaShortageDelegate;

    #region Accessors

    public virtual void SetCardImage(Sprite sprite)
    {
        if (CardImage == null)
            return;

        CardImage.sprite = sprite;
    }

    public virtual void SetManaCost(int manaCost)
    {
        if (ManaCostView == null)
            return;

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

    public virtual void SetManaCostColor()
    {
        if (ManaCostText == null)
            return;

        Color color = Color.white;

        if (IsManaShortageDelegate != null && IsManaShortageDelegate())
            color = Color.red;

        ManaCostText.color = color;
    }

    public virtual void SetName(string name)
    {
        if (NameText == null)
            return;

        NameText.text = name;
    }

    public virtual void SetDescription(string description)
    {
        if (DescriptionText == null)
            return;

        DescriptionText.text = description;
    }

    public virtual void SetSkillTypeComposite(SkillTypeComposite skillTypeComposite)
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

    public virtual void SetColor(Color color)
    {
        // _image.color = color;
    }

    public virtual void SetCardFace(Sprite cardFace)
    {
        // _image.sprite = cardFace;
    }

    public virtual void SetJingJieSprite(Sprite jingJieSprite)
    {
        if (JingJieImage != null)
        {
            JingJieImage.sprite = jingJieSprite;
        }
    }

    public virtual void SetWuXingSprite(Sprite wuXingSprite)
    {
        if (WuXingImage != null)
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

    public virtual void SetCounter(int currCounter, int maxCounter)
    {
        if (CounterImage == null)
            return;

        CounterImage.fillAmount = (float)currCounter / maxCounter;
    }

    public virtual void SetAnnotationText(string annotationText)
    {
        if (AnnotationText == null)
            return;

        AnnotationText.text = annotationText;
    }

    #endregion

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

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
        SetManaCostColor();
        SetName(skill.GetName());
        SetDescription(skill.GetAnnotatedDescription());
        SetSkillTypeComposite(skill.GetSkillTypeComposite());
        SetAnnotationText(skill.GetAnnotationText());
        SetColor(skill.GetColor());
        SetCardFace(skill.GetCardFace());
        SetJingJieSprite(skill.GetJingJieSprite());
        SetWuXingSprite(skill.GetWuXingSprite());
        SetCounter(skill.GetCurrCounter(), skill.GetMaxCounter());
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        IInteractable item = GetComponent<IInteractable>();
        if (item == null)
            return;

        InteractDelegate interactDelegate = item.GetDelegate();
        if (interactDelegate == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            interactDelegate.LMouse(item);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            interactDelegate.RMouse(item);
        }

        RunCanvas.Instance.Refresh();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        if(drag == null || drag.GetDelegate() == null || !drag.GetDelegate().CanDrag(drag))
        {
            eventData.pointerDrag = null;
            RunCanvas.Instance.SetIndexPathForSkillPreview(null);
            return;
        }

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);

        RunCanvas.Instance.SkillGhost.SetAddress(GetAddress());
        RunCanvas.Instance.SkillGhost.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();

        RunCanvas.Instance.SkillGhost.SetAddress(null);
        RunCanvas.Instance.SkillGhost.Refresh();
        RunCanvas.Instance.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        RunCanvas.Instance.Refresh();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.SkillGhost.UpdateMousePos(eventData.position);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        if (drag == null)
            return;

        IInteractable drop = GetComponent<IInteractable>();
        if (drag == drop)
            return;

        drag.GetDelegate()?.DragDrop(drag, drop);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, this, eventData);
        // if (eventData.dragging) return;
        // RunCanvas.Instance.SetIndexPathForSkillPreview(GetAddress());
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, this, eventData);
        // if (eventData.dragging) return;
        // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
        GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, this, eventData);
        // if (eventData.dragging) return;
        // RunCanvas.Instance.UpdateMousePosForSkillPreview(eventData.position);
    }
}
