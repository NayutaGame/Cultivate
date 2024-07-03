
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationView : MonoBehaviour
{
    private SimpleView SimpleView;
    public SimpleView GetSimpleView() => SimpleView;

    public void Awake()
    {
        SimpleView ??= GetComponent<SimpleView>();
        SimpleView.AwakeFunction();
    }

    public void PointerEnter(InteractBehaviour ib, PointerEventData d) => PointerEnter(ib, d, ib.GetSimpleView().GetAddress());
    public void PointerEnter(InteractBehaviour ib, PointerEventData d, Address address)
    {
        RectTransform rt = ib.GetSimpleView().GetDisplayTransform();
        RectTransform hoverRT = ib.transform.parent.GetComponent<ExtraBehaviourAnnotation>()?.HoverTransform;
        if (hoverRT == null)
            hoverRT = rt;
        UpdateCornerPos(rt, hoverRT);
        SimpleView.SetAddress(address);
        gameObject.SetActive(true);
        SimpleView.Refresh();
    }

    public void PointerExit(InteractBehaviour ib, PointerEventData d) => PointerExit(ib, d, ib.GetSimpleView().GetAddress());
    public void PointerExit(InteractBehaviour ib, PointerEventData d, Address address)
        => gameObject.SetActive(false);

    public void PointerMove(InteractBehaviour ib, PointerEventData d) => PointerMove(ib, d, ib.GetSimpleView().GetAddress());
    public void PointerMove(InteractBehaviour ib, PointerEventData d, Address address)
    {
        // UpdateMousePos(d.position);
    }

    private void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform rectTransform = SimpleView.GetDisplayTransform();
        rectTransform.pivot = pivot;
        rectTransform.position = pos;
    }

    private void UpdateCornerPos(RectTransform rt, RectTransform hoverRT)
    {
        Vector2 uiPosition = CanvasManager.Instance.World2UI(rt.position);
        Vector2 quadrant = new Vector2(Mathf.RoundToInt(uiPosition.x / Screen.width),
            Mathf.RoundToInt(uiPosition.y / Screen.height));
        
        SimpleView.GetDisplayTransform().pivot = quadrant;
        SimpleView.GetDisplayTransform().position = hoverRT.TransformPoint(
            (quadrant.x < 0.5f ? hoverRT.sizeDelta.x / 2 : -hoverRT.sizeDelta.x / 2),
            (quadrant.y > 0.5f ? hoverRT.sizeDelta.y / 2 : -hoverRT.sizeDelta.y / 2), 0);
    }
}
