using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [NonSerialized] public RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        Configure();
    }

    public virtual void Configure() { }
    public virtual void Refresh() { }
}
