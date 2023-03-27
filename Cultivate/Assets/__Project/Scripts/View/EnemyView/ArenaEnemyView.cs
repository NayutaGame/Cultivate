using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaEnemyView : EnemyView
{
    protected override void EnemyEntryChanged(int enemyEntryIndex)
    {
        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        int index = RunManager.Instance.Arena.IndexOf(enemy);
        RunManager.Instance.Arena.SetEnemy(index, enemyEntryIndex);
        RunCanvas.Instance.Refresh();
    }

    protected override void RandomEnemy()
    {
        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        int index = RunManager.Instance.Arena.IndexOf(enemy);
        RunManager.Instance.Arena.SetRandom(index);
        RunCanvas.Instance.Refresh();
    }
}
