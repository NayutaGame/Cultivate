

using UnityEngine;
using UnityEngine.UI;

public abstract class AnnotationView : XView
{
    [SerializeField] public InteractBehaviour CoverIb;
    [SerializeField] public Image Cover;
    public abstract Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d);
}