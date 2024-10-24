using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextHint : MonoBehaviour
{
    [SerializeField] protected RectTransform RectTransform;
    [SerializeField] private TMP_Text Text;
    
    public void PointerEnter(RectTransform rt, PointerEventData d, string text)
    {
        // RectTransform rt = ib.GetSimpleView().GetDisplayTransform();
        // RectTransform hoverRT = ib.transform.parent.GetComponent<ExtraBehaviourAnnotation>()?.HoverTransform;
        // if (hoverRT == null)
        //     hoverRT = rt;
        UpdateCornerPos(rt, rt);
        // SimpleView.SetAddress(address);
        gameObject.SetActive(true);
        // SimpleView.Refresh();

        Text.text = text;
    }

    public void PointerExit(PointerEventData d)
        => gameObject.SetActive(false);

    private void UpdateCornerPos(RectTransform rt, RectTransform hoverRT)
    {
        Vector2 uiPosition = CanvasManager.Instance.World2UI(rt.position);
        Vector2 quadrant = new Vector2(Mathf.RoundToInt(uiPosition.x / Screen.width),
            Mathf.RoundToInt(uiPosition.y / Screen.height));
        
        RectTransform.pivot = quadrant;
        RectTransform.position = hoverRT.TransformPoint(
            (quadrant.x < 0.5f ? hoverRT.sizeDelta.x / 2 : -hoverRT.sizeDelta.x / 2),
            (quadrant.y > 0.5f ? hoverRT.sizeDelta.y / 2 : -hoverRT.sizeDelta.y / 2), 0);
    }
}
