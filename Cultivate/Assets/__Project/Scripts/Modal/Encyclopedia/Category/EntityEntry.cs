using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEntry : Entry
{
    private string _description;
    public string Description => _description;

    public EntityEntry(string name, string description) : base(name)
    {
        _description = description;
    }

    public static implicit operator EntityEntry(string name) => Encyclopedia.EntityCategory[name];
}
