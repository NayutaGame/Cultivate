
using System.Collections.Generic;
using System.Text;
using CLLibrary;

public class AnnotationArray
{
    private Annotatable[] _array;
    public Annotatable[] GetArray() => _array;

    private AnnotationArray(Annotatable[] array)
    {
        _array = array;
    }

    public string HighlightFromDescription(string description)
    {
        StringBuilder sb = new(description);
        foreach (Annotatable annotatable in _array)
            sb = sb.Replace(annotatable.GetName(), $"<style=\"Highlight\">{annotatable.GetName()}</style>");

        return sb.ToString();
    }

    public string GetCascadeAnnotated()
    {
        StringBuilder sb = new();
        foreach (Annotatable annotatable in _array)
            sb.Append($"<style=\"Highlight\">{annotatable.GetName()}</style>\n{annotatable.GetHighlight()}\n\n");

        return sb.ToString();
    }
    
    public static AnnotationArray FromDescription(Description description)
    {
        List<Annotatable> annotations = new();
        string descriptionString = description.ToString();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!descriptionString.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!descriptionString.Contains(buffEntry.GetName()))
                continue;

            Annotatable duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        return new AnnotationArray(annotations.ToArray());
    }
    
    public static AnnotationArray FromDescriptionAndCostType(Description description, CostDescription.CostType costType)
    {
        List<Annotatable> annotations = new();
        string descriptionString = description.ToString();

        switch (costType)
        {
            case CostDescription.CostType.Channel:
                annotations.Add(Encyclopedia.KeywordCategory["吟唱"]);
                break;
            case CostDescription.CostType.Health:
                annotations.Add(Encyclopedia.KeywordCategory["燃命"]);
                break;
        }

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!descriptionString.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!descriptionString.Contains(buffEntry.GetName()))
                continue;

            Annotatable duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        return new AnnotationArray(annotations.ToArray());
    }
}
