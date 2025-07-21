
using UnityEngine;

public class RectAnnotationAlignmentDetails : AnnotationAlignmentDetails
{
    public Rect Rect;

    public RectAnnotationAlignmentDetails(Rect rect)
    {
        Rect = rect;
    }

    public override Vector3 GetProgressCirclePosition()
    {
        return new Vector3(Rect.xMax, Rect.yMax, 0);
    }

    public override Vector3 GetCenterPosition()
    {
        return Rect.center;
    }
}