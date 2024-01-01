
using UnityEngine;

public class AnnotationBehaviour : LegacyAddressBehaviour
{
    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform.pivot = pivot;
        RectTransform.position = pos;
    }
}
