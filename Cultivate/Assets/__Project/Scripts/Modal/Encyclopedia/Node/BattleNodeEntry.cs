using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleNodeEntry : NodeEntry
{
    public BattleNodeEntry(string name, string description) : base(name, description,
        create: runNode =>
        {
            BattleRunNode battleRunNode = runNode as BattleRunNode;

            // Boss战斗奖励应不应该更丰富些
            int xiuWeiValue = Mathf.RoundToInt(battleRunNode.BaseXiuWeiReward() * RandomManager.Range(0.9f, 1.1f));

            BattlePanelDescriptor A = new(battleRunNode.EntityEntry, battleRunNode.CreateEntityDetails);
            battleRunNode.AddReward(new ResourceRewardDescriptor(xiuWei: xiuWeiValue));

            DiscoverSkillPanelDescriptor B = new($"胜利！获得了{xiuWeiValue}的修为\n请选择一张卡作为奖励");
            DiscoverSkillPanelDescriptor C = new($"你没能击败对手\n虽然损失了一些命元，但还是获得了{xiuWeiValue}修为，以及可以选择一张卡作为奖励");

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
                battleRunNode.ClaimRewards();
                B.DefaultReceiveSignal(signal);
                RunManager.Instance.Map.TryFinishNode();
            };

            C._receiveSignal = signal =>
            {
                battleRunNode.ClaimRewards();
                RunManager.Instance.MingYuan -= 1;
                C.DefaultReceiveSignal(signal);
                RunManager.Instance.Map.TryFinishNode();
            };

            runNode.ChangePanel(A);
        }
    )
    {
    }
}
