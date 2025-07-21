
public class AnnotationDetails
{
    public AnnotationViewType AnnotationViewType;
    public AnnotationDetails ParentAnnotationDetails;
    public Address Address;
    public float FirstCounter;
    public float SecondCounter;
    public AnnotationAlignmentDetails AnnotationAlignmentDetails;

    public AnnotationDetails(AnnotationViewType annotationViewType,
        AnnotationDetails parentAnnotationDetails,
        Address address,
        float firstCounter,
        float secondCounter,
        AnnotationAlignmentDetails annotationAlignmentDetails)
    {
        AnnotationViewType = annotationViewType;
        ParentAnnotationDetails = parentAnnotationDetails;
        Address = address;
        FirstCounter = firstCounter;
        SecondCounter = secondCounter;
        AnnotationAlignmentDetails = annotationAlignmentDetails;
    }
}