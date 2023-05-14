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

            // 根据CreateEnemyDetail生成，比如打筑基敌人应该抽两张卡
            // Boss战斗奖励应该更丰富

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
        // create: runNode =>
        // {
        //     BattleRunNode battleRunNode = runNode as BattleRunNode;
        //
        //     // 根据CreateEnemyDetail生成，比如打筑基敌人应该抽两张卡
        //     // Boss战斗奖励应该更丰富
        //     DesignerEnvironment.AddRewardForBattleRunNode(battleRunNode);
        //
        //     BattlePanelDescriptor A = new BattlePanelDescriptor(battleRunNode.EntityEntry, battleRunNode.CreateEntityDetails);
        //     DialogPanelDescriptor B = new DialogPanelDescriptor($"胜利\n\n得到{battleRunNode.GetRewardsString()}");
        //     DialogPanelDescriptor C = new DialogPanelDescriptor($"你没能击败对手，虽然损失了一些命元，但还是获得了奖励\n\n得到{battleRunNode.GetRewardsString()}");
        //
        //     A._receiveSignal = (signal) =>
        //     {
        //         if (signal is BattleResultSignal battleResultSignal)
        //         {
        //             if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
        //             {
        //                 runNode.ChangePanel(B);
        //             }
        //             else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
        //             {
        //                 runNode.ChangePanel(C);
        //             }
        //         }
        //     };
        //
        //     B._receiveSignal = (signal) =>
        //     {
        //         battleRunNode.ClaimRewards();
        //         RunManager.Instance.Map.TryFinishNode();
        //     };
        //
        //     C._receiveSignal = (signal) =>
        //     {
        //         RunManager.Instance.MingYuan -= 1; // report.mingyuanPenalty
        //         battleRunNode.ClaimRewards();
        //         RunManager.Instance.Map.TryFinishNode();
        //     };
        //
        //     runNode.ChangePanel(A);
        // }
    )
    {
    }
}
