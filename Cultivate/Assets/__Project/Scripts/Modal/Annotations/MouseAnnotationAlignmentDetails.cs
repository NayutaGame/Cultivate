
using UnityEngine;

public class MouseAnnotationAlignmentDetails : AnnotationAlignmentDetails
{
    public Rect Rect;

    public MouseAnnotationAlignmentDetails(Rect rect)
    {
        Rect = rect;
    }

    public override Vector3 GetProgressCirclePosition()
    {
        return new Vector3(Rect.xMax, Rect.yMax, 0);
    }

    public override Vector3 GetCenterPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 quadrant = new Vector2(Mathf.RoundToInt(mousePosition.x / Screen.width),
            Mathf.RoundToInt(mousePosition.y / Screen.height));

        float offsetLength = 15;

        float basePointX = mousePosition.x;
        float basePointY = mousePosition.y;

        float offsetX = Mathf.Lerp(-offsetLength, offsetLength, quadrant.x);
        float offsetY = Mathf.Lerp(-offsetLength, offsetLength, quadrant.y);
        
        Vector2 screenPosition = new Vector2(basePointX + offsetX, basePointY + offsetY);
        return CanvasManager.Instance.UI2World(screenPosition);
    }
}