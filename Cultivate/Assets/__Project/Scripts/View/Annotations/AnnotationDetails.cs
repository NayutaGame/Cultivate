
public class AnnotationDetails
{
    public enum AlignmentMethod
    {
        CenterAlignment,
        RectAlignment,
    }
    
    public AnnotationViewType AnnotationViewType;
    public Address Address;
    public XView View;
    public float FirstCounter;
    public float SecondCounter;
    public AlignmentMethod Alignment;

    public AnnotationDetails(AnnotationViewType annotationViewType,
        Address address,
        XView view,
        float firstCounter,
        float secondCounter,
        AlignmentMethod alignment)
    {
        AnnotationViewType = annotationViewType;
        Address = address;
        View = view;
        FirstCounter = firstCounter;
        SecondCounter = secondCounter;
        Alignment = alignment;
    }
}