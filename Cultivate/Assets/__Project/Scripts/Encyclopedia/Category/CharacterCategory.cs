
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
                                       "\n化神|所有空白卡槽会置入一张化神幻化",
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
                        
                        /*
                         * 返虚幻化卡牌描述     模仿对手对位的牌||境界为返虚
                         * 非返虚幻化卡牌描述   模仿对手对位的牌||境界为两者较低||低于初始境界时失效||尝试复制对手返虚会失效
                         *
                         * 角色筑基能力描述     如果战斗中模仿了一张牌，战后奖励时可选择模仿的卡牌，优先第一张
                         * 角色化神能力描述     所有空白卡槽会置入一张化神幻化
                         *
                         * 
                         *
                         * 遍历角色的slots，如果不是幻化则跳过
                         *
                         * 发现了幻化之后，尝试查找对手对位的牌：
                         *   如果自己8张牌，对手只有6张牌的时候，没有对位的情况，置入一张Empty
                         *   如果是幻化（对手的幻化），置入一张Empty
                         *   如果是Empty，置入一张Empty
                         *   如果查找到了一张非幻化的牌，尝试复制
                         *
                         * 复制过程：
                         *   如果自己是返虚，复制成功，境界为返虚
                         *   如果对方是返虚，复制失败
                         *   取Min(幻化境界，对位牌境界)，假设这个值是金丹，如果对位牌存在金丹版本，复制成功，境界为金丹，否则，复制失败
                         */

                        if (env.Home.GetJingJie() >= JingJie.HuaShen)
                        {
                            d.Owner.TraversalCurrentSlots().Do(slot =>
                            {
                                if (slot.Skill != null && slot.Skill.GetEntry().GetName() != "幻化") return;

                                int slotIndex = slot.GetIndex();
                                // 检查索引是否在对手的有效范围内
                                if (slotIndex >= oppo.GetSlotCount())
                                {
                                    slot.PlacedSkill = null;
                                    return;
                                }
                        
                                RunSkill oppoSkill = oppo.GetSlot(slotIndex).Skill;
                                if (oppoSkill == null)
                                {
                                    slot.PlacedSkill = null;
                                    return;
                                }
        
                                if (oppoSkill.GetEntry().GetName() == "幻化")
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
                                
                                JingJie discoverJingJie = fanXuMimic ? oppoSkill.GetJingJie() : mimicedJingJie;
                                env.Memory.PerformOperation<SkillEntryDescriptor>(key, null,
                                    skill => skill ?? SkillEntryDescriptor.FromEntryJingJie(oppoSkill.GetEntry(), discoverJingJie));
                            });
                            
                            return;
                        }
                        
                        if (env.Home.GetJingJie() >= JingJie.LianQi)
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
                    new(RunClosureDict.START_RUN, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        StartRunDetails d = (StartRunDetails)eventDetails;
                        
                        RunManager.Instance.Environment.PickSkillProcedure(
                            Encyclopedia.SkillCategory.FromName("斩断"),
                            JingJie.LianQi);
                    }),
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
                runClosures: new RunClosure[]
                {
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        JingJieChangedDetails d = (JingJieChangedDetails)eventDetails;

                        if (d.FromJingJie == JingJie.LianQi && d.ToJingJie == JingJie.ZhuJi)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("天机"),
                                preferredJingJie: JingJie.ZhuJi);
                            return;
                        }

                        if (d.FromJingJie == JingJie.JinDan && d.ToJingJie == JingJie.YuanYing)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("逍遥游"),
                                preferredJingJie: JingJie.YuanYing);
                            return;
                        }

                        if (d.FromJingJie == JingJie.YuanYing && d.ToJingJie == JingJie.HuaShen)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("胜天半子"),
                                preferredJingJie: JingJie.HuaShen);
                            return;
                        }
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
                            await d.Caster.GainBuffProcedure("灵气", 1 + d.Caster.GetJingJie(), induced: true);
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
                    new(RunClosureDict.START_RUN, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        StartRunDetails d = (StartRunDetails)eventDetails;
                        
                        RunManager.Instance.Environment.PickSkillProcedure(
                            Encyclopedia.SkillCategory.FromName("凶祸"),
                            JingJie.LianQi);
                    }),
                    new(RunClosureDict.DID_JINGJIE_CHANGE, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        JingJieChangedDetails d = (JingJieChangedDetails)eventDetails;

                        if (d.FromJingJie == JingJie.LianQi && d.ToJingJie == JingJie.ZhuJi)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("速晴"),
                                JingJie.ZhuJi);
                            return;
                        }

                        if (d.FromJingJie == JingJie.ZhuJi && d.ToJingJie == JingJie.JinDan)
                        {
                            RunManager.Instance.Environment.GainHealthProcedure(30);
                            return;
                        }

                        if (d.FromJingJie == JingJie.JinDan && d.ToJingJie == JingJie.YuanYing)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("焚天"),
                                JingJie.YuanYing);
                            return;
                        }

                        if (d.FromJingJie == JingJie.YuanYing && d.ToJingJie == JingJie.HuaShen)
                        {
                            RunManager.Instance.Environment.PickSkillProcedure(
                                Encyclopedia.SkillCategory.FromName("妖刀万华"),
                                JingJie.HuaShen);
                            return;
                        }
                    }),
                    new(RunClosureDict.WIL_PLACEMENT, 0, (listener, closure, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        PlacementDetails d = (PlacementDetails)eventDetails;
                    
                        bool ownerIsHome = env.Home == d.Owner;
                        if (!ownerIsHome) return;
                        
                        /*
                         * 遍历角色的slots，如果不是妖刀万华则跳过
                         *
                         * 发现了妖刀万华之后，查找下一张牌：
                         *   如果转了一圈，又查找到了自已，此位置置入一张Empty（很稀有的boundary case，发生在一圈牌都是妖刀万华的时候，实际游玩过程中应该遇不到）
                         *   如果是Empty，此位置置入一张Empty
                         *   如果是另一张妖刀万华，继续查找下一张牌
                         *   如果是非妖刀万华的牌，置入一张这张牌
                         *
                         * 如果发生了有效置入（即置入了一张非妖刀万华的牌），而且这张妖刀万华的境界是返虚，则记录这个slot。
                         *
                         *
                         * 第二个过程，将所有此时是Empty的位置，变成刚才记录的slot之中的牌（已经是替换之后的妖刀万华了）
                         */
                        SkillSlot firstValidFanXuSlot = null;
                         int slotCount = d.Owner.GetSlotCount();

                         // 第一阶段：处理妖刀万华
                         d.Owner.TraversalCurrentSlots().Do(slot =>
                         {
                             if (slot.Skill == null || slot.Skill.GetEntry().GetName() != "妖刀万华") return;

                             int startIndex = slot.GetIndex();
                             int currentIndex = startIndex;
                             bool found = false;

                             // 循环查找下一张非妖刀万华的牌
                             for (;;)
                             {
                                 currentIndex = (currentIndex + 1) % slotCount;
                                 SkillSlot nextSlot = d.Owner.GetSlot(currentIndex);

                                 // 转了一圈回到自己
                                 if (currentIndex == startIndex)
                                 {
                                     slot.PlacedSkill = null; // Empty
                                     break;
                                 }

                                 // 如果是Empty
                                 if (nextSlot.Skill == null)
                                 {
                                     slot.PlacedSkill = null; // Empty
                                     break;
                                 }

                                 // 如果是另一张妖刀万华，继续查找
                                 if (nextSlot.Skill.GetEntry().GetName() == "妖刀万华")
                                     continue;

                                 // 找到非妖刀万华的牌
                                 slot.PlacedSkill = PlacedSkill.FromRunSkill(nextSlot.Skill);
                                 found = true;
                                 break;
                             }

                             // 如果有效置入且境界是返虚，记录
                             if (firstValidFanXuSlot == null && found && slot.Skill.GetJingJie() == JingJie.FanXu)
                             {
                                 firstValidFanXuSlot = slot;
                             }
                         });

                         // 第二阶段：填充Empty位置
                         if (firstValidFanXuSlot != null)
                         {
                             d.Owner.TraversalCurrentSlots().Do(slot =>
                             {
                                 if (slot.PlacedSkill != null) return;
                                 if (slot.Skill != null && slot.Skill.GetEntry().GetName() != "妖刀万华") return;
                                 
                                 SkillSlot sourceSlot = firstValidFanXuSlot;
                                 slot.PlacedSkill = PlacedSkill.FromEntryAndJingJie(
                                     sourceSlot.PlacedSkill.Entry,
                                     sourceSlot.PlacedSkill.JingJie
                                 );
                             });
                         }
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
                    }),
                }),
        });
    }
}
