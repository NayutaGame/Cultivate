
using TMPro;
using UnityEngine;

public class CycleAnnotationView : AnnotationView
{
    [SerializeField] private TMP_Text[] Texts;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        Texts[0].text = new Description("金").GetHighlightedString();
        Texts[1].text = new Description("水").GetHighlightedString();
        Texts[2].text = new Description("木").GetHighlightedString();
        Texts[3].text = new Description("火").GetHighlightedString();
        Texts[4].text = new Description("土").GetHighlightedString();
    }

    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        return Vector3.zero;
    }
}
