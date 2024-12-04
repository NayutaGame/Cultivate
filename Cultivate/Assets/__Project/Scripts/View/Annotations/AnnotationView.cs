
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationView : MonoBehaviour
{
    private XView _view;

    private bool _hasAwoken;
    
    private void CheckAwake()
    {
        if (_hasAwoken)
            return;
        _hasAwoken = true;
        AwakeFunction();
    }

    private void AwakeFunction()
    {
        _view ??= GetComponent<XView>();
        _view.CheckAwake();
    }

    public void Awake()
    {
        CheckAwake();
    }

    public void PointerEnter(LegacyInteractBehaviour ib, PointerEventData d)
    {
        RectTransform rt = ib.GetSimpleView().GetViewTransform();
        RectTransform hoverRT = ib.transform.parent.GetComponent<LegacyAnnotationBehaviour>()?.HoverTransform;
        if (hoverRT == null)
            hoverRT = rt;
        UpdateCornerPos(rt, hoverRT);
        _view.SetAddress(ib.GetSimpleView().GetAddress());
        gameObject.SetActive(true);
        _view.Refresh();
    }

    public void PointerExit(LegacyInteractBehaviour ib, PointerEventData d)
        => PointerExit();

    public void PointerMove(LegacyInteractBehaviour ib, PointerEventData d)
    {
        // UpdateMousePos(d.position);
    }

    public void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        RectTransform rect = ib.GetView().GetRect();
        RectTransform hoverRT = ib.GetView().GetBehaviour<AnnotationBehaviour>()?.HoverTransform;
        if (hoverRT == null)
            hoverRT = rect;
        UpdateCornerPos(rect, hoverRT);
        _view.SetAddress(ib.GetAddress());
        gameObject.SetActive(true);
        _view.Refresh();
    }

    public void PointerExit(InteractBehaviour ib, PointerEventData d)
        => PointerExit();
    public void PointerExit()
        => gameObject.SetActive(false);

    public void PointerMove(InteractBehaviour ib, PointerEventData d)
    {
        // UpdateMousePos(d.position);
    }

    private void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform rectTransform = _view.GetRect();
        rectTransform.pivot = pivot;
        rectTransform.position = pos;
    }

    private void UpdateCornerPos(RectTransform rt, RectTransform hoverRT)
    {
        Vector2 uiPosition = CanvasManager.Instance.World2UI(rt.position);
        Vector2 quadrant = new Vector2(Mathf.RoundToInt(uiPosition.x / Screen.width),
            Mathf.RoundToInt(uiPosition.y / Screen.height));
        
        _view.GetRect().pivot = quadrant;
        _view.GetRect().position = hoverRT.TransformPoint(
            (quadrant.x < 0.5f ? hoverRT.sizeDelta.x / 2 : -hoverRT.sizeDelta.x / 2),
            (quadrant.y > 0.5f ? hoverRT.sizeDelta.y / 2 : -hoverRT.sizeDelta.y / 2), 0);
    }
}
