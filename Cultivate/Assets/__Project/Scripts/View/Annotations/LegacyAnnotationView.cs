
using UnityEngine;

[RequireComponent(typeof(XView))]
public class LegacyAnnotationView : MonoBehaviour
{
    private XView _view;
    public XView GetView() => _view;

    private bool _hasAwoken;

    public void Awake()
    {
        CheckAwake();
    }
    
    public void CheckAwake()
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

    private void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform rectTransform = _view.GetRect();
        rectTransform.pivot = pivot;
        rectTransform.position = pos;
    }

    private void UpdateCornerPos(RectTransform rt, RectTransform hoverRT)
    {
        // usage:
        // UpdateCornerPos(d.Rect, d.HoverRect == null ? d.Rect : d.HoverRect);
        Vector2 uiPosition = CanvasManager.Instance.World2UI(rt.position);
        Vector2 quadrant = new Vector2(Mathf.RoundToInt(uiPosition.x / Screen.width),
            Mathf.RoundToInt(uiPosition.y / Screen.height));
        
        _view.GetRect().pivot = quadrant;
        _view.GetRect().position = hoverRT.TransformPoint(
            (quadrant.x < 0.5f ? hoverRT.sizeDelta.x / 2 : -hoverRT.sizeDelta.x / 2),
            (quadrant.y > 0.5f ? hoverRT.sizeDelta.y / 2 : -hoverRT.sizeDelta.y / 2), 0);
    }
}
