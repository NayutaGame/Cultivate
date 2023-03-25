using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Arena : Inventory<RunEnemy>
{
    public override string GetIndexPathString() => "TryGetArenaEnemy";

    public Arena()
    {
        6.Do(item => Add(new RunEnemy()));
    }

    public void SetEnemy(int index, int enemyEntryIndex)
    {
        this[index] = new RunEnemy(Encyclopedia.EnemyCategory[enemyEntryIndex], new CreateEnemyDetails());
    }

    public void SetRandom(int index)
    {
        int r = RandomManager.Range(0, Encyclopedia.EnemyCategory.GetCount());
        SetEnemy(index, r);
    }
}
