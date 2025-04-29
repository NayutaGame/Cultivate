
public interface Annotatable
{
    string GetName();
    Description GetDescription();
    
    string GetCascadeAnnotated();
    
    string GetHighlight();
    string GetHighlight(Description description);
    
    void GenerateCascade();
}
