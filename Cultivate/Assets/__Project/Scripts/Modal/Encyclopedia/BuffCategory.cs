using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        AddRange(new List<BuffEntry>()
        {
            new ("不存在的Buff", "不存在的Buff", BuffStackRule.Add, true, false),
            new ("灵气", "可以消耗灵气使用技能", BuffStackRule.Add, true, false),
            new ("跳回合", "跳过回合", BuffStackRule.Add, false, false),
            new ("跳卡牌", "行动时跳过下张卡牌", BuffStackRule.Add, false, false),
            new ("双发", "下一张牌使用两次", BuffStackRule.Wasted, true, false),
            new ("二动", "下一张牌二动", BuffStackRule.Add, true, false,
                startTurn: async (buff, d) =>
                {
                    d.Owner.Swift = true;
                    buff.Stack -= 1;
                }),
            new ("免费", "下一次耗蓝时无需灵气", BuffStackRule.Add, true, false),

            new ("延迟护甲", "下回合护甲+[层数]", BuffStackRule.Add, true, false,
                startTurn: async (buff, entity) =>
                {
                    await buff.Owner.ArmorGainSelfProcedure(buff.Stack);
                    buff.Owner.RemoveBuff(buff);
                }),

            new ("无常已至", "造成伤害：施加[伤害值，最多Stack]减甲", BuffStackRule.Add, true, false,
                damage: async (buff, d) =>
                {
                    if (buff.Owner == d.Tgt)
                        return;

                    await buff.Owner.ArmorLoseOppoProcedure(Mathf.Min(d.Value, buff.Stack));
                }),

            new ("锋锐", "每回合：[层数]攻\n受到伤害后层数-1", BuffStackRule.Add, true, true,
                endTurn: async (buff, d) => await buff.Owner.AttackProcedure(buff.Stack, wuXing: WuXing.Jin),
                damaged: async (buff, d) => buff.Stack -= 1),
            new ("森罗万象", "奇偶同时激活两个效果", BuffStackRule.Wasted, true, false),

            new ("自动灵气", "每回合：灵气+[层数]", BuffStackRule.Add, true, true,
                startTurn: async (buff, d) =>
                {
                    await buff.Owner.BuffSelfProcedure("灵气", buff.Stack);
                }),
            new ("敛息", "造成伤害时：取消伤害，施加减甲", BuffStackRule.Add, true, false,
                damage: async (buff, d) =>
                {
                    d.Cancel = true;
                    await buff.Owner.ArmorLoseOppoProcedure(d.Value);
                    buff.Stack -= 1;
                }),

            new ("吸血", "下一次攻击造成伤害时，回复生命", BuffStackRule.Add, true, true,
                attack: async (buff, d) =>
                {
                    d.LifeSteal = true;
                    buff.Stack -= 1;
                }),
            new ("凝神", "下一次受到治疗：护甲+治疗量", BuffStackRule.Add, true, true,
                healed: async (buff, d) =>
                {
                    await buff.Owner.ArmorGainSelfProcedure(d.Value);
                    buff.Stack -= 1;
                }),
            new ("永久吸血", "攻击一直具有吸血，直到使用非攻击牌", BuffStackRule.Wasted, true, false,
                attack: async (buff, d) => d.LifeSteal = true,
                startStep: async (buff, d) =>
                {
                    if (!d.Skill.GetWaiGongType().Contains(SkillTypeCollection.Attack))
                        buff.Stack -= 1;
                }),

            // new ("治疗转灵气", "受到治疗时：灵气+[Stack]", BuffStackRule.Add, true, false,
            //     healed: async (buff, d) => await d.Tgt.BuffSelfProcedure("灵气", buff.Stack)),
            // new ("治疗转二动", "被治疗时，如果实际治疗>=20，二动", BuffStackRule.Wasted, true, false,
            //     healed: (buff, d) =>
            //     {
            //         int actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
            //         d.Tgt.Swift |= actualHealed >= 20;
            //     }),
            new ("不动明王咒", "无法二动/三动", BuffStackRule.Wasted, false, false),
            new ("玄武吐息法", "治疗可以穿上限", BuffStackRule.Add, true, true,
                healed: async (buff, d) => d.Penetrate = true),
            new ("格挡", "受到攻击：攻击力-[层数]", BuffStackRule.Add, true, true,
                attacked: async (buff, d) =>
                {
                    if (d.Pierce)
                        return;

                    d.Value -= buff.Stack;
                }),
            new ("自动格挡", "每轮：格挡+[层数]", BuffStackRule.Add, true, true,
                startRound: async (buff, owner) => await owner.BuffSelfProcedure("格挡", buff.Stack)),
            new ("缠绕", "无法二动/三动\n每回合：层数-1", BuffStackRule.Add, false, true,
                endTurn: async (buff, d) => buff.Stack -= 1),

            new ("闪避", "受到攻击时，减少1层，忽略此次攻击", BuffStackRule.Add, true, true,
                attacked: async (buff, d) =>
                {
                    d.Evade = true;
                    buff.Stack -= 1;
                }),
            new ("自动闪避", "每轮：闪避补至[层数]", BuffStackRule.Add, true, true,
                startRound: async (buff, owner) => await buff.Owner.BuffSelfProcedure("闪避", buff.Stack - owner.GetStackOfBuff("闪避"))),
            new ("穿透", "下一次攻击时，忽略对方护甲/闪避/格挡", BuffStackRule.Add, true, true,
                attack: async (buff, d) =>
                {
                    d.Pierce = true;
                    buff.Stack -= 1;
                }),
            new ("力量", "攻击时，多[层数]攻", BuffStackRule.Add, true, true,
                attack: async (buff, d) =>
                {
                    d.Value += buff.Stack;
                }),
            new ("回马枪", "下次受攻击时：[层数]攻 穿透", BuffStackRule.Max, true, false,
                attacked: async (buff, d) =>
                {
                    if (!d.Recursive)
                        return;

                    await buff.Owner.AttackProcedure(buff.Stack, wuXing: WuXing.Mu, recursive: false);
                    buff.Owner.RemoveBuff(buff);
                }),

            new ("天衣无缝", "每回合：[层数]攻", BuffStackRule.Max, true, false,
                startTurn: async (buff, d) => await d.Owner.AttackProcedure(buff.Stack, wuXing: WuXing.Huo)),
            new ("业火", "消耗牌时：使用2次", BuffStackRule.Wasted, true, false,
                consumed: async (buff, d) => await d.Skill.Execute(d.Owner)),
            new ("淬体", "消耗生命时：灼烧+[层数]", BuffStackRule.Add, true, false,
                damaged: async (buff, d) =>
                {
                    if (d.Src != d.Tgt || buff.Owner != d.Src)
                        return;

                    await buff.Owner.BuffSelfProcedure("灼烧", buff.Stack);
                }),
            new ("追击", "持续[层数]次，下次攻击时，次数+1", BuffStackRule.Add, true, true),
            new ("净天地", "使用非攻击卡不消耗灵气，使用之后消耗", BuffStackRule.Add, true, false,
                startStep: async (buff, d) =>
                {
                    if (d.Skill.GetWaiGongType().Contains(SkillTypeCollection.Attack))
                        return;

                    await d.Skill.ConsumeProcedure();
                    bool noBuff = buff.Owner.GetStackOfBuff("免费") == 0;
                    if(noBuff)
                        await buff.Owner.BuffSelfProcedure("免费");
                }),

            new ("心斋", "所有耗蓝-[层数]", BuffStackRule.Add, true, false),

            new ("盛开", "收到治疗时：力量+[层数]", BuffStackRule.Add, true, false,
                evaded: async (buff, d) => await buff.Owner.BuffSelfProcedure("力量", buff.Stack)),

            new ("通透世界", "攻击具有穿透", BuffStackRule.Wasted, true, false,
                attack: async (buff, d) => d.Pierce = true),
            new ("鹤回翔", "反转出牌顺序", BuffStackRule.Wasted, true, false),

            new ("待激活的凤凰涅槃", "累计20灼烧后激活", BuffStackRule.Wasted, true, false,
                buffed: new Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>>(0, async (buff, d) =>
                {
                    if(buff.Owner.GainedBurningRecord >= 20 && buff.Owner.GetStackOfBuff("涅槃") == 0)
                        await buff.Owner.BuffSelfProcedure("涅槃");
                    return d;
                })),
            new ("涅槃", "每轮以及强制结算前：生命恢复至上限", BuffStackRule.Wasted, true, false,
                startRound: async (buff, entity) =>
                {
                    await entity.HealProcedure(entity.MaxHp - entity.Hp);
                },
                endStage: async (buff, entity) =>
                {
                    await entity.HealProcedure(entity.MaxHp - entity.Hp);
                }),
            new ("抱元守一", "每回合：消耗[层数]生命，护甲+[层数]", BuffStackRule.Wasted, true, false,
                startTurn: async (buff, d) =>
                {
                    await buff.Owner.DamageSelfProcedure(buff.Stack);
                    await buff.Owner.ArmorGainSelfProcedure(buff.Stack);
                }),

            new ("灼烧", "受到敌方攻击时：造成[层数]伤害", BuffStackRule.Add, true, false,
                attacked: async (buff, d) =>
                {
                    if (!d.Recursive || d.Src == buff.Owner)
                        return;

                    await buff.Owner.DamageOppoProcedure(buff.Stack, recursive: false);
                }),

            new ("自动护甲", "每回合：护甲+[层数]", BuffStackRule.Add, true, false,
                startTurn: async (buff, d) => await d.Owner.ArmorGainSelfProcedure(buff.Stack)),
            new ("少阳", "获得护甲：额外+[层数]", BuffStackRule.Add, true, false,
                armorGained: async (buff, d) => d.Value += buff.Stack),
            new ("少阴", "施加减甲：额外+[层数]", BuffStackRule.Add, true, false,
                armorLose: async (buff, d) =>
                {
                    if (buff.Owner == d.Tgt)
                        return;
                    d.Value += buff.Stack;
                }),
            new ("永久暴击", "攻击附带暴击", BuffStackRule.Wasted, true, false),
            new ("天人合一", "激活所有架势", BuffStackRule.Wasted, true, false),

            new ("看破", "无效化敌人下一次攻击，并且反击", BuffStackRule.Add, true, false,
                attacked: async (buff, d) =>
                {
                    if (!d.Recursive || d.Src == buff.Owner)
                        return;

                    await buff.Owner.AttackProcedure(d.Value, d.WuXing, 1, d.LifeSteal, d.Pierce, d.Crit, false, d.Damaged);
                    d.Cancel = true;
                }),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }

    public override BuffEntry Default() => this["不存在的Buff"];
}
