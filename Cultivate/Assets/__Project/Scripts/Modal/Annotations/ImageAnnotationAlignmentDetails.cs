
using UnityEngine;

public class ImageAnnotationAlignmentDetails : AnnotationAlignmentDetails
{
    public RectTransform RectTransform;

    public ImageAnnotationAlignmentDetails(RectTransform rectTransform)
    {
        RectTransform = rectTransform;
    }

    public override Vector3 GetProgressCirclePosition()
    {
        Vector3[] corners = new Vector3[4];
        RectTransform.GetWorldCorners(corners);
        return corners[2];
    }

    public override Vector3 GetCenterPosition()
    {
        return RectTransform.position;
    }
}