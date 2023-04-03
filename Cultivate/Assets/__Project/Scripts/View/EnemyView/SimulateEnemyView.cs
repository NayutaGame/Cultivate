using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimulateEnemyView : EnemyView
{
    protected override void EnemyEntryChanged(int enemyEntryIndex)
    {
        CreateEnemyDetails d = new CreateEnemyDetails();
        RunManager.Instance.Enemy = new RunEnemy(Encyclopedia.EnemyCategory[enemyEntryIndex], d);
        RunCanvas.Instance.Refresh();
    }

    protected override void RandomEnemy()
    {
        CreateEnemyDetails d = new CreateEnemyDetails();
        EnemyEntry enemyEntry = RunManager.Instance.DrawEnemy(d);
        RunManager.Instance.Enemy = new RunEnemy(enemyEntry, d);
        RunCanvas.Instance.Refresh();
    }
}
