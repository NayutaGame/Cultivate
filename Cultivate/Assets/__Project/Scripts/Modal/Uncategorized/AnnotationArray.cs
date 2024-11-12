
using System.Collections.Generic;
using System.Text;
using CLLibrary;

public class AnnotationArray
{
    private IAnnotation[] _array;

    private AnnotationArray(IAnnotation[] array)
    {
        _array = array;
    }

    public string HighlightFromDescription(string description)
    {
        StringBuilder sb = new(description);
        foreach (IAnnotation annotation in _array)
            sb = sb.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return sb.ToString();
    }

    public string GetExplanation()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _array)
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>\n{annotation.GetHighlight()}\n\n");

        return sb.ToString();
    }
    
    public static AnnotationArray FromDescription(string description)
    {
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

        return new AnnotationArray(annotations.ToArray());
    }
    
    public static AnnotationArray FromDescriptionAndCostType(string description, CostDescription.CostType costType)
    {
        List<IAnnotation> annotations = new();

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

        return new AnnotationArray(annotations.ToArray());
    }
}
