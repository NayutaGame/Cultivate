
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class RoomCategory : Category<RoomEntry>
{
    public RoomCategory()
    {
        AddRange(new List<RoomEntry>()
        {
            #region Core
            
            new(id:                                 "不存在的事件",
                description:                        "不存在的事件",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("不存在的事件");
                    return A;
                }),
            
            new(id:                                 "战斗",
                description:                        "战斗",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    BattleRoomDescriptor roomDescriptor = room.Descriptor as BattleRoomDescriptor;
                    int baseGoldReward = roomDescriptor._baseGoldReward;
                    int goldValue = Mathf.RoundToInt(baseGoldReward * RandomManager.Range(0.9f, 1.1f));

                    BattlePanelDescriptor A = new(room.Details["Entity"] as RunEntity);

                    DiscoverSkillPanelDescriptor B = new("胜利");
                    DiscoverSkillPanelDescriptor C = new("惜败");
                    DialogPanelDescriptor D = new($"按Esc退出游戏，游戏结束，感谢游玩");

                    bool shouldUpdateSlotCount = roomDescriptor.ShouldUpdateSlotCount;

                    if (!roomDescriptor._isBoss)
                    {
                        A.SetWinOperation(() =>
                        {
                            B.SetDetailedText($"获得了<style=\"Gold\">{goldValue}金钱</style>\n请选择<style=\"Red\">一张卡牌</style>作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(roomDescriptor._slotCountAfter);
                            return B;
                        });

                        A.SetLoseOperation(() =>
                        {
                            RunManager.Instance.Environment.SetDMingYuanProcedure(-2);
                            C.SetDetailedText($"<style=\"Gray\">你没能击败对手，损失了一些命元</style>" +
                                       $"\n但获得了<style=\"Gold\">{goldValue}金钱</style>，以及选择<style=\"Red\">一张卡牌</style>作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(roomDescriptor._slotCountAfter);
                            return C;
                        });
                    }
                    else if (RunManager.Instance.Environment.JingJie != JingJie.HuaShen)
                    {
                        A.SetWinOperation(() =>
                        {
                            RunManager.Instance.Environment.SetDMingYuanProcedure(3);
                            B.SetDetailedText($"跨越境界使得你的命元恢复了3" +
                                              $"\n获得了{goldValue}金，请选择<style=\"Red\">一张卡牌</style>作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(roomDescriptor._slotCountAfter);
                            return B;
                        });

                        A.SetLoseOperation(() =>
                        {
                            C.SetDetailedText($"<style=\"Gray\">你没能击败对手，幸好跨越境界抵消了你的命元伤害。</style>" +
                                              $"\n获得了<style=\"Gold\">{goldValue}金钱</style>，请选择<style=\"Red\">一张卡牌</style>作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(roomDescriptor._slotCountAfter);
                            return C;
                        });
                    }
                    else
                    {
                        A.SetWinOperation(() =>
                        {
                            D.SetDetailedText($"你击败了强大的对手，取得了最终的胜利！（按Esc退出游戏，游戏结束，感谢游玩）");
                            return D;
                        });

                        A.SetLoseOperation(() =>
                        {
                            D.SetDetailedText($"你没能击败对手，受到了致死的命元伤害。（按Esc退出游戏，游戏结束，感谢游玩）");
                            return D;
                        });
                    }

                    B._receiveSignal = signal =>
                    {
                        RunManager.Instance.Environment.SetDGoldProcedure(goldValue);
                        return B.DefaultReceiveSignal(signal);
                    };

                    C._receiveSignal = signal =>
                    {
                        RunManager.Instance.Environment.SetDGoldProcedure(goldValue);
                        return C.DefaultReceiveSignal(signal);
                    };

                    D._receiveSignal = signal => D;

                    return A;
                }),
            
            new(id:                                 "出门",
                description:                        "出门",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    // 0 -> 凌云峰，5张金牌
                    // 1 -> 逍遥海，5张水牌
                    // 2 -> 桃花宫，5张木牌
                    // 3 -> 长明殿，5张火牌
                    // 4 -> 环岳岭，5张土牌
                    // 5 -> 易宝斋，6张随机牌，5金钱
                    // 6 -> 剑池，6张攻击牌
                    // 7 -> 风雨楼，6张防御牌
                    // 8 -> 百草堂，6张随机牌，5气血上限
                    // 9 -> 星宫，3张灵气牌，2张随机牌
                    // 10 -> 天机阁，2张筑基牌，3张随机牌
                    // 11 -> 散修，8张随机牌
                    string[] titles = new string[12]
                    {
                        "凌云峰，5张金牌",
                        "逍遥海，5张水牌",
                        "桃花宫，5张木牌",
                        "长明殿，5张火牌",
                        "环岳岭，5张土牌",
                        "易宝斋，6张随机牌，5金钱",
                        "剑池，6张攻击牌",
                        "风雨楼，6张防御牌",
                        "百草堂，6张随机牌，5气血上限",
                        "星宫，3张灵气牌，2张随机牌",
                        "天机阁，2张筑基牌，3张随机牌",
                        "散修，8张随机牌",
                    };
                    
                    RunEnvironment env = RunManager.Instance.Environment;

                    Func<DialogOption, PanelDescriptor>[] selects = new Func<DialogOption, PanelDescriptor>[12]
                    {
                        option =>
                        { // 0 -> 凌云峰，5张金牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, wuXing: WuXing.Jin, count: 5));
                            return null;
                        },
                        option =>
                        { // 1 -> 逍遥海，5张水牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, wuXing: WuXing.Shui, count: 5));
                            return null;
                        },
                        option =>
                        { // 2 -> 桃花宫，5张木牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, wuXing: WuXing.Mu, count: 5));
                            return null;
                        },
                        option =>
                        { // 3 -> 长明殿，5张火牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, wuXing: WuXing.Huo, count: 5));
                            return null;
                        },
                        option =>
                        { // 4 -> 环岳岭，5张土牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, wuXing: WuXing.Tu, count: 5));
                            return null;
                        },
                        option =>
                        { // 5 -> 易宝斋，6张随机牌，5金钱
                            env.SetDGoldProcedure(5);
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, count: 6));
                            return null;
                        },
                        option =>
                        { // 6 -> 剑池，6张攻击牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, pred: s => s.GetSkillTypeComposite().Contains(SkillType.Attack), count: 6));
                            return null;
                        },
                        option =>
                        { // 7 -> 风雨楼，6张防御牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, pred: s => s.GetSkillTypeComposite().Contains(SkillType.Defend), count: 6));
                            return null;
                        },
                        option =>
                        { // 8 -> 百草堂，6张随机牌，5气血上限
                            env.SetDDHealthProcedure(5);
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, count: 6));
                            return null;
                        },
                        option =>
                        { // 9 -> 星宫，3张灵气牌，2张随机牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, pred: s => s.GetSkillTypeComposite().Contains(SkillType.Mana), count: 3));
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, count: 2));
                            return null;
                        },
                        option =>
                        { // 10 -> 天机阁，2张筑基牌，3张随机牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.ZhuJi, count: 2));
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, count: 3));
                            return null;
                        },
                        option =>
                        { // 11 -> 散修，8张随机牌
                            env.DrawSkillsProcedure(new(distinct: false, jingJie: JingJie.LianQi, count: 8));
                            return null;
                        },
                    };
                    
                    Pool<int> pool = new Pool<int>();
                    12.Do(i => pool.Populate(i));
                    pool.Shuffle();
                    int[] popped = new int[3];
                    popped.Length.Do(i => pool.TryPopItem(out popped[i]));

                    DialogOption[] dialogOptions = new DialogOption[popped.Length];
                    dialogOptions.Length.Do(i =>
                    {
                        int index = popped[i];
                        dialogOptions[i] = new DialogOption(titles[index]);
                        dialogOptions[i].SetSelect(selects[index]);
                    });
                    
                    DialogPanelDescriptor A = new($"请问想以哪一个门派出发？", dialogOptions);

                    return A;
                }),
            
            new(id:                                 "突破境界",
                description:                        "突破境界",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    // 突破时奖励，将一张卡升级到下一境界
                    // 发现一张下一境界卡牌，带描述
                    
                    // 0 -> 凌云峰，选择1张下一境界的金牌
                    // 1 -> 逍遥海，选择1张下一境界的水牌
                    // 2 -> 桃花宫，选择1张下一境界的木牌
                    // 3 -> 长明殿，选择1张下一境界的火牌
                    // 4 -> 环岳岭，选择1张下一境界的土牌
                    // 5 -> 易宝斋，得到2/4/8/16金钱，访问一次商店
                    // 6 -> 剑池，选择2张当前境界的攻击牌
                    // 7 -> 风雨楼，选择2张当前境界的防御牌
                    // 8 -> 百草堂，得到4/8/16/32气血上限
                    // 9 -> 星宫，选择2张当前境界的灵气牌
                    // 10 -> 天机阁，卡池中，当前及以下境界的牌，被移除一半
                    // 11 -> 散修，选择一张牌提升至下一境界
                    string[] titles = new string[12]
                    {
                        "凌云峰，选择1张下一境界的金牌",
                        "逍遥海，选择1张下一境界的水牌",
                        "桃花宫，选择1张下一境界的木牌",
                        "长明殿，选择1张下一境界的火牌",
                        "环岳岭，选择1张下一境界的土牌",
                        "易宝斋，得到2/4/8/16金钱，访问一次商店",
                        "剑池，选择2张当前境界的攻击牌",
                        "风雨楼，选择2张当前境界的防御牌",
                        "百草堂，得到4/8/16/32气血上限",
                        "星宫，选择2张当前境界的灵气牌",
                        "天机阁，卡池中，当前及以下境界的牌，被移除一半",
                        "散修，选择一张牌提升至下一境界",
                    };
                    
                    RunEnvironment env = RunManager.Instance.Environment;
                    JingJie currJingJie = env.JingJie;
                    JingJie nextJingJie = (env.JingJie + 1).ClampUpper(JingJie.HuaShen);

                    PanelDescriptor[] panels = new PanelDescriptor[12]
                    {
                        new DiscoverSkillPanelDescriptor("凌云峰，选择1张下一境界的金牌",
                            descriptor: new(wuXing: WuXing.Jin, pred: e => e.LowestJingJie == nextJingJie, count: 3),
                            preferredJingJie: nextJingJie),
                        new DiscoverSkillPanelDescriptor("逍遥海，选择1张下一境界的水牌",
                            descriptor: new(wuXing: WuXing.Shui, pred: e => e.LowestJingJie == nextJingJie, count: 3),
                            preferredJingJie: nextJingJie),
                        new DiscoverSkillPanelDescriptor("桃花宫，选择1张下一境界的木牌",
                            descriptor: new(wuXing: WuXing.Mu, pred: e => e.LowestJingJie == nextJingJie, count: 3),
                            preferredJingJie: nextJingJie),
                        new DiscoverSkillPanelDescriptor("长明殿，选择1张下一境界的火牌",
                            descriptor: new(wuXing: WuXing.Huo, pred: e => e.LowestJingJie == nextJingJie, count: 3),
                            preferredJingJie: nextJingJie),
                        new DiscoverSkillPanelDescriptor("环岳岭，选择1张下一境界的土牌",
                            descriptor: new(wuXing: WuXing.Tu, pred: e => e.LowestJingJie == nextJingJie, count: 3),
                            preferredJingJie: nextJingJie),
                        // 易宝斋，得到2/4/8/16金钱，访问一次商店
                        new ShopPanelDescriptor(nextJingJie, "收藏家").SetEnter(panelDescriptor =>
                            {
                                panelDescriptor.DefaultEnter(panelDescriptor);
                                env.SetDGoldProcedure(2 * RoomDescriptor.GoldRewardTable[room.Ladder]);
                            }),
                        new DialogPanelDescriptor("剑池，获得2张当前境界的攻击牌")
                            .SetReward(new DrawSkillReward("2张当前境界的攻击牌", new(jingJie: currJingJie, skillTypeComposite: SkillType.Attack, count: 2))),
                        new DialogPanelDescriptor("风雨楼，获得2张当前境界的防御牌")
                            .SetReward(new DrawSkillReward("2张当前境界的防御牌", new(jingJie: currJingJie, skillTypeComposite: SkillType.Defend, count: 2))),
                        new DialogPanelDescriptor("百草堂，得到4/8/16/32气血上限")
                            .SetReward(new ResourceReward(health: 4 * RoomDescriptor.GoldRewardTable[room.Ladder])),
                        new DialogPanelDescriptor("星宫，获得2张当前境界的灵气牌")
                            .SetReward(new DrawSkillReward("2张当前境界的灵气牌", new(jingJie: currJingJie, skillTypeComposite: SkillType.Mana, count: 2))),
                        new DialogPanelDescriptor("天机阁，卡池中，当前及以下境界的牌，被移除一半")
                            .SetEnter(panelDescriptor =>
                            {
                                panelDescriptor.DefaultEnter(panelDescriptor);
                                
                                List<SkillEntry> lowList = new();
                                while (env.SkillPool.TryPopItem(out SkillEntry skillEntry, pred: s => s.LowestJingJie <= currJingJie))
                                    lowList.Add(skillEntry);

                                int count = lowList.Count / 2;
                                count.Do(i => env.SkillPool.Populate(lowList[i]));
                                
                                env.SkillPool.Shuffle();
                            }),
                        new CardPickerPanelDescriptor("散修，选择一张牌提升至下一境界")
                            .SetConfirmOperation(iRunSkillList =>
                            {
                                foreach (var iRunSkill in iRunSkillList)
                                {
                                    if (iRunSkill is RunSkill skill)
                                    {
                                        skill.JingJie = nextJingJie;
                                        // staging
                                        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
                                    }
                                    else if (iRunSkill is SkillSlot slot)
                                    {
                                        slot.Skill.JingJie = nextJingJie;
                                        // staging
                                        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
                                    }
                                }
                                return null;
                            }),
                    };
                    
                    Pool<int> pool = new Pool<int>();
                    12.Do(i => pool.Populate(i));
                    pool.Shuffle();
                    int[] popped = new int[4];
                    popped.Length.Do(i => pool.TryPopItem(out popped[i]));

                    DialogOption[] dialogOptions = new DialogOption[popped.Length];
                    dialogOptions.Length.Do(i =>
                    {
                        int index = popped[i];
                        dialogOptions[i] = new DialogOption(titles[index]);
                        dialogOptions[i].SetSelect(option =>
                        {
                            env.NextJingJieProcedure();
                            return panels[index];
                        });
                    });
                    
                    DialogPanelDescriptor A = new("你感到很久以来的瓶颈将被突破", dialogOptions);
                    RunManager.Instance.SetBackgroundFromJingJie(nextJingJie);
                    return A;
                }),

            new(id:                                 "休息",
                description:                        "休息",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("最近有些空闲的时间，你决定要", "加紧修炼", "去温泉", "喝点人参茶");

                    JingJie targetJingJie = Mathf.Min(RunManager.Instance.Environment.JingJie + 1, JingJie.HuaShen);
                    CardPickerPanelDescriptor B = new CardPickerPanelDescriptor(
                        instruction:       $"在菩提树下坐了一段时间，对境界有了新的见解。\n请选择至多一张低于{targetJingJie}的卡牌提升至{targetJingJie}",
                        bound:              new Bound(0, 2),
                        descriptor:         RunSkillDescriptor.FromJingJieBound(JingJie.LianQi, targetJingJie));
                    B.SetConfirmOperation(iRunSkillList =>
                    {
                        foreach (var iRunSkill in iRunSkillList)
                        {
                            if (iRunSkill is RunSkill skill)
                            {
                                skill.JingJie = targetJingJie;
                                // staging
                                CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
                            }
                            else if (iRunSkill is SkillSlot slot)
                            {
                                slot.Skill.JingJie = targetJingJie;
                                // staging
                                CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
                            }
                        }

                        return null;
                    });

                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
                    DialogPanelDescriptor C = new DialogPanelDescriptor($"泡了温泉之后感到了心情畅快，获得了{baseGoldReward}点气血上限")
                        .SetReward(Reward.FromHealth(baseGoldReward));
                    
                    DialogPanelDescriptor D = new DialogPanelDescriptor("喝了几口人参茶，回复了2点命元")
                        .SetReward(Reward.FromMingYuan(2));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "胜利",
                description:                        "胜利",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("恭喜获得游戏胜利",
                    "前往结算");

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunResultState.Victory);
                        return null;
                    });

                    return A;
                }),

            #endregion

            #region Tutorial

            new(id:                                 "教学1",
                description:                        "教学1",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物1"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌1");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("这个界面是准备界面" +
                                         "\n徐福和对手准备进行战斗" +
                                         "\n战斗开始之后，徐福和对手会依次使用准备区中的牌"),
                        new ConfirmGuide("左边的-2是徐福最终血量" +
                                         "\n右边的10是对手最终血量" +
                                         "\n现在对手会以10血打败徐福"),
                        new EquipGuide("将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("劈砍"), new DeckIndex(true, 0)),
                        new ConfirmGuide("将劈砍置入后，左边血量大于右边。" +
                                         "\n表示战斗的最终结果是徐福以4血战胜对手"),
                        new ClickBattleGuide("徐福胜利后，便可以点击对决按钮，进入战斗界面" +
                                             "\n战斗界面中，徐福和对手会依次使用战斗区布置好的牌",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    playerTemplate.TraversalCurrentSlots().Do(s =>
                    {
                        SkillEntry entry = s.Skill?.GetEntry();
                        if (entry != null)
                            RunManager.Instance.Environment.AddSkillProcedure(entry);
                    });
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        playerTemplate.TraversalCurrentSlots().Do(s =>
                        {
                            SkillEntry entry = s.Skill?.GetEntry();
                            if (entry != null)
                                RunManager.Instance.Environment.AddSkillProcedure(entry);
                        });
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学2",
                description:                        "教学2",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物2"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌2");
                    
                    BattlePanelDescriptor A = new(enemyEntity);

                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("战斗结束后，气血将会回复到满格" +
                                         "\n现在徐福面临着下一个对手"),
                        new EquipGuide("现在将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("恋花"), DeckIndex.FromField(1)),
                        new ConfirmGuide("此时未能胜利是因为对方初始血量高于我们" +
                                         "\n我们注意到恋花左上角的消耗标志，表示恋花需要消耗一点灵气"),
                        new ConfirmGuide("缺少灵气时，角色会消耗一回合用于聚集灵气"),
                        new UnequipGuide("战斗区放空时，角色也会聚集一点灵气" +
                                         "\n所以我们可以将劈砍卸下",
                            SkillEntryDescriptor.FromName("劈砍")),
                        new ClickBattleGuide("此时虽然我们卡牌更少，但是不会缺少灵气，使得我们出牌更快" +
                                             "\n请点击对决以查看结算",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("劈砍"), preferredDeckIndex: DeckIndex.FromField(0));
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("恋花"));
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("劈砍"), preferredDeckIndex: DeckIndex.FromField(0));
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("恋花"));
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学3",
                description:                        "教学3",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物3"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌3");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("恋花可以吸血，刚才的战斗，因为双方互相吸血" +
                                         "\n使得战斗进行了很长的时间，所幸最终还是赢了"),
                        new ConfirmGuide("现在双方都使用恋花战斗，而且都没有缺少灵气" +
                                         "\n伤害和吸血互相抵消，都打不死对方" +
                                         "\n这种情况战斗会在120回合之后强制结束"),
                        new ConfirmGuide("结束时，双方都存活，但是因为对方气血值是15高于徐福，因此还是对方胜利"),
                        new EquipGuide("这次选择置入寻猎" +
                                       "\n虽然使用恋花的时候缺少灵气，但是寻猎可以赋予对方5破甲",
                            SkillEntryDescriptor.FromName("寻猎"), DeckIndex.FromField(0)),
                        new ConfirmGuide("寻猎之后恋花造成的伤害由4变成了9，可以由吸血回复9气血，效果更强了"),
                        new ClickBattleGuide("请开始战斗",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("恋花"), preferredDeckIndex: DeckIndex.FromField(1));
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("寻猎"));
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("恋花"), preferredDeckIndex: DeckIndex.FromField(1));
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("寻猎"));
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学4",
                description:                        "教学4",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物4"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌4");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new EquipGuide("现在对方拿着寻猎和恋花，徐福手中只有拂晓，先摆上去试试",
                            SkillEntryDescriptor.FromName("拂晓"), DeckIndex.FromField(1)),
                        new ConfirmGuide("战斗结果没有改变" +
                                         "\n徐福观察到了，对方的寻猎赋予破甲要求击伤才能触发" +
                                         "\n而寻猎的基础攻击只有2"),
                        new EquipGuide("拂晓提供了护甲" +
                                       "\n把拂晓换到前面试试",
                            SkillEntryDescriptor.FromName("拂晓"), DeckIndex.FromField(0)),
                        new ConfirmGuide("徐福先手使用了拂晓" +
                                         "\n拂晓赋予的护甲阻止了击伤效果" +
                                         "\n此时，寻猎上的击伤描述已经变成了灰色，代表效果不会触发"),
                        new ClickBattleGuide("点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("寻猎"), preferredDeckIndex: DeckIndex.FromField(0));
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("拂晓"));
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("寻猎"), preferredDeckIndex: DeckIndex.FromField(0));
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("拂晓"));
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学5",
                description:                        "教学5",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物5"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌5");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("护甲可以对抗破甲" +
                                         "\n而穿透可以忽视对方的护甲"),
                        new EquipGuide("将小松置入战斗区" +
                                       "\n这样就不怕对方拂晓带来的护甲了",
                            SkillEntryDescriptor.FromName("小松"), DeckIndex.FromField(1)),
                        new ClickBattleGuide("而且小松还有一个效果是成长" +
                                             "\n每使用一次效果都会变强" +
                                             "\n点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    playerTemplate.TraversalCurrentSlots().Do(s =>
                    {
                        SkillEntry entry = s.Skill?.GetEntry();
                        if (entry != null)
                            RunManager.Instance.Environment.AddSkillProcedure(entry);
                    });
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        playerTemplate.TraversalCurrentSlots().Do(s =>
                        {
                            SkillEntry entry = s.Skill?.GetEntry();
                            if (entry != null)
                                RunManager.Instance.Environment.AddSkillProcedure(entry);
                        });
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学6",
                description:                        "教学6",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物6"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌6");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("对方来势凶凶" +
                                         "\n徐福现在手中只有一张潜龙在渊"),
                        new EquipGuide("将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("潜龙在渊"), DeckIndex.FromField(1)),
                        new ConfirmGuide("潜龙在渊可以提供一次闪避" +
                                         "\n使敌方下次攻击无效" +
                                         "\n成长类的卡牌随着每次使用会越来越强，可以搭配防御牌将回合拖到成长牌变得强力的时候"),
                        new ClickBattleGuide("点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    playerTemplate.TraversalCurrentSlots().Do(s =>
                    {
                        SkillEntry entry = s.Skill?.GetEntry();
                        if (entry != null)
                            RunManager.Instance.Environment.AddSkillProcedure(entry);
                    });
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        playerTemplate.TraversalCurrentSlots().Do(s =>
                        {
                            SkillEntry entry = s.Skill?.GetEntry();
                            if (entry != null)
                                RunManager.Instance.Environment.AddSkillProcedure(entry);
                        });
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学7",
                description:                        "教学7",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物7"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌7");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("这次对方尝试使用闪避加成长的打法" +
                                         "\n闪避虽然可以很好的避免单次的伤害" +
                                         "\n但是如果有多次攻击机会，就可以很快的将闪避消耗掉"),
                        new EquipGuide("将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("劈砍"), DeckIndex.FromField(0)),
                        new EquipGuide("将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("云袖"), DeckIndex.FromField(1)),
                        new ConfirmGuide("失误了，徐福打出云袖，对方打出潜龙在渊" +
                                         "\n徐福到下一回合打出的劈砍不巧被闪避掉了"),
                        new EquipGuide("试试交换顺序",
                            SkillEntryDescriptor.FromName("云袖"), DeckIndex.FromField(0)),
                        new ClickBattleGuide("现在劈砍可以正好击中对手" +
                                             "\n点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    playerTemplate.TraversalCurrentSlots().Do(s =>
                    {
                        SkillEntry entry = s.Skill?.GetEntry();
                        if (entry != null)
                            RunManager.Instance.Environment.AddSkillProcedure(entry);
                    });
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        playerTemplate.TraversalCurrentSlots().Do(s =>
                        {
                            SkillEntry entry = s.Skill?.GetEntry();
                            if (entry != null)
                                RunManager.Instance.Environment.AddSkillProcedure(entry);
                        });
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学8",
                description:                        "教学8",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物8"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌8");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("多段伤害也存在应对的方法" +
                                         "\n格挡可以降低所有受到的攻击伤害，可以有效克制多段"),
                        new EquipGuide("将卡牌置入吧",
                            SkillEntryDescriptor.FromName("冰弹"), DeckIndex.FromField(1)),
                        new EquipGuide("效果强大的卡牌也伴随着需要更多灵气的消耗" +
                                       "\n正好徐福准备了专门回复灵气的卡牌",
                            SkillEntryDescriptor.FromName("吐纳"), DeckIndex.FromField(0)),
                        new ConfirmGuide("传说中，有个仙人，曾经面对多段的敌人，单依靠格挡和回血手段" +
                                         "\n最后格挡高到能够完全免除对方的伤害"),
                        new ClickBattleGuide("在一次也不攻击的情况下击败了对方，也不知道是不是真的" +
                                             "\n点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    playerTemplate.TraversalCurrentSlots().Do(s =>
                    {
                        SkillEntry entry = s.Skill?.GetEntry();
                        if (entry != null)
                            RunManager.Instance.Environment.AddSkillProcedure(entry);
                    });
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        playerTemplate.TraversalCurrentSlots().Do(s =>
                        {
                            SkillEntry entry = s.Skill?.GetEntry();
                            if (entry != null)
                                RunManager.Instance.Environment.AddSkillProcedure(entry);
                        });
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学9",
                description:                        "教学9",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物9"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌9");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("这次轮到我们受到克制了，气血还比对面低" +
                                         "\n也没到万策休矣的地步" +
                                         "\n说到可以将力量集中一点的做法" +
                                         "\n对了，试试合成"),
                        new EquipGuide("将两张牌叠起来",
                            SkillEntryDescriptor.FromName("云袖"), DeckIndex.FromField(1)),
                        new ConfirmGuide("卡牌没有合成" +
                                         "\n好像有个步骤是先将待合成的两张牌卸下至手牌区来着"),
                        new UnequipGuide("卸下一张云袖" +
                                         "\n再卸下一张云袖",
                            SkillEntryDescriptor.FromName("云袖")),
                        new MergeGuide("现在应该没问题了",
                            SkillEntryDescriptor.FromName("云袖"), SkillEntryDescriptor.FromName("云袖")),
                        new EquipGuide("将合成的云袖置入战斗区",
                            SkillEntryDescriptor.FromName("云袖"), DeckIndex.FromField(0)),
                        new ClickBattleGuide("战斗中虽然说是观察对手的招数，找出应对之策" +
                                             "\n但是在绝对的实力面前，克制关系也不过尔尔" +
                                             "\n点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("云袖"), preferredDeckIndex: DeckIndex.FromField(0));
                    RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("云袖"), preferredDeckIndex: DeckIndex.FromField(1));
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("云袖"), preferredDeckIndex: DeckIndex.FromField(0));
                        RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("云袖"), preferredDeckIndex: DeckIndex.FromField(1));
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "教学10",
                description:                        "教学10",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物10"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌10");
                    
                    BattlePanelDescriptor A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("这题我会诶，血量一样，卡牌一样" +
                                         "\n加上徐福还是先手，对手甚至还卡了费用"),
                        new EquipGuide("将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("明神"), DeckIndex.FromField(0)),
                        new EquipGuide("将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("小松"), DeckIndex.FromField(1)),
                        new ConfirmGuide("不对啊，徐福怎么可能输" +
                                         "\n是不是程序模拟错了"),
                        new ConfirmGuide("看来阵法的知识，需要解释一下" +
                                         "\n在对决中，根据战斗区的牌的成分，会形成不同的阵法" +
                                         "\n敌方和徐福都有两张木属性的牌，形成了最基础的木灵阵"),
                        new ConfirmGuide("己方阵法和敌方阵法分别在上面和下面可以看到"),
                        new ConfirmGuide("激活了阵法后，战斗时就能获得一些奖励" +
                                         "\n木灵阵就是第一张牌可以使用两次" +
                                         "\n对方虽然花了一回合聚集灵气，但是小松使用了两次"),
                        new EquipGuide("徐福已经完全懂了，调换一下卡牌顺序",
                            SkillEntryDescriptor.FromName("小松"), DeckIndex.FromField(0)),
                        new ClickBattleGuide("点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogPanelDescriptor R = new("请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(playerTemplate.GetFinalHealth());
                    
                    RunManager.Instance.Environment.ClearDeck();
                    playerTemplate.TraversalCurrentSlots().Do(s =>
                    {
                        SkillEntry entry = s.Skill?.GetEntry();
                        if (entry != null)
                            RunManager.Instance.Environment.AddSkillProcedure(entry);
                    });
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        playerTemplate.TraversalCurrentSlots().Do(s =>
                        {
                            SkillEntry entry = s.Skill?.GetEntry();
                            if (entry != null)
                                RunManager.Instance.Environment.AddSkillProcedure(entry);
                        });
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeck();
                        RunManager.Instance.Environment.Home.SetSlotCount(3);
                        RunManager.Instance.Environment.Home.SetHealthByModifyingDHealth(40);
                        return null;
                    });
                    
                    return A;
                }),

            #endregion

            #region Shop
            
            new(id:                                 "存钱",
                description:                        "存钱",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
                    DialogPanelDescriptor A = new DialogPanelDescriptor($"获得了{baseGoldReward}金钱")
                        .SetReward(Reward.FromGold(baseGoldReward));
                    return A;
                }),
            
            new(id:                                 "黑市",
                description:                        "黑市",
                ladderBound:                        new Bound(0, 8),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
                    
                    DialogPanelDescriptor A = new("你发现了一个黑市，这里有少量高境界卡牌。", "进去看一看");
                    
                    ShopPanelDescriptor B = new(RunManager.Instance.Environment.JingJie, "黑市");
                    B.SetEnter(panelDescriptor =>
                    {
                        CommodityListModel commodities = new CommodityListModel();

                        List<SkillEntry> entries = RunManager.Instance.Environment.DrawSkills(new(
                            pred: e => e.LowestJingJie - RunManager.Instance.Environment.JingJie >= 2,
                            count: 2,
                            consume: false));

                        foreach (SkillEntry e in entries)
                        {
                            int price = Mathf.RoundToInt((baseGoldReward << (e.LowestJingJie - RunManager.Instance.Environment.JingJie)) * RandomManager.Range(0.8f, 1.2f));
                            float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
                            commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie), price,
                                discount));
                        }

                        B.SetCommodities(commodities);
                    });

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),
            
            new(id:                                 "收藏家",
                description:                        "收藏家",
                ladderBound:                        new Bound(2, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你遇到了一位收藏家，他邀请你去看看他的藏品");
                    
                    ShopPanelDescriptor B = new(RunManager.Instance.Environment.JingJie, "收藏家");

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),

            new(id:                                 "以物易物",
                description:                        "以物易物",
                ladderBound:                        new Bound(2, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你收到了神秘集会的入场券，大家在集会上交换技能。");
                    
                    BarterPanelDescriptor B = new();

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),
            
            new(id:                                 "毕业季",
                description:                        "毕业季",
                ladderBound:                        new Bound(5, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("一阵噪音惊扰了你的休息，原来是灵韵宗的毕业季到了，学子们完成了学业后，纷纷将不要的技能打折卖出。");
                    
                    ShopPanelDescriptor B = new(RunManager.Instance.Environment.JingJie, "毕业季");
                    B.SetEnter(panelDescriptor =>
                    {
                        CommodityListModel commodities = new CommodityListModel();

                        List<SkillEntry> entries = RunManager.Instance.Environment.DrawSkills(new(
                            pred: e => e.LowestJingJie <= JingJie.ZhuJi,
                            count: 4,
                            consume: false));

                        foreach (SkillEntry e in entries)
                        {
                            int price = Mathf.RoundToInt((1 << e.LowestJingJie));
                            float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
                            commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie), price,
                                discount));
                        }

                        B.SetCommodities(commodities);
                    });
                    
                    A[0].SetSelect(option => B);
                    
                    return A;
                }),
            
            new(id:                                 "盲盒",
                description:                        "盲盒",
                ladderBound:                        new Bound(8, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("前面有一个盲盒商店，你想去看看这期会出什么");
                    
                    GachaPanelDescriptor B = new();

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),

            #endregion

            #region Adventure
            
            new(id:                                 "天津四",
                description:                        "天津四",
                ladderBound:                        new Bound(0, 2),
                withInPool:                         true,
                create:                             (map, room) =>
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

                    return A;
                }),

            new(id:                                 "琴仙",
                description:                        "琴仙",
                ladderBound:                        new Bound(2, 5),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你遇到了一个弹琴的人，他双目失明，衣衫褴褛，举手投足之间却让人感到大方得体，应该是一名隐士。正好前一首曲毕。向你的方向看了过来，好像知道你来了。",
                        "来一首欢快的曲子吧",
                        "来一首悲伤的曲子吧",
                        "赶路着急，没时间留下来听曲子了");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("那人哈哈大笑，然后弹了一首欢快的曲子。你回想起这一生，第一次这么有满足感，产生了一些思绪。回过神来，那人已经不见了。\n\n获得《春雨》")
                        .SetReward(new AddSkillReward("0606", RunManager.Instance.Environment.JingJie));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("那人一声叹息，然后弹了一首悲伤的曲子。你怀疑起了修仙的意义，产生了一些思绪。回过神来，那人已经不见了。\n\n获得《枯木》")
                        .SetReward(new AddSkillReward("0607", RunManager.Instance.Environment.JingJie));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("之前赶路省下的时间，正好可以用于修炼。\n\n获得一个技能")
                        .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: RunManager.Instance.Environment.JingJie)));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "赤壁赋",
                description:                        "赤壁赋",
                ladderBound:                        new Bound(5, 8),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你见到两个人在辩论。\n一人说，月亮是变化的，今天还是满月，明天就不是了。\n另一人说，月亮是不变的，上个月看是满月，今天看也还是满月。",
                        "赞同月亮是变化的",
                        "赞同月亮是不变的",
                        "变得不是月亮，而是人");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你说到：“盖将自其变者而观之，则天地曾不能以一瞬，月亮是变化的。”\n只见第一个人非常赞同你的观点，给了你一些东西。" +
                                                                        "\n\n得到《须臾》")
                        .SetReward(new AddSkillReward("0600", jingJie: RunManager.Instance.Environment.JingJie));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你说到：“自其不变者而观之，则物与我皆无尽也，月亮是不变的。”\n只见第二个人非常赞同你的观点，给了你一些东西。" +
                                                                        "\n\n得到《永远》")
                        .SetReward(new AddSkillReward("0601", jingJie: RunManager.Instance.Environment.JingJie));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你话还没说完，那两人说你是个杠精，马上留下钱买了单，换了一家茶馆去聊天。\n你发现他们还剩下了一些额外的东西。" +
                                                                        "\n\n得到4金");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),

            new(id:                                 "二子学弈",
                description:                        "二子学弈",
                ladderBound:                        new Bound(8, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你看到一个老者在教两个学童下棋，两个学童在对弈，一名学童注视棋盘，另一名学童四处张望。",
                        "注视棋盘的学童能赢",
                        "四处张望的学童能赢");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你走近了，准备称赞注视棋盘的学童，顺着他的目光看向棋盘。" +
                                                                        "\n\n你们在对弈啊，你开口道。注视棋盘的学童说，说对弈太抬举我了，我和爷爷是在请教老师。" +
                                                                        "\n\n原来四处张望的学童竟然是老师，老者却是学子。" +
                                                                        "\n\n四处张望的学童转过身来对你说，以身入局才能看到事物真正的流向，孺子可教也。给你留了点东西。\n\n得到《一心》")
                        .SetReward(new AddSkillReward("0611", jingJie: RunManager.Instance.Environment.JingJie));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你虽然相隔甚远，看不见棋盘，但是四处张望的学童神态自若，充满自信，你上去夸他。" +
                                                                        "\n\n他说到：你虽然眼神不在棋盘中，却也从场外信息判断出了我能赢，孺子可教也。给你留了点东西。\n\n得到《童趣》")
                        .SetReward(new AddSkillReward("0610", jingJie: RunManager.Instance.Environment.JingJie));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    
                    return A;
                }),

            new(id:                                 "仙人下棋",
                description:                        "仙人下棋",
                ladderBound:                        new Bound(11, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你在竹林里迷路了，走了一阵遇到两个人在下棋，其中一个人发现了你，然后继续看棋盘去了。",
                        "尝试观看两人对弈（需要一张二动牌）",
                        "请教两人路怎么走（需要一张治疗牌）");

                    CardPickerPanelDescriptor B = new CardPickerPanelDescriptor(
                        instruction:       "请提交一张二动牌",
                        bound:              new Bound(0, 2),
                        descriptor:         RunSkillDescriptor.FromSkillTypeComposite(SkillType.Swift));
                    CardPickerPanelDescriptor C = new CardPickerPanelDescriptor(
                        instruction:       "请提交一张治疗牌",
                        bound:              new Bound(0, 2),
                        descriptor:         RunSkillDescriptor.FromSkillTypeComposite(SkillType.Health));

                    DialogPanelDescriptor BWin = new("你沉下心来仔细看这盘棋，在神识飘到很远的地方之前，回想起了你曾经学过的心法，保持住了自己的神识。", "不知过了多久");
                    DialogPanelDescriptor BWin2 = new("你沉浸在自己的世界里面，两人对弈完了，你和他们互相道别。走出竹林时，你感到自己的心法又精进了一步。\n\n得到《观棋烂柯》。");
                    BWin2.SetReward(new AddSkillReward("0211", RunManager.Instance.Environment.JingJie));

                    DialogPanelDescriptor BLose = new("虽然你沉下心来想要理解棋盘中发生了什么事，只见两人下棋越来越快，一息之间，那二人已下出千百步，你想说些什么，但是身体却来不及动。", "不知过了多久");
                    DialogPanelDescriptor BLose2 = new("你醒来时，那两人已经不在了。但是莫要紧，美美睡上一觉比什么都重要。命元+2。");
                    BLose2.SetReward(Reward.FromMingYuan(2));

                    DialogPanelDescriptor CWin = new("你正向前走去，余光看到其中一人正好在一步棋点在天元。一瞬间你仿佛来到了水中，无法呼吸，你回想起了一段关于呼吸的功法，开始强迫自己吐纳，努力在这种环境下获取一些空气。", "不知过了多久");
                    DialogPanelDescriptor CWin2 = new("即使空气非常粘稠，你也可以呼吸自如。慢慢回到了正常的感觉，你悟出了一个关于吐纳的功法。");
                    CWin2.SetReward(new AddSkillReward("0608", RunManager.Instance.Environment.JingJie));

                    DialogPanelDescriptor CLose = new("你正向前走去，余光看到其中一人正好在一步棋点在天元。一瞬间你仿佛来到了水中，无法呼吸，肺部在不断哀嚎。", "不知过了多久");
                    DialogPanelDescriptor CLose2 = new("空气中的粘稠感终于消失。你赶紧大口吸气呼气，第一次感到空气是这么美好。气血上限+10。");
                    CLose2.SetReward(Reward.FromHealth(16));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    B.SetConfirmOperation(iRunSkillList =>
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

                    C.SetConfirmOperation(iRunSkillList =>
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

                    return A;
                }),

            new(id:                                 "检测仪",
                description:                        "检测仪",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A0 = new("你捡到了一个不曾见过的仪器，上面有5个按钮。你决定按下其中一个试试。",
                        "金",
                        "水",
                        "木",
                        "下一页");

                    DialogPanelDescriptor A1 = new("你捡到了一个不曾见过的仪器，上面有5个按钮。你决定按下其中一个试试。",
                        "火",
                        "土",
                        "上一页");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("仪表盘上出现了一个箭头，你顺着箭头望去，发现一本秘籍，随后仪器没电了。\n\n得到一张牌");
                    
                    A0[3].SetSelect(option => A1);
                    A1[2].SetSelect(option => A0);

                    A0[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.DrawSkillsProcedure(new(wuXing: WuXing.Jin,
                            jingJie: RunManager.Instance.Environment.JingJie));
                        return B;
                    });

                    A0[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.DrawSkillsProcedure(new(wuXing: WuXing.Shui,
                            jingJie: RunManager.Instance.Environment.JingJie));
                        return B;
                    });

                    A0[2].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.DrawSkillsProcedure(new(wuXing: WuXing.Mu,
                            jingJie: RunManager.Instance.Environment.JingJie));
                        return B;
                    });

                    A1[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.DrawSkillsProcedure(new(wuXing: WuXing.Huo,
                            jingJie: RunManager.Instance.Environment.JingJie));
                        return B;
                    });

                    A1[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.DrawSkillsProcedure(new(wuXing: WuXing.Tu,
                            jingJie: RunManager.Instance.Environment.JingJie));
                        return B;
                    });

                    return A0;
                }),

            new(id:                                 "解梦师",
                description:                        "解梦师",
                ladderBound:                        new Bound(5, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你来到一个镇上，见到了当地有名的解梦师。你请他解梦，你梦中出现了什么？",
                        "没有灵气的贫民",
                        "一些灵气的贵族",
                        "灵气充沛的灵泉");

                    ArbitraryCardPickerPanelDescriptor B = new("原来如此，你最近是否常常想着");
                    DialogPanelDescriptor C = new("这正是我现在需要的，先生真乃神医也。\n\n得到一张牌");

                    A[0].SetSelect(option =>
                    {
                        Bound manaCost = 0;

                        List<SkillEntry> entries = RunManager.Instance.Environment.DrawSkills(new(
                            pred: e => manaCost.Contains(e.GetCostDescription(RunManager.Instance.Environment.JingJie).ByType(CostDescription.CostType.Mana)),
                            jingJie: RunManager.Instance.Environment.JingJie,
                            count: 3,
                            distinct: true,
                            consume: false));
                        B.PopulateInventory(entries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie)).ToList());
                        return B;
                    });
                    A[1].SetSelect(option =>
                    {
                        Bound manaCost = new Bound(1, 10);

                        List<SkillEntry> entries = RunManager.Instance.Environment.DrawSkills(new(
                            pred: e => manaCost.Contains(e.GetCostDescription(RunManager.Instance.Environment.JingJie).ByType(CostDescription.CostType.Mana)),
                            jingJie: RunManager.Instance.Environment.JingJie,
                            count: 3,
                            distinct: true,
                            consume: false));
                        B.PopulateInventory(entries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie)).ToList());
                        return B;
                    });
                    A[2].SetSelect(option =>
                    {
                        List<SkillEntry> entries = RunManager.Instance.Environment.DrawSkills(new(
                            jingJie: RunManager.Instance.Environment.JingJie,
                            skillTypeComposite: SkillType.Mana,
                            count: 3,
                            distinct: true,
                            consume: false));
                        B.PopulateInventory(entries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie)).ToList());
                        return B;
                    });

                    B.SetConfirmOperation(skills =>
                    {
                        skills.Do(item => RunManager.Instance.Environment.AddSkillProcedure(item.Entry, item.JingJie));
                        return C;
                    });

                    return A;
                }),

            new(id:                                 "天机阁",
                description:                        "天机阁",
                ladderBound:                        new Bound(2, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你在沙漠中行走，突然眼前出来了一栋华丽的建筑，上面写着天机阁。你走入其中，前面有个牌子，请选择一张。你正在想是选择什么时，发现有十张卡牌浮在空中。");
                    ArbitraryCardPickerPanelDescriptor B = new("请从10张牌中选1张获取");
                    DialogPanelDescriptor C = new("刚一碰到那张卡牌，整个楼阁就突然消失不见，彷佛从未出现过一样。正当你不确定自己是否经历了一场幻觉时，发现留在手中的卡牌是真实的。于是你将这张卡牌收起。\n\n获得一张卡牌");

                    List<SkillEntry> entries = RunManager.Instance.Environment.DrawSkills(new(jingJie: RunManager.Instance.Environment.JingJie, count: 10, consume: false));
                    B.PopulateInventory(entries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie)).ToList());
                    B.SetConfirmOperation(skills =>
                    {
                        skills.Do(item => RunManager.Instance.Environment.AddSkillProcedure(item.Entry, item.JingJie));
                        return C;
                    });
                    
                    A[0].SetSelect(option => B);

                    return A;
                }),

            new(id:                                 "论无穷",
                description:                        "论无穷",
                ladderBound:                        new Bound(11, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你听说有奖励，于是来参加了一场考试，内容是写一篇文章，题目是“论无穷”，要如何开题呢？" +
                                                  "\n我每天跑步，只要一只能跑下去，跑的路程就是无穷的" +
                                                  "\n有一种蛇，每天吃自己的尾巴，又长出来新的蛇身，永远吃不完，此谓无穷。" +
                                                  "\n有个木桩，每天砍一半，过一万年也砍不完，这个叫做无穷。",
                        "用第一种",
                        "用第二种",
                        "用第三种");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你痛快写了800字，时间没过5分钟，已经写完了。" +
                                                  "\n\n交卷之后，一名蓝色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张二动牌", new(jingJie: RunManager.Instance.Environment.JingJie, skillTypeComposite: SkillType.Swift)));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你提笔写起来。\n\n从前有座山，山里有座庙，庙里有考试，考试来考生，考生做文章，文章道从前，" +
                                                                        "从前有座山，山里有座庙，庙里有考试，考试来考生，考生做文章，文章道从前，" +
                                                                        "从前有座山，山里有座庙。。。\n\n你的文章还没写完，考试已经结束了。" +
                                                                        "\n\n交卷之后，一名绿色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张自指牌", new(jingJie: RunManager.Instance.Environment.JingJie, skillTypeComposite: SkillType.ZiZhi)));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("考试过了一半，你只写下了一句话。又过了一半的一半，你又写下了一句话。又过了一半的一半的一半，你再写下了一句话。。。" +
                                                                        "\n\n考试结束时，你已经把所有能写字的地方都写满了。" +
                                                  "\n\n交卷之后，一名红色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张疲劳牌", new(jingJie: RunManager.Instance.Environment.JingJie, skillTypeComposite: SkillType.Exhaust)));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "分子打印机",
                description:                        "分子打印机",
                ladderBound:                        new Bound(5, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你发现了一个机器，有两个插槽。中间写着一行说明，一边放原料，一边放卡牌。",
                        "试试这个机器可以做什么",
                        "离开");

                    CardPickerPanelDescriptor B = new("请选择2张牌", bound: new Bound(0, 3));
                    DialogPanelDescriptor C = new("来路不明的机器还是不要乱碰了，这个机器还是留给有缘人吧。");
                    DialogPanelDescriptor D = new("劈里啪啦一阵响声过后，正在你担心自己的卡牌会受到什么非人的折磨的时候。机器的运转声停止了。打开后，你发现两个插槽里面的卡变成同一张了。\n\n得到两张牌");

                    B.SetConfirmOperation(iRunSkillList =>
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

                    return A;
                }),

            new(id:                                 "天界树",
                description:                        "天界树",
                ladderBound:                        new Bound(11, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你知道自己在梦境里，天界树将你拉入了他的梦境，梦境中的东西都非常真实。",
                        "吃树上的果子",
                        "尝试感悟五行相生的规律");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("久闻天界树，3000年才能开花结果，醒来之后，身上所有伤都不见了。\n\n命元+2")
                        .SetReward(Reward.FromMingYuan(2));
                    DialogPanelDescriptor C = new("你感受到了天界树的记忆。活，死，活，活，死，死，活，活，死死死死死死死。。。。。。活？" +
                                                  "所有的生命都逐渐凋零，所有的死者彷佛又有了生命。你感觉如果继续感悟下去，现在手中所有的卡牌都即将不属于自己，要继续感悟么？",
                        "停止感悟，吃树上的果子",
                        "继续感悟");
                    DialogPanelDescriptor D = new("金属遇寒，湿气冷凝成水，滴下来滋养了树苗，随即长成大树，燃烧起来，烧成了灰烬，归于尘土。" +
                                                  "到最后，你已经不知道你是树，还是树是你了。" +
                                                  "感悟了五行相生，所有五行牌都被相生的元素替换了。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    C[0].SetSelect(option => B);
                    C[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.TraversalDeckIndices().Do(deckIndex =>
                        {
                            RunSkill skill = RunManager.Instance.Environment.GetSkillAtDeckIndex(deckIndex);
                            if (skill == null)
                                return;

                            SkillEntry entry = skill.GetEntry();
                            if (!entry.WuXing.HasValue)
                                return;

                            WuXing wuXing = entry.WuXing.Value.Next;
                            JingJie jingJie = RandomManager.Range(skill.GetJingJie(), RunManager.Instance.Environment.JingJie + 1);
                            
                            RunManager.Instance.Environment.DrawSkillProcedure(
                                SkillEntryDescriptor.FromWuXingJingJie(wuXing, jingJie),
                                deckIndex);
                        });

                        return D;
                    });

                    return A;
                }),

            new(id:                                 "连抽五张",
                description:                        "连抽五张",
                ladderBound:                        new Bound(5, 8),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你近日练功，隐约感到一个瓶颈，心里略有不快。想着，如果全力一博，说不定就多一分机会窥见大道的真貌。",
                        "欲速则不达",
                        "大力出奇迹（消耗30气血上限）");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("哪怕大道难行，进一寸有一寸的欢喜。虽然进度不是很快，也并非没有收获。\n\n得到一张牌")
                        .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: RunManager.Instance.Environment.JingJie)));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("随着喷出一大口鲜血，你回过神来，原来自己还活着，感谢大道没把自己留在那边。\n\n得到五张牌")
                        .SetReward(new DrawSkillReward("获得五个技能", new(jingJie: RunManager.Instance.Environment.JingJie, count: 5)));

                    A[0].SetSelect(option => B);
                    A[1].SetCost(new CostDetails(health: 30))
                        .SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "我已膨胀",
                description:                        "我已膨胀",
                ladderBound:                        new Bound(11, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你打坐着，元神又来到了名为太虚的空间，传说达到了这个境界就会遭到天道的追杀，引来雷劫。",
                        "我已膨胀",
                        "浅尝则止");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("你在太虚之中呆了良久，感受到了天道已经在注视自己的，然后大喊了一声，你过来啊。" +
                                                                        "轰！！你觉得自己元神归窍的速度已经很快了，不知怎么的，肉体还是受到了极大损伤。气血上限变为100。" +
                                                                        "万幸，太虚境中确实对修炼的提升很大。所有牌境界提升至最高。");
                    DialogPanelDescriptor C = new DialogPanelDescriptor("在太虚的边缘进行修炼确实大有好处，你感觉自己的体魄变得更加坚韧了")
                        .SetReward(Reward.FromHealth(16));

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.SetDDHealthProcedure(-RunManager.Instance.Environment.Home.GetFinalHealth() + 100);
                        
                        RunManager.Instance.Environment.TraversalDeckIndices().Do(deckIndex =>
                        {
                            RunSkill skill = RunManager.Instance.Environment.GetSkillAtDeckIndex(deckIndex);
                            if (skill == null)
                                return;

                            skill.JingJie = skill.GetEntry().HighestJingJie;
                        });

                        return B;
                    });
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "曹操三笑",
                description:                        "曹操三笑",
                ladderBound:                        new Bound(2, 5),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("有个商人要去其他国家，听闻中间有一个险道，常常有山贼出没，托你保护他和一些货物的安全。一路上没有什么障碍，赶了几天的路之后，终于快要到目的地了。" +
                                                  "面前是一处山谷。这时候，他突然大笑起来：“哈哈哈哈哈哈哈哈。。。”",
                        "询问他何故突然大笑？",
                        "赶紧捂住他的嘴。");

                    DialogPanelDescriptor B = new("于是他解释道：”我看此等山贼都是少智无谋之辈，如果在此伏击我等，定然能让我们元气大伤。哈哈哈哈哈哈哈哈。。。“" +
                                                  "\n只见他正在笑着，然后一伙山贼就出现了。",
                        "和山贼战斗");
                    DialogPanelDescriptor C = new("他有些不悦，但也没说什么。你们平安的走完了剩下的路程。\n\n金+2");

                    map.EntityPool.TryDrawEntity(out RunEntity template, new EntityDescriptor(room.Ladder + 3));
                    BattlePanelDescriptor B1 = new(template);
                    DialogPanelDescriptor B1win = new DialogPanelDescriptor("你打过了山贼，商人对你十分感激。\n\n金+6")
                        .SetReward(Reward.FromGold(6));
                    DialogPanelDescriptor B1lose = new("你没打过山贼，货物被抢走了。索性没有人受伤。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    B[0].SetSelect(option => B1);
                    B1.SetWinOperation(() => B1win);
                    B1.SetLoseOperation(() => B1lose);

                    return A;
                }),

            new(id:                                 "神灯精灵",
                description:                        "神灯精灵",
                ladderBound:                        new Bound(8, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你捡到了一盏神灯里面跳出来了一个精灵，说可以实现你一个愿望",
                        "健康的体魄",
                        "钱币的富裕",
                        "这个愿望不被实现");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("实现了，精灵留下了这句话带着神灯飞走了。你感觉身强体壮\n\n气血上限+10")
                        .SetReward(Reward.FromHealth(8));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("实现了，精灵留下了这句话带着神灯飞走了。你包里突然出来了很多金币\n\n金+100")
                        .SetReward(Reward.FromGold(8));
                    DialogPanelDescriptor D = new("实现了。。额，实现不了。。哦，实现了。。。啊，实现不了。精灵说你比许愿再来十个愿望的人还会捣乱，召唤出来一个怪物，要来和你打一架。");

                    map.EntityPool.TryDrawEntity(out RunEntity template, new EntityDescriptor(room.Ladder + 3));
                    BattlePanelDescriptor E = new(template);
                    DialogPanelDescriptor EWin = new DialogPanelDescriptor("哎，不就是都想要么？拿去拿去，好好说话我也不会不给的啊。\n\n气血上限+10，金+100")
                        .SetReward(new ResourceReward(gold: 8, health: 8));
                    DialogPanelDescriptor ELose = new("哼，现在神灯精灵不好做了，就是因为经常碰见你这种人。下次别再让我遇见了。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);
                    D[0].SetSelect(option => E);
                    E.SetWinOperation(() => EWin);
                    E.SetLoseOperation(() => ELose);

                    return A;
                }),

            new(id:                                 "山木",
                description:                        "山木",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int trial = 0;
                    int rage = RandomManager.Range(0, 7);

                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
                    
                    DialogPanelDescriptor A = new("一位老者做在石头上向周围人传教，虚己以游世，其孰能害之。说的是，只要你不把别人当个人，别人就不会引起你生气。你突然想逗他一下。", "朝他作鬼脸", "狠狠戳他一下");

                    DialogPanelDescriptor B1 = new("他看起来有点生气了。", "朝他作鬼脸", "狠狠戳他一下");
                    DialogPanelDescriptor B2 = new("他看起来非常生气了。", "朝他作鬼脸", "狠狠戳他一下");

                    DialogPanelDescriptor D = new DialogPanelDescriptor($"你上去为自已的恶作剧道歉，他说还好，不会放在心上，这位学子应该学到了什么。\n\n获得{baseGoldReward}金")
                        .SetReward(Reward.FromGold(baseGoldReward));
                    DialogPanelDescriptor E = new DialogPanelDescriptor($"你上去为自已的恶作剧道歉，他喘了一口气，随即嘻笑开颜向大家解释道，这就是我刚才说的，不要随便生气。\n\n获得{baseGoldReward * 2}金")
                        .SetReward(Reward.FromGold(baseGoldReward * 2));
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

                    return A;
                }),

            new(id:                                 "丢尺子",
                description:                        "丢尺子",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
                    
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

                    DialogPanelDescriptor D = new DialogPanelDescriptor($"你问了管家情况，发现都是些鸡毛蒜皮的小事。并不需要什么维护，就收了车马费。\n\n金+{baseGoldReward}")
                        .SetReward(Reward.FromGold(baseGoldReward));

                    DialogPanelDescriptor E = new DialogPanelDescriptor($"这阵子，管家没有再来打扰你了，应该是去打扰别人了。心情变好了一点。\n\n气血上限+{baseGoldReward}")
                        .SetReward(Reward.FromHealth(baseGoldReward));

                    A[0].SetSelect(option => B);
                    B[0].SetSelect(option => C);
                    C[0].SetSelect(option => D);
                    A[1].SetSelect(option => E);
                    B[1].SetSelect(option => E);
                    C[1].SetSelect(option => E);

                    return A;
                }),

            new(id:                                 "酿造仙岛玉液酒",
                description:                        "酿造仙岛玉液酒",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    bool mixWater = false;
                    bool yellNoLie = false;
                    bool expert = RandomManager.value < 0.5f;
                    
                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];

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

                    DialogPanelDescriptor D = new("那人将钱交予你，把酒拿走了。劳动真光荣。", $"获得{baseGoldReward}金");

                    DialogPanelDescriptor[] EndingTable = new DialogPanelDescriptor[]
                    {
                        // mixWater, yellNoLie, expert
                        /* 0b000 */ new("算了，我还是不买了。\n\n眼看市集就快结束了，你只好平价将酒出手了。\n\n不赚不赔"),
                        /* 0b001 */ new DialogPanelDescriptor($"你今天不卖给我，我就不走了。那人出高价来买你的酒，你含泪把钱收下了。\n\n获得{2 * baseGoldReward}金").SetReward(Reward.FromGold(2 * baseGoldReward)),
                        /* ob010 */ new DialogPanelDescriptor($"那人将钱交予你，把酒拿走了。劳动真光荣。\n\n获得{baseGoldReward}金").SetReward(Reward.FromGold(baseGoldReward)),
                        /* ob011 */ new DialogPanelDescriptor($"那人将钱交予你，把酒拿走了。劳动真光荣。\n\n获得{baseGoldReward}金").SetReward(Reward.FromGold(baseGoldReward)),
                        /* 0b100 */ new("算了，我还是不买了。\n\n眼看市集就快结束了，你只好平价将酒出手了。\n\n不赚不赔"),
                        /* 0b101 */ new DialogPanelDescriptor($"哎，看你也不容易。那人虽然看出了你的酒兑了水，但还是有些良心，于是以正常价格买走了酒。\n\n获得{baseGoldReward}金").SetReward(Reward.FromGold(baseGoldReward)),
                        /* 0b110 */ new DialogPanelDescriptor($"那人将钱交予你，把酒拿走了。哇，小赚了一笔。\n\n获得{2 * baseGoldReward}金").SetReward(Reward.FromGold(2 * baseGoldReward)),
                        /* 0b111 */ new DialogPanelDescriptor($"你个奸商。那人抓住你，说要去官府。你只好摊也不顾了，赶紧溜了。\n\n失去{baseGoldReward}金").SetReward(Reward.FromGold(-baseGoldReward)),
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

                    return A;
                }),
            
            new(id:                                 "夏虫语冰",
                description:                        "夏虫语冰",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
                    DialogPanelDescriptor A = new("你要过一个桥，桥上站了一人，问你，什么时候河会变得可以行走。你说在冬季的时候。他说你是胡说八道：“一年只有三个季节，春夏秋，哪里来的冬季？”",
                        "赞同他，说一年只有三个季节",
                        "向他解释，说一年有四个季节");

                    DialogPanelDescriptor B = new DialogPanelDescriptor($"那人让你过去了，你感觉自己避免了一件麻烦事，心情大为畅快。\n\n气血上限+{baseGoldReward}")
                        .SetReward(Reward.FromHealth(baseGoldReward));
                    DialogPanelDescriptor C = new DialogPanelDescriptor($"一个月过去了，想过桥的人看到你们俩堵在桥中间，劝也劝不动，都想其他法子过桥了。那人的面容有所变化，但是嘴还是硬的。" +
                                                                        "\n两个月时间逐渐过去，周围的人已经不来这个桥了。那人竟然以肉眼可见的速度，每天变老，但是还是一口咬定冬季不存在。" +
                                                                        "\n到了第三个月，你们旁边已经修好了一个新的桥，从新桥上过去的人都已异样的眼光看着你们。那人已经连站立都感到困难了。" +
                                                                        "\n就快要到冬天了，到时候就能证明冬季了。你又一次像那人看去。那人已经老的站立都困难了。你终于发现那人是夏虫所化。一生始于春而终于秋。" +
                                                                        "你刚想松口。那人先于你出口说，你赢了，你可以过桥了，然后坐在一颗树下，永远的合上了眼。" +
                                                                        "\n你在此地过了三个月，虽然修为上没有太大的精进，但是休息了这么长时间，所有伤势都已消失不见。\n\n命元+2")
                        .SetReward(Reward.FromMingYuan(2));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "守株待兔",
                description:                        "守株待兔",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
                    DialogPanelDescriptor A = new("你见到一个人坐在树桩旁，问他在干什么，他说有兔子会撞上这个树桩，自己在等兔子撞死。",
                        "和他一起等待兔子",
                        "买一只兔子放在树桩前，然后告诉他兔子来了");

                    DialogPanelDescriptor B = new DialogPanelDescriptor($"你睡了过去，醒来时，那人说真的来了只兔子，要和你一起享用。休息好吃好后，你感觉自己状态变好了。气血+{baseGoldReward}")
                        .SetReward(Reward.FromHealth(baseGoldReward));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你叫醒了那人，那人惊叹道：原来算命的说的是真的！你解释到是你们的运气好。" +
                                                                        "\n\n他激动地说道，之前有一个算命的人和他说，这里从来没有兔子也等不来兔子，但会有贵人经过，贵人会安排好兔子后，假装兔子是撞死的。" +
                                                                        "\n\n他当时还不信，原来真的可以等来兔子，哦不，贵人。" +
                                                                        $"\n\n然后邀请你到府上做客，向你问了一些问题。还赠送了你一些钱。金+{baseGoldReward}")
                        .SetReward(Reward.FromGold(baseGoldReward));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "鸡肉面",
                description:                        "鸡肉面",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你到了餐馆，叫了碗面。只见厨师拿了一副鸡的画卷，剪开下了锅。你正疑惑他在干什么的时候，他从锅里盛出了一碗鸡肉面给你。",
                        "认为这是真的鸡肉面，吃下去",
                        "拆穿他，说这碗面不过是幻术");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("一开始还有些怀疑，然后发现就是真的面。于是美美的吃了一顿。命元+2")
                        .SetReward(Reward.FromMingYuan(2));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("一阵烟雾过后。你面前掉落了一幅画，上面赫然画着刚才的餐馆。你对之前的招式又有了新的感悟。获得一张牌")
                        .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: RunManager.Instance.Environment.JingJie)));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),
            
            #endregion

            #region Reserved

            new(id:                                 "忘忧堂",
                description:                        "忘忧堂",
                ladderBound:                        new Bound(5, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你来到了忘忧堂，听说这里的服务是将不想再见到的卡牌交给他们。忘忧堂会为您斩断与此牌的因果。",
                        "走进去看一看",
                        "离开");

                    CardPickerPanelDescriptor B = new("请选择0到5张牌", bound: new Bound(0, 6));
                    DialogPanelDescriptor C = new("果然还是难以割舍心爱的卡牌。");
                    DialogPanelDescriptor D = new("你感到身上轻了一些。");

                    B.SetConfirmOperation(iRunSkillList =>
                    {
                        int count = iRunSkillList.Count;
                        if (count == 0)
                            return C;

                        foreach (object iSkill in iRunSkillList)
                        {
                            if (iSkill is RunSkill skill)
                            {
                                RunManager.Instance.Environment.SkillPool.Depopulate(pred: e => e == skill.GetEntry());
                                RunManager.Instance.Environment.Hand.Remove(skill);
                            }
                            else if (iSkill is SkillSlot slot)
                            {
                                if (slot.Skill != null)
                                {
                                    RunManager.Instance.Environment.SkillPool.Depopulate(pred: e => e == slot.Skill.GetEntry());
                                    slot.Skill = null;
                                }
                            }
                        }
                        
                        return D;
                    });

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),
            
            new(id:                                 "人间世",
                description:                        "人间世",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你追上了他，看出他事业心很重，于是对他说：" +
                                                  "\n\n成名要趁早，我看你将来肯定是做宰相的料。" +
                                                  "\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。即使如此，你还是要追求功名么？",
                        "用第一个想法",
                        "用第二个想法");

                    DialogPanelDescriptor B1 = new("感谢你这么夸我，但是现在我也没有钱给你。", "时间一下过了60年");
                    DialogPanelDescriptor C1 = new("先生谬论不可再提，你看那胡人会因为我们不锻造兵器，充实军备而不来侵略我们么？", "时间一下过了60年");

                    DialogPanelDescriptor B2 = new DialogPanelDescriptor("当年的少年已经成为了宰相。见到了你，发现你的容貌60年没有发生变化，察觉你是仙人，于是说道，感谢仙人提拔。叫人给了你收藏的宝物。\n\n得到1机关牌");
                    DialogPanelDescriptor C2 = new DialogPanelDescriptor("当年的少年已经成为了宰相。见到了你，完全没有印象，只道是某个江湖中人来攀亲道故，于是叫下人给了你点盘缠打发了。\n\n得到50金")
                        .SetReward(Reward.FromGold(50));

                    DialogPanelDescriptor D = new("你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你正向追上他说点什么，却被一颗小石子绊倒，起身已经不见那人踪影。于是道：“罢了罢了，缘分未到。”", "时间一下过了60年");
                    DialogPanelDescriptor D2 = new("你又见到了当年的少年。现在他已经成为了宰相。你想着对他说些什么：" +
                                                   "\n\n成名要趁早，宰相一生过得荣华富贵。。。" +
                                                   "\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。哪怕功名已成恐怕也是路途险阻。",
                        "用第一个想法",
                        "用第二个想法");

                    DialogPanelDescriptor E = new DialogPanelDescriptor("只见你话还没说完，宰相就摆手示意你离开。叫下人给了你点盘缠将你打发了。\n\n得到50金")
                        .SetReward(Reward.FromGold(50));
                    DialogPanelDescriptor F = new DialogPanelDescriptor("宰相回复到，先生说的属实，若是我早点知道了这些道理，也不至于一生过的如此跌宕起伏。叫人给了你收藏的宝物。\n\n得到1机关牌");

                    A[0].SetSelect(option => B1);
                    A[1].SetSelect(option => C1);
                    B1[0].SetSelect(option => B2);
                    C1[0].SetSelect(option => C2);
                    D[0].SetSelect(option => D2);
                    D2[0].SetSelect(option => E);
                    D2[1].SetSelect(option => F);

                    bool isCatch = RandomManager.value < 0.5;
                    return isCatch ? A : D;
                }),

            new(id:                                 "照相机",
                description:                        "照相机",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
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
                    DialogPanelDescriptor C = new DialogPanelDescriptor("你把好的相片留给了机关师，这样他日后看到相片的时候，就会感到这段回忆多一分美好。这样想到，你的心情变好了。气血上限+10")
                        .SetReward(Reward.FromHealth(10));
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你对那人说：”相必先生也知道，美好的时光总是短暂的。这个机关，可以将美好的时光记录下来，之后就可以看着照片反复回忆。" +
                                                                        "但真的如此做的话，处于桃林中的我们也会因为知道相片可以反复回忆，反倒不去珍惜此时此刻的美景了。这难道不是本末倒置了么？“" +
                                                                        "\n\n那人稍微惊讶于你的说法。然后感叹道：“先生教训的在理。不应以人生长而感到美好，也不应以短而感到苦恼。这个机关已经于我无用，这便赠与先生吧。”" +
                                                                        "你收下了这个机关，但是并不会维护。与其看着它坏掉，不如将其中有用的零件取出。\n\n得到1机关");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "矛与盾",
                description:                        "矛与盾",
                ladderBound:                        new Bound(0, 2),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你看见一个人在推销他的矛和盾，说是这矛可以打碎所有的盾，这盾可以抵挡住所有的矛。周围人起哄说要他拿他的矛戳他的盾。",
                        "你感觉矛厉害",
                        "你感觉盾厉害",
                        "离开，不参与热闹");
                    
                    Puzzle bPuzzle = new(
                        description: "尝试移除对方的护甲",
                        condition: "第六回合时，目标护甲小等于0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                        }),
                        kernel: new StageKernel(async (env, turn, whosTurn, forced) =>
                        {
                            CommitDetails d = new CommitDetails(env.Entities[whosTurn]);

                            await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (forced)
                            {
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (turn < 6)
                                    return 0;

                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }

                            await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

                            if (d.Flag == 0)
                                return d.Flag;

                            env.Result.Flag = d.Flag;
                            env.Result.HomeLeftHp = env.Entities[0].Hp;
                            env.Result.AwayLeftHp = env.Entities[1].Hp;
                            env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
                            return d.Flag;
                        })
                    );
        
                    PuzzlePanelDescriptor B = new(bPuzzle);
                    DialogPanelDescriptor BPass = new DialogPanelDescriptor("获得一张攻击牌。"); // TODO
                    
                    Puzzle cPuzzle = new(
                        description: "尝试保持护甲",
                        condition: "第六回合时，护甲大于0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                        }),
                        kernel: new StageKernel(async (env, turn, whosTurn, forced) =>
                        {
                            CommitDetails d = new CommitDetails(env.Entities[whosTurn]);

                            await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (forced)
                            {
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (turn < 6)
                                    return 0;

                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }

                            await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

                            if (d.Flag == 0)
                                return d.Flag;

                            env.Result.Flag = d.Flag;
                            env.Result.HomeLeftHp = env.Entities[0].Hp;
                            env.Result.AwayLeftHp = env.Entities[1].Hp;
                            env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
                            return d.Flag;
                        })
                    );
        
                    PuzzlePanelDescriptor C = new(cPuzzle);
                    DialogPanelDescriptor CPass = new DialogPanelDescriptor("获得一张护甲牌。"); // TODO

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    B.SetOperation(s =>
                    {
                        if (s.Flag == 1)
                        {
                            return BPass;
                        }

                        return A;
                    });
                    C.SetOperation(s =>
                    {
                        if (s.Flag == 1)
                        {
                            return CPass;
                        }

                        return A;
                    });

                    return A;
                }),

            new(id:                                 "郑人买履",
                description:                        "郑人买履",
                ladderBound:                        new Bound(2, 5),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你打算买双鞋子。来到集市，看到一名男子也打算买鞋子，但是忘了带量好的尺码了，商家说现在量一下他的脚不就有尺码了么。你发现你也忘带尺码了。",
                        "回家去取尺码",
                        "现场重新量尺码");

                    DialogPanelDescriptor B = new("回家取了尺码后，你再次来到集市，发现集市已经结束了，没能买到鞋子。" +
                                                  "\n\n又发现那名男子，他也注意到了你，他将量好的尺码挂在一名关门的商户门前后，和你说，你也是来找修补匠打造机关鞋的么？" +
                                                  "好眼光啊，可惜上午来发现今天不开门。于是将量好的尺码挂着这里预定一个鞋子。你也学着他将尺码挂在了门前。" +
                                                  "\n\n一小段时间后，得到《飞鞋》");
                    DialogPanelDescriptor C = new("你本意只想买个鞋子，却承载不住商家的热情，被推销了一个东西。得到《目镜》");
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    
                    return A;
                }),

            new(id:                                 "鬼兵",
                description:                        "鬼兵",
                ladderBound:                        new Bound(2, 5),
                withInPool:                         false,
                create:                             (map, room) =>
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
                    BPick.SetConfirmOperation(iRunSkillList =>
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

                    return A;
                }),

            new(id:                                 "刻舟求剑",
                description:                        "刻舟求剑",
                ladderBound:                        new Bound(5, 8),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("坐船渡江途中，有个人不小心，随身携带的剑匣掉到江里了。然后马上打开随身的备忘录写了点东西。",
                        "和他说，快下去捞啊",
                        "下水帮他将宝剑捞上来");

                    DialogPanelDescriptor B = new DialogPanelDescriptor("他笑道，这条船，每天都这个航线，这个点渡江，我今天行装不方便下水，我已经记下了此时时刻。明天经过这里的时候在捞不迟。");
                        // .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: map.JingJie)));
                    DialogPanelDescriptor C = new DialogPanelDescriptor("他结果剑匣，惆怅的看着剑匣。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "物质还原仪",
                description:                        "物质还原仪",
                ladderBound:                        new Bound(8, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你发现了一个机器，有一个插槽。旁边有一个剪刀的按钮。",
                        "试试这个机器可以做什么",
                        "离开");

                    // CardPickerPanelDescriptor B = new("请选择1张牌", range: new Range(0, 3));
                    // DialogPanelDescriptor C = new("来路不明的机器还是不要乱碰了，这个机器还是留给有缘人吧。");
                    // DialogPanelDescriptor D = new("劈里啪啦一阵响声过后，正在你担心自己的卡牌会受到什么非人的折磨的时候。机器的运转声停止了。打开后，你发现原先的卡牌变成了两张较低境界的卡牌了。\n\n得到两张牌");
                    //
                    // B.SetConfirmOperation(iRunSkillList =>
                    // {
                    //     int count = iRunSkillList.Count;
                    //     if (count == 0 || count == 1)
                    //         return C;
                    //
                    //     RunSkill copyingSkill = null;
                    //     object copying = iRunSkillList[RandomManager.Range(0, count)];
                    //     if (copying is RunSkill runSkill)
                    //     {
                    //         copyingSkill = runSkill;
                    //     }
                    //     else if (copying is SkillSlot slot)
                    //     {
                    //         RunSkill rSkill = slot.Skill as RunSkill;
                    //         Assert.IsTrue(rSkill != null);
                    //         copyingSkill = rSkill;
                    //     }
                    //
                    //     foreach (object iSkill in iRunSkillList)
                    //     {
                    //         if (iSkill is RunSkill skill)
                    //         {
                    //             RunManager.Instance.Environment.Hand.Remove(skill);
                    //         }
                    //         else if (iSkill is SkillSlot slot)
                    //         {
                    //             slot.Skill = null;
                    //         }
                    //     }
                    //
                    //     count.Do(i => RunManager.Instance.Environment.Hand.Add(copyingSkill));
                    //     return D;
                    // });
                    //
                    // A[0].SetSelect(option => B);
                    // A[1].SetSelect(option => C);

                    return A;
                }),
            
            new(id:                                 "悟道",
                description:                        "悟道",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
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
                            RunManager.Instance.Environment.DrawSkillsProcedure(new(wuXing: options[index],
                                jingJie: RunManager.Instance.Environment.JingJie));
                        }
                        return null;
                    };

                    return A;
                }),

            new(id:                                 "愿望单",
                description:                        "愿望单",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
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
                    return A;
                }),

            #endregion

            #region Series

            new(id:                                 "后羿1",
                description:                        "后羿1",
                ladderBound:                        new Bound(0, 5),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你看到了一个少年在幸苦的练习射箭，但是进度缓慢。你发现是因为少年天生视力不好。",
                        "给少年展示技术",
                        "赠与少年一本秘籍",
                        "告诉少年实话");
                    Puzzle puzzle = new(
                        description: "尝试帮助少年击中目标",
                        condition: "目标受到伤害",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                        }),
                        kernel: new StageKernel(async (env, turn, whosTurn, forced) =>
                        {
                            CommitDetails d = new CommitDetails(env.Entities[whosTurn]);

                            await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (forced)
                            {
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (turn < 6)
                                    return 0;

                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }

                            await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

                            if (d.Flag == 0)
                                return d.Flag;

                            env.Result.Flag = d.Flag;
                            env.Result.HomeLeftHp = env.Entities[0].Hp;
                            env.Result.AwayLeftHp = env.Entities[1].Hp;
                            env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
                            return d.Flag;
                        })
                    );
        
                    PuzzlePanelDescriptor B = new(puzzle);
                    DialogPanelDescriptor BPass = new DialogPanelDescriptor("少年将你的招式记在了心里，又开始了自顾自的练习。", "继续上路");
                    
                    CardPickerPanelDescriptor C = new CardPickerPanelDescriptor(
                        instruction:       "请提交一张牌",
                        bound:              new Bound(0, 2));
        
                    DialogPanelDescriptor CWin = new("少年感谢你赠与的秘籍，已经准备好开始练习了。", "继续上路");

                    DialogPanelDescriptor D =
                        new("少年知道了自己永远也练不成一等一的弓术，哭着跑回家了。他父母不愿看着少年做无用功，却也狠不下来心打破少年的幻想。" +
                            "感谢你告诉了少年实话。将少年的弓赠与了你。" +
                            "\n你将弓卖掉换了些钱，继续上路了。", "获得2金");
                    D.SetReward(Reward.FromGold(2));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);
                    B.SetOperation(s =>
                    {
                        if (s.Flag == 1)
                        {
                            RunManager.Instance.Environment.Map.InsertRoom("后羿2");
                            return BPass;
                        }

                        return A;
                    });
                    C.SetConfirmOperation(iRunSkillList =>
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

                        RunManager.Instance.Environment.Map.InsertRoom("后羿2");
                        return CWin;
                    });

                    return A;
                }),

            new(id:                                 "后羿2",
                description:                        "后羿2",
                ladderBound:                        new Bound(5, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你之前见过的视力不好的少年，现在视力越来越弱了。甚至没能发现你的到来。",
                        "给少年展示更厉害的技术",
                        "安慰少年（需要4金）",
                        "告诉少年实话");
                    Puzzle puzzle = new(
                        description: "尝试帮助少年击中目标",
                        condition: "目标受到伤害",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                        }),
                        kernel: new StageKernel(async (env, turn, whosTurn, forced) =>
                        {
                            CommitDetails d = new CommitDetails(env.Entities[whosTurn]);

                            await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (forced)
                            {
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (turn < 6)
                                    return 0;

                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }

                            await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

                            if (d.Flag == 0)
                                return d.Flag;

                            env.Result.Flag = d.Flag;
                            env.Result.HomeLeftHp = env.Entities[0].Hp;
                            env.Result.AwayLeftHp = env.Entities[1].Hp;
                            env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
                            return d.Flag;
                        })
                    );
        
                    PuzzlePanelDescriptor B = new(puzzle);
                    DialogPanelDescriptor BPass = new DialogPanelDescriptor("少年将你的招式记在了心里，又投入了奋不顾身的练习。", "继续上路");
                    
                    DialogPanelDescriptor C = new("你和少年讲起了故事。传闻有和非人之物战斗的剑士，在对决的生死关头，领悟了可以看清周围一切的招式，打败了对手。");
                    DialogPanelDescriptor C2 = new("少年觉得你的故事逊爆了，如果去当说书人指定吃不上饭的那种。聊了过天后，表示自己要继续训练了。你看到少年过的清苦，于是留下了一些盘缠，失去4金。", "继续上路");

                    DialogPanelDescriptor D =
                        new("你还在想怎么和少年开口时。少年突然开口道：表示自己其实知道大家一直在迁就自己，自己也应该承担起一些责任了。\n\n少年将多年的心得交给了你。获得1个技能。" +
                            "\n\n临走时，你注意到了不对劲，少年是怎么察觉了你的到来的。转头发现少年已经消失了。", "继续上路");
                    D.SetReward(Reward.FromGold(50));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[1].SetCost(new CostDetails(gold: 4));
                    A[2].SetSelect(option => D);
                    B.SetOperation(s =>
                    {
                        if (s.Flag == 1)
                        {
                            RunManager.Instance.Environment.Map.InsertRoom("后羿3");
                            return BPass;
                        }

                        return A;
                    });
                    C[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.Map.InsertRoom("后羿3");
                        return C2;
                    });

                    return A;
                }),
            
            new(id:                                 "后羿3",
                description:                        "后羿3",
                ladderBound:                        new Bound(11, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new DialogPanelDescriptor("你来到了很久之前来过的竹林，当时的练箭少年已经不在，你发现了他留给你的一本秘籍。\n\n得到《射落金乌》。")
                        .SetReward(new AddSkillReward("0605", JingJie.YuanYing));
            
                    return A;
                }),

            new(id:                                 "神农氏1",
                description:                        "神农氏1",
                ladderBound:                        new Bound(0, 5),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你看见一个少年向你走来，一手拿着一个神采奕奕的仙草，另一手拿着一个可疑的蘑菇，向你说道，挑一个吃了吧。",
                        "给他展示运气抵御毒素的法门",
                        "一口抢过来蘑菇",
                        "选择仙草");
                    A[1].SetCost(new CostDetails(mingYuan: 1));
            
                    Puzzle puzzle = new(
                        description: "只要用法术治疗，就可以抵抗毒素产生的内伤，尝试帮助少年撑过6回合",
                        condition: "剩余血量 大于 0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                        }),
                        kernel: new StageKernel(async (env, turn, whosTurn, forced) =>
                        {
                            CommitDetails d = new CommitDetails(env.Entities[whosTurn]);
            
                            await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);
            
                            if (forced)
                            {
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;
            
                                if (turn < 6)
                                    return 0;
            
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
            
                            await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
            
                            if (d.Flag == 0)
                                return d.Flag;
            
                            env.Result.Flag = d.Flag;
                            env.Result.HomeLeftHp = env.Entities[0].Hp;
                            env.Result.AwayLeftHp = env.Entities[1].Hp;
                            env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
                            return d.Flag;
                        })
                    );
                    
                    PuzzlePanelDescriptor B = new(puzzle);
                    DialogPanelDescriptor BPass = new DialogPanelDescriptor("少年吃了可疑的蘑菇，幸好可以依靠你的功法抵挡毒性。\n\n于是你吃了仙草感觉身上的伤势轻了一些。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));
                    DialogPanelDescriptor C = new("你吃了可疑的蘑菇，感觉头痛欲裂\n\n命元-1");
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你吃了仙草感觉身上的伤势轻了一些。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));
                    
                    B.SetOperation(s =>
                    {
                        if (s.Flag == 1)
                        {
                            RunManager.Instance.Environment.Map.InsertRoom("神农氏2");
                            return BPass;
                        }
                        return A;
                    });
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.Map.InsertRoom("神农氏2");
                        return C;
                    });
                    A[2].SetSelect(option => D);
                    return A;
                }),
            
            new(id:                                 "神农氏2",
                description:                        "神农氏2",
                ladderBound:                        new Bound(5, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("你又见到了那个少年，他又笑嘻嘻的向你走来，又是一手拿着一个容光满面的仙草，另一手拿着一个可疑的蘑菇，向你说道，这次你想吃哪个？",
                        "给他展示运气抵御毒素的法门",
                        "你个外行，学别人采什么药，离这个蘑菇远一点",
                        "这次我就选择仙草吧");
                    A[1].SetCost(new CostDetails(mingYuan: 1));
            
                    Puzzle puzzle = new(
                        description: "只要用法术治疗，就可以抵抗毒素产生的内伤，尝试帮助少年撑过6回合",
                        condition: "剩余血量 大于 0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                            RunSkill.FromEntry("0609"),
                        }),
                        kernel: new StageKernel(async (env, turn, whosTurn, forced) =>
                        {
                            CommitDetails d = new CommitDetails(env.Entities[whosTurn]);
            
                            await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);
            
                            if (forced)
                            {
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;
            
                                if (turn < 6)
                                    return 0;
            
                                d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                            }
            
                            await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
            
                            if (d.Flag == 0)
                                return d.Flag;
            
                            env.Result.Flag = d.Flag;
                            env.Result.HomeLeftHp = env.Entities[0].Hp;
                            env.Result.AwayLeftHp = env.Entities[1].Hp;
                            env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
                            return d.Flag;
                        })
                    );
                    
                    PuzzlePanelDescriptor B = new(puzzle);
                    DialogPanelDescriptor BPass = new DialogPanelDescriptor("少年吃了可疑的蘑菇，幸好可以依靠你的功法抵挡毒性。\n\n于是你吃了仙草感觉身上的伤势轻了一些。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));
                    DialogPanelDescriptor C = new("你又一次吃下了可疑的蘑菇，感觉五脏俱焚\n\n命元-1");
                    DialogPanelDescriptor D = new DialogPanelDescriptor("你吃了仙草感觉治愈了你多年的旧伤，继续上路了。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));
                    
                    B.SetOperation(s =>
                    {
                        if (s.Flag == 1)
                        {
                            RunManager.Instance.Environment.Map.InsertRoom("神农氏3");
                            return BPass;
                        }
                        return A;
                    });
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.Map.InsertRoom("神农氏3");
                        return C;
                    });
                    A[2].SetSelect(option => D);
                    return A;
                }),
            
            new(id:                                 "神农氏3",
                description:                        "神农氏3",
                ladderBound:                        new Bound(11, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new DialogPanelDescriptor("故地重游，故人已经不在，你来到了他的墓前面，上面写着：神农氏之墓，他的后人说他给你留下来了一些东西。\n\n得到《百草集》。")
                        .SetReward(new AddSkillReward("0602", JingJie.YuanYing));
            
                    return A;
                }),
            
            #endregion

            #region ForTesting

            new(id:                                 "快速结算",
                description:                        "快速结算",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("用于测试Run结算",
                        "胜利结算",
                        "失败结算");

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunResultState.Victory);
                        return null;
                    });

                    A[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunResultState.Defeat);
                        return null;
                    });

                    return A;
                }),

            new(id:                                 "循环",
                description:                        "循环",
                ladderBound:                        new Bound(0, 5),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogPanelDescriptor A = new("循环测试",
                        "继续循环",
                        "退出循环");

                    A[0].SetSelect(option => A);
                    A[1].SetSelect(option => null);
                    return A;
                }),
            
            new(id:                                 "发现一张牌",
                description:                        "发现一张牌",
                ladderBound:                        new Bound(0, 15),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DiscoverSkillPanelDescriptor A = new("灵感");
                    A.SetDetailedText($"请选择一张卡作为奖励");

                    return A;
                }),

            #endregion
        });
    }

    public virtual RoomEntry DefaultEntry() => this["不存在的事件"];
}
