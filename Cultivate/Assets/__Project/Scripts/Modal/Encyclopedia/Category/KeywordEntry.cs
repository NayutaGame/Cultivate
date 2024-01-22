
using System.Collections.Generic;

public class KeywordEntry : Entry, IAnnotation
{
    private string _description;
    public string Description => _description;

    private IAnnotation[] _annotations;
    public IAnnotation[] GetAnnotations() => _annotations;

    public KeywordEntry(string name, string description) : base(name)
    {
        _description = description;
    }

    public void Generate()
    {
        string description = Description;

        List<IAnnotation> annotations = new();

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (description.Contains(buffEntry.GetName()))
                annotations.Add(buffEntry);
        }

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (description.Contains(keywordEntry.GetName()))
                annotations.Add(keywordEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetAnnotatedDescription(string evaluated = null)
    {
        string toRet = evaluated ?? _description;
        foreach (var annotation in _annotations)
            toRet = toRet.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return toRet;
    }
}
