
public class KeywordEntry : Entry, LegacyAnnotatable
{
    private string _description;
    private AnnotationArray _cascade;

    public KeywordEntry(string id, string description) : base(id)
    {
        _description = description;
    }
    
    public string GetName() => GetId();
    public Description GetLiteralDescription() => _description;

    public string GetHighlight()
        => GetLiteralDescription().GetHighlight(_cascade);
    
    public string GetCascadeAnnotated()
        => _cascade.GetCascadeAnnotated();
    
    public void GenerateCascade()
        => _cascade = AnnotationArray.FromDescription(GetLiteralDescription());
}
