
using System.Collections.Generic;
using UnityEngine;

public class AchievementCategory : Category<AchievementEntry>
{
    public AchievementCategory()
    {
        AddRange(new List<AchievementEntry>()
        {
            // new(id: "ACH001",
            //     name: "测试成就",
            //     conditionDescription: "累计获得100金钱",
            //     rewardDescription: "解锁累计获得100金钱成就",
            //     runClosure: new(RunClosureDict.DID_SET_D_GOLD, 0, async (owner, details) => {
            //         AchievementProfile p = (AchievementProfile)owner;
            //         SetDGoldDetails d = (SetDGoldDetails)details;

            //         if (p.IsUnlocked()) return;
            //         if (d.Value <= 0) return;

            //         const string TOTAL_GOLD_KEY = "total_gold";
            //         int totalGold = p.Memory.PerformOperation(
            //             TOTAL_GOLD_KEY, 
            //             0, 
            //             value => value + d.Value
            //         );

            //         Debug.Log($"累计获得金钱: {totalGold}");

            //         if (totalGold < 100)
            //             return;

            //         Debug.Log("解锁累计获得100金钱成就");
            //         p.Unlock();
            //     })),
            
            // 角色锁
            new(id: "ACH101",
                name: "解锁子非鱼",
                conditionDescription: "存档等级达到2级",
                rewardDescription: "可以使用子非鱼角色",
                lockIndex: LockIndex.FromCharacter("子非鱼"),
                runClosure: new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                    AchievementProfile p = (AchievementProfile)owner;
                    CommitRunDetails d = (CommitRunDetails)details;

                    if (p.IsUnlocked()) return;
                    if (AppManager.Instance.ProfileManager.GetCurrProfile().levelProfile.Level < 2) return;
                    if (!d.Env.IsWin) return;

                    p.Unlock();
                })),
            
            new(id: "ACH102",
                name: "解锁子非燕",
                conditionDescription: "存档等级达到3级",
                rewardDescription: "可以使用子非燕角色",
                lockIndex: LockIndex.FromCharacter("子非燕"),
                runClosure: new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                    AchievementProfile p = (AchievementProfile)owner;
                    CommitRunDetails d = (CommitRunDetails)details;

                    if (p.IsUnlocked()) return;
                    if (AppManager.Instance.ProfileManager.GetCurrProfile().levelProfile.Level < 3) return;
                    if (!d.Env.IsWin) return;

                    p.Unlock();
                })),
            
            new(id: "ACH103",
                name: "解锁风雨晴",
                conditionDescription: "存档等级达到5级",
                rewardDescription: "可以使用风雨晴角色",
                lockIndex: LockIndex.FromCharacter("风雨晴"),
                runClosure: new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                    AchievementProfile p = (AchievementProfile)owner;
                    CommitRunDetails d = (CommitRunDetails)details;

                    if (p.IsUnlocked()) return;
                    if (AppManager.Instance.ProfileManager.GetCurrProfile().levelProfile.Level < 5) return;
                    if (!d.Env.IsWin) return;

                    p.Unlock();
                })),
            
            new(id: "ACH104",
                name: "解锁彼此卿",
                conditionDescription: "存档等级达到7级",
                rewardDescription: "可以使用彼此卿角色",
                lockIndex: LockIndex.FromCharacter("彼此卿"),
                runClosure: new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                    AchievementProfile p = (AchievementProfile)owner;
                    CommitRunDetails d = (CommitRunDetails)details;

                    if (p.IsUnlocked()) return;
                    if (AppManager.Instance.ProfileManager.GetCurrProfile().levelProfile.Level < 7) return;
                    if (!d.Env.IsWin) return;

                    p.Unlock();
                })),
            
            new(id: "ACH105",
                name: "解锁梦乃遥",
                conditionDescription: "存档等级达到10级",
                rewardDescription: "可以使用梦乃遥角色",
                lockIndex: LockIndex.FromCharacter("梦乃遥"),
                runClosure: new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                    AchievementProfile p = (AchievementProfile)owner;
                    CommitRunDetails d = (CommitRunDetails)details;

                    if (p.IsUnlocked()) return;
                    if (AppManager.Instance.ProfileManager.GetCurrProfile().levelProfile.Level < 10) return;
                    if (!d.Env.IsWin) return;

                    p.Unlock();
                })),
            
            // 卡槽锁
            new(id: "ACH201",
                name: "解锁徐福的卡槽1",
                conditionDescription: "以徐福取得难度8的胜利",
                rewardDescription: "可以使用徐福的卡槽1",
                lockIndex: LockIndex.FromSlot(1),
                runClosure: new(RunClosureDict.DID_COMMIT_RUN, 0, async (owner, details) => {
                    AchievementProfile p = (AchievementProfile)owner;
                    CommitRunDetails d = (CommitRunDetails)details;

                    if (p.IsUnlocked()) return;
                    if (AppManager.Instance.ProfileManager.GetCurrProfile().difficultyProfile.GetEntry().Order < 8) return;
                    if (d.Env.RunConfig.characterProfile.GetEntry().GetName() != "徐福") return;
                    if (!d.Env.IsWin) return;

                    p.Unlock();
                })),
        });
    }

    public virtual AchievementEntry DefaultEntry() => this["ACH001"];
}
