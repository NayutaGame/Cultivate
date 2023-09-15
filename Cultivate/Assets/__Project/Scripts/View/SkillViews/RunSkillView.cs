
using TMPro;
using UnityEngine.EventSystems;

public class RunSkillView : SkillView
{
    public TMP_Text SiblingText;

    public override void SetSiblingIndex(int value)
    {
        base.SetSiblingIndex(value);
        if (SiblingText != null)
            SiblingText.text = value.ToString();
    }

    // public override void OnPointerClick(PointerEventData eventData) { }
    // public override void OnBeginDrag(PointerEventData eventData) { }
    // public override void OnEndDrag(PointerEventData eventData) { }
    // public override void OnDrag(PointerEventData eventData) { }
    // public override void OnDrop(PointerEventData eventData) { }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForSkillPreview(GetAddress());
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public override void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.UpdateMousePosForSkillPreview(eventData.position);
    }
}
