
public interface IAnnotation
{
    string GetName();
    void Generate();
    IAnnotation[] GetAnnotations();
    string GetAnnotatedDescription(string evaluated = null);
}
