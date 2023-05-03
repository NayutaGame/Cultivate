using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string GetName()
        => Name;

    public void Generate()
    {
        string description = Description;

        List<IAnnotation> annotations = new();

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (description.Contains(buffEntry.Name))
                annotations.Add(buffEntry);
        }

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (description.Contains(keywordEntry.Name))
                annotations.Add(keywordEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetAnnotatedDescription(string evaluated = null)
    {
        string toRet = evaluated ?? _description;
        foreach (var annotation in _annotations)
            toRet = toRet.Replace(annotation.GetName(), $"<style=\"H2\">{annotation.GetName()}</style>");

        return toRet;
    }
}
