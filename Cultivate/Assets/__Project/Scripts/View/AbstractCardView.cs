
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public abstract class AbstractCardView : ItemView,
    IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected RectTransform _rectTransform;
    private Image _image;

    [SerializeField] private GameObject ManaCostView;
    [SerializeField] private TMP_Text ManaCostText;
    [SerializeField] protected TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject[] TypeViews;
    [SerializeField] private TMP_Text[] TypeTexts;
    [SerializeField] private TMP_Text AnnotationText;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    #region Accessors

    public abstract ICardModel GetCardModel();

    public void SetManaCost(int manaCost)
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

    public void SetManaCostColor(Color color)
    {
        if (ManaCostText == null)
            return;

        ManaCostText.color = color;
    }

    public void SetName(string name)
    {
        if (NameText == null)
            return;

        NameText.text = name;
    }

    public void SetDescription(string description)
    {
        if (DescriptionText == null)
            return;

        DescriptionText.text = description;
    }

    public void SetSkillTypeCollection(SkillTypeCollection skillTypeCollection)
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

    public void SetColor(Color color)
    {
        // _image.color = color;
    }

    public void SetCardFace(Sprite cardFace)
    {
        // _image.sprite = cardFace;
    }

    public void SetAnnotationText(string annotationText)
    {
        if (AnnotationText == null)
            return;

        AnnotationText.text = annotationText;
    }

    #endregion

    public override void Refresh()
    {
        base.Refresh();

        if (GetIndexPath() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        ICardModel model = GetCardModel();
        if (model == null)
            return;

        gameObject.SetActive(model.GetReveal());
        if (!model.GetReveal())
            return;

        SetManaCost(model.GetManaCost());
        SetManaCostColor(model.GetManaCostColor());
        SetName(model.GetName());
        SetDescription(model.GetAnnotatedDescription());
        SetSkillTypeCollection(model.GetSkillTypeCollection());
        SetAnnotationText(model.GetAnnotationText());
        SetColor(model.GetColor());
        SetCardFace(model.GetCardFace());
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            enemyChipSlot.TryIncreseJingJie();
            RunCanvas.Instance.Refresh();
            return;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            eventData.pointerDrag = null;

            RunCanvas.Instance.SetIndexPathForPreview(null);
            return;
        }

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);

        RunCanvas.Instance.GhostChip.Configure(GetIndexPath());
        RunCanvas.Instance.GhostChip.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        RunCanvas.Instance.SetIndexPathForPreview(null);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();

        RunCanvas.Instance.GhostChip.Configure(null);
        RunCanvas.Instance.GhostChip.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        RunCanvas.Instance.Refresh();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.GhostChip.UpdateMousePos(eventData.position);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        if (RunManager.Get<AcquiredRunChip>(GetIndexPath()) is { } acquiredRunChip)
        {
            AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
            if (fromAcquired != null)
            {
                if (RunManager.Instance.AcquiredInventory.TryMerge(fromAcquired, acquiredRunChip)) return;
                if (RunManager.Instance.AcquiredInventory.Swap(fromAcquired, acquiredRunChip)) return;
                return;
            }

            HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
            if (fromHeroChipSlot != null)
            {
                // if (fromHeroChipSlot.TryUnequip(acquiredRunChip)) return;
                if (fromHeroChipSlot.TryUnequip()) return;
                return;
            }
        }
        else if (RunManager.Get<HeroChipSlot>(GetIndexPath()) is { } heroChipSlot)
        {
            AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
            if (fromAcquired != null)
            {
                if (heroChipSlot.TryEquip(fromAcquired)) return;
                return;
            }

            HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
            if (fromHeroChipSlot != null)
            {
                if (RunManager.Instance.Hero.HeroSlotInventory.Swap(fromHeroChipSlot, heroChipSlot)) return;
                return;
            }
        }
        else if (RunManager.Get<EnemyChipSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            RunChip fromArenaChip = RunManager.Get<RunChip>(drop.GetIndexPath());
            if (fromArenaChip != null)
            {
                if (enemyChipSlot.TryWrite(fromArenaChip)) return;
                return;
            }

            AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
            if (fromAcquired != null)
            {
                if (enemyChipSlot.TryWrite(fromAcquired)) return;
                return;
            }

            HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
            if (fromHeroChipSlot != null)
            {
                if (enemyChipSlot.TryWrite(fromHeroChipSlot)) return;
                return;
            }
        }
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
