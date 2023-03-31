using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class BattleNodeEntry : NodeEntry
{
    public BattleNodeEntry(string name, string description, CreateEnemyDetails d) : base(name, description,
        create: runNode =>
        {
            EnemyEntry enemyEntry = RunManager.Instance.DrawEnemy(d);
            BattlePanelDescriptor A = new BattlePanelDescriptor(enemyEntry, d);
            DialogPanelDescriptor B = new DialogPanelDescriptor("胜利");
            DialogPanelDescriptor C = new DialogPanelDescriptor("你没能击败对手，虽然损失了一些命元，但还是获得了奖励");

            A._receiveSignal = (signal) =>
            {
                if (signal is BattleResultSignal battleResultSignal)
                {
                    if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
                    {
                        runNode.ChangePanel(B);
                    }
                    else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
                    {
                        runNode.ChangePanel(C);
                    }
                }
            };

            B._receiveSignal = (signal) =>
            {
                RunManager.Instance.Map.TryFinishNode();
            };

            C._receiveSignal = (signal) =>
            {
                RunManager.Instance.Map.TryFinishNode();
            };

            runNode.ChangePanel(A);
        }
    )
    {
    }
}
