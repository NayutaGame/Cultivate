
using TMPro;
using UnityEngine;

public class LineAnnotationView : AnnotationView
{
    [SerializeField] private TMP_Text Description;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        return Vector3.zero;
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableLine line = d.Address.Get<AnnotatableLine>();
        Description.text = line.GetDescription().GetHighlightedString();
    }
}
