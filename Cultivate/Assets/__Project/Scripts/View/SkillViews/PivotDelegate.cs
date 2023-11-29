
using UnityEngine;

public class PivotDelegate : MonoBehaviour
{
    public RectTransform IdlePivot;
    public RectTransform HoverPivot;
    public RectTransform FollowPivot;

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}
