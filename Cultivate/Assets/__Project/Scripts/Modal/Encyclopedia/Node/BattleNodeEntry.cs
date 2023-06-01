using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class BattleNodeEntry : NodeEntry
{
    public BattleNodeEntry(string name, string description) : base(name, description,
        create: runNode =>
        {
            BattleRunNode battleRunNode = runNode as BattleRunNode;

            // Boss战斗奖励应不应该更丰富些

            BattlePanelDescriptor A = new(battleRunNode.EntityEntry, battleRunNode.CreateEntityDetails);
            DiscoverSkillPanelDescriptor B = new($"胜利，请选择一张卡作为奖励");
            DiscoverSkillPanelDescriptor C = new($"你没能击败对手，虽然损失了一些命元，但还是可以选择一张卡作为奖励");

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

            B._receiveSignal = signal =>
            {
                B.DefaultReceiveSignal(signal);
                // battleRunNode.ClaimRewards();
                RunManager.Instance.Map.TryFinishNode();
            };

            C._receiveSignal = signal =>
            {
                RunManager.Instance.MingYuan -= 1; // report.mingyuanPenalty, should take effect inside A.Exit, not here
                // battleRunNode.ClaimRewards();
                C.DefaultReceiveSignal(signal);
                RunManager.Instance.Map.TryFinishNode();
            };

            runNode.ChangePanel(A);
        }
    )
    {
    }
}
