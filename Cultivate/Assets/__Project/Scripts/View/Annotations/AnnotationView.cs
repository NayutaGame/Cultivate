
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationView : MonoBehaviour
{
    private SimpleView SimpleView;

    public void Awake()
    {
        SimpleView ??= GetComponent<SimpleView>();
        SimpleView.Awake();
    }

    public void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        SimpleView.SetAddress(ib.GetSimpleView().GetAddress());
        gameObject.SetActive(true);
        SimpleView.Refresh();
    }

    public void PointerExit(InteractBehaviour ib, PointerEventData d)
        => gameObject.SetActive(false);

    public void PointerMove(InteractBehaviour ib, PointerEventData d)
        => UpdateMousePos(d.position);

    private void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform rectTransform = SimpleView.GetDisplayTransform();
        rectTransform.pivot = pivot;
        rectTransform.position = pos;
    }
}
