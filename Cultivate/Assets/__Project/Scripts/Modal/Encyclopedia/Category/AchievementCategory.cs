
using System.Collections.Generic;
using UnityEngine;

public class AchievementCategory : Category<AchievementEntry>
{
    public AchievementCategory()
    {
        AddRange(new List<AchievementEntry>()
        {
            // #region 基础成就
            
            new(id: "ACH001",
                name: "测试成就",
                conditionDescription: "累计获得100金钱",
                rewardDescription: "解锁累计获得100金钱成就",
                runClosure: new(RunClosureDict.DID_SET_D_GOLD, 0, async (owner, details) => {
                    AchievementProfile p = (AchievementProfile)owner;
                    SetDGoldDetails d = (SetDGoldDetails)details;

                    if (p.IsUnlocked()) return;
                    if (d.Value <= 0) return;

                    const string TOTAL_GOLD_KEY = "total_gold";
                    int totalGold = p.Memory.PerformOperation(
                        TOTAL_GOLD_KEY, 
                        0, 
                        value => value + d.Value
                    );

                    Debug.Log($"累计获得金钱: {totalGold}");

                    if (totalGold < 100)
                        return;

                    Debug.Log("解锁累计获得100金钱成就");
                    p.Unlock();
                })),
            
            new(id: "ACH002",
                name: "解锁子非鱼",
                conditionDescription: "存档等级达到2级",
                rewardDescription: "可以使用子非鱼角色",
                lockIndex: LockIndex.FromCharacter("子非鱼"),
                runClosure: new(RunClosureDict.END_RUN, 0, async (owner, details) => {
                    // 需要一个新的Event，在游戏结算分数还有其他内容之后触发

                    // AchievementProfile p = (AchievementProfile)owner;
                    // EndRunDetails d = (EndRunDetails)details;

                    // if (p.IsUnlocked()) return;
                    // if (d.Value <= 0) return;

                    // const string TOTAL_GOLD_KEY = "total_gold";
                    // int totalGold = p.Memory.PerformOperation(
                    //     TOTAL_GOLD_KEY, 
                    //     0, 
                    //     value => value + d.Value
                    // );

                    // Debug.Log($"累计获得金钱: {totalGold}");

                    // if (totalGold < 100)
                    //     return;

                    // Debug.Log("解锁累计获得100金钱成就");
                    // p.Unlock();
                })),
            
            new(id: "ACH003",
                name: "解锁子非燕",
                conditionDescription: "存档等级达到3级",
                rewardDescription: "可以使用子非燕角色",
                lockIndex: LockIndex.FromCharacter("子非燕"),
                runClosure: new(RunClosureDict.END_RUN, 0, async (owner, details) => {
                    // 需要一个新的Event，在游戏结算分数还有其他内容之后触发
                })),
            
            new(id: "ACH004",
                name: "解锁风雨晴",
                conditionDescription: "存档等级达到5级",
                rewardDescription: "可以使用风雨晴角色",
                lockIndex: LockIndex.FromCharacter("风雨晴"),
                runClosure: new(RunClosureDict.END_RUN, 0, async (owner, details) => {
                    // 需要一个新的Event，在游戏结算分数还有其他内容之后触发
                })),
            
            new(id: "ACH005",
                name: "解锁彼此卿",
                conditionDescription: "存档等级达到7级",
                rewardDescription: "可以使用彼此卿角色",
                lockIndex: LockIndex.FromCharacter("彼此卿"),
                runClosure: new(RunClosureDict.END_RUN, 0, async (owner, details) => {
                    // 需要一个新的Event，在游戏结算分数还有其他内容之后触发
                })),
            
            new(id: "ACH006",
                name: "解锁梦乃遥",
                conditionDescription: "存档等级达到10级",
                rewardDescription: "可以使用梦乃遥角色",
                lockIndex: LockIndex.FromCharacter("梦乃遥"),
                runClosure: new(RunClosureDict.END_RUN, 0, async (owner, details) => {
                    // 需要一个新的Event，在游戏结算分数还有其他内容之后触发
                })),
            
            // new(id: "ACH001",
            //     name: "百战成钢",
            //     description: "在战斗中累计攻击1000次",
            //     conditionDescription: "累计攻击1000次",
            //     runClosure: new(RunClosureDict.END_RUN, 0, async (owner, details) => {
            //         if (details.Env.TotalAttackCount >= 1000)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // new(id: "ACH002",
            //     name: "一击必杀",
            //     description: "单次攻击造成超过100点伤害",
            //     conditionDescription: "造成100以上伤害",
            //     stageClosure: new(StageClosureDict.DID_DAMAGE, 0, async (owner, details) => {
            //         DamageDetails d = (DamageDetails)details;
            //         if (d.Value >= 100)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // new(id: "ACH003",
            //     name: "气贯长虹",
            //     description: "累计获得300点灵气",
            //     conditionDescription: "累计获得300灵气",
            //     stageClosure: new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, details) => {
            //         GainBuffDetails d = (GainBuffDetails)details;
            //         if (d._buffEntry.GetName() == "灵气" && d.Env.TotalQiCount >= 300)
            //         {
            //             // unlock
            //         }
            //     })),

            // #endregion

            // #region 境界成就
            
            // new(id: "ACH101",
            //     name: "金丹大道",
            //     description: "修为达到金丹境界",
            //     conditionDescription: "达到金丹境界",
            //     runClosure: new(RunClosureDict.DID_JINGJIE_CHANGE, 0, async (owner, details) => {
            //         if (details.Env.CurrentJingJie >= JingJie.JinDan)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // new(id: "ACH102",
            //     name: "元婴出窍",
            //     description: "修为达到元婴境界",
            //     conditionDescription: "达到元婴境界",
            //     runClosure: new(RunClosureDict.DID_JINGJIE_CHANGE, 0, async (owner, details) => {
            //         if (details.Env.CurrentJingJie >= JingJie.YuanYing)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // #endregion

            // #region 特殊成就
            
            // new(id: "ACH201",
            //     name: "丹药大师",
            //     description: "一局游戏中连续使用三个丹药",
            //     conditionDescription: "连续使用三个丹药",
            //     stageClosure: new(StageClosureDict.DID_STEP, 0, async (owner, details) => {
            //         EndStepDetails d = (EndStepDetails)details;
            //         if (d.Skill.GetSkillType().Contains(SkillType.DanYao) && 
            //             d.Env.ConsecutiveDanYaoCount >= 3)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // new(id: "ACH202",
            //     name: "五行圆满",
            //     description: "累积五行buff超过20层",
            //     conditionDescription: "五行buff达到20层",
            //     stageClosure: new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, details) => {
            //         GainBuffDetails d = (GainBuffDetails)details;
            //         if (d.Env.WuXingBuffCount >= 20)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // #endregion

            // #region 难度成就
            
            // new(id: "ACH301",
            //     name: "登堂入室",
            //     description: "完成难度5的挑战",
            //     conditionDescription: "通过难度5",
            //     runClosure: new(RunClosureDict.END_RUN, 0, (owner, details) => {
            //         if (details.Env.CompletedDifficulty >= 5)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // new(id: "ACH302",
            //     name: "登峰造极",
            //     description: "完成难度10的挑战",
            //     conditionDescription: "通过难度10",
            //     runClosure: new(RunClosureDict.END_RUN, 0, (owner, details) => {
            //         if (details.Env.CompletedDifficulty >= 10)
            //         {
            //             // unlock
            //         }
            //     })),
            
            // #endregion
        });
    }

    public virtual AchievementEntry DefaultEntry() => this["ACH001"];
}
