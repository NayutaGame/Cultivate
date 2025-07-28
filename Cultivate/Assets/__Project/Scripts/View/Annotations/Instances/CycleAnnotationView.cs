
using UnityEngine;

public class CycleAnnotationView : AnnotationView
{
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        return Vector3.zero;
    }
}
