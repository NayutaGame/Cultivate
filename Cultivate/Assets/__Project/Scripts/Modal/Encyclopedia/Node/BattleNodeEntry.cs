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

            int xiuWeiValue = Mathf.RoundToInt(battleRunNode.BaseXiuWeiReward() * RandomManager.Range(0.9f, 1.1f));

            BattlePanelDescriptor A = new(battleRunNode.EntityEntry, battleRunNode.CreateEntityDetails);
            battleRunNode.AddReward(new ResourceRewardDescriptor(xiuWei: xiuWeiValue));

            DiscoverSkillPanelDescriptor B = new();
            DiscoverSkillPanelDescriptor C = new();
            DialogPanelDescriptor D = new($"按Esc退出游戏，游戏结束，感谢游玩");

            A._receiveSignal = (signal) =>
            {
                BattleResultSignal battleResultSignal = signal as BattleResultSignal;
                if (battleResultSignal == null)
                    return A;

                if (!battleRunNode.CreateEntityDetails.AllowBoss)
                {
                    if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
                    {
                        B.SetDetailedText($"胜利！\n获得了{xiuWeiValue}的修为\n请选择一张卡作为奖励");
                        runNode.ChangePanel(B);
                        return B;
                    }
                    else
                    {
                        RunManager.Instance.Battle.HeroMingYuan.SetDCurr(-2);
                        C.SetDetailedText($"你没能击败对手，损失了2命元。\n获得了{xiuWeiValue}修为\n请选择一张卡作为奖励");
                        runNode.ChangePanel(C);
                        return C;
                    }
                }
                else if (battleRunNode.JingJie != JingJie.HuaShen)
                {
                    if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
                    {
                        RunManager.Instance.Battle.HeroMingYuan.SetDCurr(3);
                        B.SetDetailedText($"胜利！\n跨越境界使得你的命元恢复了3\n获得了{xiuWeiValue}的修为\n请选择一张卡作为奖励");
                        runNode.ChangePanel(B);
                        return B;
                    }
                    else
                    {
                        C.SetDetailedText($"你没能击败对手，幸好跨越境界抵消了你的命元伤害。\n获得了{xiuWeiValue}修为\n请选择一张卡作为奖励");
                        runNode.ChangePanel(C);
                        return C;
                    }
                }
                else
                {
                    if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
                    {
                        D.SetDetailedText($"你击败了强大的对手，取得了最终的胜利！（按Esc退出游戏，游戏结束，感谢游玩）");
                        runNode.ChangePanel(D);
                        return D;
                    }
                    else
                    {
                        D.SetDetailedText($"你没能击败对手，受到了致死的命元伤害。（按Esc退出游戏，游戏结束，感谢游玩）");
                        runNode.ChangePanel(D);
                        return D;
                    }
                }
            };

            B._receiveSignal = signal =>
            {
                battleRunNode.ClaimRewards();
                B.DefaultReceiveSignal(signal);
                RunManager.Instance.Map.TryFinishNode();
                return null;
            };

            C._receiveSignal = signal =>
            {
                battleRunNode.ClaimRewards();
                C.DefaultReceiveSignal(signal);
                RunManager.Instance.Map.TryFinishNode();
                return null;
            };

            D._receiveSignal = signal => D;

            runNode.ChangePanel(A);
        }
    )
    {
    }
}
