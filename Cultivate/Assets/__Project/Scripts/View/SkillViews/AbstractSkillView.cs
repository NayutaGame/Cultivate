
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public abstract class AbstractSkillView : MonoBehaviour, IIndexPath,
    IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected RectTransform _rectTransform;
    protected Image _image;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    [SerializeField] private GameObject ManaCostView;
    [SerializeField] private TMP_Text ManaCostText;
    [SerializeField] protected TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject[] TypeViews;
    [SerializeField] private TMP_Text[] TypeTexts;
    [SerializeField] private TMP_Text AnnotationText;

    #region Accessors

    public abstract ISkillModel GetSkillModel();

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

    public virtual void SetManaCostColor(Color color)
    {
        if (ManaCostText == null)
            return;

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

    public virtual void SetSkillTypeCollection(SkillTypeCollection skillTypeCollection)
    {
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
    }

    public virtual void SetColor(Color color)
    {
        // _image.color = color;
    }

    public virtual void SetCardFace(Sprite cardFace)
    {
        // _image.sprite = cardFace;
    }

    public virtual void SetAnnotationText(string annotationText)
    {
        if (AnnotationText == null)
            return;

        AnnotationText.text = annotationText;
    }

    #endregion

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public virtual void Refresh()
    {
        if (GetIndexPath() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        ISkillModel skill = GetSkillModel();
        if (skill == null || !skill.GetReveal())
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        SetManaCost(skill.GetManaCost());
        SetManaCostColor(skill.GetManaCostColor());
        SetName(skill.GetName());
        SetDescription(skill.GetAnnotatedDescription());
        SetSkillTypeCollection(skill.GetSkillTypeCollection());
        SetAnnotationText(skill.GetAnnotationText());
        SetColor(skill.GetColor());
        SetCardFace(skill.GetCardFace());
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right &&
            RunManager.Get<IInteractable>(GetIndexPath()) is { } item)
        {
            item.GetInteractDelegate().RMouse(item);
            RunCanvas.Instance.Refresh();
            return;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (RunManager.Get<IInteractable>(GetIndexPath()) is { } from && !from.GetInteractDelegate().CanDrag(from))
        {
            eventData.pointerDrag = null;
            RunCanvas.Instance.SetIndexPathForPreview(null);
            return;
        }

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);

        RunCanvas.Instance.SkillGhost.Configure(GetIndexPath());
        RunCanvas.Instance.SkillGhost.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        RunCanvas.Instance.SetIndexPathForPreview(null);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();

        RunCanvas.Instance.SkillGhost.Configure(null);
        RunCanvas.Instance.SkillGhost.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        RunCanvas.Instance.Refresh();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.SkillGhost.UpdateMousePos(eventData.position);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        IInteractable from = RunManager.Get<IInteractable>(drop.GetIndexPath());
        IInteractable to = RunManager.Get<IInteractable>(GetIndexPath());

        from.GetInteractDelegate().DragDrop(from, to);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForPreview(GetIndexPath());
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForPreview(null);
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.UpdateMousePosForPreview(eventData.position);
    }
}
