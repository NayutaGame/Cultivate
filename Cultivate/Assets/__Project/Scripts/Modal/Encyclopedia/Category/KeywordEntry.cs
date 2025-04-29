
public class KeywordEntry : Entry, Annotatable
{
    private string _description;
    private AnnotationArray _cascade;

    public KeywordEntry(string id, string description) : base(id)
    {
        _description = description;
    }
    
    public string GetName() => GetId();
    public Description GetDescription() => _description;

    public string GetHighlight(Description description)
        => description.GetHighlight(_cascade);
    public string GetHighlight()
        => GetHighlight(GetDescription());
    
    public string GetCascadeAnnotated()
        => _cascade.GetCascadeAnnotated();
    
    public void GenerateCascade()
        => _cascade = AnnotationArray.FromDescription(GetDescription());
}
