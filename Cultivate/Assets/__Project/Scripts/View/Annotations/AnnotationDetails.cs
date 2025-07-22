
using UnityEngine;

public class AnnotationDetails
{
    public AnnotationViewType AnnotationViewType;
    public RectTransform InvokerRectTransform;
    public Address Address;
    public float FirstCounter;
    public float SecondCounter;
    public AnnotationAlignmentDetails AnnotationAlignmentDetails;

    public AnnotationDetails(AnnotationViewType annotationViewType,
        RectTransform invokerRectTransform,
        Address address,
        float firstCounter,
        float secondCounter,
        AnnotationAlignmentDetails annotationAlignmentDetails)
    {
        AnnotationViewType = annotationViewType;
        InvokerRectTransform = invokerRectTransform;
        Address = address;
        FirstCounter = firstCounter;
        SecondCounter = secondCounter;
        AnnotationAlignmentDetails = annotationAlignmentDetails;
    }
}