
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public class RunChipView : MonoBehaviour, IAddress,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected Image _image;

    private Address _address;
    public Address GetIndexPath() => _address;
    public T Get<T>() => _address.Get<T>();

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public virtual void Configure(Address address)
    {
        _address = address;
    }

    public virtual void Refresh()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Get<SkillSlot>() is { } enemyChipSlot)
        {
            enemyChipSlot.TryIncreaseJingJie();
            RunCanvas.Instance.Refresh();
            return;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (Get<SkillSlot>() is { } enemyChipSlot)
        {
            eventData.pointerDrag = null;

            RunCanvas.Instance.SetIndexPathForSkillPreview(null);
            return;
        }

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragSkill(this);

        RunCanvas.Instance.SkillGhost.Configure(GetIndexPath());
        RunCanvas.Instance.SkillGhost.Refresh();
        RunCanvas.Instance.Refresh();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();

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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForSkillPreview(GetIndexPath());
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
