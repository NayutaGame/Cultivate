
using System.Collections.Generic;
using CLLibrary;

public class CharacterCategory : Category<CharacterEntry>
{
    public CharacterCategory()
    {
        AddRange(new List<CharacterEntry>()
        {
            new("Character0001", "徐福",
                rawAbilityDescription: "命元上限+2\n战斗中使用的最左边的一次性牌，战斗后返还",
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
                        d.PreserveFirstDeplete = true;
                    }),
                }),
            
            new("Character0002", "彼此卿",
                rawAbilityDescription: "游戏开始时，获得一张幻化。可以模仿一张对手的卡牌" +
                                       "\n如果战斗中模仿了一张牌，战后奖励时可选择模仿的卡牌",
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

                        GainSkillBuilder b = new();
                        b.Pick(Encyclopedia.SkillCategory.FromName("幻化"));
                        b.Create();
                        b.Add();
                        b.Invoke();
                    }),

                    new(RunClosureDict.WIL_PLACEMENT, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        PlacementDetails d = (PlacementDetails)eventDetails;
                        string key = "MimickedSkill";

                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome)
                            return;

                        if (env.AwayIsDummy())
                            return;

                        RunEntity oppo = env.Away;
                        
                        // 清空之前模仿的记录
                        env.Memory.SetVariable<SkillEntryDescriptor>(key, null);
                        
                        // 遍历所有槽位，找到幻化牌
                        d.Owner.TraversalCurrentSlots().Do(slot => 
                        {
                            if (slot.Skill == null || slot.Skill.GetEntry().GetName() != "幻化") return;
                            
                            // 获取对手对应位置的技能
                            SkillSlot oppoSlot = oppo.GetSlot(slot.GetIndex());
                            if (oppoSlot.Skill == null) return;
                            
                            // 设置模仿的技能
                            slot.PlacedSkill = PlacedSkill.FromEntryAndJingJie(
                                oppoSlot.Skill.GetEntry(), 
                                oppoSlot.Skill.GetJingJie()
                            );
                            
                            // 记录第一个模仿的技能，用于后续奖励
                            env.Memory.PerformOperation<SkillEntryDescriptor>(key, null, skill => skill ?? SkillEntryDescriptor.FromRunSkill(oppoSlot.Skill));
                        });
                    }),
                    new(RunClosureDict.WIL_DISCOVER_SKILL, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        DiscoverSkillDetails d = (DiscoverSkillDetails)eventDetails;

                        string key = "MimickedSkill";

                        SkillEntryDescriptor copiedSkillEntry = env.Memory.TryGetVariable<SkillEntryDescriptor>(key, null);
                        if (copiedSkillEntry == null)
                            return;

                        d.MimicIndex = d.Skills.Count;
                        d.Skills.Add(copiedSkillEntry);
                        env.Memory.SetVariable<SkillEntryDescriptor>(key, null);
                    }),
                }),
            
            new("Character0003", "风雨晴",
                rawAbilityDescription: "金丹后，组成阵法时，需求-1；化神，变成-2",
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
                    new(RunClosureDict.WIL_FORMATION, 0, (listener, closure, eventDetails) =>
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

            new("Character0004", "子非鱼",
                rawAbilityDescription: "第一次获得五行Buff时，根据境界额外获得1/2/3/4/5点",
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

                        bool ownerIsHome = env.Entities[0] == d.Owner;
                        if (!ownerIsHome)
                            return;

                        int stack = 1 + d.Owner.GetJingJie();

                        await d.Owner.GainBuffProcedure("空明", stack);
                    }),
                }),

            new("Character0005", "子非燕",
                rawAbilityDescription: "拥有一把奇怪的剑，此剑吞噬其他卡牌之后威力变得更强",
                packPreset: new PackPreset(new List<PackEntry> {
                    Encyclopedia.PackCategory.FromId("Pack0002"),
                    Encyclopedia.PackCategory.FromId("Pack0003"),
                    Encyclopedia.PackCategory.FromId("Pack0005"),
                    Encyclopedia.PackCategory.FromId("Pack0008"),
                    Encyclopedia.PackCategory.FromId("Pack0010"),
                    Encyclopedia.PackCategory.FromId("Pack0011"),
                    Encyclopedia.PackCategory.FromId("Pack0012"),
                }),
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;

                        bool ownerIsHome = env.Entities[0] == d.Owner;
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
