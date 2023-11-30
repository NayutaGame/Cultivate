
using UnityEngine;

public class PivotBehaviour : MonoBehaviour
{
    public RectTransform IdlePivot;
    public RectTransform HoverPivot;
    public RectTransform FollowPivot;

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}
