using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class EnemyPool : Pool<EnemyEntry>
{
    public EnemyPool()
    {
    }

    public void Populate()
    {
        Populate(Encyclopedia.EnemyCategory.Traversal);
        Shuffle();
    }

    public bool TryPopFirst(out RunEnemy runEnemy)
    {
        CreateEnemyDetails d = new();
        bool success = TryPopFirst(entry => entry.CanCreate(d), out EnemyEntry toCreate);
        runEnemy = null;
        if (!success)
            return false;

        runEnemy = toCreate.Create(d);
        return true;
    }
}
