using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private void Awake()
    {
        Configure();
    }

    public virtual void Configure() { }
    public virtual void Refresh() { }
}
