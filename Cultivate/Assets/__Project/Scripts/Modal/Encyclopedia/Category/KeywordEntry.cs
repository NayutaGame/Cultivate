
public class KeywordEntry : Entry, IAnnotation
{
    private string _description;
    public string GetDescription() => _description;

    public KeywordEntry(string name, string description) : base(name)
    {
        _description = description;
    }

    private AnnotationArray _annotationArray;
    public void GenerateAnnotations()
        => _annotationArray = AnnotationArray.FromDescription(GetDescription());
    public string GetHighlight(string description)
        => _annotationArray.HighlightFromDescription(description);
    public string GetHighlight()
        => GetHighlight(GetDescription());
    
    public string GetExplanation()
        => _annotationArray.GetExplanation();
}
