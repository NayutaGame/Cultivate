
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public abstract class RunChipView : ItemView,
    IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected Image _image;

    public TMP_Text LevelText;
    public TMP_Text ManacostText;
    public TMP_Text NameText;
    public TMP_Text PowerText;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    protected void SetColorFromJingJie(JingJie jingJie)
    {
        _image.color = CanvasManager.Instance.JingJieColors[jingJie];
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);

        RunCanvas.Instance.GhostChip.Configure(GetIndexPath());
        RunCanvas.Instance.GhostChip.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        RunCanvas.Instance.ChipPreview.Configure(null);
        RunCanvas.Instance.ChipPreview.Refresh();
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

    public abstract void OnDrop(PointerEventData eventData);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.ChipPreview.Configure(GetIndexPath());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.ChipPreview.Configure(null);
        RunCanvas.Instance.ChipPreview.Refresh();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.ChipPreview.UpdateMousePos(eventData.position);
        RunCanvas.Instance.ChipPreview.Refresh();
    }
}
