
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSkillPreview : SkillView
{
    [SerializeField] protected RectTransform _rectTransform;

    public StageSkillView SkillView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(address);
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillView.Refresh();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }
}
