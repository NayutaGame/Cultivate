using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEntry : Entry
{
    private string _description;
    public string Description => _description;

    private Func<DrawEntityDetails, bool> _canDraw;
    private Action<RunEntity> _create;

    public EntityEntry(string name, string description, Func<DrawEntityDetails, bool> canDraw, Action<RunEntity> create) : base(name)
    {
        _description = description;

        _canDraw = canDraw;
        _create = create;
    }

    public bool CanDraw(DrawEntityDetails d) => _canDraw(d);
    public void Create(RunEntity entity) => _create(entity);

    public static implicit operator EntityEntry(string name) => Encyclopedia.EntityCategory[name];
}
