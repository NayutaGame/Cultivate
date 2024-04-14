
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class NodeCategory : Category<NodeEntry>
{
    public NodeCategory()
    {
        AddRange(new List<NodeEntry>()
        {
            #region Special
            
            new("战斗", "战斗", withInPool: false,
                create: map =>
                {
                    BattleRunNode battleRunNode = map.CurrNode as BattleRunNode;
                    BattleStepDescriptor stepDescriptor = map.CurrStepDescriptor as BattleStepDescriptor;

                    int baseGoldReward = stepDescriptor._baseGoldReward;
                    int xiuWeiValue = Mathf.RoundToInt(baseGoldReward * RandomManager.Range(0.9f, 1.1f));

                    BattlePanelDescriptor A = new(battleRunNode.Entity);
                    battleRunNode.AddReward(new ResourceReward(gold: xiuWeiValue));

                    DiscoverSkillPanelDescriptor B = new("胜利");
                    DiscoverSkillPanelDescriptor C = new("惜败");
                    DialogPanelDescriptor D = new($"按Esc退出游戏，游戏结束，感谢游玩");

                    bool shouldUpdateSlotCount = stepDescriptor.ShouldUpdateSlotCount;

                    if (!stepDescriptor._isBoss)
                    {
                        A.SetWin(() =>
                        {
                            B.SetDetailedText($"胜利！\n获得了{xiuWeiValue}的修为\n请选择一张卡作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(stepDescriptor._slotCountAfter);
                            return B;
                        });

                        A.SetLose(() =>
                        {
                            RunManager.Instance.Environment.SetDMingYuanProcedure(-2);
                            C.SetDetailedText($"你没能击败对手，损失了2命元。\n获得了{xiuWeiValue}修为\n请选择一张卡作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(stepDescriptor._slotCountAfter);
                            return C;
                        });
                    }
                    else if (map.JingJie != JingJie.HuaShen)
                    {
                        A.SetWin(() =>
                        {
                            RunManager.Instance.Environment.SetDMingYuanProcedure(3);
                            B.SetDetailedText($"胜利！\n跨越境界使得你的命元恢复了3\n获得了{xiuWeiValue}的修为\n请选择一张卡作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(stepDescriptor._slotCountAfter);
                            return B;
                        });

                        A.SetLose(() =>
                        {
                            C.SetDetailedText($"你没能击败对手，幸好跨越境界抵消了你的命元伤害。\n获得了{xiuWeiValue}修为\n请选择一张卡作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(stepDescriptor._slotCountAfter);
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

                    map.CurrNode.ChangePanel(A);
                }),

            new("休息", "休息", withInPool: false,
                create: map =>
                {
                    DialogPanelDescriptor A = new("前方有人参果树和菩提树，选择一条路", "人参果树", "菩提树");
                    DialogPanelDescriptor B = new DialogPanelDescriptor("吃了一个人参果，回复了2点命元")
                        .SetReward(Reward.FromMingYuan(2));
                    
                    CardPickerPanelDescriptor C = new("在菩提树下做了一段时间，对境界有了新的见解，请选择一张卡牌提升", new Range(0, 2));
                    C.SetSelect(iRunSkillList =>
                    {
                        foreach (var iRunSkill in iRunSkillList)
                        {
                            if (iRunSkill is RunSkill skill)
                            {
                                if (skill.JingJie <= map.JingJie && skill.JingJie != JingJie.HuaShen)
                                // if (skill.JingJie != JingJie.HuaShen)
                                {
                                    skill.TryIncreaseJingJie();
                                    CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
                                }
                            }
                            else if (iRunSkill is SkillSlot slot)
                            {
                                if (slot.Skill.GetJingJie() <= map.JingJie && slot.Skill.GetJingJie() != JingJie.HuaShen)
                                // if (slot.Skill.GetJingJie() != JingJie.HuaShen)
                                {
                                    slot.TryIncreaseJingJie();
                                    CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
                                }
                            }
                        }

                        return null;
                    });
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    
                    map.CurrNode.ChangePanel(A);
                }),

            new("商店", "商店", withInPool: false,
                create: map =>
                {
                    DialogPanelDescriptor A = new("前方有商店和温泉，选择一条路", "商店", "温泉");
                    
                    ShopPanelDescriptor B = new(map.JingJie);
                    int health = (map.JingJie + 1) * 3;
                    DialogPanelDescriptor C = new DialogPanelDescriptor($"泡了温泉之后感到了心情畅快，获得了{health}点生命上限")
                        .SetReward(Reward.FromHealth(health));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    
                    map.CurrNode.ChangePanel(A);
                }),

            new("悟道", "悟道", withInPool: false,
                create: map =>
                {
                    Pool<WuXing> pool = new Pool<WuXing>();
                    pool.Populate(WuXing.Traversal);
                    pool.Shuffle();

                    WuXing[] options = new WuXing[3];
                    for (int i = 0; i < options.Length; i++)
                    {
                        pool.TryPopItem(out options[i]);
                    }

                    DialogPanelDescriptor A = new("选择一种五行，获得一张随机牌",
                        options[0]._name,
                        options[1]._name,
                        options[2]._name);

                    A._receiveSignal = signal =>
                    {
                        if (signal is SelectedOptionSignal selectedOptionSignal)
                        {
                            int index = selectedOptionSignal.Selected;
                            new DrawSkillReward("获得一张随机牌", wuXing: options[index], jingJie: RunManager.Instance.Environment.Map.JingJie).Claim();
                        }
                        return null;
                    };

                    map.CurrNode.ChangePanel(A);
                }),

            new("获得金钱", "获得金钱", withInPool: false,
                create: map =>
                {
                    int gold = Mathf.RoundToInt((map.JingJie + 1) * 21 * RandomManager.Range(0.8f, 1.2f));
                    DialogPanelDescriptor A = new DialogPanelDescriptor($"获得了{gold}金钱")
                        .SetReward(Reward.FromGold(gold));
                    map.CurrNode.ChangePanel(A);
                }),

            new("以物易物", "以物易物", withInPool: false,
                create: map =>
                {
                    BarterPanelDescriptor A = new();
                    map.CurrNode.ChangePanel(A);
                }),

            new("初入蓬莱", "初入蓬莱", withInPool: false,
                create: map =>
                {
                    RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0200", JingJie.LianQi));
                    RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0501", JingJie.LianQi));
                    RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0500", JingJie.LianQi));

                    ImagePanelDescriptor ManHua1 = new("漫画1");
                    ImagePanelDescriptor ManHua2 = new("漫画2");
                    ImagePanelDescriptor ManHua3 = new("漫画3");
                    ImagePanelDescriptor ManHua4 = new("漫画4");

                    ImagePanelDescriptor ZhiRuJiaoXue1 = new("置入教学1");
                    ImagePanelDescriptor ZhiRuJiaoXue2 = new("置入教学2");
                    ImagePanelDescriptor ZhiRuJiaoXue3 = new("置入教学3");
                    ImagePanelDescriptor ZhiRuJiaoXue4 = new("置入教学4");
                    ImagePanelDescriptor ZhiRuJiaoXue5 = new("置入教学5");

                    BattlePanelDescriptor ZhiRuBattle = new(RunEntity.FromTemplate(EditorManager.FindEntity("置入教学怪物")));

                    ImagePanelDescriptor ZhanDouJiaoXue1 = new("战斗教学1");
                    ImagePanelDescriptor ZhanDouJiaoXue2 = new("战斗教学2");
                    ImagePanelDescriptor ZhanDouJiaoXue3 = new("战斗教学3");

                    ImagePanelDescriptor LingQiJiaoXue1 = new("灵气教学1");
                    ImagePanelDescriptor LingQiJiaoXue2 = new("灵气教学2");
                    ImagePanelDescriptor LingQiJiaoXue3 = new("灵气教学3");
                    ImagePanelDescriptor LingQiJiaoXue4 = new("灵气教学4");

                    BattlePanelDescriptor LingQiBattle = new(RunEntity.FromTemplate(EditorManager.FindEntity("灵气教学怪物")));

                    ImagePanelDescriptor HeChengJiaoXue1 = new("合成教学1");
                    ImagePanelDescriptor HeChengJiaoXue2 = new("合成教学2");
                    ImagePanelDescriptor HeChengJiaoXue3 = new("合成教学3");
                    ImagePanelDescriptor HeChengJiaoXue4 = new("合成教学4");
                    ImagePanelDescriptor HeChengJiaoXue5 = new("合成教学5");

                    BattlePanelDescriptor HeChengBattle = new(RunEntity.FromTemplate(EditorManager.FindEntity("合成教学怪物")));

                    ImagePanelDescriptor ZhanBaiJiaoXue1 = new("战败教学1");
                    ImagePanelDescriptor ZhanBaiJiaoXue2 = new("战败教学2");
                    ImagePanelDescriptor ZhanBaiJiaoXue3 = new("战败教学3");

                    BattlePanelDescriptor LoseBattle = new(RunEntity.FromTemplate(EditorManager.FindEntity("战败教学怪物")));

                    ImagePanelDescriptor MingYuanJiaoXue1 = new("命元教学1");
                    ImagePanelDescriptor MingYuanJiaoXue2 = new("命元教学2");
                    ImagePanelDescriptor MingYuanJiaoXue3 = new("命元教学3");

                    ImagePanelDescriptor ManHua5 = new("漫画5");

                    ManHua1.Next = ManHua2;
                    ManHua2.Next = ManHua3;
                    ManHua3.Next = ManHua4;
                    ManHua4.Next = ZhiRuJiaoXue1;
                    ZhiRuJiaoXue1.Next = ZhiRuJiaoXue2;
                    ZhiRuJiaoXue2.Next = ZhiRuJiaoXue3;
                    ZhiRuJiaoXue3.Next = ZhiRuJiaoXue4;
                    ZhiRuJiaoXue4.Next = ZhiRuJiaoXue5;
                    ZhiRuJiaoXue5.Next = ZhiRuBattle;

                    ZhiRuBattle.SetWin(() =>
                    {
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0203", JingJie.LianQi));
                        return ZhanDouJiaoXue1;
                    });
                    ZhiRuBattle.SetLose(() =>
                    {
                        RunManager.Instance.Environment.Hand.Clear();
                        RunManager.Instance.Environment.Home.TraversalCurrentSlots().Do(s => s.Skill = null);

                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0200", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0501", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0500", JingJie.LianQi));
                        return ZhiRuJiaoXue1;
                    });

                    ZhanDouJiaoXue1.Next = ZhanDouJiaoXue2;
                    ZhanDouJiaoXue2.Next = ZhanDouJiaoXue3;
                    ZhanDouJiaoXue3.Next = LingQiJiaoXue1;
                    LingQiJiaoXue1.Next = LingQiJiaoXue2;
                    LingQiJiaoXue2.Next = LingQiJiaoXue3;
                    LingQiJiaoXue3.Next = LingQiJiaoXue4;
                    LingQiJiaoXue4.Next = LingQiBattle;

                    LingQiBattle.SetWin(() =>
                    {
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0200", JingJie.LianQi));
                        return HeChengJiaoXue1;
                    });
                    LingQiBattle.SetLose(() =>
                    {
                        RunManager.Instance.Environment.Hand.Clear();
                        RunManager.Instance.Environment.Home.TraversalCurrentSlots().Do(s => s.Skill = null);

                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0200", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0501", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0500", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0203", JingJie.LianQi));
                        return LingQiJiaoXue1;
                    });

                    HeChengJiaoXue1.Next = HeChengJiaoXue2;
                    HeChengJiaoXue2.Next = HeChengJiaoXue3;
                    HeChengJiaoXue3.Next = HeChengJiaoXue4;
                    HeChengJiaoXue4.Next = HeChengJiaoXue5;
                    HeChengJiaoXue5.Next = HeChengBattle;
                    HeChengBattle.SetWin(() =>
                    {
                        RunManager.Instance.Environment.ForceDrawSkill(jingJie: JingJie.LianQi);
                        return ZhanBaiJiaoXue1;
                    });
                    HeChengBattle.SetLose(() =>
                    {
                        RunManager.Instance.Environment.Hand.Clear();
                        RunManager.Instance.Environment.Home.TraversalCurrentSlots().Do(s => s.Skill = null);

                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0200", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0200", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0501", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0500", JingJie.LianQi));
                        RunManager.Instance.Environment.ForceAddSkill(new AddSkillDetails("0203", JingJie.LianQi));
                        return HeChengJiaoXue1;
                    });

                    ZhanBaiJiaoXue1.Next = ZhanBaiJiaoXue2;
                    ZhanBaiJiaoXue2.Next = ZhanBaiJiaoXue3;
                    ZhanBaiJiaoXue3.Next = LoseBattle;
                    LoseBattle.SetWin(() =>
                    {
                        RunManager.Instance.Environment.SetDMingYuanProcedure(-2);
                        RunManager.Instance.Environment.ForceDrawSkill(jingJie: JingJie.LianQi);
                        return MingYuanJiaoXue1;
                    });
                    LoseBattle.SetLose(() =>
                    {
                        RunManager.Instance.Environment.SetDMingYuanProcedure(-2);
                        RunManager.Instance.Environment.ForceDrawSkill(jingJie: JingJie.LianQi);
                        return MingYuanJiaoXue1;
                    });

                    MingYuanJiaoXue1.Next = MingYuanJiaoXue2;
                    MingYuanJiaoXue2.Next = MingYuanJiaoXue3;
                    MingYuanJiaoXue3._receiveSignal = signal =>
                    {
                        if (signal is ClickedSignal clickedSignal)
                        {
                            RunManager.Instance.Environment.SetDMingYuanProcedure(2);
                            return ManHua5;
                        }

                        return null;
                    };

                    map.CurrNode.ChangePanel(ManHua1);
                }),

            new("同境界合成教学", "同境界合成教学", withInPool: false,
                create: map =>
                {
                    ImagePanelDescriptor A = new("同境界合成教学1");
                    ImagePanelDescriptor B = new("同境界合成教学2");
                    ImagePanelDescriptor C = new("同境界合成教学3");
                    A.Next = B;
                    B.Next = C;
                    map.CurrNode.ChangePanel(A);
                }),

            new("愿望单", "愿望单", withInPool: false,
                create: map =>
                {
                    DialogPanelDescriptor A = new("游戏仍在制作中，请加入愿望单，以关注后续进展，感谢游玩！",
                        "Q群：216060477",
                        "游戏名：蓬莱之旅",
                        "返回标题");

                    A[0].SetSelect(option =>
                    {
                        return A;
                    });

                    A[1].SetSelect(option =>
                    {
                        return A;
                    });

                    A[2].SetSelect(option =>
                    {
                        RunManager.Instance.ReturnToTitle();
                        return null;
                    });
                    map.CurrNode.ChangePanel(A);
                }),

            new("快速结算", "快速结算", withInPool: false,
                create: map =>
                {
                    DialogPanelDescriptor A = new("用于测试Run结算",
                        "胜利结算",
                        "失败结算");

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.Result.State = RunResult.RunResultState.Victory;
                        return null;
                    });

                    A[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.Result.State = RunResult.RunResultState.Defeat;
                        return null;
                    });

                    map.CurrNode.ChangePanel(A);
                }),
            
            new("胜利", "胜利", withInPool: false,
                create: map =>
                {
                    DialogPanelDescriptor A = new("恭喜获得游戏胜利",
                    "前往结算");

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.Result.State = RunResult.RunResultState.Victory;
                        return null;
                    });

                    map.CurrNode.ChangePanel(A);
                }),
            
            new("突破境界", "突破境界", withInPool: false,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你感到很久以来的瓶颈将被突破",
                        "突破");

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.NextJingJieProcedure();
                        return null;
                    });

                    map.CurrNode.ChangePanel(A);
                }),

            #endregion

            #region Adventure

            new("山木", "山木", withInPool: true,
                create: map =>
                {
                    int trial = 0;
                    int rage = RandomManager.Range(0, 7);

                    DialogPanelDescriptor A = new("一位老者做在石头上向周围人传教，虚己以游世，其孰能害之。说的是，只要你不把别人当个人，别人就不会引起你生气。你突然想逗他一下。", "朝他作鬼脸", "狠狠戳他一下");

                    DialogPanelDescriptor B1 = new("他看起来有点生气了。", "朝他作鬼脸", "狠狠戳他一下");
                    DialogPanelDescriptor B2 = new("他看起来非常生气了。", "朝他作鬼脸", "狠狠戳他一下");

                    DialogPanelDescriptor D = new DialogPanelDescriptor("你上去为自已的恶作剧道歉，他说还好，不会放在心上，这位学子应该学到了什么。\n\n获得50金")
                        .SetReward(Reward.FromGold(50));
                    DialogPanelDescriptor E = new DialogPanelDescriptor("你上去为自已的恶作剧道歉，他喘了一口气，随即嘻笑开颜向大家解释道，这就是我刚才说的，不要随便生气。\n\n获得200金")
                        .SetReward(Reward.FromGold(200));
                    DialogPanelDescriptor F = new("你刚想上去为自己的恶作剧道歉。只见他不掩饰自己的怒火：“岂有此理啊，你有完没完啊！”你只能赶紧跑了。");

                    PanelDescriptor SelectA(DialogOption option)
                    {
                        trial += 1;
                        rage += RandomManager.Range(0, 4);

                        if (trial < 2) {
                            return rage <= 5 ? B1 : B2;
                        } else if (rage <= 7) {
                            return D;
                        } else if (rage <= 10) {
                            return E;
                        } else {
                            return F;
                        }
                    }
                    PanelDescriptor SelectB(DialogOption option)
                    {
                        trial += 1;
                        rage += RandomManager.Range(4, 7);

                        if (trial < 2) {
                            return rage <= 5 ? B1 : B2;
                        } else if (rage <= 7) {
                            return D;
                        } else if (rage <= 10) {
                            return E;
                        } else {
                            return F;
                        }
                    }

                    A[0].SetSelect(SelectA);
                    A[1].SetSelect(SelectB);
                    B1[0].SetSelect(SelectA);
                    B1[1].SetSelect(SelectB);
                    B2[0].SetSelect(SelectA);
                    B2[1].SetSelect(SelectB);

                    map.CurrNode.ChangePanel(A);
                }),

            new("赤壁赋", "赤壁赋", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你见到两个人在辩论。\n一人说，月亮是变化的，今天还是满月，明天就不是了。\n另一人说，月亮是不变的，上个月看是满月，今天看也还是满月。",
                        "赞同月亮是变化的",
                        "赞同月亮是不变的",
                        "变得不是月亮，而是人");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你说到：“盖将自其变者而观之，则天地曾不能以一瞬，月亮是变化的。”\n只见第一个人非常赞同你的观点，给了你一些东西。" +
                                                  "\n\n得到《一念》")
                        .SetReward(new AddSkillReward("0600", jingJie: map.JingJie));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你说到：“自其不变者而观之，则物与我皆无尽也，月亮是不变的。”\n只见第二个人非常赞同你的观点，给了你一些东西。" +
                                                  "\n\n得到《无量劫》")
                        .SetReward(new AddSkillReward("0601", jingJie: map.JingJie));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你话还没说完，那两人说你是个杠精，马上留下钱买了单，换了一家茶馆去聊天。\n你发现他们还剩下了一些额外的东西。" +
                                                  "\n\n得到随机零件")
                        .SetReward(new AddMechReward());

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    map.CurrNode.ChangePanel(A);
                }),

            new("论无穷", "论无穷", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你听说有奖励，于是来参加了一场考试，内容是写一篇文章，题目是“论无穷”，要如何开题呢？" +
                                                  "\n\n我跑步很快，如果有人跑步比我还快的话，只要他推着载我的车，我在车里跑，速度就比他还快，只要一直有人速度比我快，我的速度就是无穷的。" +
                                                  "\n\n我有一个木桩，我每天砍一半，过一万年也砍不完，这个叫做无穷。" +
                                                  "\n\n我养了一条蛇，他每天吃自己的尾巴，然后又能长出来新的蛇身，永远吃不完，这个叫做无穷。",
                        "用第一个想法",
                        "用第二个想法",
                        "用第三个想法");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你痛快写了800字，时间没过5分钟，已经写完了。" +
                                                  "\n\n交卷之后，一名蓝色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张二动牌", pred: e => e.SkillTypeComposite.Contains(SkillType.ErDong), jingJie: map.JingJie));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("考试过了一半，你只写下了一句话。又过了一半的一半，你又写下了一句话。又过了一半的一半的一半，你再写下了一句话。。。" +
                                                                        "\n\n考试结束时，你已经把所有能写字的地方都写满了。" +
                                                  "\n\n交卷之后，一名红色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张消耗牌", pred: e => e.SkillTypeComposite.Contains(SkillType.XiaoHao), jingJie: map.JingJie));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你提笔写起来。\n\n从前有座山，山里有座庙，庙里有考试，考试来考生，考生做文章，文章道从前，" +
                                                                        "从前有座山，山里有座庙，庙里有考试，考试来考生，考生做文章，文章道从前，" +
                                                                        "从前有座山，山里有座庙。。。\n\n你的文章还没写完，考试已经结束了。" +
                                                  "\n\n交卷之后，一名绿色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张自指牌", pred: e => e.SkillTypeComposite.Contains(SkillType.ZiZhi), jingJie: map.JingJie));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    map.CurrNode.ChangePanel(A);
                }),

            new("人间世", "人间世", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你追上了他，看出他事业心很重，于是对他说：\n\n成名要趁早，我看你将来肯定是做宰相的料。\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。即使如此，你还是要追求功名么？",
                        "用第一个想法",
                        "用第二个想法");

                    DialogPanelDescriptor B1 = new("感谢你这么夸我，但是现在我也没有钱给你。", "时间一下过了60年");
                    DialogPanelDescriptor C1 = new("先生谬论不可再提，你看那胡人会因为我们不锻造兵器，充实军备而不来侵略我们么？", "时间一下过了60年");

                    DialogPanelDescriptor B2 = new DialogPanelDescriptor("当年的少年已经成为了宰相。见到了你，发现你的容貌60年没有发生变化，察觉你是仙人，于是说道，感谢仙人提拔。叫人给了你收藏的宝物。\n\n得到1机关牌")
                        .SetReward(new AddMechReward());
                    DialogPanelDescriptor C2 = new DialogPanelDescriptor("当年的少年已经成为了宰相。见到了你，完全没有印象，只道是某个江湖中人来攀亲道故，于是叫下人给了你点盘缠打发了。\n\n得到50金")
                        .SetReward(Reward.FromGold(50));

                    DialogPanelDescriptor D = new("你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你正向追上他说点什么，却被一颗小石子绊倒，起身已经不见那人踪影。于是道：“罢了罢了，缘分未到。”", "时间一下过了60年");
                    DialogPanelDescriptor D2 = new("你又见到了当年的少年。现在他已经成为了宰相。你想着对他说些什么：\n\n成名要趁早，宰相一生过得荣华富贵。。。\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。哪怕功名已成恐怕也是路途险阻。",
                        "用第一个想法",
                        "用第二个想法");

                    DialogPanelDescriptor E = new DialogPanelDescriptor("只见你话还没说完，宰相就摆手示意你离开。叫下人给了你点盘缠将你打发了。\n\n得到50金")
                        .SetReward(Reward.FromGold(50));
                    DialogPanelDescriptor F = new DialogPanelDescriptor("宰相回复到，先生说的属实，若是我早点知道了这些道理，也不至于一生过的如此跌宕起伏。叫人给了你收藏的宝物。\n\n得到1机关牌")
                        .SetReward(new AddMechReward());

                    A[0].SetSelect(option => B1);
                    A[1].SetSelect(option => C1);
                    B1[0].SetSelect(option => B2);
                    C1[0].SetSelect(option => C2);
                    D[0].SetSelect(option => D2);
                    D2[0].SetSelect(option => E);
                    D2[1].SetSelect(option => F);

                    bool isCatch = RandomManager.value < 0.5;
                    map.CurrNode.ChangePanel(isCatch ? A : D);
                }),

            new("神灯精灵", "神灯精灵", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你捡到了一盏神灯里面跳出来了一个精灵，说可以实现你一个愿望",
                        "健康的体魄",
                        "钱币的富裕",
                        "这个愿望不被实现");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("实现了，精灵留下了这句话带着神灯飞走了。你感觉身强体壮\n\n生命上限+10")
                        .SetReward(Reward.FromHealth(10));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("实现了，精灵留下了这句话带着神灯飞走了。你包里突然出来了很多金币\n\n金+100")
                        .SetReward(Reward.FromGold(100));
                    DialogPanelDescriptor D = new("实现了。。额，实现不了。。哦，实现了。。。啊，实现不了。精灵说你比许愿再来十个愿望的人还会捣乱，召唤出来一个怪物，要来和你打一架。");

                    map.EntityPool.TryDrawEntity(out RunEntity template, new DrawEntityDetails(map.Ladder));
                    BattlePanelDescriptor E = new(template);
                    DialogPanelDescriptor EWin = new DialogPanelDescriptor("哎，不就是都想要么？拿去拿去，好好说话我也不会不给的啊。\n\n生命上限+10，金+100")
                        .SetReward(new ResourceReward(gold: 100, health: 10));
                    DialogPanelDescriptor ELose = new("哼，现在神灯精灵不好做了，就是因为经常碰见你这种人。下次别再让我遇见了。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);
                    D[0].SetSelect(option => E);
                    E.SetWin(() => EWin);
                    E.SetLose(() => ELose);

                    map.CurrNode.ChangePanel(A);
                }),

            new("分子打印机", "分子打印机", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你发现了一个机器，有两个插槽。中间写着一行说明，一边放原料，一边放卡牌。",
                        "试试这个机器可以做什么",
                        "离开");

                    CardPickerPanelDescriptor B = new("请选择2张牌", range: new Range(0, 3));
                    DialogPanelDescriptor C = new("来路不明的机器还是不要乱碰了，这个机器还是留给有缘人吧。");
                    DialogPanelDescriptor D = new("劈里啪啦一阵响声过后，正在你担心自己的卡牌会受到什么非人的折磨的时候。机器的运转声停止了。打开后，你发现两个插槽里面的卡变成同一张了。\n\n得到两张牌");

                    B.SetSelect(iRunSkillList =>
                    {
                        int count = iRunSkillList.Count;
                        if (count == 0 || count == 1)
                            return C;

                        RunSkill copyingSkill = null;
                        object copying = iRunSkillList[RandomManager.Range(0, count)];
                        if (copying is RunSkill runSkill)
                        {
                            copyingSkill = runSkill;
                        }
                        else if (copying is SkillSlot slot)
                        {
                            RunSkill rSkill = slot.Skill as RunSkill;
                            Assert.IsTrue(rSkill != null);
                            copyingSkill = rSkill;
                        }

                        foreach (object iSkill in iRunSkillList)
                        {
                            if (iSkill is RunSkill skill)
                            {
                                RunManager.Instance.Environment.Hand.Remove(skill);
                            }
                            else if (iSkill is SkillSlot slot)
                            {
                                slot.Skill = null;
                            }
                        }

                        count.Do(i => RunManager.Instance.Environment.Hand.Add(copyingSkill));
                        return D;
                    });

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    map.CurrNode.ChangePanel(A);
                }),

            new("神农氏", "神农氏", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你看见一个人向你走来，一手拿着一个神采奕奕的仙草，另一手拿着一个可疑的蘑菇，向你说道，挑一个吃了吧。",
                        "选择仙草",
                        "选择可疑的蘑菇");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你吃了仙草感觉身上的伤势轻了一些。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));
                    DialogPanelDescriptor C = new("你吃了可疑的蘑菇，感觉头痛欲裂\n\n命元-1", "过了三十年");

                    DialogPanelDescriptor D = new("你又见到了那个少年，他又笑嘻嘻的向你走来，又是一手拿着一个福光满面的仙草，另一手拿着一个可疑的蘑菇，向你说道，这次你想吃哪个？",
                        "这次我就选择仙草吧",
                        "你个外行，学别人采什么药，离这个蘑菇远一点");

                    DialogPanelDescriptor E = new DialogPanelDescriptor("你吃了仙草感觉治愈了你多年的旧伤，继续上路了。\n\n命元+2")
                        .SetReward(Reward.FromMingYuan(2));
                    DialogPanelDescriptor F = new("你又一次吃下了可疑的蘑菇，感觉五脏俱焚\n\n命元-2", "又过了三十年");

                    DialogPanelDescriptor G = new DialogPanelDescriptor("你又故地重游，故人已经不在，你来到了他的墓前面，上面写着：神农氏之墓，他的后人说他给你留下来了一些东西。\n\n得到《百草集》。")
                        .SetReward(new AddSkillReward("0602", JingJie.YuanYing));

                    A[1].SetCost(new CostDetails(mingYuan: 1));
                    D[1].SetCost(new CostDetails(mingYuan: 2));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    C[0].SetSelect(option => D);
                    D[0].SetSelect(option => E);
                    D[1].SetSelect(option => F);
                    F[0].SetSelect(option => G);

                    map.CurrNode.ChangePanel(A);
                }),

            new("天津四", "天津四", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你看见一个书生，悄悄看着一个织布的少女，应该是对她有意思。他看你道士打扮，于是问道：“先生可否帮我算一卦，算姻缘。”",
                        "祝福他的缘分",
                        "和他说不是每一段相思都能够有结果的");

                    DialogPanelDescriptor B = new("书生表情平静，实际上满心欢喜，说：“我去尝试追求她看看。”", "过了三十年");
                    DialogPanelDescriptor B1 = new DialogPanelDescriptor("你又见到了当初的书生，他说没有在当年找到合适的姻缘。他给你留下了一些东西。\n\n得到《遗憾》天津四 著")
                        .SetReward(new AddSkillReward("0603", JingJie.JinDan));

                    DialogPanelDescriptor C = new("书生表情平静，实际上内心忧愁，然后默默离开了", "过了三十年");
                    DialogPanelDescriptor C1 = new DialogPanelDescriptor("你又见到了当初的书生，他虽然当时放弃了，但是后来和其他人结成了姻缘。他给你留下了一些东西。\n\n得到《爱恋》天津四 著")
                        .SetReward(new AddSkillReward("0604", JingJie.JinDan));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    B[0].SetSelect(option => B1);
                    C[0].SetSelect(option => C1);

                    map.CurrNode.ChangePanel(A);
                }),

            new("后羿", "后羿", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你看到了一个少年在幸苦的练习射箭，但是进度缓慢，你决定",
                        "我来教你一招",
                        "你应该换一身更好的装备",
                        "不打扰少年练习了");

                    DialogPanelDescriptor B = new("你比划了一个招式，对少年说，就这样练。少年谢过你了。", "过了三十年");
                    DialogPanelDescriptor B1 = new DialogPanelDescriptor("少年感激你当初的指导，说：“现在感觉自己已经非常厉害了，哪怕是太阳也能射中”。于是把准备买装备的钱给你了。\n\n得到150金")
                        .SetReward(Reward.FromGold(150));

                    DialogPanelDescriptor C = new("你给了少年一些钱，对少年说，去置办一身新的行头对你更加有帮助。少年谢过你了", "过了三十年");
                    DialogPanelDescriptor C1 = new DialogPanelDescriptor("少年感激你当初的指导，说：“现在感觉自己已经非常厉害了，哪怕是太阳也能射中”。于是把不需要的秘笈给你了。\n\n获得《射金乌》")
                        .SetReward(new AddSkillReward("0605", JingJie.JinDan));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    B[0].SetSelect(option => B1);
                    C[0].SetSelect(option => C1);

                    map.CurrNode.ChangePanel(A);
                }),

            new("天界树", "天界树", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你知道自己在梦境里，天界树将你拉入了他的梦境，梦境中的东西都非常真实。",
                        "吃树上的果子",
                        "尝试感悟五行相生的规律");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("久闻天界树，3000年才能开花结果，醒来之后，身上所有伤都不见了。\n\n命元+2")
                        .SetReward(Reward.FromMingYuan(2));
                    DialogPanelDescriptor C = new("你看到了，冷凝成水，滴下来滋养了树苗，随即长成大树，燃烧起来，出现了火，最终归于尘土。感悟了五行相生，所有五行牌都被相生的元素替换了");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option =>
                    {
                        foreach (var slot in RunManager.Instance.Environment.Home.TraversalCurrentSlots())
                        {
                            if (slot.Skill == null)
                                continue;
                            WuXing? oldWuXing = slot.Skill.GetEntry().WuXing;
                            JingJie oldJingJie = slot.Skill.GetJingJie();

                            if (!oldWuXing.HasValue)
                                continue;

                            JingJie newJingJie = RandomManager.Range(oldJingJie, map.JingJie + 1);

                            RunManager.Instance.Environment.SkillPool.TryDrawSkill(out RunSkill newSkill, wuXing: oldWuXing.Value.Next, jingJie: newJingJie);
                            slot.Skill = newSkill;
                        }

                        for(int i = 0; i < RunManager.Instance.Environment.Hand.Count(); i++)
                        {
                            RunSkill oldSkill = RunManager.Instance.Environment.Hand[i];
                            WuXing? oldWuXing = oldSkill.GetEntry().WuXing;
                            JingJie oldJingJie = oldSkill.JingJie;

                            if (!oldWuXing.HasValue)
                                continue;

                            JingJie newJingJie = RandomManager.Range(oldJingJie, map.JingJie + 1);

                            RunManager.Instance.Environment.SkillPool.TryDrawSkill(out RunSkill newSkill, wuXing: oldWuXing.Value.Next, jingJie: newJingJie);
                            RunManager.Instance.Environment.Hand.Replace(oldSkill, newSkill);
                        }

                        return C;
                    });

                    map.CurrNode.ChangePanel(A);
                }),

            new("鬼兵", "鬼兵", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你看到鬼兵打算带走一个将死之人，但是那人请求鬼兵在给自己一点时间。鬼兵说那人的命元已尽，不该继续留在阳间",
                        "助他炼丹（需要一张牌）",
                        "给他传气（需要一命元）",
                        "帮他造业（需要100金）",
                        "装作看不见");

                    CardPickerPanelDescriptor BPick = new CardPickerPanelDescriptor("炼丹需要消耗一张牌", 1);
                    DialogPanelDescriptor B = new("你取出了一张卡牌作为原料，炼出了一枚丹药，给那人吃了。" +
                                                  "\n\n随后，那人打开了一个机关，一只乐曲从那机关中播出，然后说，这下就有仪式感了。然后心满意足的和鬼兵离开了");
                    DialogPanelDescriptor C = new("你使用了自已的命元，给他传了过去。" +
                                                  "\n\n随后，那人打开了一个机关，一只乐曲从那机关中播出，然后说，这下就有仪式感了。然后心满意足的和鬼兵离开了");
                    DialogPanelDescriptor D = new("你向鬼兵偷偷一笑，将一物塞到鬼兵怀中，鬼兵摸了一下，也向你一笑。" +
                                                  "\n\n随后，那人打开了一个机关，一只乐曲从那机关中播出，然后说，这下就有仪式感了。然后心满意足的和鬼兵离开了");

                    DialogPanelDescriptor E = new("那人已经离开。留下了这个机关在这里，你非常好奇，想必是哪位墨苑大家留下来的手笔。留在这里也是可惜，你取出了其中有用的机关带走了。\n\n得到两个机关");

                    A[0].SetSelect(option => BPick);
                    BPick.SetSelect(iRunSkillList =>
                    {
                        if (iRunSkillList.Count == 0)
                            return A;

                        foreach (object iSkill in iRunSkillList)
                        {
                            if (iSkill is RunSkill skill)
                            {
                                RunManager.Instance.Environment.Hand.Remove(skill);
                            }
                            else if (iSkill is SkillSlot slot)
                            {
                                slot.Skill = null;
                            }
                        }

                        return B;
                    });

                    A[1].SetCost(new CostDetails(mingYuan: 1))
                        .SetSelect(option => C);
                    A[2].SetCost(new CostDetails(gold: 100))
                        .SetSelect(option => D);
                    B[0].SetSelect(option => E);
                    C[0].SetSelect(option => E);
                    D[0].SetSelect(option => E);

                    E.SetReward(new AddMechReward(new AddMechDetails(count: 2)));

                    map.CurrNode.ChangePanel(A);
                }),

            new("琴仙", "琴仙", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你遇到了一个弹琴的人，他双目失明，衣衫褴褛，举手投足之间却让人感到大方得体，应该是一名隐士。正好前一首曲毕。向你的方向看了过来，好像知道你来了。",
                        "来一首欢快的曲子吧",
                        "来一首悲伤的曲子吧",
                        "赶路着急，没时间留下来听曲子了");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("那人哈哈大笑，然后弹了一首欢快的曲子。你回想起这一生，第一次这么有满足感，产生了一些思绪。回过神来，那人已经不见了。\n\n获得《春雨》")
                        .SetReward(new AddSkillReward("0606", map.JingJie));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("那人一声叹息，然后弹了一首悲伤的曲子。你怀疑起了修仙的意义，产生了一些思绪。回过神来，那人已经不见了。\n\n获得《枯木》")
                        .SetReward(new AddSkillReward("0607", map.JingJie));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("之前赶路省下的时间，正好可以用于修炼。\n\n获得一个技能")
                        .SetReward(new DrawSkillReward("获得一个技能", jingJie: map.JingJie));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    map.CurrNode.ChangePanel(A);
                }),

            new("连抽五张", "连抽五张", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你近日练功，隐约感到一个瓶颈，心里略有不快。想着，如果全力一博，说不定就多一分机会窥见大道的真貌。",
                        "欲速则不达",
                        "大力出奇迹（消耗30生命上限）");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("哪怕大道难行，进一寸有一寸的欢喜。虽然进度不是很快，也并非没有收获。\n\n得到一张牌")
                        .SetReward(new DrawSkillReward("获得一个技能", jingJie: map.JingJie));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("随着喷出一大口鲜血，你回过神来，原来自己还活着，感谢大道没把自己留在那边。\n\n得到五张牌")
                        .SetReward(new DrawSkillReward("获得一个技能", jingJie: map.JingJie, count: 5));

                    A[0].SetSelect(option => B);
                    A[1].SetCost(new CostDetails(health: 30))
                        .SetSelect(option => C);

                    map.CurrNode.ChangePanel(A);
                }),

            new("天机阁", "天机阁", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你在沙漠中行走，突然眼前出来了一栋华丽的建筑，上面写着天机阁。你走入其中，前面有个牌子，请选择一张。你正在想是选择什么时，发现有十张卡牌浮在空中。");
                    ArbitraryCardPickerPanelDescriptor B = new("请从10张牌中选1张获取");
                    DialogPanelDescriptor C = new("刚一碰到那张卡牌，整个楼阁就突然消失不见，彷佛从未出现过一样。正当你不确定自己是否经历了一场幻觉时，发现留在手中的卡牌是真实的。于是你将这张卡牌收起。\n\n获得一张卡牌");

                    RunManager.Instance.Environment.SkillPool.TryDrawSkills(out List<RunSkill> skills, jingJie: map.JingJie, count: 10, consume: false);
                    B.AddSkills(skills);

                    B.SetSelect(toAdd =>
                    {
                        RunManager.Instance.Environment.ForceAddSkills(toAdd);
                        return C;
                    });

                    A[0].SetSelect(option => B);

                    map.CurrNode.ChangePanel(A);
                }),

            new("酿造仙岛玉液酒", "酿造仙岛玉液酒", withInPool: true,
                create: map =>
                {
                    bool mixWater = false;
                    bool yellNoLie = false;
                    bool expert = RandomManager.value < 0.5f;

                    DialogPanelDescriptor A = new("你来到了一个市集，突发奇想想试试之前好不容易得到的酿酒秘方，说不定可以换些盘缠。到市集转了一圈，你发现需要的原材料价值不菲。" +
                                                  "\n这里人人都喜欢酒，其中有些人可以鉴赏出酒的品质也说不定。\n\n你决定：",
                        "购买足量的原材料",
                        "少买一些原材料，多加一些水");

                    DialogPanelDescriptor B = new("果然有一人对商品感到有兴趣。问你卖多少钱？",
                        "一百八一杯");

                    DialogPanelDescriptor BmixWater = new("果然有一人对商品感到有兴趣。问你卖多少钱？",
                        "一百八一杯",
                        "既然加了些水，卖的便宜一些也合适，就八十卖你吧");

                    DialogPanelDescriptor C = new("你敢不敢喊一声蓬莱人不骗蓬莱人",
                        "蓬莱人不骗蓬莱人",
                        "顾左右而言其他");

                    DialogPanelDescriptor D = new("那人将钱交予你，把酒拿走了。劳动真光荣。", "获得50金");

                    DialogPanelDescriptor[] EndingTable = new DialogPanelDescriptor[]
                    {
                        // mixWater, yellNoLie, expert
                        /* 0b000 */ new("算了，我还是不买了。\n\n眼看市集就快结束了，你只好平价将酒出手了。\n\n不赚不赔"),
                        /* 0b001 */ new DialogPanelDescriptor("你今天不卖给我，我就不走了。那人出高价来买你的酒，你含泪把钱收下了。\n\n获得150金").SetReward(Reward.FromGold(150)),
                        /* ob010 */ new DialogPanelDescriptor("那人将钱交予你，把酒拿走了。劳动真光荣。\n\n获得50金").SetReward(Reward.FromGold(50)),
                        /* ob011 */ new DialogPanelDescriptor("那人将钱交予你，把酒拿走了。劳动真光荣。\n\n获得50金").SetReward(Reward.FromGold(50)),
                        /* 0b100 */ new("算了，我还是不买了。\n\n眼看市集就快结束了，你只好平价将酒出手了。\n\n不赚不赔"),
                        /* 0b101 */ new DialogPanelDescriptor("哎，看你也不容易。那人虽然看出了你的酒兑了水，但还是有些良心，于是以正常价格买走了酒。\n\n获得50金").SetReward(Reward.FromGold(50)),
                        /* 0b110 */ new DialogPanelDescriptor("那人将钱交予你，把酒拿走了。哇，小赚了一笔。\n\n获得150金。").SetReward(Reward.FromGold(150)),
                        /* 0b111 */ new DialogPanelDescriptor("你个奸商。那人抓住你，说要去官府。你只好摊也不顾了，赶紧溜了。\n\n失去50金").SetReward(Reward.FromGold(-50)),
                    };

                    A[0].SetSelect(option =>
                    {
                        mixWater = false;
                        return B;
                    });

                    A[1].SetSelect(option =>
                    {
                        mixWater = true;
                        return BmixWater;
                    });

                    B[0].SetSelect(option => C);
                    BmixWater[0].SetSelect(option => C);
                    BmixWater[1].SetSelect(option => D);

                    C[0].SetSelect(option =>
                    {
                        yellNoLie = true;
                        int key = ((mixWater ? 1 : 0) << 2) +
                                  ((yellNoLie ? 1 : 0) << 1) +
                                  ((expert ? 1 : 0) << 0);
                        return EndingTable[key];
                    });

                    C[1].SetSelect(option =>
                    {
                        yellNoLie = false;
                        int key = ((mixWater ? 1 : 0) << 2) +
                                  ((yellNoLie ? 1 : 0) << 1) +
                                  ((expert ? 1 : 0) << 0);
                        return EndingTable[key];
                    });

                    map.CurrNode.ChangePanel(A);
                }),

            new("解梦师", "解梦师", withInPool: true,
                create: map =>
                {
                    WuXing? wuXing = null;

                    DialogPanelDescriptor A0 = new("你来到一个镇上，见到了当地有名的解梦师。你请他解梦，你梦中出现了",
                        "金",
                        "水",
                        "木",
                        "下一页");

                    DialogPanelDescriptor A1 = new("你来到一个镇上，见到了当地有名的解梦师。你请他解梦，你梦中出现了",
                        "火",
                        "土",
                        "上一页");

                    DialogPanelDescriptor B = new("解梦师又问你，在你梦中，你是谁？",
                        "没有灵气的贫民",
                        "一些灵气的贵族",
                        "拥有庞大灵气的方士");

                    ArbitraryCardPickerPanelDescriptor C = new("原来如此，你最近是否常常想着");
                    DialogPanelDescriptor D = new("这正是我现在需要的，先生真乃神医也。\n\n得到一张牌");

                    A0[3].SetSelect(option => A1);
                    A1[2].SetSelect(option => A0);

                    A0[0].SetSelect(option =>
                    {
                        wuXing = WuXing.Jin;
                        return B;
                    });

                    A0[1].SetSelect(option =>
                    {
                        wuXing = WuXing.Shui;
                        return B;
                    });

                    A0[2].SetSelect(option =>
                    {
                        wuXing = WuXing.Mu;
                        return B;
                    });

                    A1[0].SetSelect(option =>
                    {
                        wuXing = WuXing.Huo;
                        return B;
                    });

                    A1[1].SetSelect(option =>
                    {
                        wuXing = WuXing.Tu;
                        return B;
                    });

                    B[0].SetSelect(option =>
                    {
                        Range manaCost = 0;

                        RunManager.Instance.Environment.SkillPool.TryDrawSkills(out List<RunSkill> skills,
                            pred: e => manaCost.Contains(e.GetCostDescription(map.JingJie).ByType(CostDescription.CostType.Mana)), wuXing: wuXing, jingJie: map.JingJie, count: 3, distinct: true, consume: false);
                        C.AddSkills(skills);
                        return C;
                    });
                    B[1].SetSelect(option =>
                    {
                        Range manaCost = new Range(1, 10);

                        RunManager.Instance.Environment.SkillPool.TryDrawSkills(out List<RunSkill> skills,
                            pred: e => manaCost.Contains(e.GetCostDescription(map.JingJie).ByType(CostDescription.CostType.Mana)), wuXing: wuXing, jingJie: map.JingJie, count: 3, distinct: true, consume: false);
                        C.AddSkills(skills);
                        return C;
                    });
                    B[2].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.SkillPool.TryDrawSkills(out List<RunSkill> skills,
                            pred: e => e.SkillTypeComposite.Contains(SkillType.LingQi), wuXing: wuXing, jingJie: map.JingJie, count: 3, distinct: true, consume: false);
                        C.AddSkills(skills);
                        return C;
                    });

                    C.SetSelect(skills =>
                    {
                        RunManager.Instance.Environment.ForceAddSkills(skills);
                        return D;
                    });

                    map.CurrNode.ChangePanel(A0);
                }),

            new("夏虫语冰", "夏虫语冰", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你要过一个桥，桥上站了一人，问你，什么时候河会变得可以行走。你说在冬季的时候。他说你是胡说八道：“一年只有三个季节，春夏秋，哪里来的冬季？”",
                        "赞同他，说一年只有三个季节",
                        "向他解释，说一年有四个季节");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("那人让你过去了，你感觉自己避免了一件麻烦事，心情大为畅快。\n\n生命上限+10")
                        .SetReward(Reward.FromHealth(10));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("一个月过去了，想过桥的人看到你们俩堵在桥中间，劝也劝不动，都想其他法子过桥了。那人的面容有所变化，但是嘴还是硬的。" +
                                                  "\n两个月时间逐渐过去，周围的人已经不来这个桥了。那人竟然以肉眼可见的速度，每天变老，但是还是一口咬定冬季不存在。" +
                                                  "\n到了第三个月，你们旁边已经修好了一个新的桥，从新桥上过去的人都已异样的眼光看着你们。那人已经连站立都感到困难了。" +
                                                  "\n就快要到冬天了，到时候就能证明冬季了。你又一次像那人看去。那人已经老的站立都困难了。你终于发现那人是夏虫所化。一生始于春而终于秋。你刚想松口。那人先于你出口说，你赢了，你可以过桥了，然后坐在一颗树下，永远的合上了眼。" +
                                                  "\n你在此地过了三个月，虽然修为上没有太大的精进，但是休息了这么长时间，所有伤势都已消失不见。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    map.CurrNode.ChangePanel(A);
                }),

            new("照相机", "照相机", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("到了桃花盛开的季节，你也来欣赏桃花。见到一名机关师，向人们介绍自己最近的新发明。按一下按钮，这个机关就可以将眼前美景永远记录下来。" +
                                                  "\n周围人看了那个机关，觉得画过于真实，害怕这个机关能够摄人心魄。都纷纷不敢上前。" +
                                                  "\n那人邀请你实验一下他的新发明。你站好之后，他叫你喊，一，二，三，茄子。然后启动了两次机关。果然出现了两张相片。一张优雅俊美，另一张略有瑕疵，可能是机关启动的时机并不完美。" +
                                                  "\n那人向你说道：“先生不如选一张，然后将另一张放在我这里，这样我们看见相片就能会想起，今时今日，曾一起赏桃花。”",
                        "拿走优雅俊美的那一张",
                        "拿走略有瑕疵的那一张",
                        "先生曾听过，人生苦短，及时行乐");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你把这张相片放在了挂在了你的大堂里，寻求你帮助的人看到你俊美的相貌，愿意以更高价钱请你出力。金+100。")
                        .SetReward(Reward.FromGold(100));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你把好的相片留给了机关师，这样他日后看到相片的时候，就会感到这段回忆多一分美好。这样想到，你的心情变好了。生命上限+10")
                        .SetReward(Reward.FromHealth(10));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你对那人说：”相必先生也知道，美好的时光总是短暂的。这个机关，可以将美好的时光记录下来，之后就可以看着照片反复回忆。" +
                                                  "但真的如此做的话，处于桃林中的我们也会因为知道相片可以反复回忆，反倒不去珍惜此时此刻的美景了。这难道不是本末倒置了么？“" +
                                                  "\n\n那人稍微惊讶于你的说法。然后感叹道：“先生教训的在理。不应以人生长而感到美好，也不应以短而感到苦恼。这个机关已经于我无用，这便赠与先生吧。”" +
                                                  "你收下了这个机关，但是并不会维护。与其看着它坏掉，不如将其中有用的零件取出。\n\n得到1机关")
                        .SetReward(new AddMechReward());

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    map.CurrNode.ChangePanel(A);
                }),

            new("丢尺子", "丢尺子", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("嘀嘀嘀。灵信响了，你看了一下。是之前委托你布阵的人发的消息。" +
                                                  "\n管家：大人，之前你帮我家设置的阵法，有一笔的长度不对。会不会引起问题？" +
                                                  "\n我：哪一笔，长度怎么不对了？" +
                                                  "\n管家：坎位其中一划，我拿尺子量了，和其他的差了一分有余。",
                        "你多虑了，长度稍微差一点点没关系的。",
                        "可以把你的尺子丢了么？");

                    DialogPanelDescriptor B = new("嘀嘀嘀。灵信响了，又是两天前那个管家。" +
                                                  "\n管家：大人，我用尺子比了一下，有一笔稍微有些不直，会不会影响到阵法的效果？",
                        "你多虑了，笔划不需要完全直的。",
                        "可以把你的尺子丢了么？");

                    DialogPanelDescriptor C = new("嘀嘀嘀。灵信又响了。还是几天前那个管家。" +
                                                  "管家：大人，我又拿池子笔划了许久，发现这个阵法还是有些问题，能劳烦你来一趟么，越快越好。",
                        "没问题，我今天就赶过去。",
                        "可以把你的尺子丢了么？");

                    DialogPanelDescriptor D = new DialogPanelDescriptor("你问了管家情况，发现都是些鸡毛蒜皮的小事。并不需要什么维护，就收了车马费。\n\n金+50。")
                        .SetReward(Reward.FromGold(50));

                    DialogPanelDescriptor E = new DialogPanelDescriptor("这阵子，管家没有再来打扰你了，应该是去打扰别人了。心情变好了一点。\n\n生命上限+5")
                        .SetReward(Reward.FromHealth(5));

                    A[0].SetSelect(option => B);
                    B[0].SetSelect(option => C);
                    C[0].SetSelect(option => D);
                    A[1].SetSelect(option => E);
                    B[1].SetSelect(option => E);
                    C[1].SetSelect(option => E);

                    map.CurrNode.ChangePanel(A);
                }),

            new("曹操三笑", "曹操三笑", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("有个商人要去其他国家，听闻中间有一个险道，常常有山贼出没，托你保护他和一些货物的安全。一路上没有什么障碍，赶了几天的路之后，终于快要到目的地了。" +
                                                  "面前是一处山谷。这时候，他突然大笑起来：“哈哈哈哈哈哈哈哈。。。”",
                        "询问他何故突然大笑？",
                        "赶紧捂住他的嘴。");

                    DialogPanelDescriptor B = new("于是他解释道：”我看此等山贼都是少智无谋之辈，如果在此伏击我等，定然能让我们元气大伤。哈哈哈哈哈哈哈哈。。。“" +
                                                  "\n只见他正在笑着，然后一伙山贼就出现了。",
                        "和山贼战斗");
                    DialogPanelDescriptor C = new("他有些不悦，但也没说什么。你们平安的走完了剩下的路程。\n\n金+50");

                    map.EntityPool.TryDrawEntity(out RunEntity template, new DrawEntityDetails(map.Ladder));
                    BattlePanelDescriptor B1 = new(template);
                    DialogPanelDescriptor B1win = new DialogPanelDescriptor("你打过了山贼，商人对你十分感激。\n\n金+100")
                        .SetReward(Reward.FromGold(100));
                    DialogPanelDescriptor B1lose = new("你没打过山贼，货物被抢走了。索性没有人受伤。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    B[0].SetSelect(option => B1);
                    B1.SetWin(() => B1win);
                    B1.SetLose(() => B1lose);

                    map.CurrNode.ChangePanel(A);
                }),

            new("仙人下棋", "仙人下棋", withInPool: true,
                create: map =>
                {
                    DialogPanelDescriptor A = new("你在竹林里迷路了，走了一阵遇到两个人在下棋，其中一个人发现了你，然后继续看棋盘去了。",
                        "尝试观看两人对弈（需要一张二动牌）",
                        "请教两人路怎么走（需要一张治疗牌）");

                    CardPickerPanelDescriptor B = new CardPickerPanelDescriptor("请提交一张二动牌", new Range(0, 2), drawSkillDetails: new DrawSkillDetails(pred: s => s.SkillTypeComposite.Contains(SkillType.ErDong)));
                    CardPickerPanelDescriptor C = new CardPickerPanelDescriptor("请提交一张治疗牌", new Range(0, 2), drawSkillDetails: new DrawSkillDetails(pred: s => s.SkillTypeComposite.Contains(SkillType.ZhiLiao)));

                    DialogPanelDescriptor BWin = new("你沉下心来仔细看这盘棋，在神识飘到很远的地方之前，回想起了你曾经学过的心法，保持住了自己的神识。", "不知过了多久");
                    DialogPanelDescriptor BWin2 = new("你沉浸在自己的世界里面，两人对弈完了，你和他们互相道别。走出竹林时，你感到自己的心法又精进了一步。\n\n得到《观棋烂柯》。");
                    BWin2.SetReward(new AddSkillReward("0211", map.JingJie));

                    DialogPanelDescriptor BLose = new("虽然你沉下心来想要理解棋盘中发生了什么事，只见两人下棋越来越快，一息之间，那二人已下出千百步，你想说些什么，但是身体却来不及动。", "不知过了多久");
                    DialogPanelDescriptor BLose2 = new("你醒来时，那两人已经不在了。但是莫要紧，美美睡上一觉比什么都重要。命元+2。");
                    BLose2.SetReward(Reward.FromMingYuan(2));

                    DialogPanelDescriptor CWin = new("你正向前走去，余光看到其中一人正好在一步棋点在天元。一瞬间你仿佛来到了水中，无法呼吸，你回想起了一段关于呼吸的功法，开始强迫自己吐纳，努力在这种环境下获取一些空气。", "不知过了多久");
                    DialogPanelDescriptor CWin2 = new("即使空气非常粘稠，你也可以呼吸自如。慢慢回到了正常的感觉，你悟出了一个关于吐纳的功法。");
                    CWin2.SetReward(new AddSkillReward("0608", map.JingJie));

                    DialogPanelDescriptor CLose = new("你正向前走去，余光看到其中一人正好在一步棋点在天元。一瞬间你仿佛来到了水中，无法呼吸，肺部在不断哀嚎。", "不知过了多久");
                    DialogPanelDescriptor CLose2 = new("空气中的粘稠感终于消失。你赶紧大口吸气呼气，第一次感到空气是这么美好。生命值上限+10。");
                    CLose2.SetReward(Reward.FromHealth(10));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    B.SetSelect(iRunSkillList =>
                    {
                        if (iRunSkillList.Count == 0)
                            return BLose;

                        foreach (object iSkill in iRunSkillList)
                        {
                            if (iSkill is RunSkill skill)
                            {
                                RunManager.Instance.Environment.Hand.Remove(skill);
                            }
                            else if (iSkill is SkillSlot slot)
                            {
                                slot.Skill = null;
                            }
                        }

                        return BWin;
                    });

                    C.SetSelect(iRunSkillList =>
                    {
                        if (iRunSkillList.Count == 0)
                            return CLose;

                        foreach (object iSkill in iRunSkillList)
                        {
                            if (iSkill is RunSkill skill)
                            {
                                RunManager.Instance.Environment.Hand.Remove(skill);
                            }
                            else if (iSkill is SkillSlot slot)
                            {
                                slot.Skill = null;
                            }
                        }

                        return CWin;
                    });

                    BWin[0].SetSelect(option => BWin2);
                    BLose[0].SetSelect(option => BLose2);
                    CWin[0].SetSelect(option => CWin2);
                    CLose[0].SetSelect(option => CLose2);

                    map.CurrNode.ChangePanel(A);
                }),
            
            #endregion
        });
    }
}
