using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ArenaEditor : Inventory<RunEnemy>
{
    public override string GetIndexPathString() => "TryGetArenaEnemy";

    public ArenaEditor()
    {
        6.Do(item => Add(new RunEnemy()));
    }

    public void SetArenaEnemy(int index, int enemyEntryIndex)
    {
        this[index] = new RunEnemy(Encyclopedia.EnemyCategory[enemyEntryIndex], new CreateEnemyDetails());
    }

    public void SetArenaEnemyRandom(int index)
    {
        int r = RandomManager.Range(0, Encyclopedia.EnemyCategory.GetCount());
        SetArenaEnemy(index, r);
    }

    public void SetArenaEnemyHealth(int index, int health)
    {
        this[index].Health = health;
    }
}
