
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechView : MonoBehaviour, IAddress, IInteractable,
    IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    protected RectTransform _rectTransform;
    protected Image _image;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text CountText;

    public virtual void Configure(Address address)
    {
        _address = address;
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public virtual void Refresh()
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

        RunCanvas.Instance.MechGhost.Configure(GetAddress());
        RunCanvas.Instance.MechGhost.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);

        // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
        RunCanvas.Instance.Refresh();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();

        RunCanvas.Instance.MechGhost.Configure(null);
        RunCanvas.Instance.MechGhost.Refresh();

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        RunCanvas.Instance.Refresh();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        RunCanvas.Instance.MechGhost.UpdateMousePos(eventData.position);
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
}
