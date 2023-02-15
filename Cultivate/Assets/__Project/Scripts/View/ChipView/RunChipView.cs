
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunChipView : MonoBehaviour
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
        Refresh();
    }

    public virtual void Refresh() { }

    public void OnPointerDown(PointerEventData eventData) { }

    protected GameObject _ghostGO;
    protected RectTransform _ghostTransform;
}
