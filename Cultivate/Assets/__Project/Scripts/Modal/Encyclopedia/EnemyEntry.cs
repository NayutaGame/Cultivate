using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntry : Entry
{
    private string _description;
    public string Description;

    private Func<CreateEnemyDetails, bool> _canCreate;
    private Action<RunEnemy, CreateEnemyDetails> _create;

    public EnemyEntry(string name, string description, Func<CreateEnemyDetails, bool> canCreate, Action<RunEnemy, CreateEnemyDetails> create) : base(name)
    {
        _description = description;

        _canCreate = canCreate;
        _create = create;
    }

    public bool CanCreate(CreateEnemyDetails d) => _canCreate(d);
    public void Create(RunEnemy enemy, CreateEnemyDetails d) => _create(enemy, d);
}
