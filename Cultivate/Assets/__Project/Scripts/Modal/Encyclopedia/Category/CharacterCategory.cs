
using System.Collections.Generic;
using CLLibrary;

public class CharacterCategory : Category<CharacterEntry>
{
    public CharacterCategory()
    {
        AddRange(new List<CharacterEntry>()
        {
            new("徐福", abilityDescription: "命元上限+2\n" +
                                          "增加以物易物节点\n" +
                                          "金丹之后移除所有练气牌；化神后，所有筑基牌",
                runEventDescriptors: new RunEventDescriptor[]
                {
                    new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, 0, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        RunDetails d = (RunDetails)eventDetails;

                        env.SetMaxMingYuanProcedure(12);
                        env.SetDMingYuanProcedure(2);
                    }),
                    new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.DID_SET_JINGJIE, 0, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        SetJingJieDetails d = (SetJingJieDetails)eventDetails;

                        if (d.JingJie == JingJie.JinDan)
                        {
                            env.SkillPool.Depopulate(e => e.JingJieContains(JingJie.LianQi));
                            return;
                        }

                        if (d.JingJie == JingJie.HuaShen)
                        {
                            env.SkillPool.Depopulate(e => e.JingJieContains(JingJie.ZhuJi));
                            return;
                        }
                    }),
                    new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -2, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        RunDetails d = (RunDetails)eventDetails;

                        env.Map._r.Generator.Add("以物易物");
                    }),
                }),
            new("浮千舟", abilityDescription: "战斗开始时，获得1/2/3/4/5灵气\n" +
                                           "空白，卡费行为变成回3灵气"),
            new("语真幻", abilityDescription: "使用二动牌时，消耗。使用消耗牌时，二动。\n" +
                                           "第一次使用吟唱牌时，免除吟唱"),
            new("心斩心鬼", abilityDescription: "剑类卡牌获得集中\n" +
                                            "卡池中塞入剑阵系类套牌：素弦，苦寒，弱昙，狂焰，孤山，周天，图南，尘缘，泪颜"),
            new("子非鱼", abilityDescription: "获得2暴击"),
            new("子非燕", abilityDescription: "战斗中，第三轮开始时，获得通透世界"),
            new("墨虚雪", abilityDescription: "游戏开始时以及境界提升时，获得一张机关牌\n" +
                                           "使用机关的战斗后，获得一张随机机关牌"),
            new("念无劫", abilityDescription: "生命上限增加，战斗开始时，力量-1/2/3/4/5"),
            new("风雨晴", abilityDescription: "游戏开始时以及境界提升时，抽一张牌\n" +
                                           "金丹后，组成阵法时，需求-1；化神，变成-2"),
            new("彼此卿", abilityDescription: "卡组中第一张空位将模仿对方对位的牌\n" +
                                           "战后奖励时可选择模仿对方的卡，没有模仿时则随机一张卡",
                runEventDescriptors: new RunEventDescriptor[]
                {
                    new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.WILL_PLACEMENT, 0, (listener, eventDetails) =>
                    {
                        RunEnvironment env = (RunEnvironment)listener;
                        PlacementDetails d = (PlacementDetails)eventDetails;

                        RunEntity oppo = env.Home == d.Owner ? env.Away : d.Owner;
                        SkillSlot slotToPaste = d.Owner.TraversalCurrentSlots()
                            .FirstObj(slot => slot.Skill == null && oppo.GetSlot(slot.GetIndex()).Skill != null);

                        if (slotToPaste == null)
                            return;

                        SkillSlot slotToCopy = oppo.GetSlot(slotToPaste.GetIndex());

                        slotToPaste.PlacedSkill = PlacedSkill.FromEntryAndJingJie(slotToCopy.Skill.GetEntry(), slotToCopy.Skill.GetJingJie());
                    }),
                }),
        });
    }
}
