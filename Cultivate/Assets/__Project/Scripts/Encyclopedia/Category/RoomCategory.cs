
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
            #region 01_Core
            
            new(id:                                 "Room0001",
                name:                               "不存在的事件",
                description:                        "不存在的事件",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "不存在的事件",
                        detailedText: "不存在的事件");
                    return A;
                }),
            
            new(id:                                 "Room0002",
                name:                               "战斗",
                description:                        "战斗",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    BattleRoomDefinition roomDefinition = room.GetDescriptor() as BattleRoomDefinition;
                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);
                    int goldValue = Mathf.RoundToInt(baseGoldReward * RandomManager.Range(0.9f, 1.1f));

                    RunEntity enemy = room.GetPredrewRunEntity();
                    
                    BattleCell A = new(enemy);

                    DiscoverSkillCell B = DiscoverSkillCell.FromDefault(room.Ladder);

                    bool shouldUpdateSlotCount = roomDefinition.ShouldUpdateSlotCount;
                    
                    bool isHuaShenFinalBoss = roomDefinition._isBoss && RunManager.Instance.Environment.JingJie == JingJie.HuaShen;
                    if (isHuaShenFinalBoss)
                        RunManager.Instance.Environment.HuaShenBossEntity = enemy.GetEntry();

                    bool allowFanXu = RunManager.Instance.Environment.GetRunConfig().DifficultyProfile.GetEntry().AllowFanXuBoss;

                    bool willCommit = roomDefinition._isBoss &&
                                      RunManager.Instance.Environment.IsFinalJingJie() &&
                                      !(isHuaShenFinalBoss && allowFanXu);
                    if (willCommit)
                    {
                        A.SetWinOperation(() =>
                        {
                            RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Victorious);
                            return null;
                        });

                        A.SetLoseOperation(() =>
                        {
                            RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Defeated);
                            return null;
                        });
                    }
                    else
                    {
                        A.SetWinOperation(() =>
                        {
                            B.SetTitleText("胜利");
                            B.SetDescriptionText($"获得了<style=\"Gold\">{goldValue}金钱</style>\n请选择<style=\"Red\">一张卡牌</style>作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(roomDefinition._slotCountAfter);
                            return B;
                        });

                        A.SetLoseOperation(() =>
                        {
                            RunManager.Instance.Environment.SetDMingYuanProcedure(-2);
                            
                            B.SetTitleText("惜败");
                            B.SetDescriptionText($"<style=\"Gray\">你没能击败对手，损失了一些命元</style>" +
                                                 $"\n但获得了<style=\"Gold\">{goldValue}金钱</style>，以及选择<style=\"Red\">一张卡牌</style>作为奖励");
                            if (shouldUpdateSlotCount)
                                RunManager.Instance.Environment.Home.SetSlotCount(roomDefinition._slotCountAfter);
                            return B;
                        });
                    }

                    B._receiveSignal = signal =>
                    {
                        if (signal is PickDiscoveredSkillSignal)
                            RunManager.Instance.Environment.SetDGoldProcedure(goldValue);

                        return B.DefaultReceiveSignal(signal);
                    };

                    return A;
                }),
            
            new(id:                                 "Room0003",
                name:                               "出门",
                description:                        "出门",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEnvironment env = RunManager.Instance.Environment;

                    Dictionary<PackEntry, DialogOption> optionDict = new Dictionary<PackEntry, DialogOption>()
                    {
                        {
                            Encyclopedia.PackCategory.FromName("无常路引"), 
                            DialogOption.FromTextAndSelect("无常路引，金系，擅长暗器，攻击人的弱点",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("无常路引");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("大音希声"),
                            DialogOption.FromTextAndSelect("大音希声，金系，擅长蓄势而发，一击致命",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("大音希声");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("天河引气录"),
                            DialogOption.FromTextAndSelect("天河引气录，水系，擅长驾驭灵气，驱使强大的法术",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("天河引气录");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("御虚诀"),
                            DialogOption.FromTextAndSelect("御虚诀，水系，虚虚实实，让敌人不清楚自己是否被打中了",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("御虚诀");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("大椿功"),
                            DialogOption.FromTextAndSelect("大椿功，木系，擅长会成长的法术，法术每次使用后会逐渐变强",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("大椿功");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("游龙遁"),
                            DialogOption.FromTextAndSelect("游龙遁，木系，可以闪避敌人的攻击",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("游龙遁");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("归鸿十二步"),
                            DialogOption.FromTextAndSelect("归鸿十二步，火系，攻守兼备的剑舞，攻击如雨点般前赴后继",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("归鸿十二步");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("大焚天秘乘"),
                            DialogOption.FromTextAndSelect("大焚天秘乘，火系，消耗生命以施展技能，和敌人以命换命",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("大焚天秘乘");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("须弥妙法"),
                            DialogOption.FromTextAndSelect("须弥妙法，土系，防御力强大，待准备好之后，一击制敌",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("须弥妙法");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("锻体四则"),
                            DialogOption.FromTextAndSelect("锻体四则，土系，平时勤于修炼，气血高于同期修士，战斗中可化为天人之体以制敌",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("锻体四则");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                    };

                    int[] combination = Numeric.GetCombination(5, 3);

                    DialogOption[] dialogOptions = new DialogOption[combination.Length];
                    dialogOptions.Length.Do(i =>
                    {
                        int index = combination[i];
                        PackEntry mappedPackEntry = env.GetRunConfig().PacksToStartWith[index];
                        dialogOptions[i] = optionDict[mappedPackEntry];
                    });
                    
                    DialogCell A = new(
                        titleText: "出发",
                        detailedText: "出发之前，回想起了哪一本秘籍曾经修炼过？",
                        options: dialogOptions);

                    return A;
                }),
            
            new(id:                                 "Room0004",
                name:                               "序章出门",
                description:                        "出门",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEnvironment env = RunManager.Instance.Environment;
                    
                    env.ClearDeckProcedure();
                    env.Home.SetSlotCount(3);
                    env.SetHealthProcedure(40);

                    Dictionary<PackEntry, DialogOption> optionDict = new Dictionary<PackEntry, DialogOption>()
                    {
                        {
                            Encyclopedia.PackCategory.FromName("无常路引"), 
                            DialogOption.FromTextAndSelect("无常路引，金系，擅长暗器，攻击人的弱点",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("无常路引");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("大音希声"),
                            DialogOption.FromTextAndSelect("大音希声，金系，擅长蓄势而发，一击致命",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("大音希声");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("天河引气录"),
                            DialogOption.FromTextAndSelect("天河引气录，水系，擅长驾驭灵气，驱使强大的法术",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("天河引气录");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("御虚诀"),
                            DialogOption.FromTextAndSelect("御虚诀，水系，虚虚实实，让敌人不清楚自己是否被打中了",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("御虚诀");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("大椿功"),
                            DialogOption.FromTextAndSelect("大椿功，木系，擅长会成长的法术，法术每次使用后会逐渐变强",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("大椿功");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("游龙遁"),
                            DialogOption.FromTextAndSelect("游龙遁，木系，可以闪避敌人的攻击",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("游龙遁");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("归鸿十二步"),
                            DialogOption.FromTextAndSelect("归鸿十二步，火系，攻守兼备的剑舞，攻击如雨点般前赴后继",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("归鸿十二步");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("大焚天秘乘"),
                            DialogOption.FromTextAndSelect("大焚天秘乘，火系，消耗生命以施展技能，和敌人以命换命",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("大焚天秘乘");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("须弥妙法"),
                            DialogOption.FromTextAndSelect("须弥妙法，土系，防御力强大，待准备好之后，一击制敌",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("须弥妙法");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                        {
                            Encyclopedia.PackCategory.FromName("锻体四则"),
                            DialogOption.FromTextAndSelect("锻体四则，土系，平时勤于修炼，气血高于同期修士，战斗中可化为天人之体以制敌",
                                option =>
                                {
                                    PackEntry packEntry = Encyclopedia.PackCategory.FromName("锻体四则");
                                    
                                    GainSkillBuilder b = new();
                                    packEntry.StartCards.Do(skillEntry => b.Pick(skillEntry));
                                    SkillEntryCollectionDescriptor descriptor = new(jingJie: JingJie.LianQi, count: 2);
                                    b.Draw(descriptor);
                                    b.Create(JingJie.LianQi);
                                    b.Add();
                                    b.Invoke();
                                    return null;
                                })
                        },
                    };

                    int[] combination = Numeric.GetCombination(5, 3);

                    DialogOption[] dialogOptions = new DialogOption[combination.Length];
                    dialogOptions.Length.Do(i =>
                    {
                        int index = combination[i];
                        PackEntry mappedPackEntry = env.GetRunConfig().PacksToStartWith[index];
                        dialogOptions[i] = optionDict[mappedPackEntry];
                    });
                    
                    DialogCell A = new(
                        titleText: "出发",
                        detailedText: "出发之前，回想起了哪一本秘籍曾经修炼过？",
                        options: dialogOptions);

                    return A;
                }),
            
            new(id:                                 "Room0005",
                name:                               "突破境界",
                description:                        "突破境界",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    // 0 -> 凌云峰，发现1张下一境界的金牌
                    // 1 -> 逍遥海，发现1张下一境界的水牌
                    // 2 -> 桃花宫，发现1张下一境界的木牌
                    // 3 -> 长明殿，发现1张下一境界的火牌
                    // 4 -> 环岳岭，发现1张下一境界的土牌
                    // 5 -> 散修，发现1基础境界是下一境界的牌
                    // 6 -> 剑池，检索1张当前境界的攻击牌
                    // 7 -> 风雨楼，检索1张当前境界的防御牌
                    // 8 -> 星宫，检索1张当前境界的灵气牌
                    // 9 -> 天机阁，选择1张牌复制1次
                    // 10 -> 百草堂，选择2张牌，提升到下一境界
                    // 11 -> 易宝斋，选择1张牌卖掉，之后访问一次商店
                    
                    RunEnvironment env = RunManager.Instance.Environment;
                    JingJie currJingJie = env.JingJie;
                    JingJie nextJingJie = (env.JingJie + 1).ClampUpper(JingJie.HuaShen);
                    
                    string[] descriptions = new string[12]
                    {
                        $"凌云峰，发现1张{nextJingJie.GetName()}金牌",
                        $"逍遥海，发现1张{nextJingJie.GetName()}水牌",
                        $"桃花宫，发现1张{nextJingJie.GetName()}木牌",
                        $"长明殿，发现1张{nextJingJie.GetName()}火牌",
                        $"环岳岭，发现1张{nextJingJie.GetName()}土牌",
                        $"剑池，检索1张{currJingJie.GetName()}攻击牌",
                        $"风雨楼，检索1张{currJingJie.GetName()}防御牌",
                        $"星宫，检索1张{currJingJie.GetName()}灵气牌",
                        $"天机阁，选择1张牌，复制1次",
                        $"百草堂，选择2张不高于{currJingJie.GetName()}牌，提升到{nextJingJie.GetName()}",
                        $"易宝斋，选择1张牌卖掉，之后访问一次商店",
                        $"散修，发现1张基础境界是{nextJingJie.GetName()}期的牌",
                    };

                    Cell[] panels = new Cell[12]
                    {
                        DiscoverSkillCell.FromLingYunFeng(room.Ladder + 3),
                        DiscoverSkillCell.FromXiaoYaoHai(room.Ladder + 3),
                        DiscoverSkillCell.FromTaohuaGong(room.Ladder + 3),
                        DiscoverSkillCell.FromChangMingDian(room.Ladder + 3),
                        DiscoverSkillCell.FromHuanYueLing(room.Ladder + 3),
                        ArbitraryCardPickerCell.FromJianChi(room.Ladder),
                        ArbitraryCardPickerCell.FromFengYuLou(room.Ladder),
                        ArbitraryCardPickerCell.FromXingGong(room.Ladder),
                        CardPickerCell.FromTianJiGe(),
                        CardPickerCell.FromBaiCaoTang(room.Ladder),
                        ShopCell.FromYiBaoZhai(room.Ladder + 3),
                        DiscoverSkillCell.FromSanXiu(room.Ladder + 3),
                    };
                    
                    int[] combination;
                    if (currJingJie == JingJie.LianQi)
                    {
                        combination = Numeric.GetCombination(11, 4);
                    }
                    else
                    {
                        combination = Numeric.GetCombination(12, 4);
                    }

                    DialogOption[] dialogOptions = new DialogOption[combination.Length];
                    dialogOptions.Length.Do(i =>
                    {
                        int index = combination[i];
                        dialogOptions[i] = DialogOption.FromTextAndSelect(descriptions[index], option =>
                        {
                            env.NextJingJieProcedure();
                            return panels[index];
                        });
                    });
                    
                    DialogCell A = new(
                        titleText: "突破",
                        detailedText: "你感到周身灵气运转无比通畅，即将突破长久以来的瓶颈",
                        options: dialogOptions);
                    RunManager.Instance.SetBackgroundFromJingJie(nextJingJie);
                    return A;
                }),

            new(id:                                 "Room0006",
                name:                               "休息",
                description:                        "休息",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "休息",
                        detailedText: "最近有些空闲的时间，你决定要",
                        "加紧修炼", "去温泉", "喝点人参茶");

                    JingJie currJingJie = RunManager.Instance.Environment.JingJie;
                    JingJie nextJingJie = Mathf.Min(RunManager.Instance.Environment.JingJie + 1, JingJie.HuaShen);
                    
                    CardPickerCell B = CardPickerCell.FromConstantDetailedText(
                        titleText:          "感悟",
                        detailedText:       $"在菩提树下坐了一段时间，对境界有了新的见解。" +
                                            $"\n选择一张不高于{currJingJie}期({currJingJie.GetColorName()}色外框)的牌提升至{nextJingJie}期({nextJingJie.GetColorName()}色外框)",
                        descriptor:         RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(RunSkillDescriptor.FromJingJieBound(JingJie.LianQi, nextJingJie), 1));
                    
                    B.SetSubmitOperation(cardPickerCell =>
                    {
                        cardPickerCell.RequirementSlotList.Do(requirementSlot =>
                        {
                            if (requirementSlot.Skill == null)
                                return;
                            DeckIndex deckIndex = requirementSlot.ToDeckIndex();
                            RunManager.Instance.Environment.SkillSetJingJieProcedure(nextJingJie, deckIndex);
                            RunManager.Instance.Environment.WithdrawToHandProcedure(WithdrawToHandDetails.FromSlot(requirementSlot));
                        });
                        return null;
                    });

                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);
                    DialogCell C = new DialogCell(
                            titleText: "愉悦",
                            detailedText: $"泡了温泉之后感到了心情畅快，获得了{baseGoldReward}点气血上限")
                        .SetReward(Reward.FromHealth(baseGoldReward));
                    
                    DialogCell D = new DialogCell(
                            titleText: "喝茶",
                            detailedText: "喝了几口人参茶，回复了2点命元")
                        .SetReward(Reward.FromMingYuan(2));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "Room0007",
                name:                               "胜利",
                description:                        "胜利",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "胜利",
                        detailedText: "恭喜获得游戏胜利",
                        options: "前往结算");

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Victorious);
                        return null;
                    });

                    return A;
                }),
            
            #endregion

            #region 02_Tutorial

            new(id:                                 "Room02_001",
                name:                               "教学1",
                description:                        "教学1",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物1"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌1");
                    
                    BattleCell A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("这个界面是准备界面" +
                                         "\n徐福和对手准备进行战斗" +
                                         "\n战斗开始之后，徐福和对手会依次使用准备区中的牌"),
                        new ConfirmGuide("左边的-2是徐福最终血量" +
                                         "\n右边的10是对手最终血量" +
                                         "\n现在对手会以10血打败徐福"),
                        new EquipGuide("将卡牌置入战斗区",
                            SkillEntryDescriptor.FromName("劈砍"), new DeckIndex(SkillRegion.Field, 0)),
                        new ConfirmGuide("将劈砍置入后，左边血量大于右边。" +
                                         "\n表示战斗的最终结果是徐福以4血战胜对手"),
                        new ClickBattleGuide("徐福胜利后，便可以点击对决按钮，进入战斗界面" +
                                             "\n战斗界面中，徐福和对手会依次使用战斗区布置好的牌",
                            new Vector2(965f, 913.5f)),
                    });

                    DialogCell R = new(
                        titleText: "重试",
                        detailedText: "请重新尝试教学");
                    R[0].SetSelect(option => A);

                    RunManager.Instance.Environment.SetPlayerEqualPreset(playerTemplate, toField: false, overwrite: true);
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.SetPlayerEqualPreset(playerTemplate, toField: false, overwrite: true);
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "Room02_002",
                name:                               "教学2",
                description:                        "教学2",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物2"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌2");
                    
                    BattleCell A = new(enemyEntity);

                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("战斗结束后，气血将会完全回复" +
                                         "\n现在徐福面临着下一个对手"),
                        new ConfirmGuide("冰弹的左上角的消耗标志，表示冰弹需要两点灵气。"),
                        new EquipGuide("来将吐纳置入",
                            SkillEntryDescriptor.FromName("吐纳"), DeckIndex.FromField(0)),
                        new ConfirmGuide("灵气不足时，显示的是红色的，变成白色是因为徐福此时灵气足够释放了"),
                        new ClickBattleGuide("请点击对决以查看结算",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogCell R = new(
                        titleText: "重试",
                        detailedText: "请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.SetHealthProcedure(playerTemplate.GetHealth());
                    
                    RunManager.Instance.Environment.ClearDeckProcedure();
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("吐纳"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("冰弹"));
                    b.Create();
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.RecordDeckIndex(DeckIndex.FromField(1));
                    b.Add();
                    b.Invoke();
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeckProcedure();
                        GainSkillBuilder b = new();
                        b.Pick(Encyclopedia.SkillCategory.FromName("吐纳"));
                        b.Pick(Encyclopedia.SkillCategory.FromName("冰弹"));
                        b.Create();
                        b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                        b.RecordDeckIndex(DeckIndex.FromField(1));
                        b.Add();
                        b.Invoke();
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "Room02_003",
                name:                               "教学3",
                description:                        "教学3",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物3"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌3");
                    
                    BattleCell A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("现在徐福和对方手牌一样，并且是徐福先手"),
                        new ConfirmGuide("没能胜利的原因是对方初始气血高于徐福。鼠标放在气血上可以查看战斗开始时的气血"),
                        new UnequipGuide("可以将冲撞卸下来",
                            SkillEntryDescriptor.FromName("冲撞")),
                        new ConfirmGuide("空白的位置相当于一张回复一点灵气。应急的时候可以使用"),
                        new ConfirmGuide("此时对方的恋花还是缺少一点灵气。对方需要额外等待一回合以聚集灵气"),
                        new ClickBattleGuide("请开始战斗",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogCell R = new(
                        titleText: "重试",
                        detailedText: "请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.SetHealthProcedure(playerTemplate.GetHealth());
                    
                    RunManager.Instance.Environment.ClearDeckProcedure();
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("冲撞"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                    b.Create();
                    b.RecordDeckIndex(DeckIndex.FromField(0));
                    b.RecordDeckIndex(DeckIndex.FromField(1));
                    b.Add();
                    b.Invoke();
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeckProcedure();
                        GainSkillBuilder b = new();
                        b.Pick(Encyclopedia.SkillCategory.FromName("冲撞"));
                        b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                        b.Create();
                        b.RecordDeckIndex(DeckIndex.FromField(0));
                        b.RecordDeckIndex(DeckIndex.FromField(1));
                        b.Add();
                        b.Invoke();
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });
                    
                    return A;
                }),

            new(id:                                 "Room02_004",
                name:                               "教学4", // 同名同境界合成
                description:                        "教学4",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell Dialog = new DialogCell(
                        titleText: "合成",
                        detailedText: "突然对在之前战斗中使用的牌有些想法。");
                    
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物4"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌4");
                    
                    BattleCell A = new(enemyEntity);
                    
                    DeckIndex firstDeckIndex = DeckIndex.FromField(1);
                    SkillEntryDescriptor descriptor = SkillEntryDescriptor.FromNameJingJie("恋花", JingJie.LianQi);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("这张牌，已经有了一张诶" +
                                         "\n对了，试试合成"),
                        new EquipGuide("将两张牌叠起来",
                            descriptor, firstDeckIndex),
                        new ConfirmGuide("卡牌没有合成" +
                                         "\n好像有个步骤是先将待合成的两张牌卸下至手牌区来着"),
                        new UnequipGuide("将牌卸下到手牌区试试",
                            descriptor),
                        new MergeGuide("现在应该没问题了",
                            descriptor, descriptor),
                        new ConfirmGuide("合成后的牌卡框边缘从灰色变成了蓝色，代表境界更高了"),
                        new ConfirmGuide("卡牌的境界对应的颜色依次是灰，绿，蓝，紫，黄，如果合成之前想查看卡牌不同境界的效果，可以悬停卡牌浏览"),
                        new EquipGuide("将合成后的牌置入战斗区",
                            SkillEntryDescriptor.FromEntryJingJie(descriptor.Entry, JingJie.JinDan), firstDeckIndex),
                        new ClickBattleGuide("战斗中虽然说是观察对手的招数，找出应对之策" +
                                             "\n但是在绝对的实力面前，克制关系也不过尔尔" +
                                             "\n点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogCell R = new(
                        titleText: "重试",
                        detailedText: "请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.SetHealthProcedure(playerTemplate.GetHealth());
                    
                    RunManager.Instance.Environment.ClearDeckProcedure();
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("冲撞"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                    b.Create();
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.RecordDeckIndex(DeckIndex.FromField(1));
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.Add();
                    b.Invoke();
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeckProcedure();
                        GainSkillBuilder b = new();
                        b.Pick(Encyclopedia.SkillCategory.FromName("冲撞"));
                        b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                        b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                        b.Create();
                        b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                        b.RecordDeckIndex(DeckIndex.FromField(1));
                        b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                        b.Add();
                        b.Invoke();
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        return null;
                    });

                    Dialog[0].SetSelect(option => A);
                    
                    return Dialog;
                }),

            new(id:                                 "Room02_005",
                name:                               "教学5",
                description:                        "教学5",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物5"));
                    
                    BattleCell A = new(enemyEntity);

                    DialogCell D1 = new(
                        titleText: "耍赖",
                        detailedText: "对手竟然说自己还在练习新招式，刚才的不作数，要和徐福再比试一场。让对面知道再来几次都是一样的。");
                    DialogCell D2 = new(
                        titleText: "耍赖",
                        detailedText: "对手只是想吓唬徐福一下，心满意足的走了，没有真的想要命元。");

                    D1[0].SetSelect(option => A);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("。。。。对面卡牌竟然已经是散发着金光的化神牌，可能化神大佬闲了也喜欢找人消遣。"),
                        new ConfirmGuide("这个差距，怎么调整牌都过不去了，好在偶尔的一两次失败并不会导致游戏的失败"),
                        new ConfirmGuide("除非是生死决战，战败时只需要给予对手一些命元，对手就会放徐福一马"),
                        new ConfirmGuide("命元在最左上角查看，命元归零游戏才会失败。"),
                        new ClickBattleGuide("现在点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });

                    AppManager.Instance.ProfileManager.GetCurrProfile().SetFirstRunFinished(true);
                    
                    A.SetLoseOperation(() => D2);
                    A.SetWinOperation(() => D2);
                    
                    return D1;
                }),

            new(id:                                 "Room02_006",
                name:                               "教学6", // 同名不同境界合成
                description:                        "教学6",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物6"));
                    
                    BattleCell A = new(enemyEntity);
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("云袖"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("云袖"));
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.ZhuJi);
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.Add();
                    b.Invoke();
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("之前尝试的合成是名字相同，境界也相同。这次的两张牌，名字相同，但是境界不同" +
                                         "但是理论上来说，也有合成的可能性"),
                        new MergeGuide("让我来试试",
                            SkillEntryDescriptor.FromNameJingJie("云袖", JingJie.LianQi), SkillEntryDescriptor.FromNameJingJie("云袖", JingJie.ZhuJi)),
                        new ConfirmGuide("真的合成了诶。徐福找到了规律，同名同境界的时候，合成可以跨两个境界，即从练气到金丹"),
                        new ConfirmGuide("同名不同境界的时候，合成可以提升一个境界，即从筑基到金丹。"),
                        new ConfirmGuide("想提前确认合成之后的卡牌效果时，可以右键点击卡牌查看。"),
                        new ConfirmGuide("用现有的牌击败对手吧"),
                    });

                    DialogCell Dialog = new DialogCell(
                        titleText: "同名合成",
                        detailedText: "回想起来一些以前修行时的法术。对之前合成的规则有些疑问，正好可以试验一下。");

                    DialogCell Dialog2 = new DialogCell(
                        titleText: "同名合成",
                        detailedText: "炼丹真是深奥，同名牌竟然也可以合成。");
                    
                    Dialog[0].SetSelect(option => A);

                    A.SetWinOperation(() => Dialog2);
                    A.SetLoseOperation(() => Dialog2);
                    
                    return Dialog;
                }),

            new(id:                                 "Room02_007",
                name:                               "教学7", // 不同名同境界合成
                description:                        "教学7",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物7"));
                    
                    BattleCell A = new(enemyEntity);
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("云袖"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.LianQi);
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.Add();
                    b.Invoke();
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("记得之前有过这种情况，将两张不一样的牌合成。"),
                        new MergeGuide("让我来试试",
                            SkillEntryDescriptor.FromNameJingJie("云袖", JingJie.LianQi), SkillEntryDescriptor.FromNameJingJie("恋花", JingJie.LianQi)),
                        new ConfirmGuide("真的合成了诶。\n徐福发现了新的规律，不同名同境界两张牌，也可以合成。"),
                        new ConfirmGuide("效果是随机发现一张其他的牌。\n新发现的牌会比用于合成的牌，境界高一阶。"),
                        new ConfirmGuide("这样就可以缓解战斗区的牌质量不足，而手牌太多帮不上忙的问题了。"),
                        new ConfirmGuide("用现有的牌击败对手吧。"),
                    });

                    DialogCell Dialog = new DialogCell(
                        titleText: "不同名合成",
                        detailedText: "手牌有点多了，但是都用不到，而且准备区的牌也凑不到对子，强度有点跟不上了。");

                    DialogCell Dialog2 = new DialogCell(
                        titleText: "不同名合成",
                        detailedText: "对合成的神秘，有了更进一步的理解了。");
                    
                    Dialog[0].SetSelect(option => A);

                    A.SetWinOperation(() => Dialog2);
                    A.SetLoseOperation(() => Dialog2);
                    
                    return Dialog;
                }),

            new(id:                                 "Room02_008",
                name:                               "教学8",
                description:                        "教学8",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell Dialog = new DialogCell(
                        titleText: "阵法",
                        detailedText: "逐渐感觉修炼的得心应手起来了，最近隐约能够感到卡牌之间存在某种共鸣。");
                    
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物8"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌8");
                    
                    BattleCell A = new(enemyEntity);
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("同属性的牌同时处于战斗区就会激活阵法效果"),
                        new EquipGuide("金刃和寻猎都是金属性的牌，装备寻猎",
                            SkillEntryDescriptor.FromName("寻猎"), DeckIndex.FromField(1)),
                        new ConfirmGuide("金属性的牌达到了两张，金灵阵激活了，鼠标瞄到头像上方的阵法图标可查看具体效果"),
                        new EquipGuide("另外两张牌都是水属性的，装备激流试试",
                            SkillEntryDescriptor.FromName("激流"), DeckIndex.FromField(0)),
                        new EquipGuide("然后是空幻",
                            SkillEntryDescriptor.FromName("空幻"), DeckIndex.FromField(1)),
                        new ClickBattleGuide("虽然刚才的金灵阵效果没了，但是两张水系卡牌可以激活水系阵法" +
                                             "\n点击开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    });
                    
                    DialogCell R = new(
                        titleText: "重试",
                        detailedText: "请重新尝试教学");
                    R[0].SetSelect(option => A);
                    
                    RunManager.Instance.Environment.Home.SetSlotCount(playerTemplate.GetSlotCount());
                    RunManager.Instance.Environment.SetHealthProcedure(playerTemplate.GetHealth());
                    
                    RunManager.Instance.Environment.ClearDeckProcedure();
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("金刃"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("寻猎"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("空幻"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("激流"));
                    b.Create();
                    b.RecordDeckIndex(DeckIndex.FromField(0));
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.RecordDeckIndex(DeckIndex.FromField(1));
                    b.Add();
                    b.Invoke();
                    
                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.ClearDeckProcedure();
                        GainSkillBuilder b = new();
                        b.Pick(Encyclopedia.SkillCategory.FromName("金刃"));
                        b.Pick(Encyclopedia.SkillCategory.FromName("寻猎"));
                        b.Pick(Encyclopedia.SkillCategory.FromName("空幻"));
                        b.Pick(Encyclopedia.SkillCategory.FromName("激流"));
                        b.Create();
                        b.RecordDeckIndex(DeckIndex.FromField(0));
                        b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                        b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                        b.RecordDeckIndex(DeckIndex.FromField(1));
                        b.Add();
                        b.Invoke();
                        A.ResetGuideIndex();
                        return R;
                    });
                    
                    A.SetWinOperation(() =>
                    {
                        RunManager.Instance.Environment.SetHealthProcedure(40);
                        RunManager.Instance.Environment.Home.SetSlotCount(3);
                        RunManager.Instance.Environment.ClearDeckProcedure();
                        return null;
                    });

                    Dialog[0].SetSelect(option => A);
                    
                    return Dialog;
                }),

            new(id:                                 "Room02_009",
                name:                               "教学9", // 不同名不同境界合成
                description:                        "教学9",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物9"));
                    
                    BattleCell A = new(enemyEntity);
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("云袖"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.ZhuJi);
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                    b.Add();
                    b.Invoke();
                    
                    A.SetGuideDescriptors(new Guide[]
                    {
                        new ConfirmGuide("徐福回想起了还有一种合成方式，但是因为亏境界，所以这种合成不怎么流行。"),
                        new MergeGuide("让我来试试",
                            SkillEntryDescriptor.FromNameJingJie("云袖", JingJie.LianQi), SkillEntryDescriptor.FromNameJingJie("恋花", JingJie.ZhuJi)),
                        new ConfirmGuide("合成成功了。徐福又双叒叕发现了规律，不同名不同境界之间，也可以亏本进行合成。"),
                        new ConfirmGuide("比如，练气云袖 + 筑基恋花 = 筑基云袖\n这样没有凑到对子的云袖，境界也能提升。"),
                        new ConfirmGuide("每当需要使用的牌境界较低，而手牌的闲牌境界较高的时候就可以合成。强行将需要使用的牌拉高一个境界。"),
                        new ConfirmGuide("用现有的牌击败对手吧。"),
                    });

                    DialogCell Dialog = new DialogCell(
                        titleText: "境界置换",
                        detailedText: "熟练掌握了不同名同境界的合成之后，确实将牌境界拉的高了，常常手中金光闪闪。但是不巧，经常手中的卡牌都不是迫切需要的。");

                    DialogCell Dialog2 = new DialogCell(
                        titleText: "境界置换",
                        detailedText: "同名同境界，同名不同境界，不同名同境界，不同名不同境界，善用四种合成，将会带来极大的便利。");
                    
                    Dialog[0].SetSelect(option => A);

                    A.SetWinOperation(() => Dialog2);
                    A.SetLoseOperation(() => Dialog2);
                    
                    return Dialog;
                }),

            new(id:                                 "Room02_010",
                name:                               "教学10",
                description:                        "教学10",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell Dialog = new DialogCell(
                        titleText: "流转",
                        detailedText: "金系擅长锋锐，水系擅长格挡。每种五行的增益数量叠起来了都很厉害，就是太难做起来了，带着遗憾你进入了梦乡。");

                    DialogCell Dialog2 = new DialogCell(
                        titleText: "流转",
                        detailedText: "梦中，一棵大树出现在你面前。你叫不上来这棵树的名字，但是感觉非常熟悉。你注意到了树周围发生的奇观。" +
                                      "金属遇寒，湿气冷凝成水，滴下来滋养了树苗，随即长成大树，燃烧起来，烧成了灰烬，归于尘土。" +
                                      "你还没来得及思考这其中的意义，便遭遇了怪物。"
                    );
                    
                    DialogCell Dialog3 = new DialogCell(
                        titleText: "流转",
                        detailedText: $"五行都有一个专属的Buff（锋锐，格挡，力量，灼烧，坚毅），都有着独特的效果。" +
                                      $"\n善于利用相生规则，可以最大化利用五行的Buff。" +
                                      $"\n具体的五行流转顺序可以将鼠标瞄到屏幕上方状态栏的标注查看。"
                    );
                    
                    RunEntity enemyEntity = RunEntity.FromTemplate(EditorManager.FindEntity("教学怪物10"));
                    RunEntity playerTemplate = EditorManager.FindEntity("玩家手牌10");
                    
                    BattleCell A = new(enemyEntity);

                    Guide[] guide = new Guide[]
                    {
                        new ConfirmGuide("在梦中手中突然多出了5张牌"),
                        new EquipGuide("对面明显是一个多段攻击的角色，徐福如果能有很高的灼烧的话。。",
                            SkillEntryDescriptor.FromId("Skill09_009"), DeckIndex.FromField(4)),
                        new EquipGuide("明显单一张灼烧叠的太慢了，五行中说是木生火，如果我们获得灼烧之前有力量的话。" +
                                       "这些力量就可以一并流转成为灼烧" +
                                       "\n哈哈，这场对决，是徐福的胜利了！",
                            SkillEntryDescriptor.FromId("Skill09_008"), DeckIndex.FromField(3)),
                        new EquipGuide("果然不太够么。虽说流转一次已经叠层效率翻倍了，但是如果获得力量之前就有格挡的话？" +
                                       "水生木，格挡会一起变成力量，最终都会成为灼烧哒！",
                            SkillEntryDescriptor.FromId("Skill09_007"), DeckIndex.FromField(2)),
                        new EquipGuide("对方生命减少了，明显起到了效果，继续置入一张凝水。利用金生水的规则，将锋锐流转成格挡",
                            SkillEntryDescriptor.FromId("Skill09_006"), DeckIndex.FromField(1)),
                        new EquipGuide("最后放一张土属性的牌，提供坚毅，之后可以流转成锋锐",
                            SkillEntryDescriptor.FromId("Skill09_005"), DeckIndex.FromField(0)),
                        new ConfirmGuide("成了，巧用流转的规则，可以快速叠层本不富裕的增益"),
                        new ConfirmGuide("流转的顺序是锋锐->格挡->力量->灼烧->坚毅。可以随时查看上面的五行图标看到五行介绍和流转规则。"),
                        new ClickBattleGuide("开始战斗吧",
                            new Vector2(965f, 913.5f)),
                    };
                    A.SetGuideDescriptors(guide);
                    
                    DialogCell R = new(
                        titleText: "重试",
                        detailedText: "请重新尝试教学");
                    R[0].SetSelect(option => A);

                    Dialog2._exit = descriptor =>
                    {
                        RunManager.Instance.Environment.UnequipProcedure(UnequipDetails.FromDeckIndex(DeckIndex.FromField(0)));
                        RunManager.Instance.Environment.UnequipProcedure(UnequipDetails.FromDeckIndex(DeckIndex.FromField(1)));
                        RunManager.Instance.Environment.UnequipProcedure(UnequipDetails.FromDeckIndex(DeckIndex.FromField(2)));
                        RunManager.Instance.Environment.UnequipProcedure(UnequipDetails.FromDeckIndex(DeckIndex.FromField(3)));
                        RunManager.Instance.Environment.UnequipProcedure(UnequipDetails.FromDeckIndex(DeckIndex.FromField(4)));
                        RunManager.Instance.Environment.SetPlayerEqualPreset(playerTemplate, toField: false, overwrite: false);
                    };
                    
                    A.SetWinOperation(() =>
                    {
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0705")); // 蜕变
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0706")); // 凝水
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0707")); // 流霰
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0708")); // 养气丹
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0709")); // 燎原
                        return Dialog3;
                    });

                    A.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0705"));
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0706"));
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0707"));
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0708"));
                        RunManager.Instance.Environment.RemoveSkillProcedure(SkillEntryDescriptor.FromId("0709"));
                        return Dialog3;
                    });

                    Dialog[0].SetSelect(option => Dialog2);
                    Dialog2[0].SetSelect(option => A);
                    Dialog3[0].SetSelect(option => null);
                    
                    return Dialog;
                }),

            new(id:                                 "Room02_011",
                name:                               "漫画",
                description:                        "漫画",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    ComicCell A = new("第一张");
                    ComicCell B = new("第二张");
                    A.Next = B;
                    B.Next = null;
                    return A;
                }),

            #endregion

            #region 03_Shop
            
            new(id:                                 "Room0017",
                name:                               "存钱",
                description:                        "存钱",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);
                    DialogCell A = new DialogCell(
                            titleText: "存钱",
                            detailedText: $"获得了{baseGoldReward}金钱")
                        .SetReward(Reward.FromGold(baseGoldReward));
                    return A;
                }),
            
            new(id:                                 "Room0018",
                name:                               "黑市",
                description:                        "黑市",
                ladderBound:                        new Bound(0, 8),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "黑市",
                        detailedText: "你发现了一个黑市，这里有少量高境界卡牌。",
                        options: "进去看一看");
                    
                    ShopCell B = ShopCell.FromHeiShi(room.Ladder);

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),
            
            new(id:                                 "Room0019",
                name:                               "收藏家",
                description:                        "收藏家",
                ladderBound:                        new Bound(2, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "收藏家",
                        detailedText: "你遇到了一位收藏家，他邀请你去看看他的藏品");
                    
                    ShopCell B = ShopCell.FromShouCangJia(room.Ladder);

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),

            new(id:                                 "Room0020",
                name:                               "以物易物",
                description:                        "以物易物",
                ladderBound:                        new Bound(2, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "集会",
                        detailedText: "你收到了神秘集会的入场券，大家在集会上交换技能。");
                    
                    BarterCell B = BarterCell.FromCount();

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),
            
            new(id:                                 "Room0021",
                name:                               "毕业季",
                description:                        "毕业季",
                ladderBound:                        new Bound(5, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "毕业季",
                        detailedText: "一阵噪音惊扰了你的休息，原来是灵韵宗的毕业季到了，学子们完成了学业后，纷纷将不要的技能白送，先到先得。");
                    
                    ArbitraryCardPickerCell B = ArbitraryCardPickerCell.FromBiYeJi(room.Ladder);
                    
                    A[0].SetSelect(option => B);
                    
                    return A;
                }),
            
            new(id:                                 "Room0022",
                name:                               "盲盒",
                description:                        "盲盒",
                ladderBound:                        new Bound(8, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "盲盒",
                        detailedText: "前面有一个盲盒商店，你想去看看这期会出什么");
                    
                    GachaCell B = new(priceMultiplier: 2);

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),

            #endregion

            #region 04_Adventure
            
            new(id:                                 "Room0023",
                name:                               "天津四",
                description:                        "天津四",
                ladderBound:                        new Bound(0, 2),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "缘分",
                        detailedText: "你看见一个书生，悄悄看着一个织布的少女，应该是对她有意思。他看你道士打扮，于是问道：“先生可否帮我算一卦，算姻缘。”",
                        "祝福他的缘分", "和他说不是每一段相思都能够有结果的");

                    DialogCell B = new(
                        titleText: "缘分",
                        detailedText: "书生表情平静，实际上满心欢喜，说：“我去尝试追求她看看。”",
                        options: "过了三十年");
                    DialogCell B1 = new DialogCell(
                            titleText: "缘分",
                            detailedText: "你又见到了当初的书生，他说没有在当年找到合适的姻缘。他给你留下了一些东西。\n\n得到《遗憾》天津四 著")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("遗憾"), RunManager.Instance.Environment.JingJie));

                    DialogCell C = new(
                        titleText: "缘分",
                        detailedText: "书生表情平静，实际上内心忧愁，然后默默离开了",
                        options: "过了三十年");
                    DialogCell C1 = new DialogCell(
                            titleText: "缘分",
                            detailedText: "你又见到了当初的书生，他虽然当时放弃了，但是后来和其他人结成了姻缘。他给你留下了一些东西。\n\n得到《爱恋》天津四 著")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("爱恋"), RunManager.Instance.Environment.JingJie));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    B[0].SetSelect(option => B1);
                    C[0].SetSelect(option => C1);

                    return A;
                }),

            new(id:                                 "Room0024",
                name:                               "琴仙",
                description:                        "琴仙",
                ladderBound:                        new Bound(2, 5),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "琴仙",
                        detailedText: "你遇到了一个弹琴的人，他双目失明，衣衫褴褛，举手投足之间却让人感到大方得体，应该是一名隐士。正好前一首曲毕。向你的方向看了过来，好像知道你来了。",
                        "来一首欢快的曲子吧",
                        "来一首悲伤的曲子吧",
                        "赶路着急，没时间留下来听曲子了");

                    DialogCell B = new DialogCell(
                            titleText: "琴仙",
                            detailedText: "那人哈哈大笑，然后弹了一首欢快的曲子。你回想起这一生，第一次这么有满足感，产生了一些思绪。回过神来，那人已经不见了。\n\n获得《春雨》")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("春雨"), RunManager.Instance.Environment.JingJie));
                    DialogCell C = new DialogCell(
                            titleText: "琴仙",
                            detailedText: "那人一声叹息，然后弹了一首悲伤的曲子。你怀疑起了修仙的意义，产生了一些思绪。回过神来，那人已经不见了。\n\n获得《枯木》")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("枯木"), RunManager.Instance.Environment.JingJie));
                    DialogCell D = new DialogCell(
                            titleText: "琴仙",
                            detailedText: "之前赶路省下的时间，正好可以用于修炼。\n\n获得一个技能")
                        .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: RunManager.Instance.Environment.JingJie)));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "Room0025",
                name:                               "赤壁赋",
                description:                        "赤壁赋",
                ladderBound:                        new Bound(5, 8),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "辩论",
                        detailedText: "你见到两个人在辩论。\n一人说，月亮是变化的，今天还是满月，明天就不是了。\n另一人说，月亮是不变的，上个月看是满月，今天看也还是满月。",
                        "赞同月亮是变化的", "赞同月亮是不变的", "变的不是月亮，而是人");

                    DialogCell B = new DialogCell(
                            titleText: "辩论",
                            detailedText: "你说到：“盖将自其变者而观之，则天地曾不能以一瞬，月亮是变化的。”\n只见第一个人非常赞同你的观点，给了你一些东西。" +
                                          "\n\n得到《须臾》")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("须臾"), jingJie: RunManager.Instance.Environment.JingJie));
                    DialogCell C = new DialogCell(
                            titleText: "辩论",
                            detailedText: "你说到：“自其不变者而观之，则物与我皆无尽也，月亮是不变的。”\n只见第二个人非常赞同你的观点，给了你一些东西。" +
                                          "\n\n得到《永远》")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("永远"), jingJie: RunManager.Instance.Environment.JingJie));
                    DialogCell D = new DialogCell(
                        titleText: "辩论",
                        detailedText: "你话还没说完，那两人说你是个杠精，马上留下钱买了单，换了一家茶馆去聊天。\n你发现他们还剩下了一些额外的东西。" +
                                      "\n\n得到4金");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),

            new(id:                                 "Room0026",
                name:                               "二子学弈",
                description:                        "二子学弈",
                ladderBound:                        new Bound(8, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "学棋",
                        detailedText: "你看到一个老者在教两个学童下棋，两个学童在对弈，一名学童注视棋盘，另一名学童四处张望。",
                        "注视棋盘的学童能赢", "四处张望的学童能赢");

                    DialogCell B = new DialogCell(
                            titleText: "学棋",
                            detailedText: "你走近了，准备称赞注视棋盘的学童，顺着他的目光看向棋盘。" +
                                          "\n\n你们在对弈啊，你开口道。注视棋盘的学童说，说对弈太抬举我了，我和爷爷是在请教老师。" +
                                          "\n\n原来四处张望的学童竟然是老师，老者却是学子。" +
                                          "\n\n四处张望的学童转过身来对你说，以身入局才能看到事物真正的流向，孺子可教也。给你留了点东西。" +
                                          "\n\n得到《一心》")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("一心"), jingJie: RunManager.Instance.Environment.JingJie));
                    DialogCell C = new DialogCell(
                            titleText: "学棋",
                            detailedText: "你虽然相隔甚远，看不见棋盘，但是四处张望的学童神态自若，充满自信，你上去夸他。" +
                                          "\n\n他说到：你虽然眼神不在棋盘中，却也从场外信息判断出了我能赢，孺子可教也。给你留了点东西。" +
                                          "\n\n得到《童趣》")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("童趣"), jingJie: RunManager.Instance.Environment.JingJie));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    
                    return A;
                }),

            new(id:                                 "Room0027",
                name:                               "仙人下棋",
                description:                        "仙人下棋",
                ladderBound:                        new Bound(11, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "迷路",
                        detailedText: "你在竹林里迷路了，走了一阵遇到两个人在下棋，其中一个人发现了你，然后继续看棋盘去了。",
                        "尝试观看两人对弈（需要一张二动牌）", "请教两人路怎么走（需要一张治疗牌）");

                    CardPickerCell B = CardPickerCell.FromConstantDetailedText(
                        titleText:          "提交",
                        detailedText:       "请提交一张二动牌",
                        descriptor:         RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(RunSkillDescriptor.FromTagComposite(TagCategory.Swift), 1));
                    CardPickerCell C = CardPickerCell.FromConstantDetailedText(
                        titleText:          "提交",
                        detailedText:       "请提交一张治疗牌",
                        descriptor:         RunSkillDescriptorListModel.FromRunSkillDescriptorAndCount(RunSkillDescriptor.FromTagComposite(TagCategory.Health), 1));

                    DialogCell BWin = new(
                        titleText: "迷路",
                        detailedText: "你沉下心来仔细看这盘棋，在神识飘到很远的地方之前，回想起了你曾经学过的心法，保持住了自己的神识。",
                        options: "不知过了多久");
                    DialogCell BWin2 = new(
                        titleText: "迷路",
                        detailedText: "你沉浸在自己的世界里面，两人对弈完了，你和他们互相道别。走出竹林时，你感到自己的心法又精进了一步。\n\n得到《观棋烂柯》。");
                    BWin2.SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("观棋烂柯"), RunManager.Instance.Environment.JingJie));

                    DialogCell BLose = new(
                        titleText: "迷路",
                        detailedText: "虽然你沉下心来想要理解棋盘中发生了什么事，只见两人下棋越来越快，一息之间，那二人已下出千百步，你想说些什么，但是身体却来不及动。",
                        options: "不知过了多久");
                    DialogCell BLose2 = new(
                        titleText: "迷路",
                        detailedText: "你醒来时，那两人已经不在了。但是莫要紧，美美睡上一觉比什么都重要。命元+2。");
                    BLose2.SetReward(Reward.FromMingYuan(2));

                    DialogCell CWin = new(
                        titleText: "迷路",
                        detailedText: "你正向前走去，余光看到其中一人正好在一步棋点在天元。一瞬间你仿佛来到了水中，无法呼吸，你回想起了一段关于呼吸的功法，开始强迫自己吐纳，努力在这种环境下获取一些空气。",
                        options: "不知过了多久");
                    DialogCell CWin2 = new(
                        titleText: "迷路",
                        detailedText: "即使空气非常粘稠，你也可以呼吸自如。慢慢回到了正常的感觉，你悟出了一个关于吐纳的功法。");
                    CWin2.SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("玄武吐息法"), RunManager.Instance.Environment.JingJie));

                    DialogCell CLose = new(
                        titleText: "迷路",
                        detailedText: "你正向前走去，余光看到其中一人正好在一步棋点在天元。一瞬间你仿佛来到了水中，无法呼吸，肺部在不断哀嚎。",
                        options: "不知过了多久");
                    DialogCell CLose2 = new(
                        titleText: "迷路",
                        detailedText: "空气中的粘稠感终于消失。你赶紧大口吸气呼气，第一次感到空气是这么美好。气血上限+10。");
                    CLose2.SetReward(Reward.FromHealth(16));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    B.SetSubmitOperation(cardPickerCell =>
                    {
                        bool fulfilled = cardPickerCell.AllFulfilled();
                        if (!fulfilled)
                        {
                            cardPickerCell.RequirementSlotList.Do(requirementSlot =>
                            {
                                if (requirementSlot.Skill != null)
                                {
                                    RunManager.Instance.Environment.WithdrawToHandProcedure(WithdrawToHandDetails.FromSlot(requirementSlot));
                                }
                            });
                            return BLose;
                        }

                        return BWin;
                    });

                    C.SetSubmitOperation(cardPickerCell =>
                    {
                        bool fulfilled = cardPickerCell.AllFulfilled();
                        if (!fulfilled)
                        {
                            cardPickerCell.RequirementSlotList.Do(requirementSlot =>
                            {
                                if (requirementSlot.Skill != null)
                                {
                                    RunManager.Instance.Environment.WithdrawToHandProcedure(WithdrawToHandDetails.FromSlot(requirementSlot));
                                }
                            });
                            return CLose;
                        }

                        return CWin;
                    });

                    BWin[0].SetSelect(option => BWin2);
                    BLose[0].SetSelect(option => BLose2);
                    CWin[0].SetSelect(option => CWin2);
                    CLose[0].SetSelect(option => CLose2);

                    return A;
                }),

            new(id:                                 "Room0028",
                name:                               "检测仪",
                description:                        "检测仪",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A0 = new(
                        titleText: "检测仪",
                        detailedText: "你捡到了一个不曾见过的仪器，上面有5个按钮。你决定按下其中一个试试。",
                        "金", "水", "木", "下一页");

                    DialogCell A1 = new(
                        titleText: "检测仪",
                        detailedText: "你捡到了一个不曾见过的仪器，上面有5个按钮。你决定按下其中一个试试。",
                        "火", "土", "上一页");

                    DialogCell B = new DialogCell(
                        titleText: "检测仪",
                        detailedText: "仪表盘上出现了一个箭头，你顺着箭头望去，发现一本秘籍，随后仪器没电了。\n\n得到一张牌");
                    
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

            new(id:                                 "Room0029",
                name:                               "明心庐",
                description:                        "明心庐",
                ladderBound:                        new Bound(5, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "明心庐",
                        detailedText: "你看到一栋建筑，上面写着明心庐，见到了当地有名的解梦师。你请他解梦，他问你梦中是怎么样的？",
                        "挥舞着刀剑，大杀四方", "身披重甲，坚不可摧", "饮用着灵泉中的泉水");

                    DiscoverSkillCell B = DiscoverSkillCell.FromTitleDescription(room.Ladder, "明心庐", "选择1张梦中所想的牌");
                    
                    DialogCell C = new(
                        titleText: "明心庐",
                        detailedText: "这正是我现在需要的，先生真乃神通也。");
                    
                    JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(room.Ladder);
                    
                    A[0].SetSelect(option =>
                        B.SetDescriptor(new(tagComposite: TagCategory.Attack, jingJie: currJingJie, count: 3)));
                    A[1].SetSelect(option =>
                        B.SetDescriptor(new(tagComposite: TagCategory.Defend, jingJie: currJingJie, count: 3)));
                    A[2].SetSelect(option =>
                        B.SetDescriptor(new(tagComposite: TagCategory.Mana, jingJie: currJingJie, count: 3)));

                    B._receiveSignal = signal =>
                    {
                        if (signal is PickDiscoveredSkillSignal pickDiscoveredSkillSignal)
                        {
                            return C;
                        }
                        return B;
                    };

                    return A;
                }),

            new(id:                                 "Room0030",
                name:                               "天机阁",
                description:                        "天机阁",
                ladderBound:                        new Bound(2, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "天机阁",
                        detailedText: "你在沙漠中行走，突然眼前出来了一栋华丽的建筑，上面写着天机阁。你走入其中，前面有个牌子，请选择一张。你正在想是选择什么时，发现有十张卡牌浮在空中。");
                    ArbitraryCardPickerCell B = new(
                        titleText: "天机阁",
                        detailedText: "请从10张牌中选1张获取");
                    DialogCell C = new(
                        titleText: "天机阁",
                        detailedText: "刚一碰到那张卡牌，整个楼阁就突然消失不见，彷佛从未出现过一样。正当你不确定自己是否经历了一场幻觉时，发现留在手中的卡牌是真实的。于是你将这张卡牌收起。\n\n获得一张卡牌");

                    SkillEntryCollectionDescriptor descriptor = new(jingJie: RunManager.Instance.Environment.JingJie,
                        count: 10, consume: false);
                    GainSkillBuilder b = new();
                    b.Draw(descriptor);
                    B.PopulateInventory(b.DrawnSkillEntries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie)).ToList());
                    B.SetConfirmOperation(skills =>
                    {
                        GainSkillBuilder b = new();
                        skills.Do(item => b.Pick(item.Entry));
                        skills.Do(item => b.SingleCreate(item.JingJie));
                        b.Add();
                        b.Invoke();
                        return C;
                    });
                    
                    A[0].SetSelect(option => B);

                    return A;
                }),

            new(id:                                 "Room0031",
                name:                               "论无穷",
                description:                        "论无穷",
                ladderBound:                        new Bound(11, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "论无穷",
                        detailedText: "你听说有奖励，于是来参加了一场考试，内容是写一篇文章，题目是“论无穷”，要如何开题呢？" +
                                      "\n我每天跑步，只要一只能跑下去，跑的路程就是无穷的" +
                                      "\n有一种蛇，每天吃自己的尾巴，又长出来新的蛇身，永远吃不完，此谓无穷。" +
                                      "\n有个木桩，每天砍一半，过一万年也砍不完，这个叫做无穷。",
                        "看一眼蓝色服装考官", "看一眼绿色服装考官", "看一眼红色服装考官");

                    DialogCell B = new DialogCell(
                            titleText: "论无穷",
                            detailedText: "你痛快写了800字，时间没过5分钟，已经写完了。" +
                            "\n\n交卷之后，一名蓝色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张二动牌",
                            new(jingJie: RunManager.Instance.Environment.JingJie,
                                tagComposite: TagCategory.Swift)));
                    DialogCell C = new DialogCell(
                            titleText: "论无穷",
                            detailedText: "你提笔写起来。\n\n从前有座山，山里有座庙，庙里有考试，考试来考生，考生做文章，文章道从前，" +
                                          "从前有座山，山里有座庙，庙里有考试，考试来考生，考生做文章，文章道从前，" +
                                          "从前有座山，山里有座庙。。。\n\n你的文章还没写完，考试已经结束了。" +
                                          "\n\n交卷之后，一名绿色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张自指牌",
                            new(jingJie: RunManager.Instance.Environment.JingJie,
                                tagComposite: TagCategory.Growth)));
                    DialogCell D = new DialogCell(
                            titleText: "论无穷",
                            detailedText: "考试过了一半，你只写下了一句话。又过了一半的一半，你又写下了一句话。又过了一半的一半的一半，你再写下了一句话。。。" +
                                          "\n\n考试结束时，你已经把所有能写字的地方都写满了。" +
                                          "\n\n交卷之后，一名红色服装的考官对你的文章很有兴趣，给你留下了一些东西。")
                        .SetReward(new DrawSkillReward("得到一张升华牌",
                            new(jingJie: RunManager.Instance.Environment.JingJie,
                                tagComposite: TagCategory.Exhaust)));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "Room0032",
                name:                               "分子打印机",
                description:                        "分子打印机",
                ladderBound:                        new Bound(5, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "分子打印机",
                        detailedText: "你发现了一个机器，有两个插槽。中间写着一行说明，一边放原料，一边放卡牌。",
                        "试试这个机器可以做什么", "离开");

                    CardPickerCell B = CardPickerCell.FromConstantDetailedText(
                        titleText: "选择",
                        detailedText: "请选择2张牌，随机将其中一张变成另一张",
                        descriptor: RunSkillDescriptorListModel.FromCount(2));
                    DialogCell C = new(
                        titleText: "分子打印机",
                        detailedText: "来路不明的机器还是不要乱碰了，这个机器还是留给有缘人吧。");
                    DialogCell D = new(
                        titleText: "分子打印机",
                        detailedText: "劈里啪啦一阵响声过后，正在你担心自己的卡牌会受到什么非人的折磨的时候。机器的运转声停止了。打开后，你发现两个插槽里面的卡变成同一张了。\n\n得到两张牌");

                    B.SetSubmitOperation(cardPickerCell =>
                    {
                        bool fulfilled = cardPickerCell.AllFulfilled();
                        if (!fulfilled)
                        {
                            cardPickerCell.WithdrawAll();
                            return C;
                        }

                        int count = cardPickerCell.RequirementSlotList.Count();
                        RequirementSlot copyingSlot = cardPickerCell.RequirementSlotList[RandomManager.Range(0, count)];
                        RunSkill copyingSkill = copyingSlot.Skill;

                        cardPickerCell.RequirementSlotList.Do(requirementSlot =>
                        {
                            RunManager.Instance.Environment.ReplaceSkillProcedure(copyingSkill, requirementSlot.ToDeckIndex());
                        });
                        
                        cardPickerCell.WithdrawAll();
                        return D;
                    });

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "Room0033",
                name:                               "天界树",
                description:                        "天界树",
                ladderBound:                        new Bound(5, 15),
                difficultyBound:                    new Bound(4, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new("天界树",
                        "你知道自己在梦境里，天界树将你拉入了他的梦境，梦境中的东西都非常真实。",
                        "尝试感悟五行相生的规律");
                    
                    DialogCell B = new("天界树",
                        "你感受到了天界树的记忆。活，死，活，活，死，死，活，活，死死死死死死死。。。。。。活？" +
                                       "所有的生命都逐渐凋零，所有的死者彷佛又有了生命。你感觉如果继续感悟下去，仿佛手中的卡牌已经产生了某种变化，要继续感悟么？",
                        "继续感悟");
                    
                    DialogCell D = new("天界树", "金属遇寒，湿气冷凝成水，滴下来滋养了树苗，随即长成大树，燃烧起来，烧成了灰烬，归于尘土。" +
                                       "到最后，你已经不知道你是树，还是树是你了。" +
                                       "感悟了五行相生，所有五行牌都被相生的元素替换了。");
                    
                    CardPickerCell C = CardPickerCell.FromTianJieShu(room.Ladder, D);
            
                    A[0].SetSelect(option => B);
                    B[0].SetSelect(option => C);
            
                    return A;
                }),

            new(id:                                 "Room0033",
                name:                               "连抽五张",
                description:                        "连抽五张",
                ladderBound:                        new Bound(5, 8),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "急躁",
                        detailedText: "你近日练功，隐约感到一个瓶颈，心里略有不快。想着，如果全力一博，说不定就多一分机会窥见大道的真貌。",
                        "欲速则不达", "大力出奇迹（消耗30气血上限）");

                    DialogCell B = new DialogCell(
                            titleText: "急躁",
                            detailedText: "哪怕大道难行，进一寸有一寸的欢喜。虽然进度不是很快，也并非没有收获。\n\n得到一张牌")
                        .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: RunManager.Instance.Environment.JingJie)));
                    DialogCell C = new DialogCell(
                            titleText: "急躁",
                            detailedText: "随着喷出一大口鲜血，你回过神来，原来自己还活着，感谢大道没把自己留在那边。\n\n得到五张牌")
                        .SetReward(new DrawSkillReward("获得五个技能", new(jingJie: RunManager.Instance.Environment.JingJie, count: 5)));

                    A[0].SetSelect(option => B);
                    A[1].SetCost(new RunCostDetails(health: 30))
                        .SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "Room0034",
                name:                               "我已膨胀",
                description:                        "我已膨胀",
                ladderBound:                        new Bound(11, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "膨胀",
                        detailedText: "你打坐着，元神又来到了名为太虚的空间，传说达到了这个境界就会遭到天道的追杀，引来雷劫。",
                        "我已膨胀（高风险）", "浅尝则止");

                    DialogCell B = new DialogCell(
                        titleText: "膨胀",
                        detailedText: "你在太虚之中呆了良久，感受到了天道已经在注视自己的，然后大喊了一声，你过来啊。" +
                                      "轰！！你觉得自己元神归窍的速度已经很快了，不知怎么的，肉体还是受到了极大损伤。气血上限减少100。" +
                                      "万幸，太虚境中确实对修炼的提升很大。所有牌境界提升至最高。");
                    DialogCell C = new DialogCell(
                            titleText: "膨胀",
                            detailedText: "在太虚的边缘进行修炼确实大有好处，你感觉自己的体魄变得更加坚韧了")
                        .SetReward(Reward.FromHealth(16));

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.LoseHealthProcedure(150);
                        RunManager.Instance.Environment.TraversalDeckIndices().Do(deckIndex =>
                        {
                            RunSkill skill = RunManager.Instance.Environment.SkillFromDeckIndex(deckIndex);
                            if (skill == null)
                                return;

                            JingJie toJingJie = ((int)(skill.GetEntry().HighestJingJie)).ClampUpper(JingJie.HuaShen);
                            RunManager.Instance.Environment.SkillSetJingJieProcedure(toJingJie, deckIndex);
                        });

                        return B;
                    });
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "Room0035",
                name:                               "曹操三笑",
                description:                        "曹操三笑",
                ladderBound:                        new Bound(2, 5),
                difficultyBound:                    new Bound(2, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int normalGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);
                    int successGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder + 3);
                    
                    DialogCell A = new(
                        titleText: "护送",
                        detailedText: "有个商人要去其他国家，听闻中间有一个险道，常常有山贼出没，托你保护他和一些货物的安全。一路上没有什么障碍，赶了几天的路之后，终于快要到目的地了。" +
                                      "面前是一处山谷。这时候，他突然大笑起来：“哈哈哈哈哈哈哈哈。。。”",
                        "询问他何故突然大笑？（高风险）", "赶紧捂住他的嘴。");

                    DialogCell B = new(
                        titleText: "护送",
                        detailedText: "于是他解释道：”我看此等山贼都是少智无谋之辈，如果在此伏击我等，定然能让我们元气大伤。哈哈哈哈哈哈哈哈。。。“" +
                                      "\n只见他正在笑着，然后一伙山贼就出现了。",
                        options: "和山贼战斗");
                    DialogCell C = new DialogCell(
                        titleText: "护送",
                        detailedText: $"他有些不悦，但也没说什么。你们平安的走完了剩下的路程。\n\n获得{normalGoldReward}金")
                        .SetReward(Reward.FromGold(normalGoldReward));

                    map.EntityPool.TryDrawEntity(out RunEntity template, new EntityDescriptor(room.Ladder + 3));
                    BattleCell B1 = new(template);
                    DialogCell B1win = new DialogCell(
                            titleText: "护送",
                            detailedText: $"你打过了山贼，商人对你十分感激。\n\n获得{successGoldReward}金")
                        .SetReward(Reward.FromGold(successGoldReward));
                    DialogCell B1lose = new(
                        titleText: "护送",
                        detailedText: "你没打过山贼，货物被抢走了。所幸没有人受伤。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    B[0].SetSelect(option => B1);
                    B1.SetWinOperation(() => B1win);
                    B1.SetLoseOperation(() => B1lose);

                    return A;
                }),

            new(id:                                 "Room0036",
                name:                               "神灯精灵",
                description:                        "神灯精灵",
                ladderBound:                        new Bound(8, 11),
                difficultyBound:                    new Bound(2, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "许愿",
                        detailedText: "你捡到了一盏神灯里面跳出来了一个精灵，说可以实现你一个愿望",
                        "健康的体魄", "钱币的富裕", "这个愿望不被实现（高风险）");

                    DialogCell B = new DialogCell(
                            titleText: "许愿",
                            detailedText: "实现了，精灵留下了这句话带着神灯飞走了。你感觉身强体壮\n\n气血上限+8")
                        .SetReward(Reward.FromHealth(8));
                    DialogCell C = new DialogCell(
                            titleText: "许愿",
                            detailedText: "实现了，精灵留下了这句话带着神灯飞走了。你包里突然出来了很多金币\n\n金+8")
                        .SetReward(Reward.FromGold(8));
                    DialogCell D = new(
                        titleText: "许愿",
                        detailedText: "实现了。。额，实现不了。。哦，实现了。。。啊，实现不了。精灵说你比许愿再来十个愿望的人还会捣乱，召唤出来一个怪物，要来和你打一架。");

                    map.EntityPool.TryDrawEntity(out RunEntity template, new EntityDescriptor(room.Ladder + 3));
                    BattleCell E = new(template);
                    DialogCell EWin = new DialogCell(
                            titleText: "许愿",
                            detailedText: "哎，不就是都想要么？拿去拿去，好好说话我也不会不给的啊。\n\n气血上限+8，金+8")
                        .SetReward(new ResourceReward(gold: 8, health: 8));
                    DialogCell ELose = new(
                        titleText: "许愿",
                        detailedText: "哼，现在神灯精灵不好做了，就是因为经常碰见你这种人。下次别再让我遇见了。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);
                    D[0].SetSelect(option => E);
                    E.SetWinOperation(() => EWin);
                    E.SetLoseOperation(() => ELose);

                    return A;
                }),

            new(id:                                 "Room0037",
                name:                               "山木",
                description:                        "山木",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int trial = 0;
                    int rage = RandomManager.Range(0, 7);

                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);
                    
                    DialogCell A = new(
                        titleText: "生气",
                        detailedText: "一位老者做在石头上向周围人传教，虚己以游世，其孰能害之。说的是，只要你不把别人当个人，别人就不会引起你生气。你突然想逗他一下。",
                        "朝他作鬼脸", "狠狠戳他一下");

                    DialogCell B1 = new(
                        titleText: "生气",
                        detailedText: "他看起来有点生气了。",
                        "朝他作鬼脸", "狠狠戳他一下");
                    DialogCell B2 = new(
                        titleText: "生气",
                        detailedText: "他看起来非常生气了。",
                        "朝他作鬼脸", "狠狠戳他一下");

                    DialogCell D = new DialogCell(
                            titleText: "生气",
                            detailedText: $"你上去为自已的恶作剧道歉，他说还好，不会放在心上，这位学子应该学到了什么。\n\n获得{baseGoldReward}金")
                        .SetReward(Reward.FromGold(baseGoldReward));
                    DialogCell E = new DialogCell(
                            titleText: "生气",
                            detailedText: $"你上去为自已的恶作剧道歉，他喘了一口气，随即嘻笑开颜向大家解释道，这就是我刚才说的，不要随便生气。\n\n获得{baseGoldReward * 2}金")
                        .SetReward(Reward.FromGold(baseGoldReward * 2));
                    DialogCell F = new(
                        titleText: "生气",
                        detailedText: "你刚想上去为自己的恶作剧道歉。只见他不掩饰自己的怒火：“岂有此理啊，你有完没完啊！”你只能赶紧跑了。");

                    Cell SelectA(DialogOption option)
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
                    Cell SelectB(DialogOption option)
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

            new(id:                                 "Room0038",
                name:                               "丢尺子",
                description:                        "丢尺子",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);

                    DialogCell A = new(
                        titleText: "管家",
                        detailedText: "嘀嘀嘀。灵信响了，你看了一下。是之前委托你布阵的人发的消息。" +
                                      "\n管家：大人，之前你帮我家设置的阵法，有一笔的长度不对。会不会引起问题？" +
                                      "\n我：哪一笔，长度怎么不对了？" +
                                      "\n管家：坎位其中一划，我拿尺子量了，和其他的差了一分有余。",
                        "你多虑了，长度稍微差一点点没关系的。", "可以把你的尺子丢了么？");

                    DialogCell B = new(
                        titleText: "管家",
                        detailedText: "嘀嘀嘀。灵信响了，又是两天前那个管家。" +
                                      "\n管家：大人，我用尺子比了一下，有一笔稍微有些不直，会不会影响到阵法的效果？",
                        "你多虑了，笔划不需要完全直的。", "可以把你的尺子丢了么？");

                    DialogCell C = new(
                        titleText: "管家",
                        detailedText: "嘀嘀嘀。灵信又响了。还是几天前那个管家。" +
                                      "管家：大人，我又拿尺子笔划了许久，发现这个阵法还是有些问题，能劳烦你来一趟么，越快越好。",
                        "没问题，我今天就赶过去。", "可以把你的尺子丢了么？");

                    DialogCell D = new DialogCell(
                            titleText: "管家",
                            detailedText: $"你问了管家情况，发现都是些鸡毛蒜皮的小事。并不需要什么维护，就收了车马费。\n\n金+{baseGoldReward}")
                        .SetReward(Reward.FromGold(baseGoldReward));

                    DialogCell E = new DialogCell(
                            titleText: "管家",
                            detailedText: $"这阵子，管家没有再来打扰你了，应该是去打扰别人了。心情变好了一点。\n\n气血上限+{baseGoldReward}")
                        .SetReward(Reward.FromHealth(baseGoldReward));

                    A[0].SetSelect(option => B);
                    B[0].SetSelect(option => C);
                    C[0].SetSelect(option => D);
                    A[1].SetSelect(option => E);
                    B[1].SetSelect(option => E);
                    C[1].SetSelect(option => E);

                    return A;
                }),

            new(id:                                 "Room0039",
                name:                               "仙岛玉液酒",
                description:                        "仙岛玉液酒",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    bool mixWater = false;
                    bool yellNoLie = false;
                    bool expert = RandomManager.value < 0.5f;
                    
                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);

                    DialogCell A = new(
                        titleText: "市集",
                        detailedText: "你来到了一个市集，突发奇想想试试之前好不容易得到的酿酒秘方，说不定可以换些盘缠。到市集转了一圈，你发现需要的原材料价值不菲。" +
                                      "\n这里人人都喜欢酒，其中有些人可以鉴赏出酒的品质也说不定。\n\n你决定：",
                        "购买足量的原材料", "少买一些原材料，多加一些水");

                    DialogCell B = new(
                        titleText: "市集",
                        detailedText: "果然有一人对商品感到有兴趣。问你卖多少钱？",
                        options: "一百八一杯");

                    DialogCell BmixWater = new(
                        titleText: "市集",
                        detailedText: "果然有一人对商品感到有兴趣。问你卖多少钱？",
                        "一百八一杯", "既然加了些水，卖的便宜一些也合适，就八十卖你吧");

                    DialogCell C = new(
                        titleText: "市集",
                        detailedText: "你敢不敢喊一声蓬莱人不骗蓬莱人",
                        "蓬莱人不骗蓬莱人", "顾左右而言其他");

                    DialogCell D = new(
                        titleText: "市集",
                        detailedText: "那人将钱交予你，把酒拿走了。劳动真光荣。",
                        options: $"获得{baseGoldReward}金");

                    DialogCell[] EndingTable = new DialogCell[]
                    {
                        // mixWater, yellNoLie, expert
                        /* 0b000 */ new DialogCell(titleText: "市集", detailedText: "算了，我还是不买了。\n\n眼看市集就快结束了，你只好平价将酒出手了。\n\n不赚不赔"),
                        /* 0b001 */ new DialogCell(titleText: "市集", detailedText: $"你今天不卖给我，我就不走了。那人出高价来买你的酒，你含泪把钱收下了。\n\n获得{2 * baseGoldReward}金").SetReward(Reward.FromGold(2 * baseGoldReward)),
                        /* ob010 */ new DialogCell(titleText: "市集", detailedText: $"那人将钱交予你，把酒拿走了。劳动真光荣。\n\n获得{baseGoldReward}金").SetReward(Reward.FromGold(baseGoldReward)),
                        /* ob011 */ new DialogCell(titleText: "市集", detailedText: $"那人将钱交予你，把酒拿走了。劳动真光荣。\n\n获得{baseGoldReward}金").SetReward(Reward.FromGold(baseGoldReward)),
                        /* 0b100 */ new DialogCell(titleText: "市集", detailedText: "算了，我还是不买了。\n\n眼看市集就快结束了，你只好平价将酒出手了。\n\n不赚不赔"),
                        /* 0b101 */ new DialogCell(titleText: "市集", detailedText: $"哎，看你也不容易。那人虽然看出了你的酒兑了水，但还是有些良心，于是以正常价格买走了酒。\n\n获得{baseGoldReward}金").SetReward(Reward.FromGold(baseGoldReward)),
                        /* 0b110 */ new DialogCell(titleText: "市集", detailedText: $"那人将钱交予你，把酒拿走了。哇，小赚了一笔。\n\n获得{2 * baseGoldReward}金").SetReward(Reward.FromGold(2 * baseGoldReward)),
                        /* 0b111 */ new DialogCell(titleText: "市集", detailedText: $"你个奸商。那人抓住你，说要去官府。你只好摊也不顾了，赶紧溜了。\n\n失去{baseGoldReward}金").SetReward(Reward.FromGold(-baseGoldReward)),
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
            
            new(id:                                 "Room0040",
                name:                               "夏虫语冰",
                description:                        "夏虫语冰",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);
                    DialogCell A = new(
                        titleText: "季节",
                        detailedText: "你要过一个桥，桥上站了一人，问你，什么时候河会变得可以行走。你说在冬季的时候。他说你是胡说八道：“一年只有三个季节，春夏秋，哪里来的冬季？”",
                        "赞同他，说一年只有三个季节", "向他解释，说一年有四个季节");

                    DialogCell B = new DialogCell(
                            titleText: "季节",
                            detailedText: $"那人让你过去了，你感觉自己避免了一件麻烦事，心情大为畅快。\n\n气血上限+{baseGoldReward}")
                        .SetReward(Reward.FromHealth(baseGoldReward));
                    DialogCell C = new DialogCell(
                            titleText: "季节",
                            detailedText: $"一个月过去了，想过桥的人看到你们俩堵在桥中间，劝也劝不动，都想其他法子过桥了。那人的面容有所变化，但是嘴还是硬的。" +
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
                
            // new(id:                                 "卖剑",
            //     description:                        "卖剑",
            //     ladderBound:                        new Bound(0, 15),
            //     difficultyBound:                    new Bound(0, 11),
            //     withInPool:                         true,
            //     create:                             (map, room) =>
            //     {
            //         int baseGoldReward = RoomDescriptor.GoldRewardTable[room.Ladder];
            //         DialogPanelDescriptor A = new(
            //             titleText: "剑客",
            //             detailedText: "你遇到一位老剑客，他说自己有一把祖传宝剑要卖。" +
            //                         "\n\n"此剑削铁如泥，百炼精钢，是把难得的好剑。"老剑客说道，"只是我年事已高，已无力挥剑，不如转让有缘人。"" +
            //                         "\n\n你仔细一看，发现这把剑确实工艺精湛，但似乎并非他所说的宝剑。",
            //             "买下这把剑", "指出这把剑是赝品");

            //         DialogPanelDescriptor B = new DialogPanelDescriptor(
            //                 titleText: "剑客",
            //                 detailedText: $"你买下了这把剑。老剑客感慨道："能遇到懂剑之人，此剑不枉此生。"" +
            //                             "\n\n临走前，他教了你一招剑法。这招剑法看似普通，实则暗含玄机。" +
            //                             $"\n\n获得{baseGoldReward}气血上限")
            //             .SetReward(Reward.FromHealth(baseGoldReward));

            //         DialogPanelDescriptor C = new DialogPanelDescriptor(
            //                 titleText: "剑客",
            //                 detailedText: "老剑客大笑："年轻人好眼力！这确实不是什么宝剑，而是我年轻时随身佩剑。" +
            //                             "\n\n"我一生行走江湖，最大的领悟就是：剑在人在，剑亡人亡。重要的从来不是剑的品质，而是持剑之人的心境。"" +
            //                             $"\n\n老剑客赠你一些盘缠。获得{baseGoldReward}金")
            //             .SetReward(Reward.FromGold(baseGoldReward));
                    
            //         A[0].SetSelect(option => B);
            //         A[1].SetSelect(option => C);

            //         return A;
            //     }),

            new(id:                                 "Room0041",
                name:                               "守株待兔",
                description:                        "守株待兔",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(room.Ladder);
                    DialogCell A = new(
                        titleText: "守株待兔",
                        detailedText: "你见到一个人坐在树桩旁，问他在干什么，他说有兔子会撞上这个树桩，自己在等兔子撞死。",
                        "和他一起等待兔子", "买一只兔子放在树桩前，然后告诉他兔子来了");

                    DialogCell B = new DialogCell(
                            titleText: "守株待兔",
                            detailedText: $"你睡了过去，醒来时，那人说真的来了只兔子，要和你一起享用。休息好吃好后，你感觉自己状态变好了。气血+{baseGoldReward}")
                        .SetReward(Reward.FromHealth(baseGoldReward));
                    DialogCell C = new DialogCell(
                            titleText: "守株待兔",
                            detailedText: "你叫醒了那人，那人惊叹道：原来算命的说的是真的！你解释到是你们的运气好。" +
                                          "\n\n他激动地说道，之前有一个算命的人和他说，这里从来没有兔子也等不来兔子，但会有贵人经过，贵人会安排好兔子后，假装兔子是撞死的。" +
                                          "\n\n他当时还不信，原来真的可以等来兔子，哦不，贵人。" +
                                          $"\n\n然后邀请你到府上做客，向你问了一些问题。还赠送了你一些钱。金+{baseGoldReward}")
                        .SetReward(Reward.FromGold(baseGoldReward));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "Room0042",
                name:                               "鸡肉面",
                description:                        "鸡肉面",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         true,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "餐馆",
                        detailedText: "你到了餐馆，叫了碗面。只见厨师拿了一副鸡的画卷，剪开下了锅。你正疑惑他在干什么的时候，他从锅里盛出了一碗鸡肉面给你。",
                        "认为这是真的鸡肉面，吃下去", "拆穿他，说这碗面不过是幻术");

                    DialogCell B = new DialogCell(
                            titleText: "餐馆",
                            detailedText: "一开始还有些怀疑，然后发现就是真的面。于是美美的吃了一顿。命元+2")
                        .SetReward(Reward.FromMingYuan(2));
                    DialogCell C = new DialogCell(
                            titleText: "餐馆",
                            detailedText: "一阵烟雾过后。你面前掉落了一幅画，上面赫然画着刚才的餐馆。你对之前的招式又有了新的感悟。获得一张牌")
                        .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: RunManager.Instance.Environment.JingJie)));
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),
            
            #endregion

            #region 05_FanXu
            
            new(id:                                 "Room05_001",
                name:                               "斩断尘缘",
                description:                        "斩断尘缘",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new("斩断尘缘",
                        "和最终的首领战斗过于激烈，你的元神到达了一个未知的空间。在你还没想到要怎么回到现实世界时。" +
                        "\n一颗无比熟悉的树吸引了你的注意。你越发感觉如果继续看下去，就难以返回现实了。",
                        "继续看下去");
                    
                    DialogCell B = new("斩断尘缘",
                        "你隐约察觉这棵树在告诉你，他可能帮助你斩断和卡牌的因果，让你再也无法回忆起技能使用时的细节。" +
                        "\n你有想要忘却的过去么？",
                        "回忆开始变得模糊");
                    
                    DialogCell D = new("斩断尘缘", "事情发生得太快，一瞬间，你已经不记得自己用了什么和眼前的树做了交易，甚至连交易本身是否存在都已经不确定了。" +
                                               "\n你趁着自己还清醒着，检查了新的技能，然后悻悻离去。");
                    
                    CardPickerCell C = CardPickerCell.FromZhanDuanChenYuan(room.Ladder, D);
            
                    A[0].SetSelect(option => B);
                    B[0].SetSelect(option => C);
            
                    return A;
                }),
            
            new(id:                                 "Room05_002",
                name:                               "无名泉水",
                description:                        "无名泉水",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new("无名泉水",
                        "你走到一处泉水旁边，你查觉泉水灵力充沛，正好可以治愈自己一身的伤痛。" +
                        "\n自己是怎么受伤的？你正在思索着，然后看到前方有个穿着奇异的小孩看着自己。");
                    
                    DialogCell B = new("无名泉水",
                        "“你不该在这里的。”" +
                        "\n“妄图以区区凡人之躯，向仙人的领域发起挑战，真是傲慢啊。”");
                    
                    DialogCell C = new("无名泉水",
                        "“算了，看在你这么不知死活的份上。我帮你激发一下潜力，对身体造成的小小负担，你可要承受住了。”" +
                        "你感受到，刚刚回复的命元，正在一点一点的从体内流失。");
                    
                    DialogCell D = new("无名泉水",
                        "“不用谢我。这些身外之物你暂时也用不到了，我替你收好了。”" +
                        "给你点什么呢？再帮你强化一下身体好了。");
                    
                    DialogCell E = new("无名泉水",
                        "你看到一阵血雾向自己飞来，然后逐渐被自己所吸收。感觉经脉确实是强壮了一些。");

                    A[0].SetSelect(option =>
                    {
                        MingYuan mingYuan = RunManager.Instance.Environment.GetMingYuan();
                        int gap = mingYuan.UpperBound - mingYuan.Curr;
                        RunManager.Instance.Environment.SetDMingYuanProcedure(gap);
                        return B;
                    });

                    B[0].SetSelect(option => C);

                    C[0].SetSelect(option =>
                    {
                        MingYuan mingYuan = RunManager.Instance.Environment.GetMingYuan();
                        int space = Mathf.Max(mingYuan.Curr - 1, 0);
                        RunManager.Instance.Environment.SetDMingYuanProcedure(-space);
                        
                        GainSkillBuilder b = new();
                        for (int i = 0; i < space; i++)
                        {
                            b.Pick(Encyclopedia.SkillCategory.FromName("命石"));
                        }
                        b.Create(JingJie.HuaShen);
                        for (int i = 0; i < space; i++)
                        {
                            b.RecordDeckIndex(new NextHandDeckIndexDefinition());
                        }
                        b.Add();
                        b.Invoke();
                        
                        return D;
                    });

                    D[0].SetSelect(option =>
                    {
                        int value = RunManager.Instance.Environment.GetGold().Curr;
                        RunManager.Instance.Environment.SetDGoldProcedure(-value);
                        RunManager.Instance.Environment.GainHealthProcedure(value);
                        return E;
                    });
                    
                    return A;
                }),
            
            new(id:                                 "Room05_003",
                name:                               "气血商店",
                description:                        "气血商店",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new("气血商店",
                        "你看到一个商店，正懊恼着刚才的小孩将自己的金钱取走了。可惜了这么多商品。" +
                        "\n结果你看到商品没有标价，带着奇怪的马脸面具的店家说这里只收气血。",
                        "怪不得感到一阵阴风");

                    ShopCell B = ShopCell.FromFanXuHealthShop(room.Ladder);
                    
                    DialogCell C = new("气血商店",
                        "你想到自己气血是多么的宝贵，此时再有了这些技能对自己又有什么帮助呢。不禁一阵唏嘘。");

                    A[0].SetSelect(option => B);

                    B._receiveSignal = signal =>
                    {
                        if (signal is ExitShopSignal)
                            return C;
                        return B;
                    };
                    
                    return A;
                }),
            
            new(id:                                 "Room05_004",
                name:                               "命元商店",
                description:                        "命元商店",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new("命元商店",
                        "又路过一个商店，你在奇怪，这家商店会不会正常一些。" +
                        "\n看到阴森的街道，你感到了一丝不安。" +
                        "\n带着牛脸面具的人向你招呼道，再往前走，命石以对你无用，何不来我这里消费消费？",
                        "你虽然脚还再往前走，但是商品还是吸引到了你的注意");

                    BarterCell B = BarterCell.FromFanXuMingYuanShop();

                    DialogCell C = new("命元商店",
                        "你回想起了修仙路上的人，包括曾经的自己，曾经竟为了一时的胜负而大打出手，真是可笑。");
                        
                    A[0].SetSelect(option => B);

                    B._receiveSignal = signal =>
                    {
                        if (signal is ExitShopSignal)
                            return C;
                        return B;
                    };
                    
                    return A;
                }),
            
            new(id:                                 "Room05_005",
                name:                               "镜中世界",
                description:                        "镜中世界",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new("镜中世界",
                        "路边有一个镜子，你不知缘由走入了镜中。" +
                        "四个一模一样镜灵看着你，分别向你讨要一张卡牌。");

                    CardPickerCell B = CardPickerCell.FromConstantDetailedText(
                        titleText: "选择",
                        detailedText: "请选择0~4张牌，将所有牌变成其中随机的一张。" +
                                      "\n镜灵们看起来对你的命石很感兴趣，你有些担心他们不会还给你了。",
                        descriptor: RunSkillDescriptorListModel.FromCount(4));
                    
                    DialogCell C0 = new("镜中世界",
                        "你拒绝了镜灵的提议，决定继续前行。");
                    
                    DialogCell C1 = new("镜中世界",
                        "你选择了1张牌递给了其中一个镜灵，镜灵们都看着你摇了摇头。");
                    
                    DialogCell C2 = new("镜中世界",
                        "你选择了2张牌递给了其中两个镜灵，两个镜灵看了一眼你的卡牌，然后还给了你，你发现2张牌变成同一张牌了。");
                    
                    DialogCell C3 = new("镜中世界",
                        "你选择了3张牌递给了其中三个镜灵，三个镜灵看了一眼你的卡牌，然后还给了你，你发现3张牌变成同一张牌了。");
                    
                    DialogCell C4 = new("镜中世界",
                        "你选择了4张牌递给了每个镜灵，镜灵们看了一眼你的卡牌，然后还给了你，你发现4张牌变成同一张牌了。");
                    
                    DialogCell CN = new("镜中世界",
                        "你将牌递给了镜灵之后，镜灵不愿意将牌还给你了，并把你赶出了镜中世界。真是贪婪的镜灵。");

                    DialogCell[] CList = new DialogCell[] { C0, C1, C2, C3, C4 };

                    B.SetSubmitOperation(cardPickerCell =>
                    {
                        List<int> indices = new();
                        List<int> possibleIndices = new();
                        for (int i = 0; i < cardPickerCell.RequirementSlotList.Count(); i++)
                        {
                            if (cardPickerCell.RequirementSlotList[i].Skill == null)
                                continue;
                            if (cardPickerCell.RequirementSlotList[i].Skill.GetEntry() !=
                                Encyclopedia.SkillCategory.FromName("命石"))
                            {
                                possibleIndices.Add(i);
                            }
                            indices.Add(i);
                        }

                        int count = indices.Count;
                        int possibleCount = possibleIndices.Count;
                        if (count == 0)
                            return CList[0];

                        if (possibleCount == 0)
                            return CN;

                        int copyingIndex = possibleIndices[RandomManager.Range(0, possibleCount)];
                        RequirementSlot copyingSlot = cardPickerCell.RequirementSlotList[copyingIndex];
                        RunSkill copyingSkill = copyingSlot.Skill;

                        foreach (int index in indices)
                        {
                            RequirementSlot slot = cardPickerCell.RequirementSlotList[index];
                            RunManager.Instance.Environment.ReplaceSkillProcedure(copyingSkill, slot.ToDeckIndex());
                        }
                        
                        cardPickerCell.WithdrawAll();

                        return CList[count];
                    });

                    A[0].SetSelect(option => B);

                    return A;
                }),
            
            new(id:                                 "Room05_006",
                name:                               "空荡回廊",
                description:                        "空荡回廊",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new("空荡回廊",
                        "你走进了一个黑漆漆的大厅中。" +
                        "“有人么？”，你大喊道。",
                        "继续");
                    
                    DialogCell B = new("空荡回廊",
                        "没有得到回应。你只好继续走下去。你有预感，这场旅途总会迎来一个终点。");

                    A[0].SetSelect(option => B);
                    
                    return A;
                }),
            
            new(id:                                 "Room05_007",
                name:                               "返虚战斗",
                description:                        "返虚战斗",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new("返虚战斗",
                        "过了不知多少年，你已经变老，和孙子辈的人在吹嘘当年自己是怎么打败一个又一个的敌人。" +
                        "\n小孩们说你又在吹牛。你气急败坏，描述起了细节，我跟你们说，其中最凶险的一次。" +
                        "\n只见对方这样一个招式过来，然后我就一挡，再。。。我就一挡？然后就。。",
                        "睁眼");
                    
                    DialogCell B = new("返虚战斗",
                        "可能自己是太累了，讲着讲着就睡着了。小孩们拉起你的手，说故事不好听了，一起出去转转吧。" +
                        "\n你一边答应着，一边觉得站起来有些费劲。",
                        "睁眼");
                    
                    DialogCell C = new("返虚战斗",
                        "自己怎么趴在工作桌上睡着了。旁边是自己编撰的小说，基于年轻时的冒险经历。" +
                        "\n哎，一会吃碗面然后休息一下吧。" +
                        "\n稿件快赶不上时间了，细节的部分要跳过一些了。你想到。",
                        "睁眼！！！");
                    
                    DialogCell D = new("返虚战斗",
                        "你发现了自己正在出于和敌人的大战之中。敌人看到你还活着有些惊讶。" +
                        "\n仿佛意思是刚才那一招出手，自己应该已经死了。",
                        "战斗");
                    
                    EntityEntry finalBoss = RunManager.Instance.Environment.HuaShenBossEntity ?? Encyclopedia.EntityCategory.FromName("凌霄大圣");
                    bool encore = RunManager.Instance.Environment.GetRunConfig().DifficultyProfile.GetEntry()
                        .FanXuBossEncore;
                    
                    RunEntity[] entities = AppManager.Instance.EditorManager.EntityEditableList
                        .FilterObj(e => e.GetEntry() == finalBoss && e.GetJingJie() == JingJie.FanXu)
                        .ToArray();
                    
                    Assert.IsTrue(entities.Length >= 2);

                    FinitePool<RunEntity> pool = new FinitePool<RunEntity>();
                    pool.Populate(entities);
                    pool.Shuffle();
                    pool.TryPopItem(out RunEntity firstBoss);
                    pool.TryPopItem(out RunEntity secondBoss);
                    
                    BattleCell battleCell1 = new(firstBoss);
                    BattleCell battleCell2 = new(secondBoss);
                    
                    DialogCell firstWinNoEncore = new("返虚战斗",
                        "终于胜利了。比起胜利的喜悦，享受此时片刻的安宁对你来说更为重要。");
                    
                    DialogCell firstWinWithEncore = new("返虚战斗",
                        "你知道你的胜利只是侥幸。你的招式短暂的突破到了返虚的境界，但是肉体欠的功夫可不只是一朝一夕能达到的。" +
                        "\n只见对手并不承认你的胜利，换了一套招式，再度向你袭来。");
                    
                    DialogCell secondWin = new("返虚战斗",
                        "好几次，你都以为自己已经要被打败了，只不过到了最后关头，自己的术法总是快了一步。" +
                        "\n从未经历这种级别的战斗。出乎意料的，你现在脑海中想的不是功法啊，修为啊，这种自己一直以来追求的。" +
                        "\n自己这个境界长久不进食，也不会有问题。但是第一次，你开始思索着，今天晚饭吃什么好呢？");

                    A[0].SetSelect(option => B);
                    B[0].SetSelect(option => C);
                    C[0].SetSelect(option => D);
                    D[0].SetSelect(option => battleCell1);
                    battleCell1.SetWinOperation(() => encore ? firstWinWithEncore : firstWinNoEncore);
                    battleCell1.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Defeated);
                        return null;
                    });
                    
                    firstWinNoEncore[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Victorious);
                        return null;
                    });

                    firstWinWithEncore[0].SetSelect(option => battleCell2);
                    
                    battleCell2.SetWinOperation(() => secondWin);
                    battleCell2.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Defeated);
                        return null;
                    });
                    
                    secondWin[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Victorious);
                        return null;
                    });

                    return A;
                }),
            
            new(id:                                 "Room05_008",
                name:                               "返虚三战斗",
                description:                        "返虚三战斗",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    EntityEntry finalBoss = RunManager.Instance.Environment.HuaShenBossEntity ?? Encyclopedia.EntityCategory.FromName("凌霄大圣");
                    bool encore = RunManager.Instance.Environment.GetRunConfig().DifficultyProfile.GetEntry()
                        .FanXuBossEncore;
                    
                    RunEntity[] entities = AppManager.Instance.EditorManager.EntityEditableList
                        .FilterObj(e => e.GetEntry() == finalBoss && e.GetJingJie() == JingJie.FanXu)
                        .ToArray();
                    
                    Assert.IsTrue(entities.Length >= 3);

                    FinitePool<RunEntity> pool = new FinitePool<RunEntity>();
                    pool.Populate(entities);
                    pool.Shuffle();
                    pool.TryPopItem(out RunEntity firstBoss);
                    pool.TryPopItem(out RunEntity secondBoss);
                    pool.TryPopItem(out RunEntity thirdBoss);
                    
                    BattleCell battleCell1 = new(firstBoss);
                    BattleCell battleCell2 = new(secondBoss);
                    BattleCell battleCell3 = new(thirdBoss);
                    
                    battleCell1.SetWinOperation(() => battleCell2);
                    battleCell1.SetLoseOperation(() => battleCell2);
                    
                    battleCell2.SetWinOperation(() => battleCell3);
                    battleCell2.SetLoseOperation(() => battleCell3);
                    
                    battleCell3.SetWinOperation(() =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Victorious);
                        return null;
                    });
                    battleCell3.SetLoseOperation(() =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Defeated);
                        return null;
                    });

                    return battleCell1;
                }),
            
            #endregion

            #region 06_Reserved

            new(id:                                 "Room0043",
                name:                               "忘忧堂",
                description:                        "忘忧堂",
                ladderBound:                        new Bound(5, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "割舍",
                        detailedText: "你来到了忘忧堂，听说这里的服务是将不想再见到的卡牌交给他们。忘忧堂会为您斩断与此牌的因果。",
                        "走进去看一看", "离开");

                    CardPickerCell B = CardPickerCell.FromConstantDetailedText(
                        titleText: "割舍",
                        detailedText: "请选择0到5张牌送出",
                        descriptor: RunSkillDescriptorListModel.FromCount(5));
                    DialogCell C = new(
                        titleText: "割舍",
                        detailedText: "果然还是难以割舍心爱的卡牌。");
                    DialogCell D = new(
                        titleText: "割舍",
                        detailedText: "你感到身上轻了一些。");

                    B.SetSubmitOperation(cardPickerCell =>
                    {
                        if (!cardPickerCell.AnyFulfilled())
                        {
                            return C;
                        }
                        
                        cardPickerCell.RequirementSlotList.Do(requirementSlot =>
                        {
                            RunSkill skillToRemove = requirementSlot.Skill;
                            if (skillToRemove == null)
                                return;

                            RunManager.Instance.Environment.SkillPool.Depopulate(pred: e => e == skillToRemove.GetEntry());
                        });
                        
                        return D;
                    });

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),
            
            new(id:                                 "Room0044",
                name:                               "人间世",
                description:                        "人间世",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "功名",
                        detailedText: "你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你追上了他，看出他事业心很重，于是对他说：" +
                                      "\n\n成名要趁早，我看你将来肯定是做宰相的料。" +
                                      "\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。即使如此，你还是要追求功名么？",
                        "用第一个想法", "用第二个想法");

                    DialogCell B1 = new(
                        titleText: "功名",
                        detailedText: "感谢你这么夸我，但是现在我也没有钱给你。",
                        options: "时间一下过了60年");
                    DialogCell C1 = new(
                        titleText: "功名",
                        detailedText: "先生谬论不可再提，你看那胡人会因为我们不锻造兵器，充实军备而不来侵略我们么？",
                        options: "时间一下过了60年");

                    DialogCell B2 = new DialogCell(
                        titleText: "功名",
                        detailedText: "当年的少年已经成为了宰相。见到了你，发现你的容貌60年没有发生变化，察觉你是仙人，于是说道，感谢仙人提拔。叫人给了你收藏的宝物。\n\n得到1机关牌");
                    DialogCell C2 = new DialogCell(
                            titleText: "功名",
                            detailedText: "当年的少年已经成为了宰相。见到了你，完全没有印象，只道是某个江湖中人来攀亲道故，于是叫下人给了你点盘缠打发了。\n\n得到10金")
                        .SetReward(Reward.FromGold(10));

                    DialogCell D = new(
                        titleText: "功名",
                        detailedText: "你看到一个少年盯着功名榜。少顷，嘴角露出一抹微笑，然后转身离开。你正向追上他说点什么，却被一颗小石子绊倒，起身已经不见那人踪影。于是道：“罢了罢了，缘分未到。”",
                        options: "时间一下过了60年");
                    DialogCell D2 = new(
                        titleText: "功名",
                        detailedText: "你又见到了当年的少年。现在他已经成为了宰相。你想着对他说些什么：" +
                                      "\n\n成名要趁早，宰相一生过得荣华富贵。。。" +
                                      "\n\n你看这些树，长了果子的树枝遭人摧残而早死，木质良好的被人砍去做成船了，就这棵无用的树才活得长久。哪怕功名已成恐怕也是路途险阻。",
                        "用第一个想法", "用第二个想法");

                    DialogCell E = new DialogCell(
                            titleText: "功名",
                            detailedText: "只见你话还没说完，宰相就摆手示意你离开。叫下人给了你点盘缠将你打发了。\n\n得到10金")
                        .SetReward(Reward.FromGold(10));
                    DialogCell F = new DialogCell(
                        titleText: "功名",
                        detailedText: "宰相回复到，先生说的属实，若是我早点知道了这些道理，也不至于一生过的如此跌宕起伏。叫人给了你收藏的宝物。\n\n得到1机关牌");

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

            new(id:                                 "Room0045",
                name:                               "照相机",
                description:                        "照相机",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "赏花",
                        detailedText: "到了桃花盛开的季节，你也来欣赏桃花。见到一名机关师，向人们介绍自己最近的新发明。按一下按钮，这个机关就可以将眼前美景永远记录下来。" +
                                      "\n周围人看了那个机关，觉得画过于真实，害怕这个机关能够摄人心魄。都纷纷不敢上前。" +
                                      "\n那人邀请你实验一下他的新发明。你站好之后，他叫你喊，一，二，三，茄子。然后启动了两次机关。果然出现了两张相片。一张优雅俊美，另一张略有瑕疵，可能是机关启动的时机并不完美。" +
                                      "\n那人向你说道：“先生不如选一张，然后将另一张放在我这里，这样我们看见相片就能会想起，今时今日，曾一起赏桃花。”",
                        "拿走优雅俊美的那一张", "拿走略有瑕疵的那一张", "先生曾听过，人生苦短，及时行乐");

                    DialogCell B = new DialogCell(
                            titleText: "赏花",
                            detailedText: "你把这张相片放在了挂在了你的大堂里，寻求你帮助的人看到你俊美的相貌，愿意以更高价钱请你出力。金+20。")
                        .SetReward(Reward.FromGold(20));
                    DialogCell C = new DialogCell(
                            titleText: "赏花",
                            detailedText: "你把好的相片留给了机关师，这样他日后看到相片的时候，就会感到这段回忆多一分美好。这样想到，你的心情变好了。气血上限+10")
                        .SetReward(Reward.FromHealth(10));
                    DialogCell D = new DialogCell(
                        titleText: "赏花",
                        detailedText: "你对那人说：”相必先生也知道，美好的时光总是短暂的。这个机关，可以将美好的时光记录下来，之后就可以看着照片反复回忆。" +
                                      "但真的如此做的话，处于桃林中的我们也会因为知道相片可以反复回忆，反倒不去珍惜此时此刻的美景了。这难道不是本末倒置了么？“" +
                                      "\n\n那人稍微惊讶于你的说法。然后感叹道：“先生教训的在理。不应以人生长而感到美好，也不应以短而感到苦恼。这个机关已经于我无用，这便赠与先生吧。”" +
                                      "你收下了这个机关，但是并不会维护。与其看着它坏掉，不如将其中有用的零件取出。\n\n得到1机关");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[2].SetSelect(option => D);

                    return A;
                }),
            
            new(id:                                 "Room0046",
                name:                               "矛与盾",
                description:                        "矛与盾",
                ladderBound:                        new Bound(0, 2),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "矛盾",
                        detailedText: "你看见一个人在推销他的矛和盾，说是这矛可以打碎所有的盾，这盾可以抵挡住所有的矛。周围人起哄说要他拿他的矛戳他的盾。",
                        "你感觉矛厉害", "你感觉盾厉害", "离开，不参与热闹");
                    
                    Puzzle bPuzzle = new(
                        description: "尝试移除对方的护甲",
                        condition: "第六回合时，目标护甲小等于0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                        }),
                        kernel: new StageKernel(async d =>
                        {
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (d.Forced)
                            {
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (d.Turn < 6)
                                    return 0;

                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }

                            if (d.Flag == 0)
                                return d.Flag;

                            await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
                            
                            d.Env.RecordResult(d.Flag);
                            
                            return d.Flag;
                        })
                    );
        
                    PuzzleCell B = new(bPuzzle);
                    DialogCell BPass = new DialogCell(
                        titleText: "矛盾",
                        detailedText: "获得一张攻击牌。"); // TODO
                    
                    Puzzle cPuzzle = new(
                        description: "尝试保持护甲",
                        condition: "第六回合时，护甲大于0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                        }),
                        kernel: new StageKernel(async d =>
                        {
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (d.Forced)
                            {
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (d.Turn < 6)
                                    return 0;

                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }

                            if (d.Flag == 0)
                                return d.Flag;

                            await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
                            
                            d.Env.RecordResult(d.Flag);
                            
                            return d.Flag;
                        })
                    );
        
                    PuzzleCell C = new(cPuzzle);
                    DialogCell CPass = new DialogCell(
                        titleText: "矛盾",
                        detailedText: "获得一张护甲牌。"); // TODO

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

            new(id:                                 "Room0047",
                name:                               "郑人买履",
                description:                        "郑人买履",
                ladderBound:                        new Bound(2, 5),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "逛街",
                        detailedText: "你打算买双鞋子。来到集市，看到一名男子也打算买鞋子，但是忘了带量好的尺码了，商家说现在量一下他的脚不就有尺码了么。你发现你也忘带尺码了。",
                        "回家去取尺码", "现场重新量尺码");

                    DialogCell B = new(
                        titleText: "逛街",
                        detailedText: "回家取了尺码后，你再次来到集市，发现集市已经结束了，没能买到鞋子。" +
                                      "\n\n又发现那名男子，他也注意到了你，他将量好的尺码挂在一名关门的商户门前后，和你说，你也是来找修补匠打造机关鞋的么？" +
                                      "好眼光啊，可惜上午来发现今天不开门。于是将量好的尺码挂着这里预定一个鞋子。你也学着他将尺码挂在了门前。" +
                                      "\n\n一小段时间后，得到《飞鞋》");
                    DialogCell C = new(
                        titleText: "逛街",
                        detailedText: "你本意只想买个鞋子，却承载不住商家的热情，被推销了一个东西。得到《目镜》");
                    
                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    
                    return A;
                }),

            new(id:                                 "Room0048",
                name:                               "鬼兵",
                description:                        "鬼兵",
                ladderBound:                        new Bound(2, 5),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "仪式感",
                        detailedText: "你看到鬼兵打算带走一个将死之人，但是那人请求鬼兵在给自己一点时间。鬼兵说那人的命元已尽，不该继续留在阳间",
                        "助他炼丹（需要一张牌）", "给他传气（需要一命元）", "帮他造业（需要100金）", "装作看不见");

                    CardPickerCell BPick = CardPickerCell.FromConstantDetailedText(
                        titleText: "仪式感",
                        detailedText: "炼丹需要消耗一张牌",
                        descriptor: RunSkillDescriptorListModel.FromCount(1));
                    DialogCell B = new(
                        titleText: "仪式感",
                        detailedText: "你取出了一张卡牌作为原料，炼出了一枚丹药，给那人吃了。" +
                                      "\n\n随后，那人打开了一个机关，一只乐曲从那机关中播出，然后说，这下就有仪式感了。然后心满意足的和鬼兵离开了");
                    DialogCell C = new(
                        titleText: "仪式感",
                        detailedText: "你使用了自已的命元，给他传了过去。" +
                                      "\n\n随后，那人打开了一个机关，一只乐曲从那机关中播出，然后说，这下就有仪式感了。然后心满意足的和鬼兵离开了");
                    DialogCell D = new(
                        titleText: "仪式感",
                        detailedText: "你向鬼兵偷偷一笑，将一物塞到鬼兵怀中，鬼兵摸了一下，也向你一笑。" +
                                      "\n\n随后，那人打开了一个机关，一只乐曲从那机关中播出，然后说，这下就有仪式感了。然后心满意足的和鬼兵离开了");

                    DialogCell E = new(
                        titleText: "仪式感",
                        detailedText: "那人已经离开。留下了这个机关在这里，你非常好奇，想必是哪位墨苑大家留下来的手笔。留在这里也是可惜，你取出了其中有用的机关带走了。\n\n得到两个机关");

                    A[0].SetSelect(option => BPick);
                    BPick.SetSubmitOperation(cardPickerCell =>
                    {
                        if (!cardPickerCell.AnyFulfilled())
                            return A;
                        
                        return B;
                    });

                    A[1].SetCost(new RunCostDetails(mingYuan: 1))
                        .SetSelect(option => C);
                    A[2].SetCost(new RunCostDetails(gold: 100))
                        .SetSelect(option => D);
                    B[0].SetSelect(option => E);
                    C[0].SetSelect(option => E);
                    D[0].SetSelect(option => E);

                    return A;
                }),

            new(id:                                 "Room0049",
                name:                               "刻舟求剑",
                description:                        "刻舟求剑",
                ladderBound:                        new Bound(5, 8),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "渡江",
                        detailedText: "坐船渡江途中，有个人不小心，随身携带的剑匣掉到江里了。然后马上打开随身的备忘录写了点东西。",
                        "和他说，快下去捞啊", "下水帮他将宝剑捞上来");

                    DialogCell B = new DialogCell(
                        titleText: "渡江",
                        detailedText: "他笑道，这条船，每天都这个航线，这个点渡江，我今天行装不方便下水，我已经记下了此时时刻。明天经过这里的时候在捞不迟。");
                        // .SetReward(new DrawSkillReward("获得一个技能", new(jingJie: map.JingJie)));
                    DialogCell C = new DialogCell(
                        titleText: "渡江",
                        detailedText: "他结果剑匣，惆怅的看着剑匣。");

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),

            new(id:                                 "Room0050",
                name:                               "物质还原仪",
                description:                        "物质还原仪",
                ladderBound:                        new Bound(8, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "物质还原仪",
                        detailedText: "你发现了一个机器，有一个插槽。旁边有一个剪刀的按钮。",
                        "试试这个机器可以做什么", "离开");

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
            
            new(id:                                 "Room0051",
                name:                               "悟道",
                description:                        "悟道",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    int[] combination = Numeric.GetCombination(5, 3);
                    WuXing[] options = new WuXing[3];
                    for (int i = 0; i < options.Length; i++)
                        options[i] = WuXing.FromIndex(combination[i]);

                    DialogCell A = new(
                        titleText: "悟道",
                        detailedText: "选择一种五行，获得一张随机牌",
                        options[0].GetName(), options[1].GetName(), options[2].GetName());

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

            new(id:                                 "Room0052",
                name:                               "愿望单",
                description:                        "愿望单",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "愿望单",
                        detailedText: "游戏仍在制作中，请加入愿望单，以关注后续进展，感谢游玩！",
                        "Q群：216060477", "游戏名：蓬莱之旅", "返回标题");

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

            #region 07_Series

            new(id:                                 "Room0053",
                name:                               "后羿1",
                description:                        "后羿1",
                ladderBound:                        new Bound(0, 5),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "后羿",
                        detailedText: "你看到了一个少年在幸苦的练习射箭，但是进度缓慢。你发现是因为少年天生视力不好。",
                        "给少年展示技术", "赠与少年一本秘籍", "告诉少年实话");
                    Puzzle puzzle = new(
                        description: "尝试帮助少年击中目标",
                        condition: "目标受到伤害",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                        }),
                        kernel: new StageKernel(async d =>
                        {
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (d.Forced)
                            {
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (d.Turn < 6)
                                    return 0;

                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }

                            if (d.Flag == 0)
                                return d.Flag;

                            await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
                            
                            d.Env.RecordResult(d.Flag);
                            
                            return d.Flag;
                        })
                    );
        
                    PuzzleCell B = new(puzzle);
                    DialogCell BPass = new DialogCell(
                        titleText: "后羿",
                        detailedText: "少年将你的招式记在了心里，又开始了自顾自的练习。",
                        options: "继续上路");
                    
                    CardPickerCell C = CardPickerCell.FromConstantDetailedText(
                        titleText:          "后羿",
                        detailedText:       "请提交一张牌",
                        descriptor:         RunSkillDescriptorListModel.FromCount(1));
        
                    DialogCell CWin = new(
                        titleText: "后羿",
                        detailedText: "少年感谢你赠与的秘籍，已经准备好开始练习了。",
                        options: "继续上路");

                    DialogCell D =
                        new(
                            titleText: "后羿",
                            detailedText: "少年知道了自己永远也练不成一等一的弓术，哭着跑回家了。他父母不愿看着少年做无用功，却也狠不下来心打破少年的幻想。" +
                                          "感谢你告诉了少年实话。将少年的弓赠与了你。" +
                                          "\n你将弓卖掉换了些钱，继续上路了。",
                            options: "获得2金");
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
                    C.SetSubmitOperation(cardPickerCell =>
                    {
                        if (!cardPickerCell.AllFulfilled())
                            return A;

                        RunManager.Instance.Environment.Map.InsertRoom("后羿2");
                        return CWin;
                    });

                    return A;
                }),

            new(id:                                 "Room0054",
                name:                               "后羿2",
                description:                        "后羿2",
                ladderBound:                        new Bound(5, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "后羿",
                        detailedText: "你之前见过的视力不好的少年，现在视力越来越弱了。甚至没能发现你的到来。",
                        "给少年展示更厉害的技术", "安慰少年（需要4金）", "告诉少年实话");
                    Puzzle puzzle = new(
                        description: "尝试帮助少年击中目标",
                        condition: "目标受到伤害",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                        }),
                        kernel: new StageKernel(async d =>
                        {
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                            if (d.Forced)
                            {
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;

                                if (d.Turn < 6)
                                    return 0;

                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }

                            if (d.Flag == 0)
                                return d.Flag;

                            await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
                            
                            d.Env.RecordResult(d.Flag);
                            
                            return d.Flag;
                        })
                    );
        
                    PuzzleCell B = new(puzzle);
                    DialogCell BPass = new DialogCell(
                        titleText: "后羿",
                        detailedText: "少年将你的招式记在了心里，又投入了奋不顾身的练习。",
                        options: "继续上路");
                    
                    DialogCell C = new(
                        titleText: "后羿",
                        detailedText: "你和少年讲起了故事。传闻有和非人之物战斗的剑士，在对决的生死关头，领悟了可以看清周围一切的招式，打败了对手。");
                    DialogCell C2 = new(
                        titleText: "后羿",
                        detailedText: "少年觉得你的故事逊爆了，如果去当说书人指定吃不上饭的那种。聊了过天后，表示自己要继续训练了。你看到少年过的清苦，于是留下了一些盘缠，失去4金。",
                        options: "继续上路");

                    DialogCell D = new(
                        titleText: "后羿",
                        detailedText: "你还在想怎么和少年开口时。少年突然开口道：表示自己其实知道大家一直在迁就自己，自己也应该承担起一些责任了。\n\n少年将多年的心得交给了你。获得1个技能。" +
                                      "\n\n临走时，你注意到了不对劲，少年是怎么察觉了你的到来的。转头发现少年已经消失了。",
                        options: "继续上路");
                    D.SetReward(Reward.FromGold(50));

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);
                    A[1].SetCost(new RunCostDetails(gold: 4));
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
            
            new(id:                                 "Room0055",
                name:                               "后羿3",
                description:                        "后羿3",
                ladderBound:                        new Bound(11, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new DialogCell(
                            titleText: "后羿",
                            detailedText: "你来到了很久之前来过的竹林，当时的练箭少年已经不在，你发现了他留给你的一本秘籍。\n\n得到《射落金乌》。")
                        .SetReward(new AddSkillReward(Encyclopedia.SkillCategory.FromName("射落金乌"), JingJie.YuanYing)); // 射落金乌
            
                    return A;
                }),

            new(id:                                 "Room0056",
                name:                               "神农氏1",
                description:                        "神农氏1",
                ladderBound:                        new Bound(0, 5),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "神农",
                        detailedText: "你看见一个少年向你走来，一手拿着一个神采奕奕的仙草，另一手拿着一个可疑的蘑菇，向你说道，挑一个吃了吧。",
                        "给他展示运气抵御毒素的法门", "一口抢过来蘑菇", "选择仙草");
                    A[1].SetCost(new RunCostDetails(mingYuan: 1));
            
                    Puzzle puzzle = new(
                        description: "只要用法术治疗，就可以抵抗毒素产生的内伤，尝试帮助少年撑过6回合",
                        condition: "剩余血量 大于 0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                        }),
                        kernel: new StageKernel(async d =>
                        {
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);
            
                            if (d.Forced)
                            {
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;
            
                                if (d.Turn < 6)
                                    return 0;
            
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
            
                            if (d.Flag == 0)
                                return d.Flag;
            
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
                            
                            d.Env.RecordResult(d.Flag);
                            
                            return d.Flag;
                        })
                    );
                    
                    PuzzleCell B = new(puzzle);
                    DialogCell BPass = new DialogCell(
                            titleText: "神农",
                            detailedText: "少年吃了可疑的蘑菇，幸好可以依靠你的功法抵挡毒性。\n\n于是你吃了仙草感觉身上的伤势轻了一些。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));
                    DialogCell C = new(
                        titleText: "神农",
                        detailedText: "你吃了可疑的蘑菇，感觉头痛欲裂\n\n命元-1");
                    DialogCell D = new DialogCell(
                            titleText: "神农",
                            detailedText: "你吃了仙草感觉身上的伤势轻了一些。\n\n命元+1")
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
            
            new(id:                                 "Room0057",
                name:                               "神农氏2",
                description:                        "神农氏2",
                ladderBound:                        new Bound(5, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "神农",
                        detailedText: "你又见到了那个少年，他又笑嘻嘻的向你走来，又是一手拿着一个容光满面的仙草，另一手拿着一个可疑的蘑菇，向你说道，这次你想吃哪个？",
                        "给他展示运气抵御毒素的法门", "你个外行，学别人采什么药，离这个蘑菇远一点", "这次我就选择仙草吧");
                    A[1].SetCost(new RunCostDetails(mingYuan: 1));
            
                    Puzzle puzzle = new(
                        description: "只要用法术治疗，就可以抵抗毒素产生的内伤，尝试帮助少年撑过6回合",
                        condition: "剩余血量 大于 0",
                        home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
                        away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
                        {
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                            RunSkill.FromEntry(Encyclopedia.SkillCategory.FromName("毒性")),
                        }),
                        kernel: new StageKernel(async d =>
                        {
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);
            
                            if (d.Forced)
                            {
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
                            else
                            {
                                if (d.Cancel)
                                    return 0;
            
                                if (d.Turn < 6)
                                    return 0;
            
                                d.Flag = d.Env.Home.Hp > 0 ? 1 : 2;
                            }
            
                            if (d.Flag == 0)
                                return d.Flag;
            
                            await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);
                            
                            d.Env.RecordResult(d.Flag);
                            
                            return d.Flag;
                        })
                    );
                    
                    PuzzleCell B = new(puzzle);
                    DialogCell BPass = new DialogCell(
                            titleText: "神农",
                            detailedText: "少年吃了可疑的蘑菇，幸好可以依靠你的功法抵挡毒性。\n\n于是你吃了仙草感觉身上的伤势轻了一些。\n\n命元+1")
                        .SetReward(Reward.FromMingYuan(1));
                    DialogCell C = new(
                        titleText: "神农",
                        detailedText: "你又一次吃下了可疑的蘑菇，感觉五脏俱焚\n\n命元-1");
                    DialogCell D = new DialogCell(
                            titleText: "神农",
                            detailedText: "你吃了仙草感觉治愈了你多年的旧伤，继续上路了。\n\n命元+1")
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
            
            new(id:                                 "Room0058",
                name:                               "神农氏3",
                description:                        "神农氏3",
                ladderBound:                        new Bound(11, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    // DialogPanelDescriptor A = new DialogPanelDescriptor(
                    //         titleText: "神农",
                    //         detailedText: "故地重游，故人已经不在，你来到了他的墓前面，上面写着：神农氏之墓，他的后人说他给你留下来了一些东西。\n\n得到《百草集》。")
                    //     .SetReward(new AddSkillReward(SkillEntry.FromNameOrId("百草集"), JingJie.YuanYing));
                    DialogCell A = new DialogCell(
                            titleText: "神农",
                            detailedText: "故地重游，故人已经不在，你来到了他的墓前面，上面写着：神农氏之墓，他的后人说他给你留下来了一些东西。\n\n得到《百草集》。(未实现)");
            
                    return A;
                }),
            
            #endregion

            #region 08_ForTesting

            new(id:                                 "Room0059",
                name:                               "动画测试",
                description:                        "动画测试",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity homeA = EditorManager.FindEntity("多段HomeA");
                    RunEntity awayA = RunEntity.FromTemplate(EditorManager.FindEntity("多段AwayA"));
                    RunEntity homeB = EditorManager.FindEntity("多段HomeB");
                    RunEntity awayB = RunEntity.FromTemplate(EditorManager.FindEntity("多段AwayB"));
                    RunEntity homeC = EditorManager.FindEntity("多段HomeC");
                    RunEntity awayC = RunEntity.FromTemplate(EditorManager.FindEntity("多段AwayC"));
                    RunEntity homeD = EditorManager.FindEntity("多段HomeD");
                    RunEntity awayD = RunEntity.FromTemplate(EditorManager.FindEntity("多段AwayD"));

                    DialogCell selecting = new("动画测试", "动画测试", 
                        "攻击表现",
                        "闪避表现",
                        "破甲表现",
                        "护甲表现");
                    
                    BattleCell optionA = new BattleCell(awayA)
                        .SetWinOperation(() =>
                        {
                            return selecting;
                        })
                        .SetLoseOperation(() =>
                        {
                            return selecting;
                        });

                    BattleCell optionB = new BattleCell(awayB)
                        .SetWinOperation(() =>
                        {
                            return selecting;
                        })
                        .SetLoseOperation(() =>
                        {
                            return selecting;
                        });

                    BattleCell optionC = new BattleCell(awayC)
                        .SetWinOperation(() =>
                        {
                            return selecting;
                        })
                        .SetLoseOperation(() =>
                        {
                            return selecting;
                        });

                    BattleCell optionD = new BattleCell(awayD)
                        .SetWinOperation(() =>
                        {
                            return selecting;
                        })
                        .SetLoseOperation(() =>
                        {
                            return selecting;
                        });
                    
                    selecting[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.SetPlayerEqualPreset(homeA, toField: true, overwrite: true);
                        return optionA;
                    });
                    selecting[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.SetPlayerEqualPreset(homeB, toField: true, overwrite: true);
                        return optionB;
                    });
                    selecting[2].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.SetPlayerEqualPreset(homeC, toField: true, overwrite: true);
                        return optionC;
                    });
                    selecting[3].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.SetPlayerEqualPreset(homeD, toField: true, overwrite: true);
                        return optionD;
                    });
                    
                    return selecting;
                }),

            new(id:                                 "Room0060",
                name:                               "快速结算",
                description:                        "快速结算",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "结算",
                        detailedText: "用于测试Run结算",
                        "胜利结算", "失去所有命元", "失败结算");

                    A[0].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Victorious);
                        return null;
                    });

                    A[1].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.SetDMingYuanProcedure(-20);
                        return A;
                    });

                    A[2].SetSelect(option =>
                    {
                        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Defeated);
                        return null;
                    });

                    return A;
                }),

            new(id:                                 "Room0061",
                name:                               "循环",
                description:                        "循环",
                ladderBound:                        new Bound(0, 5),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "循环",
                        detailedText: "循环测试",
                        "继续循环", "退出循环");

                    A[0].SetSelect(option => A);
                    A[1].SetSelect(option => null);
                    return A;
                }),
            
            new(id:                                 "Room0062",
                name:                               "发现一张牌",
                description:                        "发现一张牌",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    return DiscoverSkillCell.FromDefault(room.Ladder);
                }),

            new(id:                                 "Room0063",
                name:                               "排局1",
                description:                        "排局1",
                ladderBound:                        new Bound(8, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("空幻"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("吐纳"));
                    b.SingleCreate(JingJie.ZhuJi);
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.LianQi);
                    b.Add();
                    b.Invoke();
                    
                    RunEntity template = EditorManager.Instance.EntityEditableList.FirstObj(runEntity => runEntity.GetEntry().GetName() == "排局1");
                    BattleCell A = new(template);
                    A.SetWinOperation(() => null);
                    A.SetLoseOperation(() => null);

                    return A;
                }),

            new(id:                                 "Room0064",
                name:                               "排局2",
                description:                        "排局2",
                ladderBound:                        new Bound(8, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    GainSkillBuilder b = new();
                    b.Pick(Encyclopedia.SkillCategory.FromName("吐纳"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("吐纳"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("空幻"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("空幻"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("恋花"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("止水"));
                    b.Pick(Encyclopedia.SkillCategory.FromName("回春"));
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.LianQi);
                    b.SingleCreate(JingJie.ZhuJi);
                    b.SingleCreate(JingJie.ZhuJi);
                    b.SingleCreate(JingJie.ZhuJi);
                    b.Add();
                    b.Invoke();
                    
                    RunEntity template = EditorManager.Instance.EntityEditableList.FirstObj(runEntity => runEntity.GetEntry().GetName() == "排局2");
                    BattleCell A = new(template);
                    A.SetWinOperation(() => null);
                    A.SetLoseOperation(() => null);

                    return A;
                }),

            new(id:                                 "Room0065",
                name:                               "排局3抽牌",
                description:                        "排局3抽牌",
                ladderBound:                        new Bound(8, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    SkillEntryCollectionDescriptor descriptor =
                        new SkillEntryCollectionDescriptor(count: 30, jingJie: JingJie.LianQi, distinct: false);
                    GainSkillBuilder b = new();
                    b.Draw(descriptor);
                    b.Create(JingJie.LianQi);
                    b.Add();
                    b.Invoke();

                    DialogCell A = new("排局3抽牌", "排局3抽牌");

                    return A;
                }),

            new(id:                                 "Room0066",
                name:                               "排局3",
                description:                        "排局3",
                ladderBound:                        new Bound(8, 11),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    RunEntity template = EditorManager.Instance.EntityEditableList.FirstObj(runEntity => runEntity.GetEntry().GetName() == "排局3");
                    BattleCell A = new(template);
                    A.SetWinOperation(() => null);
                    A.SetLoseOperation(() => null);

                    return A;
                }),

            new(id:                                 "Room0067",
                name:                               "提交测试",
                description:                        "提交测试",
                ladderBound:                        new Bound(0, 15),
                difficultyBound:                    new Bound(0, 11),
                withInPool:                         false,
                create:                             (map, room) =>
                {
                    DialogCell A = new(
                        titleText: "提交测试",
                        detailedText: "请提交每种五行的牌各一张",
                        "试试", "离开");

                    RunSkillDescriptorListModel list = new RunSkillDescriptorListModel();
                    list.Add(RunSkillDescriptor.FromWuXing(WuXing.Jin));
                    list.Add(RunSkillDescriptor.FromWuXing(WuXing.Shui));
                    list.Add(RunSkillDescriptor.FromWuXing(WuXing.Mu));
                    list.Add(RunSkillDescriptor.FromWuXing(WuXing.Huo));
                    list.Add(RunSkillDescriptor.FromWuXing(WuXing.Tu));

                    CardPickerCell B = CardPickerCell.FromConstantDetailedText(
                        titleText: "选择",
                        detailedText: "请提交每种五行的牌各一张",
                        descriptor: list);
                    
                    DialogCell C = new(
                        titleText: "提交测试",
                        detailedText: "提交失败");
                    
                    DialogCell D = new(
                        titleText: "提交测试",
                        detailedText: "提交成功");

                    B.SetSubmitOperation(cardPickerCell =>
                    {
                        if (!cardPickerCell.AllFulfilled())
                        {
                            cardPickerCell.WithdrawAll();
                            return C;
                        }
                        
                        return D;
                    });

                    A[0].SetSelect(option => B);
                    A[1].SetSelect(option => C);

                    return A;
                }),

            #endregion
        });
    }

    // public override RoomEntry DefaultEntry() => this["不存在的事件"];
}
