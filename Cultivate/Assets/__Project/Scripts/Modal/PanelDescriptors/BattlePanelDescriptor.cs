using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePanelDescriptor : PanelDescriptor
{
    private EnemyEntry _enemyEntry;
    public EnemyEntry EnemyEntry => _enemyEntry;

    private CreateEnemyDetails _createEnemyDetails;

    public BattlePanelDescriptor(string enemyName, CreateEnemyDetails createEnemyDetails) : this(Encyclopedia.EnemyCategory[enemyName], createEnemyDetails) { }
    public BattlePanelDescriptor(EnemyEntry enemyEntry, CreateEnemyDetails createEnemyDetails)
    {
        _enemyEntry = enemyEntry;
        _createEnemyDetails = createEnemyDetails;
    }

    public override void Enter()
    {
        base.Enter();

        RunManager.Instance.SetEnemy(_enemyEntry.Create(_createEnemyDetails));
    }
}
