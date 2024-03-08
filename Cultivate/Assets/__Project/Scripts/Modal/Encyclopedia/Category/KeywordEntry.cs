
using System.Collections.Generic;
using System.Text;
using CLLibrary;

public class KeywordEntry : Entry, IAnnotation
{
    private string _description;
    public string GetDescription() => _description;

    private IAnnotation[] _annotations;

    public KeywordEntry(string name, string description) : base(name)
    {
        _description = description;
    }

    public void GenerateAnnotations()
    {
        string description = GetDescription();

        List<IAnnotation> annotations = new();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!description.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!description.Contains(buffEntry.GetName()))
                continue;

            IAnnotation duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetHighlight() => GetHighlight(GetDescription());
    public string GetHighlight(string description)
    {
        StringBuilder sb = new(description);
        foreach (IAnnotation annotation in _annotations)
            sb = sb.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return sb.ToString();
    }
    public string GetExplanation()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _annotations)
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>\n{annotation.GetHighlight()}\n\n");

        return sb.ToString();
    }
}
