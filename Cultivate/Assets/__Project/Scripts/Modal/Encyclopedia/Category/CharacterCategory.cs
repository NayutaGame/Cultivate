
using System.Collections.Generic;
using CLLibrary;

public class CharacterCategory : Category<CharacterEntry>
{
    public CharacterCategory()
    {
        AddRange(new List<CharacterEntry>()
        {
            new("徐福", abilityDescription: "命元上限+2",
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.START_RUN, 0, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        RunDetails d = (RunDetails)eventDetails;
                    
                        env.SetMaxMingYuanProcedure(12);
                        env.SetDMingYuanProcedure(2);
                    }),
                }),
            new("子非鱼", abilityDescription: "使用五行卡牌后，发生对应的流转",
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;

                        bool ownerIsHome = env.Entities[0] == d.Owner;
                        if (!ownerIsHome)
                            return;

                        await d.Owner.GainBuffProcedure("五行亲和");
                    }),
                }),
            new("子非燕", abilityDescription: "流转步数为2",
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;

                        bool ownerIsHome = env.Entities[0] == d.Owner;
                        if (!ownerIsHome)
                            return;

                        await d.Owner.GainBuffProcedure("相克流转");
                    }),
                }),
            new("风雨晴", abilityDescription: "金丹后，组成阵法时，需求-1；化神，变成-2",
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.WIL_FORMATION, 0, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        RunFormationDetails d = (RunFormationDetails)eventDetails;

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (d.Owner.GetJingJie() < JingJie.JinDan)
                            return;

                        if (d.Owner.GetJingJie() < JingJie.HuaShen)
                        {
                            d.Proficiency = 1;
                            return;
                        }

                        d.Proficiency = 2;
                    }),
                }),
            new("彼此卿", abilityDescription: "卡组中第一张空位将模仿对方对位的牌" +
                                           "\n如果战斗中使用了模仿，并且模仿的牌不是机关，战后奖励时可选择模仿的卡",
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.WILL_PLACEMENT, 0, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        PlacementDetails d = (PlacementDetails)eventDetails;

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (env.AwayIsDummy())
                            return;

                        RunEntity oppo = env.Away;
                        SkillSlot slotToPaste = d.Owner.TraversalCurrentSlots()
                            .FirstObj(slot => slot.Skill == null && oppo.GetSlot(slot.GetIndex()).Skill != null);

                        if (slotToPaste == null)
                        {
                            env.SetVariable<RunSkill>("CopiedSkill", null);
                            return;
                        }

                        SkillSlot slotToCopy = oppo.GetSlot(slotToPaste.GetIndex());

                        slotToPaste.PlacedSkill = PlacedSkill.FromEntryAndJingJie(slotToCopy.Skill.GetEntry(), slotToCopy.Skill.GetJingJie());

                        RunSkill copiedSkill = slotToCopy.Skill;

                        env.SetVariable("CopiedSkill", copiedSkill != null ? SkillEntryDescriptor.FromRunSkill(copiedSkill) : null);
                    }),
                    new(RunClosureDict.WILL_DISCOVER_SKILL, 0, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        DiscoverSkillDetails d = (DiscoverSkillDetails)eventDetails;

                        SkillEntryDescriptor copiedSkillEntry = env.TryGetVariable<SkillEntryDescriptor>("CopiedSkill", null);
                        if (copiedSkillEntry == null)
                            return;

                        d.Skills.Add(copiedSkillEntry);
                        env.SetVariable<SkillEntryDescriptor>("CopiedSkill", null);
                    }),
                }),
            new("梦乃遥", abilityDescription: "梦乃遥的能力",
                runClosures: new RunClosure[]
                {
                    // new(RunClosureDict.WILL_PLACEMENT, 0, (listener, eventDetails) =>
                    // {
                    //     RunEnvironment env = (RunEnvironment)listener;
                    //     PlacementDetails d = (PlacementDetails)eventDetails;
                    //
                    //     bool ownerIsHome = env.Home == d.Owner;
                    //     if (!ownerIsHome)
                    //         return;
                    //
                    //     if (env.AwayIsDummy())
                    //         return;
                    //
                    //     RunEntity oppo = env.Away;
                    //     SkillSlot slotToPaste = d.Owner.TraversalCurrentSlots()
                    //         .FirstObj(slot => slot.Skill == null && oppo.GetSlot(slot.Index).Skill != null);
                    //
                    //     if (slotToPaste == null)
                    //     {
                    //         env.SetVariable<RunSkill>("CopiedSkill", null);
                    //         return;
                    //     }
                    //
                    //     SkillSlot slotToCopy = oppo.GetSlot(slotToPaste.Index);
                    //
                    //     slotToPaste.PlacedSkill = PlacedSkill.FromEntryAndJingJie(slotToCopy.Skill.GetEntry(), slotToCopy.Skill.GetJingJie());
                    //
                    //     RunSkill copiedSkill = slotToCopy.Skill as RunSkill;
                    //
                    //     env.SetVariable("CopiedSkill", copiedSkill != null ? SkillEntryDescriptor.FromRunSkill(copiedSkill) : null);
                    // }),
                    // new(RunClosureDict.WILL_DISCOVER_SKILL, 0, (listener, eventDetails) =>
                    // {
                    //     RunEnvironment env = (RunEnvironment)listener;
                    //     DiscoverSkillDetails d = (DiscoverSkillDetails)eventDetails;
                    //
                    //     SkillEntryDescriptor copiedSkillEntry = env.GetVariable<SkillEntryDescriptor>("CopiedSkill");
                    //     if (copiedSkillEntry == null)
                    //         return;
                    //
                    //     d.Skills.Add(copiedSkillEntry);
                    //     env.SetVariable<SkillEntryDescriptor>("CopiedSkill", null);
                    // }),
                }),
            // new("浮千舟", abilityDescription: "失去灵气时获得1点",
            //     stageClosures: new StageClosure[]
            //     {
            //         new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
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
            //         new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
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
            //         new(RunClosureDict.DID_SET_JINGJIE, 0, (listener, eventDetails) =>
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
            // new("心斩心鬼", abilityDescription: "剑类卡牌获得集中\n" +
            //                                 "卡池中塞入剑阵系类套牌：素弦，苦寒，弱昙，狂焰，孤山，周天，图南，尘缘，泪颜"),
            // new("墨虚雪", abilityDescription: "游戏开始时以及境界提升时，获得一张机关牌\n" +
            //                                "战斗后，可返还至多一张被使用的机关牌",
            //     runEventDescriptors: new RunEventDescriptor[]
            //     {
            //         new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.DID_DEPLETE, 0, (listener, eventDetails) =>
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
