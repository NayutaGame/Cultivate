using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class NodeCategory : Category<NodeEntry>
{
    public NodeCategory()
    {
        List = new()
        {
            new MarketNodeEntry("Market", "Market", null),

            new BattleNodeEntry("敌人", "敌人"),

            new AdventureNodeEntry("神殿事件", "",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new DialogPanelDescriptor("来到一处神殿", "我必凯旋", "我已膨胀");
                    BattlePanelDescriptor B = new BattlePanelDescriptor("鶸", new CreateEnemyDetails());
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你无法再获得命元，所有牌获得二动");
                    DialogPanelDescriptor D = new DialogPanelDescriptor("胜利");
                    DialogPanelDescriptor E = new DialogPanelDescriptor("你没能击败对手，虽然损失了一些命元，但还是获得了奖励");

                    A._receiveSignal = (signal) =>
                    {
                        SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
                        if (selectedOptionSignal == null)
                            return;

                        runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
                    };

                    B._receiveSignal = (signal) =>
                    {
                        if (signal is BattleResultSignal battleResultSignal)
                        {
                            if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
                            {
                                runNode.ChangePanel(D);
                            }
                            else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
                            {
                                runNode.ChangePanel(E);
                            }
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
                    DialogPanelDescriptor B = new DialogPanelDescriptor("获得了一张随机牌");
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
                        RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    C._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    runNode.ChangePanel(A);
                }),

            new AdventureNodeEntry("池塘", "",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new DialogPanelDescriptor(
                        "你经过一滩清池，一座亭子立于池中，你不禁驻足。墨绿色的亭柱顶着朱红色的宝顶，绕池一周，你并未看到通向亭子的道路，你决定：",
                        "饮一口池水（回血）",
                        "在池边停下修炼（回血药）",
                        "尝试飞身跃入亭中（拥有一张筑基牌，则抽一张牌）。那亭子一看便不是凡物，里面说不好有一番机缘，你决定尝试飞身跃入亭中一探究竟。");
                    DialogPanelDescriptor B = new DialogPanelDescriptor("池水清冽，你沿路奔波的劳累一扫而光。");
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你不禁暗想：这块宝地许是某位修士前辈开辟的宝地，不妨在这修炼一段时间，肯定大有裨益。");
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你纵深一跃，在空中一个翻身，稳稳落入亭中，亭子正中的石桌上，有一本《XXX》。");
                    DialogPanelDescriptor E = new DialogPanelDescriptor("你纵身一跃，结果距离亭子仍有数尺，你落入水中，等你浮上水面时亭子已经消失了，你大感可惜，悻悻离去。");

                    A._receiveSignal = (signal) =>
                    {
                        if (signal is SelectedOptionSignal selectedOptionSignal)
                        {
                            if (selectedOptionSignal.Selected == 0)
                            {
                                runNode.ChangePanel(B);
                            }
                            else if (selectedOptionSignal.Selected == 1)
                            {
                                runNode.ChangePanel(C);
                            }
                            else
                            {
                                bool flag = RunManager.Instance.AcquiredInventory.FirstObj(acquired =>
                                                acquired.GetJingJie() >= JingJie.ZhuJi) != null ||
                                            RunManager.Instance.Hero.HeroSlotInventory.Traversal.FirstObj(slot =>
                                                slot.GetJingJie().HasValue && slot.GetJingJie() >= JingJie.ZhuJi) !=
                                            null;
                                if (flag)
                                {
                                    runNode.ChangePanel(D);
                                }
                                else
                                {
                                    runNode.ChangePanel(E);
                                }
                            }
                        }
                    };

                    B._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    C._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    D._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    E._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    runNode.ChangePanel(A);
                }),

            new AdventureNodeEntry("路途劳累", "",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new DialogPanelDescriptor(
                        "旅行数日，即便你已踏上修炼之道，路途上的劳累也无法完全消除（【惩罚】生命值惩罚5），你前往一处聚落决定休息几日，凝练修为。路过坊市时，一位清新脱俗的姑娘映入你的眼帘，",
                        "不禁入神，驻足停留（【惩罚】【条件 驻足停留】 修为减少）",
                        "加快脚步前往客栈（【奖励】回复生命值10，修为增加）",
                        "前往坊市转转（跳转至商店界面）");
                    DialogPanelDescriptor B = new DialogPanelDescriptor("看到她，你想起了江南的雨，你楞在原地，看出了神，直到背后的人推了你一把，你才回过神来。你暗暗一惊：你的道心不稳。修仙长路漫漫，需要长年累月的苦修，男女之情是修仙路上的一大阻碍，跟何况对方还是一位没有踏上仙路的女子。你赶快加快脚步离开此地。");
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你来到客栈后，就开始运转功法，一段时间后，旅途的劳累一扫而空，并巩固了修为。");
                    // DialogPanelDescriptor D = new MarketPanelDescriptor("你纵深一跃，在空中一个翻身，稳稳落入亭中，亭子正中的石桌上，有一本《XXX》。");

                    A._receiveSignal = (signal) =>
                    {
                        if (signal is SelectedOptionSignal selectedOptionSignal)
                        {
                            if (selectedOptionSignal.Selected == 0)
                            {
                                runNode.ChangePanel(B);
                            }
                            else if (selectedOptionSignal.Selected == 1)
                            {
                                runNode.ChangePanel(C);
                            }
                            else
                            {
                            }
                        }
                    };

                    B._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    C._receiveSignal = (signal) =>
                    {
                        RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
                        RunManager.Instance.Map.TryFinishNode();
                    };

                    runNode.ChangePanel(A);
                }),
        };
    }
}
