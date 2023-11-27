
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechView : ItemView, IInteractable,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    public RectTransform _rectTransform;
    public Image _image;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text CountText;

    public override void Refresh()
    {
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Mech mech = Get<Mech>();
        if (mech == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        SetMechType(mech.GetMechType());
        SetCount(mech.Count);
    }

    private void SetMechType(MechType mechType)
    {
        NameText.text = mechType.ToString();
    }

    private void SetCount(int count)
    {
        CountText.text = count.ToString();
    }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    public virtual void OnBeginDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.BEGIN_DRAG, this, eventData);
    public virtual void OnEndDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.END_DRAG, this, eventData);
    public virtual void OnDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.DRAG, this, eventData);
    public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);

    #endregion

    public void BeginDrag(PointerEventData eventData)
    {
        CanvasManager.Instance.MechGhost.SetAddress(GetAddress());
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
        CanvasManager.Instance.MechGhost._rectTransform.position = eventData.position;
    }
}
