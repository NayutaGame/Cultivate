
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechInteractBehaviour : InteractBehaviour,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    public Image _image;

    public void BeginDrag(PointerEventData eventData)
    {
        CanvasManager.Instance.MechGhost.SetAddress(GetSimpleView().GetAddress());
        CanvasManager.Instance.MechGhost.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);
    }

    public void EndDrag(PointerEventData eventData)
    {
        CanvasManager.Instance.MechGhost.SetAddress(null);
        CanvasManager.Instance.MechGhost.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);
    }

    public void Drag(PointerEventData eventData)
    {
        CanvasManager.Instance.MechGhost.GetDisplayTransform().position = eventData.position;
    }
}
