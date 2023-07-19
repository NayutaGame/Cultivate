
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public class RunChipView : MonoBehaviour, IIndexPath,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected Image _image;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public virtual void Refresh()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DataManager.Get<SkillSlot>(GetIndexPath()) is { } enemyChipSlot)
        {
            enemyChipSlot.TryIncreaseJingJie();
            RunCanvas.Instance.Refresh();
            return;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (DataManager.Get<SkillSlot>(GetIndexPath()) is { } enemyChipSlot)
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
