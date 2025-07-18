
using System.Collections.Generic;
using System.Text;
using CLLibrary;

public class AnnotationArray
{
    private LegacyAnnotatable[] _array;
    public LegacyAnnotatable[] GetArray() => _array;

    private AnnotationArray(LegacyAnnotatable[] array)
    {
        _array = array;
    }

    public string HighlightFromDescription(string description)
    {
        StringBuilder sb = new(description);
        foreach (LegacyAnnotatable annotatable in _array)
            sb = sb.Replace(annotatable.GetName(), $"<style=\"Highlight\">{annotatable.GetName()}</style>");

        return sb.ToString();
    }

    public string GetCascadeAnnotated()
    {
        StringBuilder sb = new();
        foreach (LegacyAnnotatable annotatable in _array)
            sb.Append($"<style=\"Highlight\">{annotatable.GetName()}</style>\n{annotatable.GetHighlight()}\n\n");

        return sb.ToString();
    }
    
    public static AnnotationArray FromDescription(Description description)
    {
        List<LegacyAnnotatable> annotations = new();
        string descriptionString = description.ToString();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory)
        {
            if (!descriptionString.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory)
        {
            if (!descriptionString.Contains(buffEntry.GetName()))
                continue;

            LegacyAnnotatable duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        return new AnnotationArray(annotations.ToArray());
    }
    
    public static AnnotationArray FromDescriptionAndCostType(Description description, CostType costType)
    {
        List<LegacyAnnotatable> annotations = new();
        string descriptionString = description.ToString();

        switch (costType)
        {
            case CostType.Channel:
                annotations.Add(Encyclopedia.KeywordCategory["吟唱"]);
                break;
            case CostType.Health:
                annotations.Add(Encyclopedia.KeywordCategory["燃命"]);
                break;
        }

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory)
        {
            if (!descriptionString.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory)
        {
            if (!descriptionString.Contains(buffEntry.GetName()))
                continue;

            LegacyAnnotatable duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        return new AnnotationArray(annotations.ToArray());
    }
}
