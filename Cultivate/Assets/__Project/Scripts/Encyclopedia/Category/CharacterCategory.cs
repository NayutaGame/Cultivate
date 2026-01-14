
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class CharacterCategory : Category<CharacterEntry>
{
    public CharacterCategory()
    {
        AddRange(new List<CharacterEntry>()
        {
            new("Character0001", "徐福",
                rawAbilityDescription: "练气|获得2命元上限" +
                                       "\n筑基|战斗中使用的最左边的一次性牌，战斗后返还" +
                                       "\n金丹|获得小零食" +
                                       "\n元婴|获得[skill:花海]" +
                                       "\n化神|获得长生不老药",
                packPreset: new PackPreset(new List<PackEntry> {
                    Encyclopedia.PackCategory.FromId("Pack0001"),
                    Encyclopedia.PackCategory.FromId("Pack0003"),
                    Encyclopedia.PackCategory.FromId("Pack0005"),
                    Encyclopedia.PackCategory.FromId("Pack0007"),
                    Encyclopedia.PackCategory.FromId("Pack0009"),
                    Encyclopedia.PackCategory.FromId("Pack0011"),
                    Encyclopedia.PackCategory.FromId("Pack0012"),
                }),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.START_RUN, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        StartRunDetails d = (StartRunDetails)eventDetails;
                    
                        env.SetMaxMingYuanProcedure(12);
                        env.SetDMingYuanProcedure(2);
                    }),
                    new(RunClosureDict.WIL_DEPLETE, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        DepleteDetails d = (DepleteDetails)eventDetails;
                        
                        d.PreserveFirstDeplete |= d.Owner.GetJingJie() >= JingJie.ZhuJi;
                    }),
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        JingJieChangedDetails d = (JingJieChangedDetails)eventDetails;

                        if (d.FromJingJie == JingJie.ZhuJi && d.ToJingJie == JingJie.JinDan)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("小零食"),
                                preferredJingJie: JingJie.JinDan);
                            return;
                        }

                        if (d.FromJingJie == JingJie.JinDan && d.ToJingJie == JingJie.YuanYing)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("花海"),
                                preferredJingJie: JingJie.YuanYing);
                            return;
                        }

                        if (d.FromJingJie == JingJie.YuanYing && d.ToJingJie == JingJie.HuaShen)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("长生不老药"),
                                preferredJingJie: JingJie.HuaShen);
                            return;
                        }
                    }),
                }),
            
            new("Character0002", "彼此卿",
                rawAbilityDescription: "练气|获得一张幻化" +
                                       "\n筑基|如果战斗中模仿了一张牌，战后奖励时可选择模仿的卡牌，优先第一张" +
                                       "\n金丹|开局：二动+1" +
                                       "\n元婴|获得第二张幻化" +
                                       "\n化神|所有空白卡槽会置入一张幻化",
                packPreset: new PackPreset(new List<PackEntry> {
                    Encyclopedia.PackCategory.FromId("Pack0001"),
                    Encyclopedia.PackCategory.FromId("Pack0004"),
                    Encyclopedia.PackCategory.FromId("Pack0006"),
                    Encyclopedia.PackCategory.FromId("Pack0008"),
                    Encyclopedia.PackCategory.FromId("Pack0009"),
                    Encyclopedia.PackCategory.FromId("Pack0011"),
                    Encyclopedia.PackCategory.FromId("Pack0012"),
                }),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.START_RUN, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        StartRunDetails d = (StartRunDetails)eventDetails;
                        
                        RunManager.Instance.Environment.PickSkillProcedure(
                            Encyclopedia.SkillCategory.FromName("幻化"),
                            JingJie.LianQi);
                    }),
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        JingJieChangedDetails d = (JingJieChangedDetails)eventDetails;

                        if (d.FromJingJie == JingJie.JinDan && d.ToJingJie == JingJie.YuanYing)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("幻化"),
                                preferredJingJie: JingJie.YuanYing);
                            return;
                        }
                    }),
                    new(RunClosureDict.WIL_PLACEMENT, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        PlacementDetails d = (PlacementDetails)eventDetails;
                        string key = "FirstMimickedSkill";

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (env.AwayIsDummy())
                            return;

                        RunEntity oppo = env.Away;
                        
                        env.Memory.SetVariable<SkillEntryDescriptor>(key, null);

                        if (env.Home.GetJingJie() >= JingJie.HuaShen)
                        {
                            d.Owner.TraversalCurrentSlots().Do(slot =>
                            {
                                if (slot.Skill != null && slot.Skill.GetEntry().GetName() != "幻化") return;

                                RunSkill oppoSkill = oppo.GetSlot(slot.GetIndex()).Skill;
                                if (oppoSkill == null)
                                {
                                    slot.PlacedSkill = null;
                                    return;
                                }

                                bool fanXuMimic = slot.Skill != null &&
                                                  slot.Skill.GetEntry().GetName() == "幻化" &&
                                                  slot.Skill.GetJingJie() == JingJie.FanXu;

                                JingJie mimicedJingJie = fanXuMimic ? JingJie.FanXu : Mathf.Min(JingJie.HuaShen, oppoSkill.GetJingJie());
                                bool successMimic = mimicedJingJie >= oppoSkill.GetEntry().LowestJingJie;
                                if (oppoSkill.GetJingJie() == JingJie.FanXu && mimicedJingJie != JingJie.FanXu)
                                    successMimic = false;
                                if (!successMimic)
                                {
                                    slot.PlacedSkill = null;
                                    return;
                                }
                            
                                slot.PlacedSkill = PlacedSkill.FromEntryAndJingJie(
                                    oppoSkill.GetEntry(),
                                    mimicedJingJie
                                );
                            
                                env.Memory.PerformOperation<SkillEntryDescriptor>(key, null,
                                    skill => skill ?? SkillEntryDescriptor.FromEntryJingJie(oppoSkill.GetEntry(), mimicedJingJie));
                            });
                            
                            return;
                        }

                        if (env.Home.GetJingJie() >= JingJie.ZhuJi)
                        {
                            d.Owner.TraversalCurrentSlots().Do(slot =>
                            {
                                if (slot.Skill == null || slot.Skill.GetEntry().GetName() != "幻化") return;

                                RunSkill oppoSkill = oppo.GetSlot(slot.GetIndex()).Skill;
                                if (oppoSkill == null)
                                {
                                    slot.PlacedSkill = null;
                                    return;
                                }

                                bool fanXuMimic = slot.Skill.GetJingJie() == JingJie.FanXu;

                                JingJie mimicedJingJie = fanXuMimic ? JingJie.FanXu : Mathf.Min(slot.Skill.GetJingJie(), oppoSkill.GetJingJie());
                                bool successMimic = mimicedJingJie >= oppoSkill.GetEntry().LowestJingJie;
                                if (oppoSkill.GetJingJie() == JingJie.FanXu && mimicedJingJie != JingJie.FanXu)
                                    successMimic = false;
                                if (!successMimic)
                                {
                                    slot.PlacedSkill = null;
                                    return;
                                }
                            
                                slot.PlacedSkill = PlacedSkill.FromEntryAndJingJie(
                                    oppoSkill.GetEntry(),
                                    mimicedJingJie
                                );
                            
                                env.Memory.PerformOperation<SkillEntryDescriptor>(key, null,
                                    skill => skill ?? SkillEntryDescriptor.FromEntryJingJie(oppoSkill.GetEntry(), mimicedJingJie));
                            });
                            
                            return;
                        }
                    }),
                    new(RunClosureDict.WIL_DISCOVER_SKILL, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        DiscoverSkillDetails d = (DiscoverSkillDetails)eventDetails;

                        string key = "FirstMimickedSkill";

                        SkillEntryDescriptor copiedSkillEntry = env.Memory.TryGetVariable<SkillEntryDescriptor>(key, null);
                        if (copiedSkillEntry == null)
                            return;

                        d.MimicIndex = d.Skills.Count;
                        d.Skills.Add(copiedSkillEntry);
                        env.Memory.SetVariable<SkillEntryDescriptor>(key, null);
                    }),
                },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (d.Owner.RunEntity.GetJingJie() <= JingJie.ZhuJi)
                            return;

                        await d.Owner.GainBuffProcedure("二动");
                    }),
                }),
            
            new("Character0003", "风雨晴",
                rawAbilityDescription: "练气|获得一张斩断" +
                                       "\n筑基|组成阵法时，需求-1" +
                                       "\n金丹|开局：闪避+1" +
                                       "\n元婴|获得千象" +
                                       "\n化神|组成阵法时，需求额外再-1",
                packPreset: new PackPreset(new List<PackEntry> {
                    Encyclopedia.PackCategory.FromId("Pack0001"),
                    Encyclopedia.PackCategory.FromId("Pack0003"),
                    Encyclopedia.PackCategory.FromId("Pack0006"),
                    Encyclopedia.PackCategory.FromId("Pack0007"),
                    Encyclopedia.PackCategory.FromId("Pack0009"),
                    Encyclopedia.PackCategory.FromId("Pack0011"),
                    Encyclopedia.PackCategory.FromId("Pack0012"),
                }),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        JingJieChangedDetails d = (JingJieChangedDetails)eventDetails;

                        if (d.FromJingJie == JingJie.JinDan && d.ToJingJie == JingJie.YuanYing)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("千象"),
                                preferredJingJie: JingJie.YuanYing);
                            return;
                        }
                    }),
                    new(RunClosureDict.WIL_FORMATION, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        RunFormationDetails d = (RunFormationDetails)eventDetails;

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (d.Owner.GetJingJie() <= JingJie.LianQi)
                            return;

                        if (d.Owner.GetJingJie() <= JingJie.YuanYing)
                        {
                            d.Proficiency = 1;
                            return;
                        }

                        d.Proficiency = 2;
                    }),
                },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (d.Owner.RunEntity.GetJingJie() <= JingJie.ZhuJi)
                            return;

                        await d.Owner.GainBuffProcedure("闪避");
                    }),
                }),

            new("Character0004", "子非鱼",
                rawAbilityDescription: "练气|聚气术和灵气匮乏，获得额外灵气层数，练气时多1，筑基时多2，以此类推" +
                                       "\n筑基|获得天机" +
                                       "\n金丹|开局：格挡+1" +
                                       "\n元婴|获得逍遥游" +
                                       "\n化神|获得胜天半子",
                packPreset: new PackPreset(new List<PackEntry> {
                    Encyclopedia.PackCategory.FromId("Pack0001"),
                    Encyclopedia.PackCategory.FromId("Pack0004"),
                    Encyclopedia.PackCategory.FromId("Pack0005"),
                    Encyclopedia.PackCategory.FromId("Pack0007"),
                    Encyclopedia.PackCategory.FromId("Pack0009"),
                    Encyclopedia.PackCategory.FromId("Pack0011"),
                    Encyclopedia.PackCategory.FromId("Pack0012"),
                }),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (d.Owner.RunEntity.GetJingJie() <= JingJie.ZhuJi)
                            return;

                        await d.Owner.GainBuffProcedure("格挡");
                    }),
                    new(StageClosureDict.DID_CAST, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        CastDetails d = (CastDetails)eventDetails;

                        bool casterIsHome = env.Home == d.Caster;
                        if (!casterIsHome)
                            return;

                        string name = d.Skill.Entry.GetName();
                        if (name is "聚气术" or "灵气匮乏")
                        {
                            await d.Caster.GainBuffProcedure("灵气", 1 + d.Caster.GetJingJie());
                        }
                    }),
                }),

            new("Character0005", "子非燕",
                rawAbilityDescription: "练气|获得凶祸" +
                                       "\n筑基|获得速晴" +
                                       "\n金丹|获得30气血上限" +
                                       "\n元婴|获得焚天" +
                                       "\n化神|获得妖刀万华",
                packPreset: new PackPreset(new List<PackEntry> {
                    Encyclopedia.PackCategory.FromId("Pack0002"),
                    Encyclopedia.PackCategory.FromId("Pack0003"),
                    Encyclopedia.PackCategory.FromId("Pack0005"),
                    Encyclopedia.PackCategory.FromId("Pack0008"),
                    Encyclopedia.PackCategory.FromId("Pack0010"),
                    Encyclopedia.PackCategory.FromId("Pack0011"),
                    Encyclopedia.PackCategory.FromId("Pack0012"),
                }),
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        JingJieChangedDetails d = (JingJieChangedDetails)eventDetails;

                        if (d.FromJingJie == JingJie.ZhuJi && d.ToJingJie == JingJie.JinDan)
                        {
                            RunManager.Instance.Environment.GainHealthProcedure(30);
                            return;
                        }
                    }),
                    // new(RunClosureDict.WIL_PLACEMENT, 0, (listener, closure, eventDetails) =>
                    // {
                    //     RunEnvironment env = (RunEnvironment)listener;
                    //     PlacementDetails d = (PlacementDetails)eventDetails;
                    //
                    //     bool ownerIsHome = env.Home == d.Owner;
                    //     if (!ownerIsHome) return;
                    //
                    //     if (env.Home.GetJingJie() >= JingJie.FanXu)
                    //     {
                    //         d.Owner.TraversalCurrentSlots().Do(slot =>
                    //         {
                    //             if (slot.Skill != null && slot.Skill.GetEntry().GetName() != "妖刀万华") return;
                    //
                    //             RunSkill oppoSkill = oppo.GetSlot(slot.GetIndex()).Skill;
                    //             if (oppoSkill == null)
                    //             {
                    //                 slot.PlacedSkill = null;
                    //                 return;
                    //             }
                    //
                    //             bool fanXuMimic = slot.Skill != null &&
                    //                               slot.Skill.GetEntry().GetName() == "幻化" &&
                    //                               slot.Skill.GetJingJie() == JingJie.FanXu;
                    //
                    //             JingJie mimicedJingJie = fanXuMimic ? JingJie.FanXu : Mathf.Min(JingJie.HuaShen, oppoSkill.GetJingJie());
                    //             bool successMimic = mimicedJingJie >= oppoSkill.GetEntry().LowestJingJie;
                    //             if (oppoSkill.GetJingJie() == JingJie.FanXu && mimicedJingJie != JingJie.FanXu)
                    //                 successMimic = false;
                    //             if (!successMimic)
                    //             {
                    //                 slot.PlacedSkill = null;
                    //                 return;
                    //             }
                    //         
                    //             slot.PlacedSkill = PlacedSkill.FromEntryAndJingJie(
                    //                 oppoSkill.GetEntry(),
                    //                 mimicedJingJie
                    //             );
                    //         });
                    //         
                    //         return;
                    //     }
                    //     
                    //     d.Owner.TraversalCurrentSlots().Do(slot =>
                    //     {
                    //         if (slot.Skill == null || slot.Skill.GetEntry().GetName() != "幻化") return;
                    //
                    //         RunSkill oppoSkill = oppo.GetSlot(slot.GetIndex()).Skill;
                    //         if (oppoSkill == null)
                    //         {
                    //             slot.PlacedSkill = null;
                    //             return;
                    //         }
                    //
                    //         bool fanXuMimic = slot.Skill.GetJingJie() == JingJie.FanXu;
                    //
                    //         JingJie mimicedJingJie = fanXuMimic ? JingJie.FanXu : Mathf.Min(slot.Skill.GetJingJie(), oppoSkill.GetJingJie());
                    //         bool successMimic = mimicedJingJie >= oppoSkill.GetEntry().LowestJingJie;
                    //         if (oppoSkill.GetJingJie() == JingJie.FanXu && mimicedJingJie != JingJie.FanXu)
                    //             successMimic = false;
                    //         if (!successMimic)
                    //         {
                    //             slot.PlacedSkill = null;
                    //             return;
                    //         }
                    //         
                    //         slot.PlacedSkill = PlacedSkill.FromEntryAndJingJie(
                    //             oppoSkill.GetEntry(),
                    //             mimicedJingJie
                    //         );
                    //     });
                    // }),
                },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;
                    }),
                }),
            
            // new("斩心鬼", abilityDescription: "拥有一把奇怪的剑，此剑吞噬其他卡牌之后威力变得更强"),
            
            // new("梦乃遥",
            //     abilityDescription: "梦乃遥的能力",
            //     runClosures: new RunClosure[]
            //     {
            //     }),
            // new("浮千舟", abilityDescription: "失去灵气时获得1点",
            //     stageClosures: new StageClosure[]
            //     {
            //         new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
            //         {
            //             StageEnvironment env = (StageEnvironment)listener;
            //             StageDetails d = (StageDetails)eventDetails;
            //
            //             bool ownerIsHome = env.Entities[0] == d.Owner;
            //             if (!ownerIsHome)
            //                 return;
            //
            //             await d.Owner.GainBuffProcedure("灵气返还");
            //         }),
            //     }),
            // new("语真幻", abilityDescription: "使用二动牌时，获得1闪避",
            //     stageClosures: new StageClosure[]
            //     {
            //         new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
            //         {
            //             StageEnvironment env = (StageEnvironment)listener;
            //             StageDetails d = (StageDetails)eventDetails;
            //
            //             bool ownerIsHome = env.Entities[0] == d.Owner;
            //             if (!ownerIsHome)
            //                 return;
            //
            //             await d.Owner.GainBuffProcedure("灵敏");
            //         }),
            //     }),
            // new("花辞树", abilityDescription: "金丹之后移除所有练气牌；化神后移除所有筑基牌",
            //     runClosures: new RunClosure[]
            //     {
            //         new(RunClosureDict.DID_SET_JINGJIE, 0, (listener, closure, eventDetails) =>
            //         {
            //             RunEnvironment env = (RunEnvironment)listener;
            //             SetJingJieDetails d = (SetJingJieDetails)eventDetails;
            //         
            //             if (d.ToJingJie == JingJie.JinDan)
            //             {
            //                 env.SkillPool.Depopulate(e => e.JingJieContains(JingJie.LianQi));
            //                 return;
            //             }
            //         
            //             if (d.ToJingJie == JingJie.HuaShen)
            //             {
            //                 env.SkillPool.Depopulate(e => e.JingJieContains(JingJie.ZhuJi));
            //                 return;
            //             }
            //         }),
            //     }),
            // new("墨虚雪", abilityDescription: "游戏开始时以及境界提升时，获得一张机关牌\n" +
            //                                "战斗后，可返还至多一张被使用的机关牌",
            //     runEventDescriptors: new RunEventDescriptor[]
            //     {
            //         new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.DID_DEPLETE, 0, (listener, closure, eventDetails) =>
            //         {
            //             RunEnvironment env = (RunEnvironment)listener;
            //             DepleteDetails d = (DepleteDetails)eventDetails;
            //
            //             bool ownerIsHome = env.Home == d.Owner;
            //             if (!ownerIsHome)
            //                 return;
            //
            //             foreach (var item in d.DepletedSkills)
            //             {
            //                 Debug.Log(item.GetName());
            //             }
            //         }),
            //     }),
        });
    }
}
