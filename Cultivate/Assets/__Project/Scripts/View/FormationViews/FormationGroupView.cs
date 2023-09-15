using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public class FormationGroupView : ItemView, IInteractable,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private ListView SubFormationInventoryView; // FormationView
    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public virtual bool IsSelected() => _selected;
    public virtual void SetSelected(bool selected)
    {
        _selected = selected;
        if (SelectionImage != null)
            SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    }

    public override void Refresh()
    {
        base.Refresh();
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        FormationGroupEntry formationGroup = Get<FormationGroupEntry>();
        if (formationGroup == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        SetName(formationGroup.Name);
        SetSubFormations();
    }

    public virtual void SetName(string s)
    {
        NameText.text = s;
    }

    public virtual void SetSubFormations()
    {
        if (SubFormationInventoryView == null)
            return;
        SubFormationInventoryView.SetAddress(GetAddress().Append(".SubFormations"));
        SubFormationInventoryView.Refresh();
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
        // IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        // if(drag == null || drag.GetDelegate() == null || !drag.GetDelegate().CanDrag(drag))
        // {
        //     eventData.pointerDrag = null;
        //     RunCanvas.Instance.SetIndexPathForPreview(null);
        //     return;
        // }
        //
        // // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
        //
        // RunCanvas.Instance.SkillGhost.Configure(GetIndexPath());
        // RunCanvas.Instance.SkillGhost.Refresh();
        // RunCanvas.Instance.Refresh();
        //
        // if (_image != null)
        //     _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);
        //
        // RunCanvas.Instance.SetIndexPathForPreview(null);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        // // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
        //
        // RunCanvas.Instance.SkillGhost.Configure(null);
        // RunCanvas.Instance.SkillGhost.Refresh();
        // RunCanvas.Instance.Refresh();
        //
        // if (_image != null)
        //     _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);
        //
        // RunCanvas.Instance.Refresh();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        // RunCanvas.Instance.SkillGhost.UpdateMousePos(eventData.position);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        // IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        // if (drag == null)
        //     return;
        //
        // IInteractable drop = GetComponent<IInteractable>();
        // if (drag == drop)
        //     return;
        //
        // drag.GetDelegate()?.DragDrop(drag, drop);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // if (eventData.dragging) return;
        // RunCanvas.Instance.SetIndexPathForPreview(GetIndexPath());
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // if (eventData.dragging) return;
        // RunCanvas.Instance.SetIndexPathForPreview(null);
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
        // if (eventData.dragging) return;
        // RunCanvas.Instance.UpdateMousePosForPreview(eventData.position);
    }
}
