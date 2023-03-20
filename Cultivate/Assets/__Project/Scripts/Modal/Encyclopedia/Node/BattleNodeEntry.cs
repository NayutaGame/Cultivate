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
            DialogPanelDescriptor C = new DialogPanelDescriptor("逃跑成功");

            A._receiveSignal = (signal) =>
            {
                BattleResultSignal battleResultSignal = signal as BattleResultSignal;
                if (battleResultSignal == null)
                    return;

                if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
                {
                    runNode.ChangePanel(B);
                }
                else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Escape)
                {
                    runNode.ChangePanel(C);
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
