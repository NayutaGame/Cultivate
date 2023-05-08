using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEntry : Entry
{
    private string _description;
    public string Description => _description;

    private Func<CreateEntityDetails, bool> _canCreate;
    private Action<RunEntity, CreateEntityDetails> _create;

    public EntityEntry(string name, string description, Func<CreateEntityDetails, bool> canCreate, Action<RunEntity, CreateEntityDetails> create) : base(name)
    {
        _description = description;

        _canCreate = canCreate;
        _create = create;
    }

    public bool CanCreate(CreateEntityDetails d) => _canCreate(d);
    public void Create(RunEntity entity, CreateEntityDetails d) => _create(entity, d);

    public static implicit operator EntityEntry(string name) => Encyclopedia.EntityCategory[name];
}
