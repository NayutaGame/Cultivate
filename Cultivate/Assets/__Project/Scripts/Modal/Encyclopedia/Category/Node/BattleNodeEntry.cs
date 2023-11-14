
using UnityEngine;

public class BattleNodeEntry : NodeEntry
{
    public BattleNodeEntry(string name, string description) : base(name, description, normal: true,
        create: runNode =>
        {
            BattleRunNode battleRunNode = runNode as BattleRunNode;

            int xiuWeiValue = Mathf.RoundToInt(battleRunNode.BaseXiuWeiReward() * RandomManager.Range(0.9f, 1.1f));

            BattlePanelDescriptor A = new(battleRunNode.Template);
            battleRunNode.AddReward(new ResourceReward(gold: xiuWeiValue));

            DiscoverSkillPanelDescriptor B = new("胜利");
            DiscoverSkillPanelDescriptor C = new("惜败");
            DialogPanelDescriptor D = new($"按Esc退出游戏，游戏结束，感谢游玩");

            if (!battleRunNode.DrawEntityDetails.AllowBoss)
            {
                A.SetWin(() =>
                {
                    B.SetDetailedText($"胜利！\n获得了{xiuWeiValue}的修为\n请选择一张卡作为奖励");
                    return B;
                });

                A.SetLose(() =>
                {
                    RunManager.Instance.Environment.SetDMingYuan(-2);
                    C.SetDetailedText($"你没能击败对手，损失了2命元。\n获得了{xiuWeiValue}修为\n请选择一张卡作为奖励");
                    return C;
                });
            }
            else if (battleRunNode.JingJie != JingJie.HuaShen)
            {
                A.SetWin(() =>
                {
                    RunManager.Instance.Environment.SetDMingYuan(3);
                    B.SetDetailedText($"胜利！\n跨越境界使得你的命元恢复了3\n获得了{xiuWeiValue}的修为\n请选择一张卡作为奖励");
                    return B;
                });

                A.SetLose(() =>
                {
                    C.SetDetailedText($"你没能击败对手，幸好跨越境界抵消了你的命元伤害。\n获得了{xiuWeiValue}修为\n请选择一张卡作为奖励");
                    return C;
                });
            }
            else
            {
                A.SetWin(() =>
                {
                    D.SetDetailedText($"你击败了强大的对手，取得了最终的胜利！（按Esc退出游戏，游戏结束，感谢游玩）");
                    return D;
                });

                A.SetLose(() =>
                {
                    D.SetDetailedText($"你没能击败对手，受到了致死的命元伤害。（按Esc退出游戏，游戏结束，感谢游玩）");
                    return D;
                });
            }

            B._receiveSignal = signal =>
            {
                battleRunNode.ClaimRewards();
                B.DefaultReceiveSignal(signal);
                return null;
            };

            C._receiveSignal = signal =>
            {
                battleRunNode.ClaimRewards();
                C.DefaultReceiveSignal(signal);
                return null;
            };

            D._receiveSignal = signal => D;

            runNode.ChangePanel(A);
        }
    )
    {
    }
}
