using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour, IIndexPath, IInteractable,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate()
        => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate)
        => InteractDelegate = interactDelegate;

    public SkillView SkillView;
    private Image _image;

    private bool IsManaShortage()
    {
        SkillSlot slot = DataManager.Get<SkillSlot>(GetIndexPath());
        return slot.IsManaShortage;
    }

    public virtual bool IsSelected()
    {
        if (SkillView == null)
            return false;
        return SkillView.IsSelected();
    }

    public virtual void SetSelected(bool selected)
    {
        if (SkillView != null)
            SkillView.SetSelected(selected);
    }

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _image = GetComponent<Image>();

        SkillView.Configure(new IndexPath($"{GetIndexPath()}.Skill"));
        SkillView.GetComponent<CanvasGroup>().blocksRaycasts = false;

        SkillView.ClearIsManaShortage();
        SkillView.IsManaShortageDelegate += IsManaShortage;
    }

    public void Refresh()
    {
        SkillSlot slot = DataManager.Get<SkillSlot>(GetIndexPath());

        bool locked = slot.State == SkillSlot.SkillSlotState.Locked;
        gameObject.SetActive(!locked);
        if (locked)
            return;

        bool occupied = slot.State == SkillSlot.SkillSlotState.Occupied;
        SkillView.gameObject.SetActive(occupied);
        if (occupied)
            SkillView.Refresh();
    }

    public void OnPointerClick(PointerEventData eventData)
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        if(drag == null || drag.GetDelegate() == null || !drag.GetDelegate().CanDrag(drag))
        {
            eventData.pointerDrag = null;
            RunCanvas.Instance.SetIndexPathForSkillPreview(null);
            return;
        }

        SkillSlot slot = DataManager.Get<SkillSlot>(GetIndexPath());
        if (slot.State != SkillSlot.SkillSlotState.Occupied)
        {
            eventData.pointerDrag = null;
            RunCanvas.Instance.SetIndexPathForSkillPreview(null);
            return;
        }

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);

        RunCanvas.Instance.SkillGhost.Configure(SkillView.GetIndexPath());
        RunCanvas.Instance.SkillGhost.Refresh();
        RunCanvas.Instance.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();

        RunCanvas.Instance.SkillGhost.Configure(null);
        RunCanvas.Instance.SkillGhost.Refresh();
        RunCanvas.Instance.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        RunCanvas.Instance.Refresh();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.SkillGhost.UpdateMousePos(eventData.position);
    }

    public void OnDrop(PointerEventData eventData)
    {
        IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        if (drag == null)
            return;

        IInteractable drop = GetComponent<IInteractable>();
        if (drag == drop)
            return;

        drag.GetDelegate()?.DragDrop(drag, drop);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillSlot slot = DataManager.Get<SkillSlot>(GetIndexPath());
        if (slot.State != SkillSlot.SkillSlotState.Occupied)
        {
            RunCanvas.Instance.SetIndexPathForSkillPreview(null);
            return;
        }

        RunCanvas.Instance.SetIndexPathForSkillPreview(SkillView.GetIndexPath());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.UpdateMousePosForSkillPreview(eventData.position);
    }
}
