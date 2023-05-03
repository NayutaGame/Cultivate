using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnnotation
{
    string GetName();
    void Generate();
    IAnnotation[] GetAnnotations();
    string GetAnnotatedDescription(string evaluated = null);
}
