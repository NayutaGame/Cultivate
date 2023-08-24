using System.Collections;
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
            new BattleNodeEntry("敌人", "敌人"),

            new RewardNodeEntry("选择一种五行，获得一张随机牌", "选择一种五行，获得一张随机牌", "悟道",
                create: runNode =>
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
                            new DrawSkillRewardDescriptor("获得一张随机牌", wuXing: options[index], jingJie: RunManager.Instance.Map.JingJie).Claim();
                        }
                        RunManager.Instance.Map.TryFinishNode();
                        return null;
                    };

                    runNode.ChangePanel(A);
                }),

            new RewardNodeEntry("回复命元", "回复命元", "人参果",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new("回复了2点命元");
                    A._reward = new ResourceRewardDescriptor(mingYuan: 2);
                    runNode.ChangePanel(A);
                }),

            new RewardNodeEntry("获得金钱", "获得金钱", "金钱",
                create: runNode =>
                {
                    int xiuWeiValue = Mathf.RoundToInt((runNode.JingJie + 1) * 21 * RandomManager.Range(0.8f, 1.2f));
                    DialogPanelDescriptor A = new($"获得了{xiuWeiValue}金钱");
                    A._reward = new ResourceRewardDescriptor(xiuWei: xiuWeiValue);
                    runNode.ChangePanel(A);
                }),

            new RewardNodeEntry("提升境界", "提升境界", "修炼",
                create: runNode =>
                {
                    CardPickerPanelDescriptor A = new("可以将一张低于化神境界的牌提升到主角的下一境界，请选择想要提升的牌", new Range(0, 2),
                        action: iRunSkillList =>
                        {
                            foreach (var iRunSkill in iRunSkillList)
                            {
                                if (iRunSkill is RunSkill skill)
                                {
                                    if (skill.JingJie <= runNode.JingJie && skill.JingJie != JingJie.HuaShen)
                                        skill.TryIncreaseJingJie();
                                }
                                else if (iRunSkill is SkillSlot slot)
                                {
                                    if (slot.Skill.GetJingJie() <= runNode.JingJie && slot.Skill.GetJingJie() != JingJie.HuaShen)
                                        slot.TryIncreaseJingJie();
                                }
                            }
                        });
                    runNode.ChangePanel(A);
                }),

            new RewardNodeEntry("加生命上限", "加生命上限", "温泉",
                create: runNode =>
                {
                    int healthValue = (runNode.JingJie + 1) * 3;
                    DialogPanelDescriptor A = new($"找到了一个人参果，吃了之后获得了{healthValue}点生命上限");
                    A._reward = new ResourceRewardDescriptor(health: healthValue);
                    runNode.ChangePanel(A);
                }),

            new RewardNodeEntry("商店", "商店", "商店",
                create: runNode =>
                {
                    ShopPanelDescriptor A = new(runNode.JingJie);
                    runNode.ChangePanel(A);
                }),

            new RewardNodeEntry("以物易物", "以物易物", "以物易物",
                create: runNode =>
                {
                    BarterPanelDescriptor A = new();
                    runNode.ChangePanel(A);
                }),

            new RewardNodeEntry("算卦", "算卦", "算卦",
                canCreate: x => RunManager.Instance.Map.HasAdventrueAfterwards(x),
                create: runNode =>
                {
                    DialogPanelDescriptor A = new($"占卜到前方的冒险事件是\n{RunManager.Instance.Map.NextAdventure().Name}",
                        "换一个",
                        "保持现状");

                    A[0]._select = option =>
                    {
                        RunManager.Instance.Map.RerollNextAdventure();
                        RunManager.Instance.Map.TryFinishNode();
                        return null;
                    };

                    runNode.ChangePanel(A);
                }),









            // 复杂事件

            // new AdventureNodeEntry("山木", "山木",
            //     create: runNode =>
            //     {
            //         int trial = 0;
            //         int rage = RandomManager.Range(0, 7);
            //
            //         DialogPanelDescriptor A = new("一位老者做在石头上向周围人传教，虚己以游世，其孰能害之。说的是，只要你不把别人当个人，别人就不会引起你生气。你突然想逗他一下。", "朝他作鬼脸", "戳他一下");
            //
            //         DialogPanelDescriptor B1 = new("他看起来有点生气了。", "朝他作鬼脸", "戳他一下");
            //         DialogPanelDescriptor B2 = new("他看起来非常生气了。", "朝他作鬼脸", "戳他一下");
            //
            //         DialogPanelDescriptor D = new("你上去为自已的恶作剧道歉，他说还好，不会放在心上，这位学子应该学到了什么。\n获得50金");
            //         DialogPanelDescriptor E = new("你上去为自已的恶作剧道歉，他喘了一口气，随即嘻笑开颜向大家解释道，这就是我刚才说的，不要随便生气。\n获得200金");
            //         DialogPanelDescriptor F = new("你刚想上去为自己的恶作剧道歉。只见他不掩饰自己的怒火：“岂有此理啊，你有完没完啊！”你只能赶紧跑了。");
            //
            //         D._reward = new ResourceRewardDescriptor(xiuWei: 50);
            //         E._reward = new ResourceRewardDescriptor(xiuWei: 200);
            //
            //         PanelDescriptor SelectA(DialogOption option)
            //         {
            //             trial += 1;
            //             rage += RandomManager.Range(0, 4);
            //
            //             PanelDescriptor panelDescriptor;
            //             if (trial < 2) {
            //                 panelDescriptor = rage <= 5 ? B1 : B2;
            //             } else if (rage <= 7) {
            //                 panelDescriptor = D;
            //             } else if (rage <= 10) {
            //                 panelDescriptor = E;
            //             } else {
            //                 panelDescriptor = F;
            //             }
            //             runNode.ChangePanel(panelDescriptor);
            //             return panelDescriptor;
            //         }
            //         PanelDescriptor SelectB(DialogOption option)
            //         {
            //             trial += 1;
            //             rage += RandomManager.Range(4, 7);
            //
            //             PanelDescriptor panelDescriptor;
            //             if (trial < 2) {
            //                 panelDescriptor = rage <= 5 ? B1 : B2;
            //             } else if (rage <= 7) {
            //                 panelDescriptor = D;
            //             } else if (rage <= 10) {
            //                 panelDescriptor = E;
            //             } else {
            //                 panelDescriptor = F;
            //             }
            //             runNode.ChangePanel(panelDescriptor);
            //             return panelDescriptor;
            //         }
            //
            //         A[0]._select = SelectA;
            //         A[1]._select = SelectB;
            //         B1[0]._select = SelectA;
            //         B1[1]._select = SelectB;
            //         B2[0]._select = SelectA;
            //         B2[1]._select = SelectB;
            //
            //         runNode.ChangePanel(A);
            //     }),

            // new AdventureNodeEntry("赤壁赋", "赤壁赋",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你见到两个人在辩论。\n一人说，月亮是变化的，今天还是满月，明天就不是了。\n另一人说，月亮是不变的，上个月看是满月，今天看也还是满月。",
            //             "赞同月亮是变化的",
            //             "赞同月亮是不变的",
            //             "变得不是月亮，而是人");
            //
            //         DialogPanelDescriptor B = new("你说到：“盖将自其变者而观之，则天地曾不能以一瞬，月亮是变化的。”\n\n只见第一个人非常赞同你的观点，给了你一些东西。");
            //         DialogPanelDescriptor C = new("你说到：“自其不变者而观之，则物与我皆无尽也，月亮是不变的。”\n\n只见第二个人非常赞同你的观点，给了你一些东西。");
            //         DialogPanelDescriptor D = new("你话还没说完，那两人说你是个杠精，马上留下钱买了单，换了一家茶馆去聊天。\n\n你发现他们还剩下了一些额外的东西。");
            //
            //         A[0]._select = option =>
            //         {
            //             runNode.ChangePanel(B);
            //             return B;
            //         };
            //
            //         A[1]._select = option =>
            //         {
            //             runNode.ChangePanel(C);
            //             return C;
            //         };
            //
            //         A[2]._select = option =>
            //         {
            //             runNode.ChangePanel(D);
            //             return D;
            //         };
            //
            //         B[0]._select = option =>
            //         {
            //             // 加一张牌
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         C[0]._select = option =>
            //         {
            //             // 加一张牌
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         D[0]._select = option =>
            //         {
            //             // 加一张随机零件
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),

            // new AdventureNodeEntry("天下", "天下",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你听说有奖励，于是来参加了一场考试，内容是写一篇文章，题目是“论无穷”，要如何开题呢？\n\n我跑步很快，如果有人跑步比我还快的话，只要他推着载我的车，我在车里跑，速度就比他还快，只要一直有人速度比我快，我的速度就是无穷的。\n\n我有一个木桩，我每天砍一半，过一万年也砍不完，这个叫做无穷。\n\n我养了一条蛇，他每天吃自己的尾巴，然后又能长出来新的蛇身，永远吃不完，这个叫做无穷。",
            //             "用第一个想法",
            //             "用第二个想法",
            //             "用第三个想法");
            //
            //         DialogPanelDescriptor B = new("你痛快写了800字，时间没过5分钟，已经写完了。\n\n交卷之后，一名考官对你的文章很有兴趣，给你留下了一些东西。");
            //         DialogPanelDescriptor C = new("考试过了一半，你只写下了一句话。又过了一半的一半，你又写下了一句话。又过了一半的一半的一半，你再写下了一句话。。。考试结束时，你已经把所有能写字的地方都写满了话。\n\n交卷之后，一名考官对你的文章很有兴趣，给你留下了一些东西。");
            //         DialogPanelDescriptor D = new("你写了一段话。又换了个角度将你的观点重述了一遍，又再换了个角度将你的观点重述了一遍。。。你的角度还没用完，考试已经结束了。\n\n交卷之后，一名考官对你的文章很有兴趣，给你留下了一些东西。");
            //
            //         A[0]._select = option =>
            //         {
            //             runNode.ChangePanel(B);
            //             return B;
            //         };
            //
            //         A[1]._select = option =>
            //         {
            //             runNode.ChangePanel(C);
            //             return C;
            //         };
            //
            //         A[2]._select = option =>
            //         {
            //             runNode.ChangePanel(D);
            //             return D;
            //         };
            //
            //         B[0]._select = option =>
            //         {
            //             // 加一张牌
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         C[0]._select = option =>
            //         {
            //             // 加一张牌
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         D[0]._select = option =>
            //         {
            //             // 加一张牌
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),

            new AdventureNodeEntry("人间世", "人间世",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new("你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你追上了他，看出他事业心很重，于是对他说：\n\n成名要趁早，我看你将来肯定是做宰相的料。\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。即使如此，你还是要追求功名么？",
                        "用第一个想法",
                        "用第二个想法");

                    DialogPanelDescriptor B1 = new("感谢你这么夸我，但是现在我也没有钱给你。", "时间一下过了60年");
                    DialogPanelDescriptor C1 = new("先生谬论不可再提，你看那胡人会因为我们不锻造兵器，充实军备而不来侵略我们么？", "时间一下过了60年");

                    DialogPanelDescriptor B2 = new("当年的少年已经成为了宰相。见到了你，发现你的容貌60年没有发生变化，察觉你是仙人，于是说道，感谢仙人提拔。给了你一大笔钱。");
                    DialogPanelDescriptor C2 = new("当年的少年已经成为了宰相。见到了你，完全没有印象，只道是某个江湖中人来攀亲道故，于是叫下人给了你点盘缠打发了。");

                    DialogPanelDescriptor D = new("你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你正向追上他说点什么，却被一颗小石子绊倒，起身已经不见那人踪影。于是道：“罢了罢了，缘分未到。”", "时间一下过了60年");
                    DialogPanelDescriptor D2 = new("你又见到了当年的少年。现在他已经成为了宰相。你想着对他说些什么：\n\n成名要趁早，宰相一生过得荣华富贵。。。\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。哪怕功名已成恐怕也是路途险阻。",
                        "用第一个想法",
                        "用第二个想法");

                    DialogPanelDescriptor E = new("只见你话还没说完，宰相就摆手示意你离开。叫下人给了你点盘缠将你打发了。");
                    DialogPanelDescriptor F = new("宰相回复到，先生说的属实，若是我早点知道了这些道理，也不至于一生过的如此跌宕起伏。叫人给了你一大笔钱。");

                    A[0]._select = option =>
                    {
                        runNode.ChangePanel(B1);
                        return B1;
                    };

                    A[1]._select = option =>
                    {
                        runNode.ChangePanel(C1);
                        return C1;
                    };

                    B1[0]._select = option =>
                    {
                        runNode.ChangePanel(B2);
                        return B2;
                    };

                    C1[0]._select = option =>
                    {
                        runNode.ChangePanel(C2);
                        return C2;
                    };

                    D[0]._select = option =>
                    {
                        runNode.ChangePanel(D2);
                        return D2;
                    };

                    D2[0]._select = option =>
                    {
                        runNode.ChangePanel(E);
                        return E;
                    };

                    D2[1]._select = option =>
                    {
                        runNode.ChangePanel(F);
                        return F;
                    };

                    bool isCatch = RandomManager.value < 0.5;
                    runNode.ChangePanel(isCatch ? A : D);
                }),

            //
            // new AdventureNodeEntry("镜花水月", "镜花水月",
            //     create: runNode =>
            //     {
            //         CardPickerPanelDescriptor A = new("请选择至多4张牌移除，将会从中选择一张，每移除一张牌，返还一份", range: new Range(0, 5),
            //             action: iSkills =>
            //             {
            //                 int count = iSkills.Count;
            //                 if (count == 0)
            //                     return;
            //
            //                 RunSkill copyingSkill = null;
            //                 object copying = iSkills[RandomManager.Range(0, count)];
            //                 if (copying is RunSkill runSkill)
            //                 {
            //                     copyingSkill = runSkill;
            //                 }
            //                 else if (copying is SkillSlot slot)
            //                 {
            //                     RunSkill rSkill = slot.Skill as RunSkill;
            //                     Assert.IsTrue(rSkill != null);
            //                     copyingSkill = rSkill;
            //                 }
            //
            //                 foreach (object iSkill in iSkills)
            //                 {
            //                     if (iSkill is RunSkill skill)
            //                     {
            //                         RunManager.Instance.Battle.SkillInventory.RemoveSkill(skill);
            //                     }
            //                     else if (iSkill is SkillSlot slot)
            //                     {
            //                         slot.Skill = null;
            //                     }
            //                 }
            //
            //                 count.Do(i => RunManager.Instance.Battle.SkillInventory.AddSkill(copyingSkill));
            //             });
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("感悟五行相生", "感悟五行相生",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("是否尝试感悟五行相生的规律",
            //             "尝试感受（将会将你的所有牌替换成相生五行的牌）",
            //             "离开");
            //
            //         A[0]._select = option =>
            //         {
            //             foreach (var slot in RunManager.Instance.Battle.Hero.TraversalCurrentSlots())
            //             {
            //                 if (slot.Skill == null)
            //                     continue;
            //                 WuXing? oldWuXing = slot.Skill.GetEntry().WuXing;
            //                 JingJie oldJingJie = slot.Skill.GetJingJie();
            //
            //                 if (!oldWuXing.HasValue)
            //                     continue;
            //
            //                 JingJie newJingJie = RandomManager.Range(oldJingJie, runNode.JingJie + 1);
            //
            //                 RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill newSkill, wuXing: oldWuXing.Value.Next, jingJie: newJingJie);
            //                 slot.Skill = newSkill;
            //             }
            //
            //             for(int i = 0; i < RunManager.Instance.Battle.SkillInventory.Count; i++)
            //             {
            //                 RunSkill oldSkill = RunManager.Instance.Battle.SkillInventory[i];
            //                 WuXing? oldWuXing = oldSkill.GetEntry().WuXing;
            //                 JingJie oldJingJie = oldSkill.JingJie;
            //
            //                 if (!oldWuXing.HasValue)
            //                     continue;
            //
            //                 JingJie newJingJie = RandomManager.Range(oldJingJie, runNode.JingJie + 1);
            //
            //                 RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill newSkill, wuXing: oldWuXing.Value.Next, jingJie: newJingJie);
            //                 RunManager.Instance.Battle.SkillInventory.ReplaceSkill(oldSkill, newSkill);
            //             }
            //
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("连抽五张", "连抽五张",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("连抽五张事件",
            //             "连抽五张，需要消耗一半生命上限",
            //             "只抽一张，无需消耗生命上限");
            //
            //         A[0]._select = option =>
            //         {
            //             RunManager.Instance.SkillPool.TryDrawSkills(out List<RunSkill> skills, jingJie: RunManager.Instance.Map.JingJie, count: 5);
            //             RunManager.Instance.Battle.SkillInventory.AddSkills(skills);
            //
            //             int dHealth = RunManager.Instance.Battle.Hero.GetFinalHealth() / 2;
            //             RunManager.Instance.Battle.Hero.SetDHealth(-dHealth);
            //
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         A[1]._select = option =>
            //         {
            //             RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill skill, jingJie: RunManager.Instance.Map.JingJie);
            //             RunManager.Instance.Battle.SkillInventory.AddSkill(skill);
            //
            //             RunManager.Instance.Map.TryFinishNode();
            //             return null;
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("天机阁", "天机阁",
            //     create: runNode =>
            //     {
            //         SkillInventory inventory = new();
            //         RunManager.Instance.SkillPool.TryDrawSkills(out List<RunSkill> skills, jingJie: RunManager.Instance.Map.JingJie, count: 10, consume: false);
            //         inventory.AddSkills(skills);
            //         ArbitraryCardPickerPanelDescriptor A = new("请从10张牌中选1张获取", inventory: inventory,
            //             action: toAdd => RunManager.Instance.Battle.SkillInventory.AddSkills(toAdd));
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("温泉", "温泉",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("遇到温泉", "泡", "下潜", "离开");
            //         DialogPanelDescriptor B = new("命元 + 2");
            //         B._reward = new ResourceRewardDescriptor(mingYuan: 2);
            //         DiscoverSkillPanelDescriptor C = new("随机水属当前境界卡牌1张", wuXing: WuXing.Shui);
            //         DiscoverSkillPanelDescriptor D = new("随机火属当前境界卡牌1张", wuXing: WuXing.Huo);
            //
            //         A[0]._select = option =>
            //         {
            //             runNode.ChangePanel(B);
            //             return B;
            //         };
            //         A[1]._select = option =>
            //         {
            //             runNode.ChangePanel(C);
            //             return C;
            //         };
            //         A[2]._select = option =>
            //         {
            //             runNode.ChangePanel(D);
            //             return D;
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("集市地摊", "集市地摊",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("在集市地摊上感应到模糊的灵气",
            //             new DialogOption("5金币买下", new CostDetails(xiuWei: 5)),
            //             new DialogOption("60金币整摊包了", new CostDetails(xiuWei: 60)),
            //             "在此摆摊赚钱");
            //         DialogPanelDescriptor B1 = new("判定失败");
            //         DialogPanelDescriptor B2 = new("判定成功");
            //         DiscoverSkillPanelDescriptor C = new("随机获得下品法宝", pred: s => s.SkillTypeCollection.Contains(SkillType.LingQi));
            //         DialogPanelDescriptor D = new("获得35金币");
            //         D._reward = new ResourceRewardDescriptor(xiuWei: 35);
            //
            //         A[0]._select = option => runNode.ChangePanel(RandomManager.value < 0.5f ? B1 : B2);
            //         A[1]._select = option => runNode.ChangePanel(C);
            //         A[2]._select = option => runNode.ChangePanel(D);
            //
            //         B2[0]._select = option => runNode.ChangePanel(C);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("镇上饭馆", "镇上饭馆",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A1 = new("你路过镇上，前往饭馆打工挣钱，遇到后厨不同师傅之间闹矛盾",
            //             "专心打工赚钱",
            //             "帮烧火师傅说话",
            //             "帮砍柴师傅说话",
            //             "下页");
            //         DialogPanelDescriptor A2 = new ("你路过镇上，前往饭馆打工挣钱，遇到后厨不同师傅之间闹矛盾",
            //             "上页",
            //             "帮挑水师傅说话",
            //             "帮扫地师傅说话",
            //             "帮切菜师傅说话");
            //
            //         DialogPanelDescriptor jinBi = new("获得50金币");
            //         jinBi._reward = new ResourceRewardDescriptor(xiuWei: 50);
            //
            //         DiscoverSkillPanelDescriptor jin = new("获得金属卡牌1张", wuXing: WuXing.Jin);
            //         DiscoverSkillPanelDescriptor shui = new("获得水属卡牌1张", wuXing: WuXing.Shui);
            //         DiscoverSkillPanelDescriptor mu = new("获得木属卡牌1张", wuXing: WuXing.Mu);
            //         DiscoverSkillPanelDescriptor huo = new("获得火属卡牌1张", wuXing: WuXing.Huo);
            //         DiscoverSkillPanelDescriptor tu = new("获得土属卡牌1张", wuXing: WuXing.Tu);
            //
            //         A1[0]._select = option => runNode.ChangePanel(jinBi);
            //         A1[1]._select = option => runNode.ChangePanel(huo);
            //         A1[2]._select = option => runNode.ChangePanel(mu);
            //         A1[3]._select = option => runNode.ChangePanel(A2);
            //         A2[0]._select = option => runNode.ChangePanel(A1);
            //         A2[1]._select = option => runNode.ChangePanel(shui);
            //         A2[2]._select = option => runNode.ChangePanel(tu);
            //         A2[3]._select = option => runNode.ChangePanel(jin);
            //
            //         runNode.ChangePanel(A1);
            //     }),
            //
            // new AdventureNodeEntry("夜中山间泉", "夜中山间泉",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("夜晚你在山上准备灌些泉水，水流在夜间乌黑发亮，捧起发现这水本身就呈黑色，恍惚间有奇怪的声音响起，一颗黑皮球从上流滚落下来",
            //             "捡起黑皮球",
            //             "品尝一下黑色泉水",
            //             "赶快逃离这个鬼地方");
            //
            //         DialogPanelDescriptor A0 = new("你捡起黑皮球，表面柔软，摸起来像一颗快烂了的水蜜桃，你切开它，红色的汁液流了一地，里面露出了一枚淡黄色的果子",
            //             "用它修炼（未实现）",
            //             "吸收其中灵气");
            //
            //         DialogPanelDescriptor A00 = new("选择一张卡牌境界提升（未实现）");
            //
            //         DialogPanelDescriptor A01 = new("命元 + 2");
            //         A01._reward = new ResourceRewardDescriptor(mingYuan: 2);
            //
            //         DiscoverSkillPanelDescriptor A1 = new("你的身体理解了黑水的本质，获得水属性金丹期卡牌1张", wuXing: WuXing.Shui, jingJie: JingJie.JinDan);
            //
            //         DiscoverSkillPanelDescriptor A2 = new ("你从梦中醒来，手里握着一个木属性道具", wuXing: WuXing.Mu);
            //
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(A1);
            //         A[2]._select = option => runNode.ChangePanel(A2);
            //
            //         A0[0]._select = option => runNode.ChangePanel(A00);
            //         A0[1]._select = option => runNode.ChangePanel(A01);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("灵山", "灵山",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你在一座灵山上勘探时，寻获了一处宝物，其一旁写着“孕育愿望之壶”",
            //             "你对着“孕育愿望之壶”许愿改变你的命运",
            //             "总之先拿走（未实现）");
            //
            //         DialogPanelDescriptor A0 = new("一时间白天黑夜，星河流转，你的卡牌发生了改变，“五行相生”");
            //         DialogPanelDescriptor A1 = new("获得下品法宝“孕育愿望之壶”（未实现）");
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(A1);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("龙脉", "龙脉",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你寻得一处龙脉所在灵气聚集之地",
            //             "潜心闭关修炼（未实现）",
            //             "温养体内命元");
            //
            //         DialogPanelDescriptor A0 = new("提升自己一张卡牌的境界（未实现）");
            //         DialogPanelDescriptor A1 = new("命元 + 3");
            //         A1._reward = new ResourceRewardDescriptor(mingYuan: 3);
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(A1);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("幽亭", "幽亭",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你的面前是一滩清池，池水中立着一尊幽亭，墨绿色的亭柱顶着猩红色的宝顶。",
            //             "饮一口池水（未实现）",
            //             "在池边停下修炼",
            //             "跃入亭子一探究竟");
            //
            //         DialogPanelDescriptor A0 = new("池水清冽，你沿路奔波的劳累一扫而光。获得卡牌“精力充沛”（未实现）");
            //         DiscoverSkillPanelDescriptor A1 = new("你不禁暗想，这块宝地许是某位修士前辈开辟的宝地，不妨在这修炼一段时间，肯定大有裨益。（获得水属性卡牌1张）", wuXing: WuXing.Shui);
            //         DialogPanelDescriptor A20 = new("你纵身一跃，但池水仿佛有着一股引力，将你拉入水中，而这水下竟望不到底，入目皆是幽黑，你用尽全力向水面挣扎，终于在意识模糊前探出水面。而亭子也同引力一并消失了。");
            //         DiscoverSkillPanelDescriptor A21 = new("你纵身一跃，一个跟斗，稳稳落入亭中，亭子正中的石桌上，有一本秘籍。", wuXing: WuXing.Huo);
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(A1);
            //         A[2]._select = option => runNode.ChangePanel(RandomManager.value < 0.5 ? A20 : A21);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("问剑村", "问剑村",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你来到了“问剑村”，最近村中时常发生强盗事件，村民苦不堪言。",
            //             "我必拔刀相助（未实现）",
            //             "离开村庄");
            //
            //         DiscoverSkillPanelDescriptor A0 = new("（先直接获得奖励，在敌人设计完成后再更新）");
            //         DialogPanelDescriptor A1 = new("世事无常，对于此你已无悲喜（生命上限+10）");
            //         A1._reward = new ResourceRewardDescriptor(health: 10);
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(A1);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("夜间洞穴", "夜间洞穴",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你在夜间行路，却听到一些似有似无的嘈杂声响，循声来到一处洞穴前，其中传来了打斗和欢呼声",
            //             "步入洞中（未实现）",
            //             "噢，我架子上烤着鸡呢，我得回去看火");
            //
            //         DiscoverSkillPanelDescriptor A0 = new("洞内是一处地下擂台，你的到来引起了庄家的注意，不一会儿，几名大汉前来邀请你参与擂台。（未实现）");
            //         DialogPanelDescriptor A1 = new("离开此处，做了一个吃烤鸡的美梦。（生命上限+10）");
            //         A1._reward = new ResourceRewardDescriptor(health: 10);
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(A1);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("元神不稳", "元神不稳",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你正在修炼，突然感到元神不稳，你会怎么做？",
            //             "继续冥想调整元神",
            //             new DialogOption("停下来仔细观察元神", new CostDetails(xiuWei: 10)));
            //
            //         DialogPanelDescriptor A0 = new("你成功稳定了元神，修为提升了10。");
            //         A0._reward = new ResourceRewardDescriptor(xiuWei: 10);
            //
            //         DiscoverSkillPanelDescriptor A1 = new("你因为没有及时处理，元神出现了一些微小的损伤，修为下降，但你的仔细观察倒也有些许感悟。");
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(A1);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("禁忌秘籍", "禁忌秘籍",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new("你意外得到了一本失传已久的修炼秘籍，但是此书似乎有禁忌，你会怎么做？",
            //             "毫不犹豫地开始修炼（未实现）",
            //             "先研究禁忌再说",
            //             "将书藏起来，不再碰它。");
            //
            //         DialogPanelDescriptor A0 = new("你开始了修炼，秘籍带给你惊人的修为提升，但是在修炼尾声书中蹦出一只怪物，打伤了你，并接着朝你袭来。（未实现）");
            //         DialogPanelDescriptor A1s = new("你研究了禁忌并发现其中隐含的危险，解除禁忌后，你顺利的完成了修炼。（未实现）");
            //         DialogPanelDescriptor A1f = new("你不小心触发了禁忌。书中蹦出一只怪物，还好你早有防备，避开了怪物的攻击，你看到书损毁了，同时怪物向你袭来。");
            //         DialogPanelDescriptor A2 = new("你将秘籍藏在了一个安全的地方，虽然错过了修炼的机会，但是避免了潜在的风险。");
            //
            //         A[0]._select = option => runNode.ChangePanel(A0);
            //         A[1]._select = option => runNode.ChangePanel(RandomManager.value < 0.5f ? A1s : A1f);
            //         A[2]._select = option => runNode.ChangePanel(A2);
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            //
            // new AdventureNodeEntry("狂吾师叔事件", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor("你遇见了狂吾师叔，他问你狂剑的见解", "说出自己的见解(需要有狂剑牌)", "说自己不懂，请师叔赐教");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("获得了一张练气牌");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("获得了一张练气牌");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             if (signal is SelectedOptionSignal selectedOptionSignal)
            //             {
            //                 runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
            //             }
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             bool success = RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill skill, jingJie: JingJie.LianQi);
            //             if (success)
            //                 RunManager.Instance.Battle.SkillInventory.AddSkill(skill);
            //
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             bool success = RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill skill, jingJie: JingJie.LianQi);
            //             if (success)
            //                 RunManager.Instance.Battle.SkillInventory.AddSkill(skill);
            //
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("神殿事件", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor("来到一处神殿", "我必凯旋", "我已膨胀");
            //         BattlePanelDescriptor B = new BattlePanelDescriptor("鶸", new CreateEntityDetails(RunManager.Instance.Map.JingJie));
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("你无法再获得命元，所有牌获得二动");
            //         DialogPanelDescriptor D = new DialogPanelDescriptor("胜利");
            //         DialogPanelDescriptor E = new DialogPanelDescriptor("你没能击败对手，虽然损失了一些命元，但还是获得了奖励");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             if (signal is BattleResultSignal battleResultSignal)
            //             {
            //                 if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
            //                 {
            //                     runNode.ChangePanel(D);
            //                 }
            //                 else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
            //                 {
            //                     runNode.ChangePanel(E);
            //                 }
            //             }
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         D._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         E._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("土匪事件", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor("你经过一处村庄，村口的石碑上刻着“问剑村”，村子里的村民各个面色凝重，这时一位老者向你走来，他告诉你：村庄饱受土匪强盗侵扰。你决定：", "迅速离去", "拔刀相助");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("自从筑基成功，你就正式踏上修仙的道路，凡尘的准则与琐事都与你无关。");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("修仙者走的就是一条逆天而行的路，常年累月的修行换来的力量就是为了能够以自己的意志行走于大地之上。“老伯你放心，今日，我定给‘问剑村’一个安宁。”说罢，你一个飞身往土匪的藏身处遁去。");
            //         BattlePanelDescriptor D = new BattlePanelDescriptor("鶸", new CreateEntityDetails(RunManager.Instance.Map.JingJie));//先用了你声明过得关键字
            //         BattlePanelDescriptor E = new BattlePanelDescriptor("鶸", new CreateEntityDetails(RunManager.Instance.Map.JingJie));
            //         DialogPanelDescriptor F = new DialogPanelDescriptor("你带着土匪老大的头颅返回了问剑村，村长奖励了你一些XXX");
            //         DialogPanelDescriptor G = new DialogPanelDescriptor("你没能击败对手，损失了一些命元");
            //         DialogPanelDescriptor H = new DialogPanelDescriptor("土匪老大落败后提出：他在问剑村找到一本功法秘籍。他用秘籍作为交换希望你能放他一命。你决定：","同意","直接杀死土匪老大");
            //         DialogPanelDescriptor I = new DialogPanelDescriptor("土匪老大带你来到洞窟深处，打开某个暗门后，你看见一个石台上放着一本秘籍，趁你被秘籍吸引了注意，他掏出一个护符握在手中，护符化作一道金光划过天际，你回头一看，土匪老大逃走了。你决定：","遵从交易，放过他","追杀土匪老大");
            //         DialogPanelDescriptor J = new DialogPanelDescriptor("你没有管逃跑的土匪老大，继续在藏宝处寻找，在又找到一些宝藏后，你离开洞窟，继续赶路去了。");
            //         DialogPanelDescriptor K = new DialogPanelDescriptor("你成功追上了土匪老大，砍下了他的头颅，由于追的太投入，你找不到返回藏宝处的路，只能提着土匪老大的头返回了问剑村，村长奖励了你一些XXX");
            //         DialogPanelDescriptor L = new DialogPanelDescriptor("你拼命追赶可还是被土匪老大逃掉了,由于追的太投入，你找不到返回藏宝处的路，只能拿着秘籍离开。");
            //         DialogPanelDescriptor M = new DialogPanelDescriptor("杀死土匪老大后，你砍下了他的头颅，你决定：", "尝试在洞窟中寻找秘籍", "带着头颅返回问剑村");
            //         DialogPanelDescriptor N = new DialogPanelDescriptor("你在洞窟深处发现一道暗门，打开暗门后你发现了一本功法秘籍还有一些其他的宝藏。收集完宝藏后，你带着土匪老大的头颅返回了问剑村，村长奖励了你一些XXX");
            //         DialogPanelDescriptor O = new DialogPanelDescriptor("你在洞窟中四处寻找，可是一无所获，还触发了一些陷阱，损失了一些命元，所幸没有大碍。你带着土匪老大的头颅返回了问剑村，村长奖励了你一些XXX");
            //
            //
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //            runNode.ChangePanel(D);
            //         };
            //
            //         D._receiveSignal = (signal) =>
            //         {
            //             if (signal is BattleResultSignal battleResultSignal)
            //             {
            //                 if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
            //                 {
            //                     runNode.ChangePanel(E);
            //                 }
            //                 else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
            //                 {
            //                     runNode.ChangePanel(G);
            //                 }
            //             }
            //         };
            //
            //         E._receiveSignal = (signal) =>
            //         {
            //             if (signal is BattleResultSignal battleResultSignal)
            //             {
            //                 if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
            //                 {
            //                     runNode.ChangePanel(H);
            //                 }
            //                 else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
            //                 {
            //                     runNode.ChangePanel(G);
            //                 }
            //             }
            //         };
            //
            //         F._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         G._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         H._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? I : M);
            //         };
            //
            //         I._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? J : K/*or L need judge*/);
            //         };
            //
            //         J._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         K._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         L._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         M._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? N/*or O need judge*/ : F);
            //         };
            //
            //         N._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         O._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            //
            // new AdventureNodeEntry("池塘", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor(
            //             "你经过一滩清池，一座亭子立于池中，你不禁驻足。墨绿色的亭柱顶着朱红色的宝顶，绕池一周，你并未看到通向亭子的道路，你决定：",
            //             "饮一口池水（回血）",
            //             "在池边停下修炼（回血药）",
            //             "尝试飞身跃入亭中（拥有一张筑基牌，则抽一张牌）。那亭子一看便不是凡物，里面说不好有一番机缘，你决定尝试飞身跃入亭中一探究竟。");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("池水清冽，你沿路奔波的劳累一扫而光。");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("你不禁暗想：这块宝地许是某位修士前辈开辟的宝地，不妨在这修炼一段时间，肯定大有裨益。");
            //         DialogPanelDescriptor D = new DialogPanelDescriptor("你纵深一跃，在空中一个翻身，稳稳落入亭中，亭子正中的石桌上，有一本《XXX》。");
            //         DialogPanelDescriptor E = new DialogPanelDescriptor("你纵身一跃，结果距离亭子仍有数尺，你落入水中，等你浮上水面时亭子已经消失了，你大感可惜，悻悻离去。");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             if (signal is SelectedOptionSignal selectedOptionSignal)
            //             {
            //                 if (selectedOptionSignal.Selected == 0)
            //                 {
            //                     runNode.ChangePanel(B);
            //                 }
            //                 else if (selectedOptionSignal.Selected == 1)
            //                 {
            //                     runNode.ChangePanel(C);
            //                 }
            //                 else
            //                 {
            //                     bool flag = RunManager.Instance.AcquiredInventory.FirstObj(acquired =>
            //                                     acquired.GetJingJie() >= JingJie.ZhuJi) != null ||
            //                                 RunManager.Instance.Hero.HeroSlotInventory.Traversal.FirstObj(slot =>
            //                                     slot.GetJingJie().HasValue && slot.GetJingJie() >= JingJie.ZhuJi) !=
            //                                 null;
            //                     if (flag)
            //                     {
            //                         runNode.ChangePanel(D);
            //                     }
            //                     else
            //                     {
            //                         runNode.ChangePanel(E);
            //                     }
            //                 }
            //             }
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         D._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         E._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("路途劳累", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor(
            //             "旅行数日，即便你已踏上修炼之道，路途上的劳累也无法完全消除（【惩罚】生命值惩罚5），你前往一处聚落决定休息几日，凝练修为。路过坊市时，一位清新脱俗的姑娘映入你的眼帘，",
            //             "不禁入神，驻足停留（【惩罚】【条件 驻足停留】 修为减少）",
            //             "加快脚步前往客栈（【奖励】回复生命值10，修为增加）",
            //             "前往坊市转转（跳转至商店界面）");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("看到她，你想起了江南的雨，你楞在原地，看出了神，直到背后的人推了你一把，你才回过神来。你暗暗一惊：你的道心不稳。修仙长路漫漫，需要长年累月的苦修，男女之情是修仙路上的一大阻碍，跟何况对方还是一位没有踏上仙路的女子。你赶快加快脚步离开此地。");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("你来到客栈后，就开始运转功法，一段时间后，旅途的劳累一扫而空，并巩固了修为。");
            //         // DialogPanelDescriptor D = new MarketPanelDescriptor("你纵深一跃，在空中一个翻身，稳稳落入亭中，亭子正中的石桌上，有一本《XXX》。");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             if (signal is SelectedOptionSignal selectedOptionSignal)
            //             {
            //                 if (selectedOptionSignal.Selected == 0)
            //                 {
            //                     runNode.ChangePanel(B);
            //                 }
            //                 else if (selectedOptionSignal.Selected == 1)
            //                 {
            //                     runNode.ChangePanel(C);
            //                 }
            //                 else
            //                 {
            //                 }
            //             }
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
        });
    }
}
