using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCategory : Category<NodeEntry>
{
    public NodeCategory()
    {
        List = new()
        {
            new BossNodeEntry("Boss", "Boss", null),
            new MarketNodeEntry("Market", "Market", null),

            new BattleNodeEntry("敌人", "敌人", new CreateEnemyDetails()),

            new AdventureNodeEntry("神殿事件", "",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new DialogPanelDescriptor("来到一处神殿", "我必凯旋", "我已膨胀");
                    BattlePanelDescriptor B = new BattlePanelDescriptor("鶸", new CreateEnemyDetails());
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你无法再获得命元，所有牌获得二动");
                    DialogPanelDescriptor D = new DialogPanelDescriptor("胜利");
                    DialogPanelDescriptor E = new DialogPanelDescriptor("逃跑成功");

                    A._receiveSignal = (signal) =>
                    {
                        SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
                        if (selectedOptionSignal == null)
                            return;

                        runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
                    };

                    B._receiveSignal = (signal) =>
                    {
                        BattleResultSignal battleResultSignal = signal as BattleResultSignal;
                        if (battleResultSignal == null)
                            return;

                        if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
                        {
                            runNode.ChangePanel(D);
                        }
                        else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Escape)
                        {
                            runNode.ChangePanel(E);
                        }
                    };

                    C._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    D._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    E._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    runNode.ChangePanel(A);
                }),

            new AdventureNodeEntry("狂吾师叔事件", "",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new DialogPanelDescriptor("你遇见了狂吾师叔，他问你狂剑的见解", "说出自己的见解(需要有狂剑牌)", "说自己不懂，请师叔赐教");
                    DialogPanelDescriptor B = new DialogPanelDescriptor("获得灵气收集器");
                    DialogPanelDescriptor C = new DialogPanelDescriptor("获得了一张随机牌");

                    A._receiveSignal = (signal) =>
                    {
                        SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
                        if (selectedOptionSignal == null)
                            return;

                        runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
                    };

                    B._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.DrawChip("灵气收集器");
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    C._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.TryDrawWaiGong();
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    runNode.ChangePanel(A);
                })
        };
    }
}
