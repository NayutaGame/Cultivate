
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementCategory : Category<AchievementEntry>
{
    public AchievementCategory()
    {
        AddRange(new List<AchievementEntry>()
        {
            new(id: "ACH001",
                name: "徐福专精",
                conditionDescription: "以徐福取得难度8的胜利",
                rewardDescription: "徐福可以修改第一个卡包",
                lockIndex: LockIndex.FromSlot("徐福", 0),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.RunEnvironment.GetRunConfig().GetCharacter() != (CharacterEntry)"徐福") return;
                        if (d.RunEnvironment.GetRunConfig().GetDifficulty() < 8) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),

            new(id: "ACH002",
                name: "剑心通明",
                conditionDescription: "一场战斗中，置入5张攻击牌，并取得胜利",
                rewardDescription: "徐福可以修改第二个卡包",
                lockIndex: LockIndex.FromSlot("徐福", 1),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        int attackCount = d.Env.Home._skills.Count(s => s.Entry.GetSkillTypeComposite().Contains(SkillType.Attack));
                        if (attackCount < 5) return;
                        if (d.Flag != 1) return;

                        p.Unlock();
                    })
                }),

            new(id: "ACH003",
                name: "五彩缤纷",
                conditionDescription: "一场战斗中，拥有10种不同的Buff，并取得胜利",
                rewardDescription: "徐福可以修改第三个卡包",
                lockIndex: LockIndex.FromSlot("徐福", 2),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageDetails d = (StageDetails)details;

                        if (p.IsUnlocked()) return;
                        string key = "MaxBuffCount";
                        p.Memory.SetVariable(key, 0);
                    }),
                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        GainBuffDetails d = (GainBuffDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Tgt != StageManager.Instance.Environment.Home) return;
                        int count = d.Tgt.TraversalBuffs().Count();
                        string key = "MaxBuffCount";
                        p.Memory.PerformOperation(key, 0, c => Mathf.Max(c, count));
                    }),
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        
                        string key = "MaxBuffCount";
                        int maxBuffCount = p.Memory.TryGetVariable(key, 0);
                        p.Memory.SetVariable(key, 0);
                        
                        if (d.Flag != 1) return;
                        if (maxBuffCount < 10) return;

                        p.Unlock();
                    })
                }),

            new(id: "ACH004",
                name: "灵气灌顶",
                conditionDescription: "一场战斗中，灵气达到20，并取得胜利",
                rewardDescription: "徐福可以修改第四个卡包",
                lockIndex: LockIndex.FromSlot("徐福", 3),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        StageEntity home = StageManager.Instance.Environment.Home;

                        if (p.IsUnlocked()) return;
                        int HighestMana = home.Memory.TryGetVariable(StageEntity.HighestManaKey, 0);
                        p.Memory.PerformOperation(StageEntity.HighestManaKey, 0, v => Mathf.Max(HighestMana, v));
                        
                        if (d.Flag != 1) return;
                        if (HighestMana < 20) return;

                        p.Unlock();
                    })
                }),

            new(id: "ACH005",
                name: "不染凡尘",
                conditionDescription: "手牌和准备区共计5张丹药牌",
                rewardDescription: "徐福可以修改第五个卡包",
                lockIndex: LockIndex.FromSlot("徐福", 4),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.WIL_CHANGE_PANEL, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        PanelChangedDetails d = (PanelChangedDetails)details;


                        if (p.IsUnlocked()) return;
                        
                        int depleteCardCounts = RunManager.Instance.Environment.TraversalSkills().Count(runSkill =>
                            runSkill.GetEntry().GetSkillTypeComposite().Contains(SkillType.Deplete));

                        string key = "MaxDepleteCardCounts";
                        p.Memory.PerformOperation(key, 0, v => Mathf.Max(depleteCardCounts, v));

                        if (depleteCardCounts < 5) return;

                        p.Unlock();
                    })
                }),

            new(id: "ACH006",
                name: "道基初成",
                conditionDescription: "累计游玩了10局游戏",
                rewardDescription: "徐福可以修改第六个卡包",
                lockIndex: LockIndex.FromSlot("徐福", 5),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;

                        string key = "TotalGames";
                        int totalGames = p.Memory.PerformAddOne(key);

                        if (totalGames < 10) return;

                        p.Unlock();
                    })
                }),

            new(id: "ACH007",
                name: "丹火正旺",
                conditionDescription: "在一场战斗中，携带了至少2种丹药牌，并取得胜利",
                rewardDescription: "徐福可以修改第七个卡包",
                lockIndex: LockIndex.FromSlot("徐福", 6),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int depleteCardCount = home.TraversalSkills().Count(s =>
                            s.Entry.GetSkillTypeComposite().Contains(SkillType.Deplete));
                            
                        const string USED_DEPLETE_KEY = "UsedDepleteCards";
                        p.Memory.PerformOperation(USED_DEPLETE_KEY, 0, v => Mathf.Max(depleteCardCount, v));
                        
                        if (depleteCardCount < 2) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH008",
                name: "子非鱼专精",
                conditionDescription: "以子非鱼取得难度8的胜利",
                rewardDescription: "子非鱼可以修改第一个卡包",
                lockIndex: LockIndex.FromSlot("子非鱼", 0),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.RunEnvironment.GetRunConfig().GetCharacter() != (CharacterEntry)"子非鱼") return;
                        if (d.RunEnvironment.GetRunConfig().GetDifficulty() < 8) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH009",
                name: "风驰电掣",
                conditionDescription: "一场战斗中，置入3张开局牌，并取得胜利",
                rewardDescription: "子非鱼可以修改第二个卡包",
                lockIndex: LockIndex.FromSlot("子非鱼", 1),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int usedStartStageCardsCount = home.TraversalSkills().Count(s => s.Entry.HasStartStageCast());
                        const string USED_START_STAGE_CARDS_COUNT_KEY = "UsedStartStageCardsCount";
                        p.Memory.PerformMax(USED_START_STAGE_CARDS_COUNT_KEY, usedStartStageCardsCount);
                        
                        if (usedStartStageCardsCount < 3) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH010",
                name: "五行化生",
                conditionDescription: "一场战斗中，置入5种不同五行的牌，并取得胜利",
                rewardDescription: "子非鱼可以修改第三个卡包",
                lockIndex: LockIndex.FromSlot("子非鱼", 2),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int uniqueWuXingCount = home.TraversalSkills().Select(s => s.Entry.GetWuXing()).Distinct().Count();
                            
                        const string USED_UNIQUE_WUXING_KEY = "UsedUniqueWuXingCards";
                        p.Memory.PerformMax(USED_UNIQUE_WUXING_KEY, uniqueWuXingCount);
                        
                        if (uniqueWuXingCount < 5) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH011",
                name: "国士无双",
                conditionDescription: "一场战斗中，激活5种五行的阵法，并取得胜利",
                rewardDescription: "子非鱼可以修改第四个卡包",
                lockIndex: LockIndex.FromSlot("子非鱼", 3),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        string[] toBeActivated = new string[]{ "金灵阵", "木灵阵", "水灵阵", "火灵阵", "土灵阵" };
                        
                        int activatedWuXingCount = home.TraversalFormations()
                            .Select(f => f.IsActivated() && toBeActivated.Contains(f.GetEntry().GetFormationGroupEntry().GetName()))
                            .Count();
                            
                        const string ACTIVATED_WUXING_FORMATIONS_COUNT_KEY = "ActivatedWuXingFormations";
                        p.Memory.PerformMax(ACTIVATED_WUXING_FORMATIONS_COUNT_KEY, activatedWuXingCount);
                        
                        if (activatedWuXingCount < 5) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH012",
                name: "顺风顺水",
                conditionDescription: "到达金丹期之前，命元没有受到过伤害",
                rewardDescription: "子非鱼可以修改第五个卡包",
                lockIndex: LockIndex.FromSlot("子非鱼", 4),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.START_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StartRunDetails d = (StartRunDetails)details;
                        if (p.IsUnlocked()) return;

                        const string TAKEN_DAMAGE_THIS_RUN_KEY = "HasTakenDamageThisRun";
                        SerializableDictionary intMemory = RunManager.Instance.Environment.IntMemory;
                        intMemory.SetVariable(TAKEN_DAMAGE_THIS_RUN_KEY, 0);
                    }),

                    new(RunClosureDict.DID_SET_D_MINGYUAN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        SetDMingYuanDetails d = (SetDMingYuanDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Value >= 0) return;
                        if (RunManager.Instance.Environment.JingJie >= JingJie.JinDan) return;

                        const string TAKEN_DAMAGE_THIS_RUN_KEY = "HasTakenDamageThisRun";
                        SerializableDictionary intMemory = RunManager.Instance.Environment.IntMemory;
                        intMemory.PerformAggregate(TAKEN_DAMAGE_THIS_RUN_KEY, -d.Value);
                    }),

                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        JingJieChangedDetails d = (JingJieChangedDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.ToJingJie != JingJie.JinDan) return;

                        const string TAKEN_DAMAGE_THIS_RUN_KEY = "HasTakenDamageThisRun";
                        SerializableDictionary intMemory = RunManager.Instance.Environment.IntMemory;
                        int damageTaken = intMemory.TryGetVariable(TAKEN_DAMAGE_THIS_RUN_KEY, 0);
                        
                        if (damageTaken > 0) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH013",
                name: "杀伐果断",
                conditionDescription: "累计击败100名敌人",
                rewardDescription: "子非鱼可以修改第六个卡包",
                lockIndex: LockIndex.FromSlot("子非鱼", 5),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        const string DEFEATED_ENEMIES_KEY = "TotalDefeatedEnemies";
                        int totalDefeated = p.Memory.PerformAddOne(DEFEATED_ENEMIES_KEY);
                        if (totalDefeated < 100) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH014",
                name: "斡旋造化",
                conditionDescription: "一次性流转了20层五行buff",
                rewardDescription: "子非鱼可以修改第七个卡包",
                lockIndex: LockIndex.FromSlot("子非鱼", 6),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.DID_CYCLE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        CycleDetails d = (CycleDetails)details;

                        if (p.IsUnlocked()) return;

                        if (d.Flow < 20) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH015",
                name: "子非燕专精",
                conditionDescription: "以子非燕取得难度8的胜利",
                rewardDescription: "子非燕可以修改第一个卡包",
                lockIndex: LockIndex.FromSlot("子非燕", 0),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.RunEnvironment.GetRunConfig().GetCharacter() != (CharacterEntry)"子非燕") return;
                        if (d.RunEnvironment.GetRunConfig().GetDifficulty() < 8) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH016",
                name: "破而后立",
                conditionDescription: "累计合成了20张卡牌",
                rewardDescription: "子非燕可以修改第二个卡包",
                lockIndex: LockIndex.FromSlot("子非燕", 1),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_MERGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        MergeDetails d = (MergeDetails)details;

                        if (p.IsUnlocked()) return;

                        const string MERGE_COUNT_KEY = "MergeCount";
                        int totalSynthesis = p.Memory.PerformAddOne(MERGE_COUNT_KEY);
                        if (totalSynthesis < 20) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH017",
                name: "千机百变",
                conditionDescription: "手牌和准备区中，共计多于20张牌",
                rewardDescription: "子非燕可以修改第三个卡包",
                lockIndex: LockIndex.FromSlot("子非燕", 2),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.WIL_CHANGE_PANEL, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        PanelChangedDetails d = (PanelChangedDetails)details;

                        if (p.IsUnlocked()) return;

                        int totalCards = RunManager.Instance.Environment.TraversalSkills().Count();
                        if (totalCards <= 20) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH018",
                name: "一日筑基",
                conditionDescription: "到达筑基时，只有练气卡牌",
                rewardDescription: "子非燕可以修改第四个卡包",
                lockIndex: LockIndex.FromSlot("子非燕", 3),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        JingJieChangedDetails d = (JingJieChangedDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.ToJingJie != JingJie.ZhuJi) return;

                        int aboveLianQiCount = RunManager.Instance.Environment.TraversalSkills()
                            .Count(s => s.GetJingJie() >= JingJie.ZhuJi);

                        if (aboveLianQiCount > 1) return;
                        
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH019",
                name: "乾坤大挪移",
                conditionDescription: "一场战斗中，任一五行buff大于20，并取得胜利",
                rewardDescription: "子非燕可以修改第五个卡包",
                lockIndex: LockIndex.FromSlot("子非燕", 4),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        const string MAX_WUXING_BUFF_KEY = "MaxWuXingBuffStacks";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        home.Memory.SetVariable(MAX_WUXING_BUFF_KEY, 0);
                    }),

                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        GainBuffDetails d = (GainBuffDetails)details;

                        if (p.IsUnlocked()) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Tgt != home) return;

                        int maxWuXingBuffStacks = d.Tgt.TraversalBuffs()
                            .Where(buff => buff.GetEntry().GetCorrespondingWuXing() != null)
                            .Max(buff => buff.Stack);

                        const string MAX_WUXING_BUFF_KEY = "MaxWuXingBuffStacks";
                        home.Memory.PerformOperation(MAX_WUXING_BUFF_KEY, 0, current => Math.Max(current, maxWuXingBuffStacks));
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        const string MAX_WUXING_BUFF_KEY = "MaxWuXingBuffStacks";
                        int maxBuffStacks = home.Memory.TryGetVariable(MAX_WUXING_BUFF_KEY, 0);
                        if (maxBuffStacks < 20) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH020",
                name: "财运亨通",
                conditionDescription: "累计获得1000金钱",
                rewardDescription: "子非燕可以修改第六个卡包",
                lockIndex: LockIndex.FromSlot("子非燕", 5),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_SET_D_GOLD, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        SetDGoldDetails d = (SetDGoldDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Value <= 0) return;

                        const string TOTAL_GOLD_KEY = "TotalGainedGold";
                        int totalGold = p.Memory.PerformAggregate(TOTAL_GOLD_KEY, d.Value);

                        if (totalGold < 1000) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH021",
                name: "九莲宝灯",
                conditionDescription: "一场战斗中，激活一个化神阵法，并取得胜利",
                rewardDescription: "子非燕可以修改第七个卡包",
                lockIndex: LockIndex.FromSlot("子非燕", 6),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int huaShenFormationCount = home.TraversalFormations()
                            .Count(f => f.GetActivatedJingJie() == JingJie.HuaShen);

                        if (huaShenFormationCount < 1) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH022",
                name: "彼此卿专精",
                conditionDescription: "以彼此卿取得难度8的胜利",
                rewardDescription: "彼此卿可以修改第一个卡包",
                lockIndex: LockIndex.FromSlot("彼此卿", 0),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.RunEnvironment.GetRunConfig().GetCharacter() != (CharacterEntry)"彼此卿") return;
                        if (d.RunEnvironment.GetRunConfig().GetDifficulty() < 8) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH023",
                name: "雷劫余韵",
                conditionDescription: "在一次攻击中，受到了超过100点伤害，并取得胜利",
                rewardDescription: "彼此卿可以修改第二个卡包",
                lockIndex: LockIndex.FromSlot("彼此卿", 1),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        StageEntity home = StageManager.Instance.Environment.Home;
                        const string MAX_DAMAGE_KEY = "MaxSingleDamage";
                        home.Memory.SetVariable(MAX_DAMAGE_KEY, 0);
                    }),

                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        DamageDetails d = (DamageDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Tgt != home) return;

                        const string MAX_DAMAGE_KEY = "MaxSingleDamage";
                        home.Memory.PerformOperation(MAX_DAMAGE_KEY, 0, currentMax => Math.Max(currentMax, d.Value));
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        const string MAX_DAMAGE_KEY = "MaxSingleDamage";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        int maxDamage = home.Memory.TryGetVariable(MAX_DAMAGE_KEY, 0);

                        if (maxDamage < 100) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH024",
                name: "妙手空空",
                conditionDescription: "利用幻化复制了高于自己境界的牌",
                rewardDescription: "彼此卿可以修改第三个卡包",
                lockIndex: LockIndex.FromSlot("彼此卿", 2),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_DISCOVER_SKILL, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        DiscoverSkillDetails d = (DiscoverSkillDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.MimicIndex == null) return;
                        
                        JingJie currentJingJie = RunManager.Instance.Environment.JingJie;
                        JingJie mimickedJingJie = d.Skills[d.MimicIndex.Value].GetJingJie();
                        if (mimickedJingJie <= currentJingJie) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH025",
                name: "精打细算",
                conditionDescription: "在一场战斗中，以1点气血结束战斗，并取得胜利",
                rewardDescription: "彼此卿可以修改第四个卡包",
                lockIndex: LockIndex.FromSlot("彼此卿", 3),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (home.Hp != 1) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH026",
                name: "返朴归真",
                conditionDescription: "达到化神境界",
                rewardDescription: "彼此卿可以修改第五个卡包",
                lockIndex: LockIndex.FromSlot("彼此卿", 4),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        JingJieChangedDetails d = (JingJieChangedDetails)details;

                        if (p.IsUnlocked()) return;
                        
                        if (d.ToJingJie != JingJie.HuaShen) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH027",
                name: "身强体壮",
                conditionDescription: "累计额外获得100点气血，不包含境界提升带来的气血提升",
                rewardDescription: "彼此卿可以修改第六个卡包",
                lockIndex: LockIndex.FromSlot("彼此卿", 5),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_SET_HEALTH, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        SetHealthDetails d = (SetHealthDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.IsFromIncreaseJingJie) return;
                        if (d.Diff <= 0) return;

                        const string TOTAL_EXTRA_HP_KEY = "TotalExtraHP";
                        int totalExtraHP = p.Memory.PerformAggregate(TOTAL_EXTRA_HP_KEY, d.Diff);
                        if (totalExtraHP < 100) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH028",
                name: "斗转星移",
                conditionDescription: "在一场战斗中，使用复制的牌击败对手",
                rewardDescription: "彼此卿可以修改第七个卡包",
                lockIndex: LockIndex.FromSlot("彼此卿", 6),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.DID_CAST, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        CastDetails d = (CastDetails)details;

                        if (p.IsUnlocked()) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Caster != home) return;

                        const string LAST_CASTED_SKILL_INDEX = "LastCastedSkillIndex";
                        home.Memory.SetVariable(LAST_CASTED_SKILL_INDEX, d.Skill.SlotIndex);
                    }),
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Owner != home) return;

                        const string LAST_CASTED_SKILL_INDEX = "LastCastedSkillIndex";
                        int lastCastedSkillIndex = home.Memory.TryGetVariable(LAST_CASTED_SKILL_INDEX, -1);
                        if (lastCastedSkillIndex == -1) return;

                        if (home.RunEntity.GetSlot(lastCastedSkillIndex).Skill.GetEntry() != (SkillEntry)"幻化") return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH029",
                name: "风雨晴专精",
                conditionDescription: "以风雨晴取得难度8的胜利",
                rewardDescription: "风雨晴可以修改第一个卡包",
                lockIndex: LockIndex.FromSlot("风雨晴", 0),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.RunEnvironment.GetRunConfig().GetCharacter() != (CharacterEntry)"风雨晴") return;
                        if (d.RunEnvironment.GetRunConfig().GetDifficulty() < 8) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH030",
                name: "逆天改命",
                conditionDescription: "一场战斗中，生命低于0，并取得胜利",
                rewardDescription: "风雨晴可以修改第二个卡包",
                lockIndex: LockIndex.FromSlot("风雨晴", 1),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (home.Hp > 0) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH031",
                name: "缘法天成",
                conditionDescription: "手牌和准备区中，共计多于3张无属性牌",
                rewardDescription: "风雨晴可以修改第三个卡包",
                lockIndex: LockIndex.FromSlot("风雨晴", 2),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.WIL_CHANGE_PANEL, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        PanelChangedDetails d = (PanelChangedDetails)details;

                        if (p.IsUnlocked()) return;

                        int noAttributeCardCount = RunManager.Instance.Environment
                            .TraversalSkills()
                            .Count(skill => !skill.GetEntry().GetWuXing().HasValue);

                        if (noAttributeCardCount <= 3) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH032",
                name: "真元澎湃",
                conditionDescription: "一场战斗中，置入3张灵气消耗大于3的牌，并取得胜利",
                rewardDescription: "风雨晴可以修改第四个卡包",
                lockIndex: LockIndex.FromSlot("风雨晴", 3),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int count = home.TraversalSkills().Count(s =>
                        {
                            CostDescription costDescription = s.Entry.GetCostDescription(s.GetJingJie());
                            return costDescription.Type == CostDescription.CostType.Mana && costDescription.Value >= 3;
                        });

                        if (count < 3) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH033",
                name: "逍遥游",
                conditionDescription: "一场战斗中，没有置入任何攻击牌，并取得胜利",
                rewardDescription: "风雨晴可以修改第五个卡包",
                lockIndex: LockIndex.FromSlot("风雨晴", 4),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) =>
                    {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int count = home.AttackCount;
                        if (count > 0) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH034",
                name: "金碧辉煌",
                conditionDescription: "一场战斗中，置入12张化神牌，并取得胜利",
                rewardDescription: "风雨晴可以修改第六个卡包",
                lockIndex: LockIndex.FromSlot("风雨晴", 5),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int huashenCardsCount = home.TraversalSkills().Count(s => s.GetJingJie() == JingJie.HuaShen);
                        if (huashenCardsCount < 12) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH035",
                name: "七星连珠",
                conditionDescription: "一场战斗中，激活7个阵法，并取得胜利",
                rewardDescription: "风雨晴可以修改第七个卡包",
                lockIndex: LockIndex.FromSlot("风雨晴", 6),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int count = home.TraversalFormations().Count(f => f.IsActivated());
                        if (count < 7) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH101",
                name: "百炼成钢",
                conditionDescription: "累计在战斗中攻击1000次",
                rewardDescription: "可以使用无常路引卡包",
                lockIndex: LockIndex.FromPack("无常路引"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.DID_FULL_ATTACK, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        AttackDetails d = (AttackDetails)details;

                        if (p.IsUnlocked()) return;
                        
                        if (d.Src != StageManager.Instance.Environment.Home) return;

                        const string TOTAL_ATTACKS_KEY = "TotalAttackCounts";
                        int totalAttacks = p.Memory.PerformAggregate(TOTAL_ATTACKS_KEY, d.Times);
                        if (totalAttacks < 1000) return;
                        
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH102",
                name: "剑气冲霄",
                conditionDescription: "在一次攻击中，造成100伤害，并取得了该场战斗的胜利",
                rewardDescription: "可以使用大音希声卡包",
                lockIndex: LockIndex.FromPack("大音希声"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        const string MAX_DAMAGE_KEY = "MaxSingleDamageDealt";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        home.Memory.SetVariable(MAX_DAMAGE_KEY, 0);
                    }),

                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        DamageDetails d = (DamageDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Src != home) return;

                        const string MAX_DAMAGE_KEY = "MaxSingleDamageDealt";
                        home.Memory.PerformOperation(MAX_DAMAGE_KEY, 0, currentMax => Math.Max(currentMax, d.Value));
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        const string MAX_DAMAGE_KEY = "MaxSingleDamageDealt";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Owner != home) return;
                        
                        int maxDamage = home.Memory.TryGetVariable(MAX_DAMAGE_KEY, 0);
                        if (maxDamage < 100) return;
                        
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH103",
                name: "气贯长虹",
                conditionDescription: "累计在战斗中获得300灵气",
                rewardDescription: "可以使用天河引气录卡包",
                lockIndex: LockIndex.FromPack("天河引气录"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        GainBuffDetails d = (GainBuffDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Tgt != home) return;
                        if (d._buffEntry.GetName() != "灵气") return;

                        const string TOTAL_MANA_GAINED_KEY = "TotalManaGained";
                        int totalManaGained = p.Memory.PerformAggregate(TOTAL_MANA_GAINED_KEY, d._stack);
                        if (totalManaGained < 300) return;
                        
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH104",
                name: "生生不息",
                conditionDescription: "一场战斗中，结束时气血高于初始，并取得胜利",
                rewardDescription: "可以使用御虚诀卡包",
                lockIndex: LockIndex.FromPack("御虚诀"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        StageEntity home = StageManager.Instance.Environment.Home;
                        const string INITIAL_HP_KEY = "InitialHP";
                        home.Memory.SetVariable(INITIAL_HP_KEY, home.Hp);
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        const string INITIAL_HP_KEY = "InitialHP";
                        int initialHp = home.Memory.TryGetVariable(INITIAL_HP_KEY, 0);
                        int finalHp = home.Hp;

                        if (finalHp <= initialHp) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH105",
                name: "融会贯通",
                conditionDescription: "一场战斗中，有一张牌使用多于6次，并取得胜利",
                rewardDescription: "可以使用大椿功卡包",
                lockIndex: LockIndex.FromPack("大椿功"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        int count = home.TraversalSkills().Count(s => s.TotalStageCastedCount > 6);
                        if (count < 1) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH106",
                name: "凌波微步",
                conditionDescription: "一场战斗中，没有任何护甲并且没有受到伤害，并取得胜利",
                rewardDescription: "可以使用游龙遁卡包",
                lockIndex: LockIndex.FromPack("游龙遁"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        const string CANNOT_UNLOCK_YOU_LONG_DUN_KEY = "CannotUnlockYouLongDun";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        home.Memory.SetVariable(CANNOT_UNLOCK_YOU_LONG_DUN_KEY, 0);
                    }),

                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        DamageDetails d = (DamageDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Tgt != home) return;

                        const string CANNOT_UNLOCK_YOU_LONG_DUN_KEY = "CannotUnlockYouLongDun";
                        home.Memory.SetVariable(CANNOT_UNLOCK_YOU_LONG_DUN_KEY, 1);
                    }),

                    new(StageClosureDict.DID_GAIN_ARMOR, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        GainArmorDetails d = (GainArmorDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Tgt != home) return;
                        if (d.Value <= 0) return;

                        const string CANNOT_UNLOCK_YOU_LONG_DUN_KEY = "CannotUnlockYouLongDun";
                        home.Memory.SetVariable(CANNOT_UNLOCK_YOU_LONG_DUN_KEY, 1);
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        const string CANNOT_UNLOCK_YOU_LONG_DUN_KEY = "CannotUnlockYouLongDun";
                        int cannotUnlockYouLongDun = home.Memory.TryGetVariable(CANNOT_UNLOCK_YOU_LONG_DUN_KEY, 1);
                        if (cannotUnlockYouLongDun == 1) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH107",
                name: "倾国倾城",
                conditionDescription: "一场战斗中，战意层数大于20，并取得胜利",
                rewardDescription: "可以使用归鸿十二步卡包",
                lockIndex: LockIndex.FromPack("归鸿十二步"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        const string MAX_ZHANYI_STACKS_KEY = "MaxZhanYiStacks";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        home.Memory.SetVariable(MAX_ZHANYI_STACKS_KEY, 0);
                    }),

                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        GainBuffDetails d = (GainBuffDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Tgt != home) return;
                        if (d._buffEntry.GetName() != "战意") return;

                        int stack = d.Tgt.GetStackOfBuff("战意");

                        const string MAX_ZHANYI_STACKS_KEY = "MaxZhanYiStacks";
                        home.Memory.PerformOperation(
                            MAX_ZHANYI_STACKS_KEY,
                            0,
                            currentMax => Math.Max(currentMax, stack)
                        );
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        const string MAX_ZHANYI_STACKS_KEY = "MaxZhanYiStacks";
                        int maxZhanyiStacks = home.Memory.TryGetVariable(MAX_ZHANYI_STACKS_KEY, 0);

                        if (maxZhanyiStacks < 20) return;
                        
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH108",
                name: "无欲则刚",
                conditionDescription: "累计通过燃命受到2000伤害",
                rewardDescription: "可以使用大焚天秘乘卡包",
                lockIndex: LockIndex.FromPack("大焚天秘乘"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.DID_BURN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        BurnDetails d = (BurnDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Owner != home) return;

                        // 累计燃命伤害
                        const string TOTAL_BURN_DAMAGE_KEY = "TotalBurnDamage";
                        int totalBurnDamage = p.Memory.PerformAggregate(TOTAL_BURN_DAMAGE_KEY, d.Value);
                        if (totalBurnDamage < 2000) return;
                        
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH109",
                name: "巍然矗立",
                conditionDescription: "一场战斗中，护甲超过300，并取得胜利",
                rewardDescription: "可以使用须弥妙法卡包",
                lockIndex: LockIndex.FromPack("须弥妙法"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        const string MAX_ARMOR_KEY = "MaxArmorValue";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        home.Memory.SetVariable(MAX_ARMOR_KEY, 0);
                    }),

                    new(StageClosureDict.DID_GAIN_ARMOR, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        GainArmorDetails d = (GainArmorDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Tgt != home) return;

                        int armor = home.Armor;
                        const string MAX_ARMOR_KEY = "MaxArmorValue";
                        home.Memory.PerformOperation(
                            MAX_ARMOR_KEY,
                            0,
                            currentMax => Math.Max(currentMax, armor)
                        );
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        const string MAX_ARMOR_KEY = "MaxArmorValue";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        int maxArmor = home.Memory.TryGetVariable(MAX_ARMOR_KEY, 0);
                        if (maxArmor < 300) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH110",
                name: "一锤定音",
                conditionDescription: "一场战斗中，置入正好一张攻击牌，并由该攻击牌击败对方胜利",
                rewardDescription: "可以使用锻体四则卡包",
                lockIndex: LockIndex.FromPack("锻体四则"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.DID_CAST, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        CastDetails d = (CastDetails)details;

                        if (p.IsUnlocked()) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Caster != home) return;

                        const string LAST_CASTED_SKILL_INDEX = "LastCastedSkillIndex";
                        home.Memory.SetVariable(LAST_CASTED_SKILL_INDEX, d.Skill.SlotIndex);
                    }),
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Owner != home) return;

                        int count = home.AttackCount;
                        if (count != 1) return;

                        const string LAST_CASTED_SKILL_INDEX = "LastCastedSkillIndex";
                        int lastCastedSkillIndex = home.Memory.TryGetVariable(LAST_CASTED_SKILL_INDEX, -1);
                        if (lastCastedSkillIndex == -1) return;

                        StageSkill lastCastedSkill = home._skills[lastCastedSkillIndex];
                        bool lastCastedSkillIsAttack = lastCastedSkill.GetSkillType().Contains(SkillType.Attack);
                        if (!lastCastedSkillIsAttack) return;
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH111",
                name: "腾云驾雾",
                conditionDescription: "累计携带过50丹药牌",
                rewardDescription: "可以使用丹兵道卡包",
                lockIndex: LockIndex.FromPack("丹兵道"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Owner != home) return;

                        int count = home.TraversalSkills().Count(s => s.GetSkillType().Contains(SkillType.Deplete));
                        
                        const string TOTAL_DEPLETE_USED_KEY = "TotalDepleteSkillsUsed";
                        int totalDepleteUsed = p.Memory.PerformAggregate(TOTAL_DEPLETE_USED_KEY, count);

                        if (totalDepleteUsed < 50) return;
                        
                        p.Unlock();
                    })
                }),
            
            new(id: "ACH112",
                name: "流转达人",
                conditionDescription: "一场战斗中，流转超过10次，并取得胜利",
                rewardDescription: "可以使用化哉卡包",
                lockIndex: LockIndex.FromPack("化哉"),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        if (p.IsUnlocked()) return;
                        
                        const string WUXING_CYCLE_COUNT_KEY = "WuXingCycleCount";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        home.Memory.SetVariable(WUXING_CYCLE_COUNT_KEY, 0);
                    }),

                    new(StageClosureDict.DID_CYCLE, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        CycleDetails d = (CycleDetails)details;

                        if (p.IsUnlocked()) return;
                        StageEntity home = StageManager.Instance.Environment.Home;
                        if (d.Owner != home) return;

                        const string WUXING_CYCLE_COUNT_KEY = "WuXingCycleCount";
                        home.Memory.PerformOperation(WUXING_CYCLE_COUNT_KEY, 0, v => v + 1);
                    }),

                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        StageCommitDetails d = (StageCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (d.Flag != 1) return;

                        const string WUXING_CYCLE_COUNT_KEY = "WuXingCycleCount";
                        StageEntity home = StageManager.Instance.Environment.Home;
                        int cycleCount = home.Memory.TryGetVariable(WUXING_CYCLE_COUNT_KEY, 0);

                        if (cycleCount < 10) return;
                        p.Unlock();
                    })
                }),
            
            // 角色锁
            new(id: "ACH201",
                name: "初窥门径",
                conditionDescription: "存档等级达到2级",
                rewardDescription: "可以使用子非鱼角色",
                lockIndex: LockIndex.FromCharacter("子非鱼"),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (AppManager.Instance.ProfileManager.GetCurrProfile().LevelProfile.Level < 2) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH202",
                name: "略有小成",
                conditionDescription: "存档等级达到3级",
                rewardDescription: "可以使用子非燕角色",
                lockIndex: LockIndex.FromCharacter("子非燕"),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (AppManager.Instance.ProfileManager.GetCurrProfile().LevelProfile.Level < 3) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH203",
                name: "渐入佳境",
                conditionDescription: "存档等级达到5级",
                rewardDescription: "可以使用风雨晴角色",
                lockIndex: LockIndex.FromCharacter("风雨晴"),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (AppManager.Instance.ProfileManager.GetCurrProfile().LevelProfile.Level < 5) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH204",
                name: "出神入化",
                conditionDescription: "存档等级达到7级",
                rewardDescription: "可以使用彼此卿角色",
                lockIndex: LockIndex.FromCharacter("彼此卿"),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (AppManager.Instance.ProfileManager.GetCurrProfile().LevelProfile.Level < 7) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
            
            new(id: "ACH205",
                name: "登峰造极",
                conditionDescription: "存档等级达到10级",
                rewardDescription: "可以使用梦乃遥角色",
                lockIndex: LockIndex.FromCharacter("梦乃遥"),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                        AchievementProfile p = (AchievementProfile)owner;
                        RunCommitDetails d = (RunCommitDetails)details;

                        if (p.IsUnlocked()) return;
                        if (AppManager.Instance.ProfileManager.GetCurrProfile().LevelProfile.Level < 10) return;
                        if (d.RunEnvironment.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious) return;

                        p.Unlock();
                    })
                }),
        });
    }

    // public virtual AchievementEntry DefaultEntry() => this["ACH001"];
}
