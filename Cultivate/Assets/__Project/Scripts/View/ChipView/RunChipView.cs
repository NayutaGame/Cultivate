
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public abstract class RunChipView : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    protected RectTransform _transform;
    protected Image _image;

    public TMP_Text InfoText;

    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public virtual void Refresh() { }

    public void OnPointerDown(PointerEventData eventData) { }

    protected GameObject _ghostGO;
    protected RectTransform _ghostTransform;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _ghostGO = Instantiate(gameObject, CanvasManager.Instance.GhostHolder);
        _ghostGO.GetComponent<Image>().raycastTarget = false;
        _ghostTransform = _ghostGO.GetComponent<RectTransform>();

        GridLayoutGroup glg = _transform.parent.GetComponent<GridLayoutGroup>();
        Vector2 size = glg != null ? glg.cellSize : _ghostTransform.sizeDelta;

        _ghostTransform.sizeDelta = size;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_ghostGO);
        _ghostGO = null;
        _ghostTransform = null;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        CanvasManager.Instance.Refresh();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _ghostTransform.position = eventData.position;
    }

    public abstract void OnDrop(PointerEventData eventData);
}
