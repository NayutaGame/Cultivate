
public interface IAnnotation
{
    string GetName();
    string GetDescription();
    
    void GenerateAnnotations();
    string GetHighlight();
    string GetHighlight(string description);
    string GetExplanation();
}
