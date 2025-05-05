
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CLLibrary;

public class SkillCategory : Category<SkillEntry>
{
    private static readonly StageClosure Crit = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Crit = true;
                                                    }, key: "CritClosure", description: "暴击", checkListener: true);

    private static readonly StageClosure LifeSteal = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.LifeSteal = true;
                                                    }, key: "LifeStealClosure", description: "吸血", checkListener: true);

    private static readonly StageClosure Penetrate = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Penetrate = true;
                                                    }, key: "PenetrateClosure", description: "穿透", checkListener: true);

    private static readonly StageClosure Shatter = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Shatter = true;
                                                    }, key: "ShatterClosure", description: "碎防", checkListener: true);

    private static readonly StageClosure WeakNoEffect = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int weak = d.Src.GetStackOfBuff("软弱");
                                                        d.Value += weak;
                                                    }, checkListener: true);

    private static readonly StageClosure WeakProvideAttack = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int weak = d.Src.GetStackOfBuff("软弱");
                                                        d.Value += 2 * weak;
                                                    }, checkListener: true);

    private static readonly StageClosure ConsumeArmorToAttack = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int toConsume = Mathf.Max(0, d.Src.Armor);
                                                        if (toConsume > 0)
                                                        {
                                                            await d.Src.LoseArmorProcedure(toConsume, induced: true);
                                                            d.Value += toConsume;
                                                        }
                                                    }, checkListener: true);

    private static readonly StageClosure ArmorToAttack = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int armor = Mathf.Max(0, d.Src.Armor);
                                                        if (armor > 0)
                                                            d.Value += armor;
                                                    }, checkListener: true);

    private static readonly StageClosure DamageToConstantArmor = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        
                                                        await d.Src.GainArmorProcedure(7 + 5 * skill.Dj, induced: true);
                                                        d.CastResult.AppendCond(true);
                                                    }, checkListener: true);

    private static readonly StageClosure DamageToArmor = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        
                                                        await d.Src.GainArmorProcedure(d.Value, induced: true);
                                                        d.CastResult.AppendCond(true);
                                                    }, checkListener: true);

    private static readonly StageClosure DamageToFragile = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        
                                                        await d.Tgt.LoseArmorProcedure(d.Value, induced: true);
                                                        d.CastResult.AppendCond(true);
                                                    }, checkListener: true);

    private static readonly StageClosure SetAttackHighest = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        string key = "HighestAttackRecord";
                                                        int highestAttackRecord = d.Src.Memory.TryGetVariable(key, 0);
                                                        d.Value = Mathf.Max(d.Value, highestAttackRecord);
                                                    }, checkListener: true);

    private static readonly StageClosure SetAttackHighestGainFromDuanTi = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        string key = "HighestAttackRecord";
                                                        int highestAttackRecord = d.Src.Memory.TryGetVariable(key, 0);
                                                        int duanTi = d.Src.GetStackOfBuff("锻体");
                                                        d.Value = Mathf.Max(d.Value, highestAttackRecord) + duanTi;
                                                    }, checkListener: true);

    private static readonly StageClosure QiShiClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill initiator = listener as StageSkill;
                                                        int value = 1 + initiator.Dj;
                                                        d.CastResult["QiShiLingQiGain"] = value.ToString();
                                                        await d.Src.GainBuffProcedure("灵气", value, induced: true);
                                                    }, key: "QiShiClosure", description: "击伤：灵气+[QiShiLingQiGain]", checkListener: true);

    private static readonly StageClosure LianXiClosure = new(StageClosureDict.WIL_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.RemoveArmorProcedure(d.Value, induced: true);
                                                        d.Cancel = true;
                                                    }, key: "LianXiClosure", description: "击伤：伤害转为破甲", checkListener: true);

    private static readonly StageClosure PanXuanClosure = new(StageClosureDict.WIL_GAIN_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainArmorDetails d = closureDetails as GainArmorDetails;
                                                        int gain = (-d.Src.Opponent().Armor).ClampLower(0);
                                                        if (gain <= 0) return; // write as cond
                                                        d.Value += gain;
                                                    }, key: "PanXuanClosure", description: "每1破甲，护甲+1", checkListener: true);

    private static readonly StageClosure BaiRenClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        d.Src.SetActionPoint(2);
                                                    }, key: "BaiRenClosure", description: "击伤：二动", checkListener: true);

    private static readonly StageClosure ShanFengClosure = new(StageClosureDict.WIL_CYCLE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CycleDetails d = closureDetails as CycleDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;
                                                        int shanFengConvert = 15 - 5 * initiator.Dj;
                                                        int gain = d.Owner.Armor / shanFengConvert;
                                                        d.Gain += gain;
                                                        d.CastResult["ShanFengConvert"] = shanFengConvert.ToString();
                                                    }, key: "ShanFengClosure", description: $"每[ShanFengConvert]护甲，锋锐+1", checkListener: true);

    private static readonly StageClosure XunLieClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;
                                                        int xunLieFragile = 5 + 5 * initiator.Dj;
                                                        await d.Src.RemoveArmorProcedure(xunLieFragile, induced: true);
                                                        d.CastResult["XunLieFragile"] = xunLieFragile.ToString();
                                                    }, key: "XunLieClosure", description: "击伤：施加[XunLieFragile]破甲", checkListener: true);

    private static readonly StageClosure ZhiShuiClosure = new(StageClosureDict.UNDAMAGED, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;
                                                        int zhiShuiHealth = 4 + 4 * initiator.Dj;
                                                        await d.Src.HealProcedure(zhiShuiHealth, induced: true);
                                                        d.CastResult["ZhiShuiHealth"] = zhiShuiHealth.ToString();
                                                    }, key: "ZhiShuiClosure", description: $"未击伤：气血+[ZhiShuiHealth]", checkListener: true);

    private static readonly StageClosure QiuLuBaiClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int qiuLuBaiConvert = 1 + skill.Dj;
                                                        int mul = d.Src.Memory.TryGetVariable(StageEntity.OppoLoseArmorTimesKey, 0);
                                                        d.Value += qiuLuBaiConvert * mul;
                                                        d.CastResult["QiuLuBaiConvert"] = qiuLuBaiConvert.ToString();
                                                    }, key: "QiuLuBaiClosure", description: $"对手护甲每降低过1次，多[QiuLuBaiConvert]攻", checkListener: true);

    private static readonly StageClosure TianDiTongShouClosure = new(StageClosureDict.WIL_LOSE_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        LoseArmorDetails d = closureDetails as LoseArmorDetails;
                                                        int fragile = -d.Src.Armor.ClampUpper(0);
                                                        d.Value += fragile;
                                                    }, key: "TianDiTongShouClosure", description: $"自身每1破甲，多1", checkListener: true);

    private static readonly StageClosure YiLianTuoShengClosure = new(StageClosureDict.WIL_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        int critStack = d.Src.GetStackOfBuff("暴击");
                                                        await d.Src.TryConsumeProcedure("暴击", critStack);
                                                        d.Value *= 1 + critStack;
                                                    }, key: "YiLianTuoShengClosure", description: "暴击释放", checkListener: true);

    private static readonly StageClosure ShanJiClosure1 = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;

                                                        int oppoFragileBeforeAttack = initiator.Owner.Opponent().Armor;
                                                        if (oppoFragileBeforeAttack < 0)
                                                            initiator.Owner.Memory.SetVariable("OppoFragileBeforeAttack", oppoFragileBeforeAttack);
                                                    }, key: "ShanJiClosure1", description: "", checkListener: true);

    private static readonly StageClosure ShanJiClosure2 = new(StageClosureDict.DID_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;

                                                        int oppoFragileBeforeAttack =
                                                            initiator.Owner.Memory.TryGetVariable("OppoFragileBeforeAttack", 0);

                                                        int gap = initiator.Owner.Opponent().Armor - oppoFragileBeforeAttack;
                                                        if (oppoFragileBeforeAttack < 0 && gap > 0)
                                                        {
                                                            await initiator.Owner.Opponent().LoseArmorProcedure(gap, induced: true);
                                                        }
                                                                        
                                                        initiator.Owner.Memory.SetVariable("OppoFragileBeforeAttack", 0);
                                                    }, key: "ShanJiClosure2", description: "破甲将补齐至攻击前的数值", checkListener: true);

    private static readonly StageClosure HaiXiaoClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;
                                                        int haiXiaoConvert = 3 + initiator.Dj;
                                                        d.CastResult["HaiXiaoConvert"] = haiXiaoConvert.ToString();
                                                        
                                                        int mana = d.Src.GetStackOfBuff("灵气");
                                                        await d.Src.TryConsumeProcedure("灵气", mana);
                                                        d.Value += mana * haiXiaoConvert;
                                                    }, key: "HaiXiaoClosure", description: "消耗每1灵气，多[HaiXiaoConvert]攻", checkListener: true);

    private static readonly StageClosure CaiHong2Closure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;

                                                        int times = initiator.GetJingJie() <= JingJie.YuanYing ? 1 : 2;
                                                        int mana = d.Src.GetStackOfBuff("灵气");

                                                        d.Stack += times * mana;
                                                    }, key: "CaiHong2Closure", description: "灵气翻倍", checkListener: true);

    private static readonly StageClosure CaiHong3Closure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill initiator = d.Listener as StageSkill;

                                                        int times = initiator.GetJingJie() <= JingJie.YuanYing ? 1 : 2;
                                                        int mana = d.Src.GetStackOfBuff("灵气");

                                                        d.Stack += times * mana;
                                                    }, key: "CaiHong3Closure", description: "灵气变成三倍", checkListener: true);

    private static readonly StageClosure YiMengRuShiClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.GainBuffProcedure("一梦如是", induced: true);
                                                    }, key: "YiMengRuShiClosure", "击伤：下1次受伤转为治疗", checkListener: true);

    private static readonly StageClosure XieYiClosure = new(StageClosureDict.DID_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        string critKey = "TriggeredCrit";
                                                        string lifestealKey = "TriggeredLifesteal";
                                                        string penetrateKey = "TriggeredPenetrate";
                                                        bool crit = d.Src.Memory.TryGetVariable(critKey, false);
                                                        bool lifeSteal = d.Src.Memory.TryGetVariable(lifestealKey, false);
                                                        bool penetrate = d.Src.Memory.TryGetVariable(penetrateKey, false);
                                                        if (crit) await d.Src.GainBuffProcedure("暴击", induced: true);
                                                        if (lifeSteal) await d.Src.GainBuffProcedure("吸血", induced: true);
                                                        if (penetrate) await d.Src.GainBuffProcedure("穿透", induced: true);
                                                        
                                                        d.CastResult["XieYiCrit"] = crit ? "暴击" : "暴击".ApplyInactive();
                                                        d.CastResult["XieYiLifeSteal"] = lifeSteal ? "吸血" : "吸血".ApplyInactive();
                                                        d.CastResult["XieYiPenetrate"] = penetrate ? "穿透" : "穿透".ApplyInactive();
                                                    }, key: "XieYiClosure", description: "返还触发过的[XieYiCrit]/[XieYiLifeSteal]/[XieYiPenetrate]", checkListener: true);

    private static readonly StageClosure QiTunShanHeClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        
                                                        int qiTunShanHeExtra = 1 + skill.Dj;
                                                        int highestMana = d.Src.Memory.TryGetVariable(StageEntity.HighestManaKey, 0);
                                                        int space = highestMana + qiTunShanHeExtra - d.Src.GetStackOfBuff("灵气");
                                                        d.Stack += space;
                                                        d.CastResult["QiTunShanHeExtra"] = qiTunShanHeExtra.ToString();
                                                    }, key: "QiTunShanHeClosure", description: $"灵气补至本局最高+[QiTunShanHeExtra]", checkListener: true);

    private static readonly StageClosure TunTianClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;

                                                        int healed = d.Src.Memory.TryGetVariable(StageEntity.ActualHealKey, 0);
                                                        int tunTianConvert = 5 - skill.Dj;
                                                        int gain = healed / tunTianConvert;
                                                        d.Value += gain;
                                                        d.CastResult["TunTianConvert"] = tunTianConvert.ToString();
                                                    }, key: "TunTianClosure", description: "每[TunTianConvert]累计治疗，多1攻", checkListener: true);

    private static readonly StageClosure XiaoSongClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int xiaoSongGrow = Fib.ToValue(3 + skill.Dj);
                                                        d.Value += xiaoSongGrow * skill.TotalStageCastedCount;
                                                        d.CastResult["XiaoSongGrow"] = xiaoSongGrow.ToString();
                                                    }, key: "XiaoSongClosure", description: $"成长：多[XiaoSongGrow]", checkListener: true);

    private static readonly StageClosure RuMuSanFenClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Value += d.Src.Opponent().Armor / 2;
                                                    }, key: "RuMuSanFenClosure", description: "对方每有2护甲，多1", checkListener: true);

    private static readonly StageClosure MingShenClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int mingShenGrow = skill.GetJingJie() >= JingJie.HuaShen ? 2 : 1;
                                                        d.Stack += mingShenGrow * skill.TotalStageCastedCount;
                                                        d.CastResult["MingShenGrow"] = mingShenGrow.ToString();
                                                    }, key: "MingShenClosure", description: "成长：多[MingShenGrow]", checkListener: true);

    private static readonly StageClosure LuoYingClosure = new(StageClosureDict.WIL_CYCLE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CycleDetails d = closureDetails as CycleDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int luoYingConvert = 5 - skill.Dj;

                                                        int manaStack = d.Owner.GetStackOfBuff("灵气");
                                                        int flow = manaStack / luoYingConvert;

                                                        int consume = flow * luoYingConvert;
                                                        await d.Owner.TryConsumeProcedure("灵气", consume);

                                                        d.Gain += flow;

                                                        d.CastResult["LuoYingConvert"] = luoYingConvert.ToString();
                                                    }, key: "LuoYingClosure", description: $"消耗每[LuoYingConvert]灵气，多1", checkListener: true);

    private static readonly StageClosure YiXinYiJianClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int power = d.Src.GetStackOfBuff("力量") + d.Src.GetStackOfBuff("剑意");
                                                        int yiXinYiJianConvert = 5 + skill.Dj;
                                                        d.Value += (yiXinYiJianConvert - 1) * power;
                                                        d.CastResult["YiXinYiJianConvert"] = yiXinYiJianConvert.ToString();
                                                    }, key: "YiXinYiJianClosure", description: $"力量/剑意具有[YiXinYiJianConvert]倍效果", checkListener: true);

    private static readonly StageClosure ShengJiClosure = new(StageClosureDict.WIL_HEAL, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        HealDetails d = closureDetails as HealDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int shengJiGrow = Fib.ToValue(2 + skill.Dj);
                                                        d.Value += skill.TotalStageCastedCount * shengJiGrow;
                                                        d.CastResult["ShengJiGrow"] = shengJiGrow.ToString();
                                                    }, key: "ShengJiClosure", description: $"成长：多[ShengJiGrow]", checkListener: true);

    private static readonly StageClosure YiNianWuLiangJieClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        if (d.BuffEntry.GetName() != "多重") return; // write as cond
                                                        int convert = 8;
                                                        int manaStack = d.Src.GetStackOfBuff("灵气");
                                                        int flow = (manaStack / convert).ClampUpper(20);
                                                        int consume = flow * convert;
                                                        await d.Src.TryConsumeProcedure("灵气", consume);
                                                        d.Stack += flow;
                                                    }, key: "YiNianWuLiangJieClosure", description: $"消耗每8灵气，多重+1", checkListener: true);

    private static readonly StageClosure JianWangXingClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.GainBuffProcedure("灵气", induced: true);
                                                    }, key: "JianWangXingClosure", description: "击伤：灵气+1", checkListener: true);

    private static readonly StageClosure YiWuJingHongClosure = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Times += d.Src.ExhaustedCount;
                                                    }, key: "YiWuJingHongClosure", description: "每1已升华牌，多1次", checkListener: true);

    private static readonly StageClosure ChangXiaClosure = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int value = d.Src.Memory.TryGetVariable(StageEntity.BurnTimesKey, 0);
                                                        d.Value += value;
                                                    }, key: "ChangXiaClosure", description: "每燃命过1次，多1攻", checkListener: true);
    
    
    
    
    private static readonly MergeRule NoMerge = new(
        name: "无法合成",
        errorMessage: "特殊卡牌不可参与合成",
        order: -1,
        processMerge: d =>
        {
            d.MergeTarget = new(
                mergeType: "无法合成",
                valid: false,
                errorMessage: "特殊卡牌不可参与合成",
                resultEntry: null,
                resultJingJie: null,
                resultWuXing: null,
                pred: null);
            d.State = MergeDetails.MergeState.Cancel;
        });

    private static readonly MergeRule DreamCard = new(
        name: "梦中卡牌",
        errorMessage: "梦中卡牌不可参与合成",
        order: -1,
        processMerge: d =>
        {
            d.MergeTarget = new(
                mergeType: "梦中卡牌",
                valid: false,
                errorMessage: "梦中卡牌不可参与合成",
                resultEntry: null,
                resultJingJie: null,
                resultWuXing: null,
                pred: null);
            d.State = MergeDetails.MergeState.Cancel;
        });

    public SkillCategory()
    {
        AddRange(new List<SkillEntry>()
        {
            #region 标准牌

            new(id:                         "0101",
                name:                       "金刃",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(4 + dj)),
                    new RemoveArmorProcedureDefinition(Fib.ToValue(3 + dj), induced: false),
                }),

            new(id:                         "0104",
                name:                       "起势",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("QiShiLingQiGain", (1 + dj).ToString()),
                    new AttackProcedureDefinition(4, closures: new [] { QiShiClosure }),
                    new GainBuffProcedureDefinition("灵气", 1 + dj, induced: false)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "0118",
                name:                       "敛息",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(6 + 4 * dj, closures: new [] { LianXiClosure }),
                }),

            new(id:                         "0109",
                name:                       "流云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(3 + 3 * dj),
                    new GainBuffProcedureDefinition("延迟攻", 3 + 3 * dj, induced: true)
                        .SetDescription((procedureDefinition, costResult, castResult) => $"下回合{3 + 3 * dj}攻"),
                }),
            
            new(id:                         "0125",
                name:                       "刃雨",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{6 + 4 * dj}攻".ApplyAttack() +
                    $"\n每相邻1张金，多1次",
                castGenerator:              async d =>
                {
                    bool prevIsJin = d.Skill.Prev(true).Entry.GetWuXing() == WuXing.Jin;
                    bool nextIsJin = d.Skill.Next(true).Entry.GetWuXing() == WuXing.Jin;

                    int times = 1 + (prevIsJin ? 1 : 0) + (nextIsJin ? 1 : 0);

                    await d.AttackProcedure(6 + 4 * d.Dj, times: times);
                }),
            
            new(id:                         "0126",
                name:                       "盘旋",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new RemoveArmorProcedureDefinition(5 + 5 * dj, induced: false),
                    new GainArmorProcedureDefinition(0, closures: new [] { PanXuanClosure }, induced: true)
                        .SetDescription(GainArmorProcedureDefinition.OnlyClosure),
                }),

            new(id:                         "0128",
                name:                       "白刃",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Swift,
                costGenerator:              CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(10 + 8 * dj, closures: new [] { BaiRenClosure }),
                }),
            
            new(id:                         "0115",
                name:                       "山风",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("ShanFengConvert", (15 - 5 * dj).ToString()),
                    new GainArmorProcedureDefinition(15 + 5 * dj, induced: false),
                    new CycleProcedureDefinition(WuXing.Jin, closures: new [] { ShanFengClosure })
                        .SetDescription(CycleProcedureDefinition.OnlyClosure),
                }),

            new(id:                         "0113",
                name:                       "无妄",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(6, times: 3, closures: new[] { Crit }),
                }),

            new(id:                         "0102",
                name:                       "寻猎",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("XunLieFragile", (5 + 5 * dj).ToString()),
                    new AttackProcedureDefinition(2, closures: new[] { XunLieClosure }),
                }),

            new(id:                         "0124",
                name:                       "秋露白",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("QiuLuBaiConvert", (1 + dj).ToString()),
                    new AttackProcedureDefinition(4 + 4 * dj, closures: new[] { QiuLuBaiClosure }),
                }),

            new(id:                         "0110",
                name:                       "天地同寿",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new RemoveArmorProcedureDefinition(Fib.ToValue(4 + dj), closures: new []{ TianDiTongShouClosure }),
                    new RemoveArmorProcedureDefinition(Fib.ToValue(4 + dj))
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "0123",
                name:                       "醉意",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(5 + 4 * dj, closures: new[] { Shatter }),
                }),
            
            new(id:                         "0127",
                name:                       "摇曳",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: 1 + dj),
                    new GainBuffProcedureDefinition("摇曳")
                        .SetDescription((procedureDefinition, costResult, castResult) => "锋锐变为施加破甲"),
                }),
            
            new(id:                         "0108",
                name:                       "刺穴",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 4),
                    new GainBuffProcedureDefinition("滞气", 4 - dj),
                    new SetActionPointProcedureDefinition(2),
                }),

            new(id:                         "0116",
                name:                       "袖里乾坤",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("暴击", 1 + dj),
                    new GainArmorProcedureDefinition(6, induced: true),
                    new GainBuffProcedureDefinition("暴击", 1 + dj)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "0117",
                name:                       "一莲托生",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1, closures: new []{ YiLianTuoShengClosure }),
                    new GiveBuffProcedureDefinition("跳行动")
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "0122",
                name:                       "闪击",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1, closures: new [] { ShanJiClosure1, ShanJiClosure2 } ),
                }),
            
            new(id:                         "0201",
                name:                       "恋花",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(3 + dj) * 2, closures: new [] { LifeSteal } ),
                }),

            new(id:                         "0204",
                name:                       "吐纳",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 2 + dj),
                    new GainMaxHealthProcedureDefinition(4 + 4 * dj),
                    new GainBuffProcedureDefinition("玄武吐息法")
                        .SetDescription((procedureDefinition, costResult, castResult) => "治疗可以穿上限")
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),
            
            new(id:                         "0205",
                name:                       "止水",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("ZhiShuiHealth", (4 + 4 * dj).ToString()),
                    new AttackProcedureDefinition(4 + 4 * dj, closures: new [] { ZhiShuiClosure } ),
                }),
            
            new(id:                         "0223",
                name:                       "调和",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+1".ApplyMana() +
                    $"\n气血+{1 + 4 * dj}".ApplyHeal() +
                    $"\n终结：翻倍".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(false);
                    int bitShift = endTuple.Item1 ? 1 : 0;

                    await d.GainBuffProcedure("灵气", 1 << bitShift);
                    await d.HealProcedure((1 + 4 * d.Dj) << bitShift, induced: true);

                    d.CastResult.AppendEndTuple(endTuple);
                }),
            
            new(id:                         "0213",
                name:                       "大鱼",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend | SkillType.Swift,
                costGenerator:              CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(4 * (1 << dj)),
                    new GainArmorProcedureDefinition(4 * (1 << dj), induced: true),
                    new SetActionPointProcedureDefinition(2),
                }),
            
            new(id:                         "0506",
                name:                       "海啸",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("HaiXiaoConvert", (3 + dj).ToString()),
                    new AttackProcedureDefinition(14, closures: new [] { HaiXiaoClosure }),
                }),

            new(id:                         "0225",
                name:                       "彩虹",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                costGenerator:              CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 0, closures: new [] { CaiHong2Closure })
                        .SetDescription((procedureDefinition, costResult, castResult) => "灵气翻倍")
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    new GainBuffProcedureDefinition("灵气", 0, closures: new [] { CaiHong3Closure })
                        .SetDescription((procedureDefinition, costResult, castResult) => "灵气变成三倍")
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new GainBuffProcedureDefinition("禁止灵气")
                        .SetDescription((procedureDefinition, costResult, castResult) => "之后无法获得灵气"),
                }),
            
            new(id:                         "0219",
                name:                       "飞鸿踏雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("飞鸿踏雪", 0, closures: new [] { CaiHong3Closure })
                        .SetDescription((procedureDefinition, costResult, castResult) => "二动时：获得1格挡")
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new CycleProcedureDefinition(WuXing.Shui, gain: 1),
                    new SetActionPointProcedureDefinition(2),
                }),

            new(id:                         "0217",
                name:                       "一梦如是",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1, closures: new [] { YiMengRuShiClosure }),
                }),
            
            new(id:                         "0206",
                name:                       "空幻",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              async (env, entity, skill, recursive) =>
                    new ManaCostResult(3 + skill.Dj - entity.TraversalSkills().Count(s => s.Entry.WuXing == WuXing.Shui)),
                costDescription:            CostDescription.ManaFromDj(dj => 3 + dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{6 + 6 * dj}攻".ApplyAttack() +
                    $"\n每携带1水：消耗-1",
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(6 + 6 * d.Dj);
                }),
            
            new(id:                         "0222",
                name:                       "激流",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                costGenerator:              CostResult.ManaFromDj(dj => 3 + dj),
                costDescription:            CostDescription.ManaFromDj(dj => 3 + dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(8 + 4 * dj),
                    new GainBuffProcedureDefinition("灵气", 3 + dj, induced: true),
                }),
            
            new(id:                         "0224",
                name:                       "潮汐",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Attack | SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}".ApplyMana() +
                    $"\n爆能{5 + 5 * dj}：{20 + 10 * dj}攻 吸血".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气", 1 + d.Dj);
                    
                    bool cond = await d.Caster.TryConsumeProcedure("灵气", 5 + 5 * d.Dj);

                    if (cond)
                    {
                        await d.AttackProcedure(20 + 10 * d.Dj,
                            closures: new [] { LifeSteal });
                    }

                    d.CastResult.AppendCond(cond);
                }),
            
            new(id:                         "0208",
                name:                       "踏浪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                costGenerator:              CostResult.ManaFromDj(dj => 2 + dj),
                costDescription:            CostDescription.ManaFromDj(dj => 2 + dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 5 + 2 * dj),
                }),
            
            new(id:                         "0209",
                name:                       "写意",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_DAMAGE, 1, async (listener, closure, closureDetails) =>
                    {
                        StageSkill s = listener as StageSkill;
                        DamageDetails d = (DamageDetails)closureDetails;
                    
                        if (s.Owner != d.Src) return;
                    
                        string critKey = "TriggeredCrit";
                        s.Owner.Memory.PerformOperation(critKey, false, record => record | d.Crit);
                        
                        string lifestealKey = "TriggeredLifesteal";
                        s.Owner.Memory.PerformOperation(lifestealKey, false, record => record | d.LifeSteal);
                    }),
                    new(StageClosureDict.WIL_ATTACK, 1, async (listener, closure, closureDetails) =>
                    {
                        StageSkill s = listener as StageSkill;
                        AttackDetails d = (AttackDetails)closureDetails;
                    
                        if (s.Owner != d.Src) return;
                        
                        string penetrateKey = "TriggeredPenetrate";
                        s.Owner.Memory.PerformOperation(penetrateKey, false, record => record | d.Penetrate);
                    }),
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("XieYiCrit", "暴击"),
                    new SetValueProcedureDefinition("XieYiLifeSteal", "吸血"),
                    new SetValueProcedureDefinition("XieYiPenetrate", "穿透"),
                    new AttackProcedureDefinition(10 + 4 * dj, closures: new [] { XieYiClosure }),
                }),

            new(id:                         "0216",
                name:                       "气吞山河",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("QiTunShanHeExtra", (1 + dj).ToString()),
                    new GainBuffProcedureDefinition("灵气", 0, closures: new [] { QiTunShanHeClosure })
                        .SetDescription(GainBuffProcedureDefinition.OnlyClosure),
                }),
            
            new(id:                         "0218",
                name:                       "吞天",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("TunTianConvert", (5 - dj).ToString()),
                    new AttackProcedureDefinition(1, closures: new []{ TunTianClosure }),
                }),
            
            new(id:                         "0211",
                name:                       "瑞雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                costGenerator:              CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Shui, gain: 1 + dj),
                    new GainBuffProcedureDefinition("瑞雪", induced: false)
                        .SetDescription((procedureDefinition, costResult, castResult) => "格挡变成治疗"),
                    new GainBuffProcedureDefinition("禁止二动", induced: false)
                        .SetDescription((procedureDefinition, costResult, castResult) => "无法二动"),
                }),
            
            new(id:                         "0221",
                name:                       "镜花水月",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Mana | SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"消耗每40气血，灵气+1".ApplyMana() +
                    $"\n消耗每1灵气，气血+10".ApplyHeal(),
                castGenerator:              async d =>
                {
                    int healthConsume = (d.Caster.Hp - 1).ClampLower(0) / 40 * 40;
                    int manaConsume = d.Caster.GetStackOfBuff("灵气");

                    if (healthConsume > 0)
                        await d.LoseHealthProcedure(healthConsume, causedByAttack: false, induced: true);
                    if (manaConsume > 0)
                        await d.LoseBuffProcedure("灵气", manaConsume);

                    int healthGain = manaConsume * 10;
                    int manaGain = healthConsume / 40;

                    if (healthGain > 0)
                        await d.HealProcedure(healthGain, induced: true);
                    if (manaGain > 0)
                        await d.GainBuffProcedure("灵气", manaGain);
                }),
            
            new(id:                         "0301",
                name:                       "小松",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.ZiZhi,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("XiaoSongGrow", Fib.ToValue(3 + dj).ToString()),
                    new AttackProcedureDefinition(Fib.ToValue(4 + dj), closures: new []{ XiaoSongClosure }),
                }),

            new(id:                         "0309",
                name:                       "入木三分",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(4 + 4 * dj, closures: new []{ RuMuSanFenClosure }),
                }),
            
            new(id:                         "0304",
                name:                       "明神",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.ZiZhi,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}".ApplyMana() +
                    $"\n成长：多{(j >= JingJie.HuaShen ? 2 : 1)}",
                castGenerator:              async d =>
                {
                    int mul = d.J >= JingJie.HuaShen ? 2 : 1;
                    await d.GainBuffProcedure("灵气", 1 + d.Dj + d.Cc * mul);
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("MingShenGrow", (j >= JingJie.HuaShen ? 2 : 1).ToString()),
                    new GainBuffProcedureDefinition("灵气", 1 + dj, closures: new []{ MingShenClosure }),
                }),

            new(id:                         "0307",
                name:                       "回春",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Health,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(6 + 4 * dj, induced: false)
                        .SetDescription((procedureDefinition, costResult, castResult) => $"双方护甲+{6 + 4 * dj}"),
                    new GiveArmorProcedureDefinition(6 + 4 * dj, induced: true)
                        .SetDescription((procedureDefinition, costResult, castResult) => ""),
                    new HealProcedureDefinition(6 + 4 * dj, induced: false)
                        .SetDescription((procedureDefinition, costResult, castResult) => $"双方气血+{6 + 4 * dj}"),
                    new HealOppoProcedureDefinition(6 + 4 * dj, induced: true)
                        .SetDescription((procedureDefinition, costResult, castResult) => ""),
                }),

            new(id:                         "0315",
                name:                       "落英",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("LuoYingConvert", (5 - dj).ToString()),
                    new CycleProcedureDefinition(WuXing.Mu, gain: 1 + dj, closures: new []{ LuoYingClosure }),
                }),
            
            new(id:                         "0312",
                name:                       "钟声",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("钟声", 1 + dj)
                        .SetDescription((procedureDefinition, costResult, castResult) => $"使下{1 + dj}张牌升级"),
                }),

            new(id:                         "0324",
                name:                       "一心一剑",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("YiXinYiJianConvert", (5 + dj).ToString()),
                    new AttackProcedureDefinition(4 + 4 * dj, closures: new []{ YiXinYiJianClosure }),
                }),
            
            new(id:                         "0323",
                name:                       "梅开二度",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                costGenerator:              CostResult.ChannelFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 1 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("二重", 1 + dj)
                        .SetDescription((procedureDefinition, costResult, castResult) => $"下{1 + dj}张牌使用两次"),
                }),

            new(id:                         "0317",
                name:                       "一叶知秋",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_FULL_ATTACK, -1, async (listener, closure, closureDetails) =>
                    {
                        StageSkill s = listener as StageSkill;
                        AttackDetails d = (AttackDetails)closureDetails;

                        if (s.Owner != d.Src) return;
                        if (d.Listener == s) return;
                        StageSkill initiator = d.Listener as StageSkill;
                        if (initiator == null) return;

                        string key = "UsedClosureDict";
                        s.Owner.Memory.PerformOperation(key, new Dictionary<StageSkill, StageClosure[]>(), record =>
                        {
                            if (record.ContainsKey(initiator))
                            {
                                bool newHasMore = d.Closures.Length > record[initiator].Length;
                                if (newHasMore)
                                    record[initiator] = d.Closures;
                                return record;
                            }
                            record[initiator] = d.Closures;
                            return record;
                        });
                    }),
                },
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n具有所有已触发的攻击描述",
                castGenerator:              async d =>
                {
                    string key = "UsedClosureDict";
                    Dictionary<StageSkill, StageClosure[]> dict = d.Caster.Memory.TryGetVariable(key, new Dictionary<StageSkill, StageClosure[]>());
                    List<StageClosure> flattenedList = new List<StageClosure>();
                    dict.Do(kvp => flattenedList.AddRange(kvp.Value));
                    
                    StageClosure[] closures = flattenedList.ToArray();
                    await d.AttackProcedure(1, closures: closures);
                }),

            new(id:                         "0302",
                name:                       "潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend | SkillType.ZiZhi,
                costGenerator:              CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避")
                        .SetPostCondDefinition(PostCondDefinition.FromCc(2 + dj, false)),
                    new AttackProcedureDefinition((4 + 2 * dj) * (4 + 2 * dj))
                        .SetPostCondDefinition(PostCondDefinition.FromCc(2 + dj, true)),
                }),

            new(id:                         "0306",
                name:                       "见龙在田",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend | SkillType.ZiZhi,
                costGenerator:              async (env, entity, skill, recursive) => new ChannelCostResult(5 - skill.Dj - skill.TotalStageCastedCount),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻".ApplyAttack() + " 闪避+2".ApplyDefend() +
                    $"\n成长：吟唱-1",
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(10);
                    await d.GainBuffProcedure("闪避", 2);
                }),
            
            new(id:                         "0321",
                name:                       "生机",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetValueProcedureDefinition("ShengJiGrow", Fib.ToValue(2 + dj).ToString()),
                    new HealProcedureDefinition(2 + 4 * dj, closures: new []{ ShengJiClosure }),
                }),
            
            new(id:                         "0308",
                name:                       "清泉",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                costGenerator:              async (env, entity, skill, recursive) => new ChannelCostResult(skill.IsFirstTime ? 3 : 0),
                costDescription:            CostDescription.ChannelFromValue(3),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+{4 + 2 * dj}".ApplyMana() +
                    $"\n非初次：无需吟唱",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气", 4 + 2 * d.Dj);
                }),

            new(id:                         "0310",
                name:                       "飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.ZiZhi,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避"),
                    new GainBuffProcedureDefinition("飞龙在天", 2 + 2 * dj, induced: true)
                        .SetPostCondDefinition(PostCondDefinition.FirstTime)
                        .SetDescription((procedureDefinition, costResult, castResult) => $"跳过下{2 + 2 * dj}张牌，使其成长"),
                }),

            new(id:                         "0316",
                name:                       "回响",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                costGenerator:              CostResult.ManaFromJ(j => (j <= JingJie.JinDan ? 2 : 0)),
                costDescription:            CostDescription.ManaFromJ(j => (j <= JingJie.JinDan ? 2 : 0)),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"使用第一张牌\n已升华的牌无效" : $"使用前两张牌\n已升华的牌无效"),
                castGenerator:              async d =>
                {
                    if (!d.Recursive)
                        return;
                    if (!d.Caster._skills[0].Exhausted)
                        await d.Caster.CastProcedure(d.Caster._skills[0], false);
                    
                    if (d.J >= JingJie.HuaShen)
                        if (!d.Caster._skills[1].Exhausted)
                            await d.Caster.CastProcedure(d.Caster._skills[1], false);
                }),

            new(id:                         "0318",
                name:                       "亢龙有悔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Mu, gain: 2 + dj, induced: true),
                    new GainBuffProcedureDefinition("闪避", 2 + dj, induced: true),
                    new AttackProcedureDefinition(2 + dj, times: 2 + dj),
                    new GainBuffProcedureDefinition("不堪一击", induced: true)
                        .SetDescription((procedureDefinition, costResult, castResult) => "遭受不堪一击"),
                }),
    
            new(id:                         "0314",
                name:                       "鹤回翔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Swift,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"交换左右牌" +
                    (j <= JingJie.YuanYing ? $"" :
                        $"\n二动"),
                castGenerator:              async d =>
                {
                    int leftIndex = d.Skill.Prev(true).SlotIndex;
                    int rightIndex = d.Skill.Next(true).SlotIndex;
                    
                    var tempStageSkill = d.Caster._skills[leftIndex];
                    d.Caster._skills[leftIndex] = d.Caster._skills[rightIndex];
                    d.Caster._skills[rightIndex] = tempStageSkill;

                    var tempIndex = d.Caster._skills[leftIndex].SlotIndex;
                    d.Caster._skills[leftIndex].SlotIndex = d.Caster._skills[rightIndex].SlotIndex;
                    d.Caster._skills[rightIndex].SlotIndex = tempIndex;
                    
                    if (d.J > JingJie.YuanYing)
                        d.Caster.SetActionPoint(2);
                }),

            new(id:                         "0320",
                name:                       "一念无量劫",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("多重", 0, closures: new []{ YiNianWuLiangJieClosure })
                        .SetDescription(GainBuffProcedureDefinition.OnlyClosure),
                },
                trivia:"在个人量子超算还没普及的时代，凡人只能体验个二十劫意思意思"),
            
            new(id:                         "0401",
                name:                       "云袖",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(2 + 2 * dj, times: 2),
                    new GainArmorProcedureDefinition(2 + 2 * dj, induced: false),
                }),

            new(id:                         "0403",
                name:                       "正念",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Exhaust,
                costGenerator:              CostResult.ChannelFromDj(dj => 5 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GainArmorProcedureDefinition(10 + 10 * dj),
                }),

            new(id:                         "0413",
                name:                       "剑王行",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1, times: 2 + dj, closures: new []{ JianWangXingClosure }),
                }),
            
            new(id:                         "0423",
                name:                       "战意",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("剑意", 2 + dj),
                    new GainArmorProcedureDefinition(2 + dj, induced: true),
                }),
            
            new(id:                         "0409",
                name:                       "天衣无缝",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(2, induced: true),
                    new GainBuffProcedureDefinition("天衣无缝", 1 + 4 * dj)
                        .SetDescription((procedureDefinition, costResult, castResult) => $"直到使用攻击牌：每回合{1 + 4 * dj}攻不消耗剑意"),
                }),
            
            new(id:                         "0404",
                name:                       "拂晓",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                costGenerator:              CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                skillTypeComposite:         SkillType.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GainBuffProcedureDefinition("剑意", 3),
                }),

            new(id:                         "0426",
                name:                       "长明",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灼烧+1" +
                    $"\n每1灼烧，剑意+1" +
                    (j <= JingJie.YuanYing ? "" : "\n下1次攻击保留剑意"),
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Huo, gain: 1);
                    int stack = d.Caster.GetStackOfBuff("灼烧");
                    await d.GainBuffProcedure("剑意", stack, induced: true);

                    if (d.J <= JingJie.YuanYing)
                        ;
                    else
                        await d.GainBuffProcedure("长明", induced: true);
                }),

            new(id:                         "0412",
                name:                       "登宝塔",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                costGenerator:              CostResult.ChannelFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 1 - dj),
                skillTypeComposite:         SkillType.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GainBuffProcedureDefinition("升华")
                        .SetDescription((procedureDefinition, costResult, castResult) => "下一张牌具有升华"),
                }),
            
            new(id:                         "0417",
                name:                       "一舞惊鸿",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1, closures: new []{ YiWuJingHongClosure }),
                }),

            new(id:                         "0402",
                name:                       "轰天",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                costGenerator:              CostResult.HealthFromDj(dj => Fib.ToValue(5 + dj)),
                costDescription:            CostDescription.HealthFromDj(dj => Fib.ToValue(5 + dj)),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(6 + dj)}攻".ApplyAttack() +
                    $"\n满血：多{Fib.ToValue(6 + dj)}".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            StageSkill s = d.Listener as StageSkill;
                            if (listener != d.Listener) return;
                            d.Value += Fib.ToValue(6 + s.Dj);
                        });

                    bool cond = d.Caster.IsFullHealth;
                    await d.AttackProcedure(Fib.ToValue(6 + d.Dj),
                        closures: cond ? new [] { closure } : null);
                    
                    d.CastResult.AppendCond(cond);
                }),

            new(id:                         "0422",
                name:                       "明镜",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Health,
                costGenerator:              async (env, entity, skill, recursive) =>
                    new HealthCostResult(entity.IsLowHealth ? 1 : Fib.ToValue(5 + skill.Dj)),
                costDescription:            CostDescription.HealthFromDj(dj => Fib.ToValue(5 + dj)),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(5 + dj)}".ApplyDefend() +
                    $"\n残血：只需1消耗".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(Fib.ToValue(5 + d.Dj), induced: false);
                    bool cond = d.Caster.IsLowHealth;
                    d.CastResult.AppendCond(cond);
                }),
            
            new(id:                         "0424",
                name:                       "怒瞳",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              async (env, entity, skill, recursive) =>
                    new ChannelCostResult(3 + skill.Dj - entity.Memory.TryGetVariable(StageEntity.BurnTimesKey, 0)),
                costDescription:            CostDescription.ChannelFromDj(dj => 3 + dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻".ApplyAttack() +
                    $"\n每燃命1次，吟唱-1",
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(10 + 10 * d.Dj);
                }),
            
            new(id:                         "0408",
                name:                       "舍生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Health,
                costGenerator:              async (env, entity, skill, recursive) =>
                    new HealthCostResult(entity.IsLowHealth ? 0 : 8),
                costDescription:            CostDescription.HealthFromValue(8),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}".ApplyMana() +
                    $"\n锻体+5" +
                    $"\n残血：免除消耗".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气", 2 + d.Dj);
                    await d.GainBuffProcedure("锻体", 5);

                    bool cond = d.Caster.IsLowHealth;
                    d.CastResult.AppendCond(cond);
                }),

            new(id:                         "0406",
                name:                       "不动明王诀",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{30 + 10 * dj}攻".ApplyAttack() +
                    " 成为残血" +
                    "\n初次：3回合，气血无法降低至0".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(30 + 10 * d.Dj);
                    await d.BecomeLowHealth();
                    bool cond = d.Skill.IsFirstTime;

                    if (cond)
                        await d.GainBuffProcedure("不屈", 3, induced: true);
                    
                    d.CastResult.AppendCond(cond);
                }),

            new(id:                         "0407",
                name:                       "浴火",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Health,
                costGenerator:              CostResult.HealthFromValue(8),
                costDescription:            CostDescription.HealthFromValue(8),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Huo, gain: 1 + dj),
                    new GainBuffProcedureDefinition("浴火")
                        .SetDescription((procedureDefinition, costResult, castResult) => "燃命时：根据灼烧造成伤害"),
                }),

            new(id:                         "0416",
                name:                       "晚霞",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                costGenerator:              CostResult.HealthFromDj(dj => Fib.ToValue(8 + dj)),
                costDescription:            CostDescription.HealthFromDj(dj => Fib.ToValue(8 + dj)),
                skillTypeComposite:         SkillType.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("剑意", 6 + 2 * dj),
                }),

            new(id:                         "0415",
                name:                       "观众生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Exhaust,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华左边牌" + (j == JingJie.HuaShen ? "，两次" : ""),
                castGenerator:              async d =>
                {
                    for (int i = 0; i < 1 + d.Dj; i++)
                    {
                        StageSkill skill = d.Skill.Prevs(false).FirstObj(skill => !skill.Exhausted) ?? d.Skill;
                        await skill.ExhaustProcedure();
                    }
                }),

            new(id:                         "0425",
                name:                       "常夏",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              CostResult.ChannelFromValue(5),
                costDescription:            CostDescription.ChannelFromValue(5),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(9, times: 9, closures: new []{ ChangXiaClosure }),
                    new GainBuffProcedureDefinition("禁止行动")
                        .SetDescription((procedureDefinition, costResult, castResult) => "禁止行动"),
                },
                trivia: "用完之后就会体力耗尽动弹不得"),

            new(id:                         "0501",
                name:                       "寸劲",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(6 + dj)}攻".ApplyAttack() +
                    $"\n遭受{(int)j switch { 0 => 4, 1 => 6, 2 => 8, 3 => 11, _ => 15 }}软弱".ApplyDebuff() +
                    $"\n终结：软弱不影响攻击".ApplyEnd(castResult) +
                    $"\n大终结：软弱提供攻击".ApplyDoubleEnd(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(allowDoubleEnd: true);
                    int value = d.J switch { 0 => 4, 1 => 6, 2 => 8, 3 => 11, _ => 15 };

                    if (endTuple.Item2)
                    {
                        await d.GainBuffProcedure("软弱", value);
                        await d.AttackProcedure(Fib.ToValue(6 + d.Dj),
                            closures: new[] { WeakProvideAttack });
                    }
                    else if (endTuple.Item1)
                    {
                        await d.AttackProcedure(Fib.ToValue(6 + d.Dj),
                            closures: new[] { WeakNoEffect });
                        await d.GainBuffProcedure("软弱", value);
                    }
                    else
                    {
                        await d.AttackProcedure(Fib.ToValue(6 + d.Dj));
                        await d.GainBuffProcedure("软弱", value);
                    }
                    
                    d.CastResult.AppendEndTuple(endTuple);
                }),

            new(id:                         "0502",
                name:                       "八极拳",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(4 + dj)}".ApplyDefend() +
                    $"\n终结：消耗每1护甲，1攻".ApplyEnd(castResult) +
                    $"\n大终结：每1护甲，1攻".ApplyDoubleEnd(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(allowDoubleEnd: true);
                    
                    if (endTuple.Item2)
                    {
                        await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), induced: true);
                        await d.AttackProcedure(1,
                            closures: new[] { ArmorToAttack });
                    }
                    else if (endTuple.Item1)
                    {
                        await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), induced: true);
                        await d.AttackProcedure(1,
                            closures: new[] { ConsumeArmorToAttack });
                    }
                    else
                    {
                        await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), induced: false);
                    }
                    
                    d.CastResult.AppendEndTuple(endTuple);
                }),

            new(id:                         "0522",
                name:                       "活步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(5 + dj)}".ApplyDefend() +
                    $"\n遭受{2 + 2 * dj}软弱".ApplyDebuff(),
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(Fib.ToValue(5 + d.Dj), induced: false);
                    await d.GainBuffProcedure("软弱", stack: 2 + 2 * d.Dj, induced: true);
                }),

            new(id:                         "0523",
                name:                       "滑步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(4 + dj)}".ApplyDefend() +
                    $"\n终结：{Fib.ToValue(5 + dj)}攻".ApplyEnd(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(allowDoubleEnd: false);

                    if (endTuple.Item1)
                    {
                        await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), induced: true);
                        await d.AttackProcedure(Fib.ToValue(4 + d.Dj));
                    }
                    else
                    {
                        await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), induced: false);
                    }
                    
                    d.CastResult.AppendEndTuple(endTuple);
                }),

            new(id:                         "0513",
                name:                       "震脚",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{7 + 5 * dj}攻".ApplyAttack() +
                    ($"\n击伤：" + $"护甲+{7 + 5 * dj}".ApplyDefend()).ApplyCond(castResult) +
                    $"\n终结：根据击伤值".ApplyEnd(castResult) +
                    $"\n大终结：根据击伤值，效果变为破甲".ApplyDoubleEnd(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(allowDoubleEnd: true);
                    d.CastResult.AppendCond(false);
                    
                    if (endTuple.Item2)
                    {
                        await d.AttackProcedure(7 + 5 * d.Dj,
                            closures: new[] { DamageToFragile });
                    }
                    else if (endTuple.Item1)
                    {
                        await d.AttackProcedure(7 + 5 * d.Dj,
                            closures: new[] { DamageToArmor });
                    }
                    else
                    {
                        await d.AttackProcedure(7 + 5 * d.Dj,
                            closures: new[] { DamageToConstantArmor });
                    }
                    
                    d.CastResult.AppendEndTuple(endTuple);
                }),

            new(id:                         "0407",
                name:                       "无畏",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"坚毅+{1 + dj}" +
                    $"\n每1坚毅，净化1",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("坚毅", 1 + d.Dj);
                    await d.CycleProcedure(WuXing.Tu);
                    int stack = d.Caster.GetStackOfBuff("坚毅");
                    await d.DispelProcedure(stack);
                }),

            new(id:                         "0510",
                name:                       "崩山掌",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+20".ApplyDefend() +
                    $"\n下{1 + dj}次失去护甲时，返还",
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(20, induced: false);
                    await d.GainBuffProcedure("护甲返还", 1 + d.Dj, induced: true);
                }),

            new(id:                         "0530",
                name:                       "架势",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                costGenerator:              CostResult.ChannelFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 1 - dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"激活下1次终结" +
                    $"\n终结：大终结".ApplyEnd(castResult) +
                    $"\n大终结：2次大终结".ApplyDoubleEnd(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(allowDoubleEnd: true);
                    
                    if (endTuple.Item2)
                    {
                        await d.GainBuffProcedure("大终结", 2);
                    }
                    else if (endTuple.Item1)
                    {
                        await d.GainBuffProcedure("大终结");
                    }
                    else
                    {
                        await d.GainBuffProcedure("终结");
                    }
                    
                    d.CastResult.AppendEndTuple(endTuple);
                }),
            
            new(id:                         "0529",
                name:                       "须弥结界",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"闪避+1".ApplyDefend() +
                    $"\n开局：" +
                    $"护甲+100".ApplyDefend() +
                    $"\n无法获得护甲".ApplyDebuff(),
                // startStageCast:             async d =>
                // {
                //     await d.Caster.GainArmorProcedure(100, induced: false);
                //     await d.Caster.GainBuffProcedure("禁止护甲", induced: false);
                // },
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("闪避");
                }),

            new(id:                         "SKILL_DTSZ_001",
                name:                       "固元",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"合成：气血上限增加{Fib.ToValue(3 + dj)}",
                overridingMergeRule:        new MergeRule(
                    name:                       "固元",
                    errorMessage:               null,
                    order:                      -101,
                    processMerge:               d =>
                    {
                        int value = Fib.ToValue(3 + d.Src.Dj);
                        d.AddSideEffect(() =>
                        {
                            RunManager.Instance.Environment.GainHealthProcedure(value);
                        });
                        d.State = MergeDetails.MergeState.Continue;
                    })),

            new(id:                         "SKILL_DTSZ_002",
                name:                       "锻骨",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                {
                    return $"气血+{6 + dj}\n" + (int)j switch
                    {
                        0 => "锻体+3",
                        1 => "锻体+3\n" + "满血：多3".ApplyStyle(castResult, "0"),
                        2 => "锻体+4\n" + "满血：多4".ApplyStyle(castResult, "0"),
                        3 => "锻体+4\n" + "满血".ApplyStyle(castResult, "0") + "/" + "残血".ApplyStyle(castResult, "1") + "：多4",
                        _ => "锻体+5\n" + "满血".ApplyStyle(castResult, "0") + "/" + "残血".ApplyStyle(castResult, "1") + "：多5"
                    };
                },
                castGenerator:              async d =>
                {
                    await d.HealProcedure(6 + d.Dj, induced: true);
                    bool cond0 = d.Caster.IsFullHealth;
                    bool cond1 = d.Caster.IsLowHealth;
                    int value = d.J switch {
                        0 => 3,
                        1 => 3 + (cond0 ? 3 : 0),
                        2 => 4 + (cond0 ? 4 : 0),
                        3 => 4 + (cond0 ? 4 : 0) + (cond1 ? 4 : 0),
                        _ => 5 + (cond0 ? 5 : 0) + (cond1 ? 5 : 0)
                    };
                    await d.GainBuffProcedure("锻体", value);
                    d.CastResult.AppendBools(cond0, cond1);
                }),

            new(id:                         "SKILL_DTSZ_003",
                name:                       "守势",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(4 + dj)}".ApplyDefend() +
                    $"\n满血：三倍".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    int value = Fib.ToValue(4 + d.Dj);
                    bool cond = d.Caster.IsFullHealth;
                    int times = cond ? 3 : 1;
                    await d.GainArmorProcedure(value * times, induced: false);

                    d.CastResult.AppendCond(cond);
                }),

            new(id:                         "0505",
                name:                       "一力降十会",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                {
                    return $"{dj switch { 0 => 10, 1 => 25, 2 => 45, 3 => 70, _ => 100 }}攻".ApplyAttack() +
                           $"\n遭受4跳行动".ApplyDebuff() +
                           $"\n唯一攻击牌：免除".ApplyCond(castResult);
                },
                castGenerator:              async d =>
                {
                    int value = d.Dj switch { 0 => 10, 1 => 25, 2 => 45, 3 => 70, _ => 100 };
                    await d.AttackProcedure(value);

                    bool cond = d.Skill.NoOtherAttack;
                    if (!cond)
                        await d.GainBuffProcedure("跳行动", 4);
                    d.CastResult.AppendCond(cond);
                }),
            
            new(id:                         "0521",
                name:                       "顺势斩",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                costGenerator:              CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_ATTACK, -1, async (listener, closure, closureDetails) =>
                    {
                        StageSkill s = listener as StageSkill;
                        AttackDetails d = (AttackDetails)closureDetails;

                        if (s.Owner != d.Src) return;

                        string key = "HighestAttackRecord";
                        s.Owner.Memory.PerformOperation(key, 0, record => Mathf.Max(record, d.Value));
                    }),
                },
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"造成本局最高攻击".ApplyAttack() +
                    $"\n终结：锻体提供攻击".ApplyEnd(castResult) +
                    $"\n大终结：锻体提供攻击，攻击2次".ApplyDoubleEnd(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(allowDoubleEnd: true);

                    if (endTuple.Item2)
                    {
                        await d.AttackProcedure(1, times: 2,
                            closures: new[] { SetAttackHighestGainFromDuanTi });
                    }
                    else if (endTuple.Item1)
                    {
                        await d.AttackProcedure(1,
                            closures: new[] { SetAttackHighestGainFromDuanTi });
                    }
                    else
                    {
                        await d.AttackProcedure(1,
                            closures: new[] { SetAttackHighest });
                    }
                    
                    d.CastResult.AppendEndTuple(endTuple);
                }),

            new(id:                         "0528",
                name:                       "磐石",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"坚毅+{1 + dj}" +
                    $"\n开局：" +
                    $"吟唱时：" + $"坚毅+{1 + dj}".ApplyDefend(),
                // startStageCast:             async d =>
                // {
                //     await d.Caster.GainBuffProcedure("磐石", 1 + d.Skill.Dj, induced: false);
                // },
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Tu, gain: 1 + d.Dj);
                }),

            new(id:                         "SKILL_DTSZ_005",
                name:                       "锻髓",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Health,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锻体+{6 + dj}" +
                    $"\n每1锻体，治疗{1 + dj}".ApplyHeal() +
                    $"\n移除多余气血上限",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("锻体", 6 + d.Dj);
                    int stack = d.Caster.GetStackOfBuff("锻体");
                    await d.HealProcedure(stack * (1 + d.Dj), induced: true);
                    d.Caster.MaxHp = d.Caster.Hp;
                }),

            new(id:                         "SKILL_DTSZ_008",
                name:                       "养生",
                wuXing:                     WuXing.Tu,
                skillTypeComposite:         SkillType.Health,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"回复{10 + 10 * dj}气血" +
                    $"\n满血：二动".ApplyStyle(castResult, "0") +
                    $"\n残血：翻倍".ApplyStyle(castResult, "1"),
                castGenerator:              async d =>
                {
                    bool cond0 = d.Caster.IsFullHealth;
                    bool cond1 = d.Caster.IsLowHealth;
                    
                    await d.HealProcedure((10 + 10 * d.Dj) << (cond1 ? 1 : 0), induced: false);
                    
                    if (cond0)
                        d.Caster.SetActionPoint(2);

                    d.CastResult.AppendBools(cond0, cond1);
                }),

            new(id:                         "SKILL_DTSZ_009",
                name:                       "疯魔",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻".ApplyAttack() +
                    $"\n锻体+5" +
                    $"\n终结：遭受1跳走步".ApplyEnd(castResult) +
                    $"\n大终结：遭受1跳走步\n施加禁止治疗".ApplyDoubleEnd(castResult),
                castGenerator:              async d =>
                {
                    Tuple<bool, bool> endTuple = await d.IsEnd(allowDoubleEnd: true);
                    
                    await d.AttackProcedure(10);
                    await d.GainBuffProcedure("锻体", 5, induced: true);

                    if (endTuple.Item2)
                    {
                        await d.GainBuffProcedure("跳走步", induced: true);
                        await d.GiveBuffProcedure("禁止治疗");
                    }
                    else if (endTuple.Item1)
                    {
                        await d.GainBuffProcedure("跳走步", induced: true);
                    }
                    else
                    {
                    }
                    
                    d.CastResult.AppendEndTuple(endTuple);
                }),

            new(id:                         "SKILL_DB_001",
                name:                       "硬化蛊",
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"对手失去{10 + 4 * dj}气血" +
                    $"\n给予10护甲" +
                    $"\n一次性",
                castGenerator:              async d =>
                {
                    await d.RemoveHealthProcedure(10 + 4 * d.Dj, induced: false);
                    await d.GiveArmorProcedure(10, induced: false);
                }),

            new(id:                         "SKILL_DB_002",
                name:                       "雷火弹",
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(5 + dj)}攻" +
                    $"\n碎防" +
                    $"\n一次性",
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(Fib.ToValue(5 + d.Dj),
                        closures: new [] { Shatter });
                }),

            new(id:                         "SKILL_DB_003",
                name:                       "养气丹",
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"力量+{Fib.ToValue(2 + dj)}" +
                    $"\n一次性",
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Mu, gain: Fib.ToValue(d.Dj + 2), induced: false);
                }),
            
            new(id:                         "SKILL_DB_004",
                name:                       "天雷丸",
                jingJieBound:              JingJie.LianQi2HuaShen,
                skillTypeComposite:        SkillType.Attack | SkillType.Exhaust | SkillType.Deplete,
                descriptionGenerator:           (j, dj, costResult, castResult) =>
                {
                    int damage = dj switch { 0 => 10, 1 => 25, 2 => 45, 3 => 70, _ => 100 };
                    
                    return $"{damage}攻".ApplyAttack() +
                        $"\n升华" +
                        $"\n一次性";
                },
                castGenerator:                      async d =>
                {
                    int value = d.Dj switch { 0 => 10, 1 => 25, 2 => 45, 3 => 70, _ => 100 };
                    await d.AttackProcedure(value, induced: false);
                    await d.Skill.ExhaustProcedure();
                }),

            new(id:                         "SKILL_DB_005",
                name:                       "同心蛊",
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"下{1 + dj}次受治疗时，对敌方造成等量伤害" +
                    $"\n一次性",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("同心蛊", 1 + d.Dj, induced: false);
                }),

            new(id:                         "SKILL_DB_006",
                name:                       "七彩蛊",
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"遭受{20 - 5 * dj}腐朽".ApplyDebuff() +
                    $"\n下次攻击具有暴击，吸血，穿透" +
                    $"\n一次性",
                castGenerator:              async d =>
            {
                await d.GainBuffProcedure("腐朽", 20 - 5 * d.Dj);
                await d.GainBuffProcedure("暴击", 1);
                await d.GainBuffProcedure("吸血", 1);
                await d.GainBuffProcedure("穿透", 1);
            }),
            
            new(id:                         "SKILL_DB_007",
                name:                       "破境丹",
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Swift | SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    j <= JingJie.YuanYing ? 
                        "二动\n升华2次\n一次性" : 
                        "三动\n升华3次\n一次性",
                castGenerator:              async d =>
                {
                    bool isYuanYing = d.J <= JingJie.YuanYing;
                    d.Caster.SetActionPoint(isYuanYing ? 2 : 3);
                    await d.Skill.ExhaustProcedure();
                    await d.GainBuffProcedure("升华", isYuanYing ? 1 : 2);
                }),

            new(id:                         "SKILL_DB_008",
                name:                       "小雷劫",
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    j <= JingJie.YuanYing ? 
                        "开局：双方失去1/4气血\n一次性" : 
                        "开局：双方失去1/3气血\n一次性"
                // startStageCast:             async d =>
                // {
                //     float ratio = d.Skill.GetJingJie() <= JingJie.YuanYing ? 0.25f : 0.333f;
                //     
                //     int selfValue = (int)(d.Caster.MaxHp * ratio);
                //     int oppoValue = (int)(d.Caster.Opponent().MaxHp * ratio);
                //     
                //     await d.Caster.LoseHealthProcedure(selfValue, causedByAttack: false, induced: false);
                //     await d.Caster.Opponent().LoseHealthProcedure(oppoValue, causedByAttack: false, induced: false);
                // }
                ),

            new(id:                         "SKILL_DB_009",
                name:                       "补天丹",
                jingJieBound:               JingJie.HuaShenOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"合成：另外一张牌变成化神",
                overridingMergeRule:        new MergeRule(
                    name:                       "补天丹",
                    errorMessage:               "补天丹不可作用于已经处于最高境界的卡牌",
                    order:                      -1,
                    processMerge:               d =>
                    {
                        RunSkill lhs = d.Lhs;
                        RunSkill rhs = d.Rhs;
                        RunSkill src = d.Src;
                        RunSkill tgt = d.Tgt;

                        bool cond = tgt.GetJingJie() != tgt.GetEntry().HighestJingJie;
                        if (!cond)
                        {
                            d.MergeTarget = new(
                                mergeType:              "补天丹",
                                valid:                  false,
                                errorMessage:           "补天丹不可作用于已经处于最高境界的卡牌",
                                resultEntry:            null,
                                resultJingJie:          null,
                                resultWuXing:           null,
                                pred:                   null);
                            d.State = MergeDetails.MergeState.Cancel;
                            return;
                        }

                        d.MergeTarget = new(
                            mergeType:              "补天丹",
                            valid:                  true,
                            errorMessage:           null,
                            resultEntry:            tgt.GetEntry(),
                            resultJingJie:          tgt.GetEntry().HighestJingJie,
                            resultWuXing:           tgt.GetWuXing(),
                            pred:                   null);
                        d.State = MergeDetails.MergeState.Success;
                    })),

            new(id:                         "SKILL_HZ_001",
                name:                       "缭乱",
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{2 + 4 * dj}攻".ApplyAttack() +
                    $"\n初次：力量+{Fib.ToValue(2 + dj)}".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(2 + 4 * d.Dj);
                    bool cond = d.Skill.IsFirstTime;
                    if (cond)
                        await d.GainBuffProcedure("力量", 2 + d.Dj);
                    
                    d.CastResult.AppendCond(cond);
                }),
            
            new(id:                         "SKILL_HZ_004",
                name:                       "蜕变",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"坚毅+{1 + dj}" +
                    $"\n失去所有护甲",
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Tu, gain: 1 + d.Dj);
                    int value = d.Caster.Armor;
                    if (value > 0)
                        await d.LoseArmorProcedure(value, induced: false);
                }),
            
            new(id:                         "0203",
                name:                       "流霰",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                costGenerator:              CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{9 + 3 * dj}攻".ApplyAttack() +
                    $"\n每造成{9 - dj}点伤害，格挡+1".ApplyDefend(),
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;

                            int gain = d.Value / (9 - initiator.Dj);
                            await d.Src.CycleProcedure(WuXing.Shui, gain: gain);
                        });

                    await d.AttackProcedure(9 + 3 * d.Dj,
                        closures: new [] { closure });
                }),

            new(id:                         "SKILL_HZ_005",
                name:                       "燎原",
                wuXing:                     WuXing.Huo,
                skillTypeComposite:         SkillType.ZiZhi,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ?
                        $"灼烧+{1 + dj}" +
                        $"\n成长:多1"
                        :
                        $"灼烧+3" +
                        $"\n成长:多2"),
                castGenerator:              async d =>
                {
                    int value;
                    if (d.J < JingJie.HuaShen)
                    {
                        value = 1 + d.Dj + d.Cc;
                    }
                    else
                    {
                        value = 3 + d.Cc * 2;
                    }
                    await d.CycleProcedure(WuXing.Huo, gain: value);
                }),

            new(id:                         "0602",
                name:                       "百草集",
                wuXing:                     null,
                jingJieBound:               JingJie.JinDan2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"流转最高五行" +
                    $"\n额外获得{1 + dj}层",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    WuXing? highestWuXing = d.Caster.GetHighestWuXing();
                    if (highestWuXing.HasValue)
                        await d.Caster.CycleProcedure(highestWuXing.Value.Next, gain: 1 + d.Dj);
                }),
            
            new(id:                         "0119",
                name:                       "停云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj}" +
                    $"\n开局：锋锐+{1 + dj}",
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Jin, gain: 1 + d.Dj);
                }
                // startStageCast:             async d =>
                // {
                //     await d.Caster.CycleProcedure(WuXing.Jin, gain: 1 + d.Skill.Dj);
                // }
                ),

            new(id:                         "SKILL_HZ_007",
                name:                       "常仪",
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Swift,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    j <= JingJie.YuanYing ?
                        "流转时：造成伤害\n流转最高五行" :
                        "流转时：造成伤害\n流转最高五行\n二动",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("常仪");
                    
                    WuXing? highestWuXing = d.Caster.GetHighestWuXing();
                    if (highestWuXing.HasValue)
                        await d.Caster.CycleProcedure(highestWuXing.Value.Next);

                    if (d.J <= JingJie.YuanYing)
                        ;
                    else
                        d.Caster.SetActionPoint(2);
                }),

            new(id:                         "0112",
                name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj}" +
                    $"\n每1锋锐，灵气+1".ApplyMana(),
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Jin, gain: 1 + d.Dj);
                    int stack = d.Caster.GetStackOfBuff("锋锐");
                    await d.GainBuffProcedure("灵气", stack, induced: true);
                }),
                
            new(id:                         "SKILL_HZ_009",
                name:                       "羲和",
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Health | SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    "流转最高五行\n使用牌时：流转对应五行",
                castGenerator:              async d =>
                {
                    
                    WuXing? highestWuXing = d.Caster.GetHighestWuXing();
                    if (highestWuXing.HasValue)
                        await d.Caster.CycleProcedure(highestWuXing.Value.Next);

                    await d.GainBuffProcedure("羲和");
                }),
            
            #endregion
            
            #region 待选池子

            // new(id:                         "0405",
            //     name:                       "炎爆",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ChannelFromValue(5),
            //     costDescription:            CostDescription.ChannelFromValue(5),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{9 + 2 * dj}攻x{9 + 2 * dj}".ApplyAttack() +
            //         $"\n禁止行动".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         await d.AttackProcedure(9 + 2 * d.Dj, times: 9 + 2 * d.Dj);
            //         await d.GainBuffProcedure("禁止行动");
            //     },
            //     trivia: "用完之后就会体力耗尽动弹不得"),

            new(id:                         "SKILL_DTSZ_004",
                name:                       "隼击",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{4 + 4 * dj}攻".ApplyAttack() +
                    $"\n满血：翻倍".ApplyCond(castResult) +
                    (j >= JingJie.HuaShen ? $"\n天人形态：施加禁止治疗" : ""),
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            if (!d.Src.IsFullHealth) return;
                            d.Value += 4 + 4 * initiator.Dj;
                            
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(4 + 4 * d.Dj,
                        closures: new [] { closure });

                    if (d.J >= JingJie.HuaShen)
                    {
                        bool cond = d.Caster.GetStackOfBuff("天人形态") > 0;
                        if (cond)
                            await d.GiveBuffProcedure("禁止治疗", induced: true);
                    }
                }),

            new(id:                         "SKILL_DTSZ_006",
                name:                       "塑魂",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Health | SkillType.Mana,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锻体+{Fib.ToValue(4 + dj)}" +
                    $"\n持续：灵气不足时，可消耗3锻体代替1灵气",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("锻体", Fib.ToValue(4 + d.Dj));
                    await d.GainBuffProcedure("塑魂", 3, induced: true);
                }),

            new(id:                         "SKILL_DTSZ_007",
                name:                       "天人五衰",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                {
                    BuffEntry[] debuffs = new BuffEntry[] { "滞气", "缠绕", "软弱", "腐朽", "内伤", "脆弱" };

                    int slotIndex = costResult?.Skill.SlotIndex ?? 0;
                    slotIndex = slotIndex % debuffs.Length;
                    BuffEntry debuff = debuffs[slotIndex];
                    return $"幻影：遭受{3 + 2 * dj}{debuff.GetName().ApplyDebuff()}" +
                           $"\n每1{debuff.GetName()}，锻体+1";
                },
                castGenerator:              async d =>
                {
                    BuffEntry[] debuffs = new BuffEntry[] { "滞气", "缠绕", "软弱", "腐朽", "内伤", "脆弱" };
                    
                    int slotIndex = d.Skill.SlotIndex % debuffs.Length;
                    BuffEntry debuff = debuffs[slotIndex];

                    await d.Caster.GainBuffProcedure(debuff, 3 + 2 * d.Dj);
                    int value = d.Caster.GetStackOfBuff(debuff);
                    await d.Caster.GainBuffProcedure("锻体", value);
                }),

            new(id:                         "0512",
                name:                       "箭疾步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}".ApplyMana() +
                    $"\n每1灵气，" + $"护甲+{1 + dj}".ApplyDefend(),
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气", 2 + d.Dj);
                    await d.GainArmorProcedure(d.Caster.GetStackOfBuff("灵气") * (1 + d.Dj), induced: false);
                }),
            
            new(id:                         "0420",
                name:                       "窑土",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灼烧+{1 + dj}" +
                    $"\n每1灼烧，护甲+2".ApplyDefend(),
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Huo, gain: 1 + d.Dj);
                    int stack = d.Caster.GetStackOfBuff("灼烧");
                    await d.GainArmorProcedure(2 * stack, induced: false);
                }),

            new(id:                         "0419",
                name:                       "净天地",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Exhaust,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n使用所有已升华牌",
                castGenerator:              async d =>
                {
                    if (!d.Recursive)
                        return;
                    
                    foreach (StageSkill s in d.Caster._skills)
                    {
                        if (!s.Exhausted)
                            continue;
                        await d.Caster.CastProcedure(s, recursive: false);
                    }

                    await d.Skill.ExhaustProcedure();
                }),

            new(id:                         "0226",
                name:                       "玄武吐息法",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Health | SkillType.Exhaust,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n气血回复至上限" +
                    $"\n治疗可以穿上限",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int gap = d.Caster.MaxHp - d.Caster.Hp;
                    await d.Caster.HealProcedure(gap);
                    await d.Skill.ExhaustProcedure();
                    await d.GainBuffProcedure("玄武吐息法", induced: true);
                }),
            
            new(id:                         "0111",
                name:                       "凛冽",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锋锐+2" +
                    $"\n锋锐具有吸血".ApplyHeal() +
                    $"\n无法攻击",
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Jin, gain: 2);
                    await d.GainBuffProcedure("凛冽", induced: true);
                    await d.GainBuffProcedure("无法攻击", induced: true);
                }),

            new(id:                         "0526",
                name:                       "出其不意",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n开局：{Fib.ToValue(6 + dj)}攻".ApplyAttack(),
                // startStageCast: async d =>
                // {
                //     await d.Caster.AttackProcedure(Fib.ToValue(6 + d.Skill.Dj), initiator: d.Skill);
                // },
                castGenerator:              async d =>
                {
                    await d.Skill.ExhaustProcedure();
                }),

            new(id:                         "0527",
                name:                       "勤练",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"气血+4".ApplyHeal() +
                    $"\n开局：气血及上限+{8 << (1 + dj)}",
                // startStageCast:             async d =>
                // {
                //     int value = 8 << (1 + d.Skill.Dj);
                //     d.Caster.MaxHp += value;
                //     await d.Caster.HealProcedure(value, induced: false);
                // },
                castGenerator:              async d =>
                {
                    await d.HealProcedure(4, induced: false);
                }),

            new(id:                         "0515",
                name:                       "龟息",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"坚毅+{Fib.ToValue(3 + dj)}",
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Tu, gain: Fib.ToValue(3 + d.Dj));
                }),
            
            new(id:                         "0525",
                name:                       "金刚不坏",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"气血+40".ApplyHeal() +
                    $"\n至多受到40伤害" +
                    $"\n无法获得护甲",
                castGenerator:              async d =>
                {
                    await d.HealProcedure(40, induced: false);
                    await d.GainBuffProcedure("伤害上限", 40, induced: true);
                    await d.GainBuffProcedure("禁止护甲", induced: true);
                }),
            
            // new(id:                         "0121",
            //     name:                       "贪狼",
            //     wuXing:                     WuXing.Jin,
            //     skillTypeComposite:         SkillType.Attack | SkillType.Mana,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"10攻".ApplyAttack() +
            //         $"\n灵气+{1 + dj}".ApplyMana() +
            //         $"\n击伤：移除{1 + dj}灵气".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 DamageDetails d = closureDetails as DamageDetails;
            //                 if (owner != d.SrcSkill) return;
            //                 await d.Src.RemoveBuffProcedure("灵气", 1 + d.SrcSkill.Dj, induced: true);
            //                 d.CastResult.AppendCond(true);
            //             });
            //
            //         d.CastResult.AppendCond(false);
            //         await d.AttackProcedure(10,
            //             closures: new [] { closure });
            //         await d.GainBuffProcedure("灵气", 1 + d.Dj, induced: true);
            //     }),
            //
            //
            // new(id:                         "0120",
            //     name:                       "弹指",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"灵气+4".ApplyMana() +
            //         $"\n消耗1暴击：翻倍".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         bool cond = await d.TryConsumeProcedure("暴击");
            //         int bitShift = cond ? 1 : 0;
            //         await d.GainBuffProcedure("灵气", 4 << bitShift);
            //         d.CastResult.AppendCond(cond);
            //     }),
            
            // new(id:                         "0210",
            //     name:                       "无念无想",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Health,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"治疗{20 + 10 * dj}".ApplyHeal() +
            //         $"\n遭受5缠绕".ApplyDebuff(),
            //     cast:                       async d =>
            //     {
            //         await d.HealProcedure(20 + 10 * d.Dj, induced: false);
            //         await d.GainBuffProcedure("缠绕", 5);
            //     }),
            
            // new(id:                         "0220",
            //     name:                       "奔腾",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Swift,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         (j <= JingJie.YuanYing ? "二动" : "三动"),
            //     cast:                       async d =>
            //     {
            //         d.Caster.SetActionPoint(d.J <= JingJie.YuanYing ? 2 : 3);
            //     }),
            //
            // new(id:                         "0322",
            //     name:                       "缭乱",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"力量+1" +
            //         $"\n6攻".ApplyAttack(),
            //     cast:                       async d =>
            //     {
            //         await d.CycleProcedure(WuXing.Mu, gain: 1);
            //         await d.AttackProcedure(6);
            //     }),
            //
            // new(id:                         "0311",
            //     name:                       "彼岸花",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"力量+{2 + dj}" +
            //         $"\n遭受{5 + 2 * dj}腐朽".ApplyDebuff(),
            //     cast:                       async d =>
            //     {
            //         await d.CycleProcedure(WuXing.Mu, gain: 2 + d.Dj);
            //         await d.GainBuffProcedure("腐朽", 5 + 2 * d.Dj);
            //     }),
            //
            // new(id:                         "0305",
            //     name:                       "水滴石穿",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{7 + 3 * dj}攻".ApplyAttack() +
            //         $"\n终结：穿透".ApplyStyle(castResult, "0") +
            //         $"\n击伤：穿透+1".ApplyStyle(castResult, "1"),
            //     cast:                       async d =>
            //     {
            //
            //         StageClosure wilAttack = new(StageClosureDict.WIL_ATTACK, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 AttackDetails d = closureDetails as AttackDetails;
            //                 if (owner != d.SrcSkill) return;
            //                 bool cond = await d.SrcSkill.IsEnd(useFocus: true);
            //                 d.Penetrate |= cond;
            //                 d.CastResult.AppendBool(0, cond);
            //             });
            //         
            //         StageClosure didDamage = new(StageClosureDict.DID_DAMAGE, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 DamageDetails d = closureDetails as DamageDetails;
            //                 if (owner != d.SrcSkill) return;
            //                 await d.Src.GainBuffProcedure("穿透", induced: true);
            //                 d.CastResult.AppendBool(1, true);
            //             });
            //
            //         d.CastResult.AppendCond(false);
            //         await d.AttackProcedure(7 + 3 * d.Dj,
            //             closures: new [] { wilAttack, didDamage });
            //     }),
            //
            // new(id:                         "0319",
            //     name:                       "刹那芳华",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Defend | SkillType.Exhaust | SkillType.ZiZhi,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"升华 力量/" + "闪避".ApplyDefend() + "+1" +
            //         $"\n成长：多1",
            //     cast:                       async d =>
            //     {
            //         await d.Skill.ExhaustProcedure();
            //         await d.GainBuffProcedure("力量", 1 + d.Cc);
            //         await d.GainBuffProcedure("闪避", 1 + d.Cc);
            //     }),
            //
            // new(id:                         "0410",
            //     name:                       "坐忘",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust | SkillType.Defend,
            //     cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
            //     withinPool:                 false,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"升华" +
            //         $"\n护甲+6".ApplyDefend() +
            //         $"\n每1张已升华牌，多6",
            //     cast:                       async d =>
            //     {
            //         await d.Skill.ExhaustProcedure();
            //         await d.GainArmorProcedure(6 * (1 + d.Caster.ExhaustedCount), induced: false);
            //     }),
            //
            // new(id:                         "0517",
            //     name:                       "一诺五岳",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"1攻 ".ApplyAttack() +
            //         $"残血".ApplyStyle(castResult, "0") +
            //         $"|" +
            //         $"无护甲".ApplyStyle(castResult, "1") +
            //         $"|" +
            //         $"无灵气".ApplyStyle(castResult, "2") +
            //         $"|" +
            //         $"吟唱".ApplyStyle(castResult, "3") +
            //         $"|" +
            //         $"滞气".ApplyStyle(castResult, "4") +
            //         $"|" +
            //         $"缠绕".ApplyStyle(castResult, "5") +
            //         $"|" +
            //         $"软弱".ApplyStyle(castResult, "6") +
            //         $"|" +
            //         $"内伤".ApplyStyle(castResult, "7") +
            //         $"|" +
            //         $"腐朽".ApplyStyle(castResult, "8") +
            //         $"|" +
            //         $"架势".ApplyStyle(castResult, "9") +
            //         $"|" +
            //         $"终结".ApplyStyle(castResult, "10") +
            //         $"|" +
            //         $"初次".ApplyStyle(castResult, "11") +
            //         $"：翻倍",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         bool cond0 = d.Caster.IsLowHealth || await d.Caster.IsFocused();
            //         bool cond1 = (d.Caster.Armor <= 0) || await d.Caster.IsFocused();
            //         bool cond2 = (d.Caster.GetStackOfBuff("灵气") == 0) || await d.Caster.IsFocused();
            //         bool cond3 = (d.Caster.HasChannelRecord) || await d.Caster.IsFocused();
            //         bool cond4 = (d.Caster.HasZhiQiRecord) || await d.Caster.IsFocused();
            //         bool cond5 = (d.Caster.HasChanRaoRecord) || await d.Caster.IsFocused();
            //         bool cond6 = (d.Caster.HasRuanRuoRecord) || await d.Caster.IsFocused();
            //         bool cond7 = (d.Caster.HasNeiShangRecord) || await d.Caster.IsFocused();
            //         bool cond8 = (d.Caster.HasFuXiuRecord) || await d.Caster.IsFocused();
            //         bool cond9 = d.Caster.TriggeredJiaShiRecord || await d.Caster.JiaShiProcedure();
            //         bool cond10 = d.Caster.TriggeredEndRecord || await d.Skill.IsEnd(useFocus: true);
            //         bool cond11 = d.Caster.TriggeredFirstTimeRecord || await d.Skill.IsFirstTime(useFocus: true);
            //
            //         int bitShift = (cond0 ? 1 : 0) +
            //                        (cond1 ? 1 : 0) +
            //                        (cond2 ? 1 : 0) +
            //                        (cond3 ? 1 : 0) +
            //                        (cond4 ? 1 : 0) +
            //                        (cond5 ? 1 : 0) +
            //                        (cond6 ? 1 : 0) +
            //                        (cond7 ? 1 : 0) +
            //                        (cond8 ? 1 : 0) +
            //                        (cond9 ? 1 : 0) +
            //                        (cond10 ? 1 : 0) +
            //                        (cond11 ? 1 : 0);
            //         await d.AttackProcedure(1 << bitShift);
            //         d.CastResult.AppendBools(cond0, cond1, cond2, cond3, cond4, cond5, cond6, cond7, cond8, cond9, cond10, cond11);
            //     }),
            //
            // new(id:                         "0503",
            //     name:                       "点穴",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() +
            //         $"\n对手有灵气：多{Fib.ToValue(4 + dj)}攻".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 AttackDetails d = closureDetails as AttackDetails;
            //                 if (owner != d.SrcSkill) return;
            //                 bool cond = d.Src.Opponent().GetStackOfBuff("灵气") > 0;
            //                 d.Value += cond ? Fib.ToValue(4 + d.SrcSkill.Dj) : 0;
            //                 d.CastResult.AppendCond(cond);
            //             });
            //
            //         await d.AttackProcedure(Fib.ToValue(4 + d.Dj),
            //             closures: new [] { closure });
            //     }),
            //
            // new(id:                         "0504",
            //     name:                       "流沙",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Defend | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"护甲+{3 + 2 * dj}".ApplyDefend() +
            //         $"\n灵气+{1 + dj / 2}".ApplyMana(),
            //     cast:                       async d =>
            //     {
            //         await d.GainArmorProcedure(3 + 2 * d.Dj, induced: false);
            //         await d.GainBuffProcedure("灵气", 1 + d.Dj / 2);
            //     }),
            //
            // new(id:                         "0516",
            //     name:                       "冰肌",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"坚毅+{Fib.ToValue(4 + dj)}" +
            //         $"\n遭受{Fib.ToValue(4 + dj) * 2}腐朽".ApplyDebuff(),
            //     cast:                       async d =>
            //     {
            //         await d.CycleProcedure(WuXing.Tu, gain: Fib.ToValue(4 + d.Dj));
            //         await d.GainBuffProcedure("腐朽", Fib.ToValue(4 + d.Dj) * 2);
            //     }),
            //
            // new(id:                         "0511",
            //     name:                       "玉骨",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     cost:                       CostResult.ChannelFromValue(1),
            //     costDescription:            CostDescription.ChannelFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"坚毅+{2 + dj}" +
            //         $"\n每4坚毅，暴击+1",
            //     cast:                       async d =>
            //     {
            //         await d.CycleProcedure(WuXing.Tu, 2 + d.Dj);
            //         int stack = d.Caster.GetStackOfBuff("坚毅");
            //         int add = stack / 4;
            //         await d.GainBuffProcedure("暴击", add);
            //     }),

            new(id:                         "0114",
                name:                       "素弦",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                costGenerator:              CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n灵气+3".ApplyMana() +
                    $"\n下{1 + dj}次攻击也触发",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            await d.Src.GainBuffProcedure("灵气", 3);
                            await d.Src.GainBuffProcedure("素弦", 1 + initiator.Dj);
                        });

                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0214",
                name:                       "苦寒",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Swift,
                costGenerator:              CostResult.ManaFromValue(8),
                costDescription:            CostDescription.ManaFromValue(8),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n二动+1" +
                    $"\n下{2 + dj}次攻击也触发",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            if (d.Src.GetActionPoint() < 2)
                                d.Src.SetActionPoint(2);
                            else
                                await d.Src.GainBuffProcedure("二动");
                            await d.Src.GainBuffProcedure("苦寒", 2 + initiator.Dj);
                        });

                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0313",
                name:                       "弱昙",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n力量+1" +
                    $"\n下{1 + dj}次攻击也触发",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            await d.Src.GainBuffProcedure("力量");
                            await d.Src.GainBuffProcedure("弱昙", 1 + initiator.Dj);
                        });

                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0414",
                name:                       "狂焰",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n攻击多8攻".ApplyAttack() +
                    $"\n下{1 + dj}次攻击也触发",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (listener != d.Listener) return;
                            d.Value += 8;
                            // await d.Src.GainBuffProcedure("狂焰", 1 + d.SrcSkill.Dj);
                        });
                    
                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                    
                    await d.GainBuffProcedure("狂焰", 1 + d.Dj);
                }),

            new(id:                         "0514",
                name:                       "孤山",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"2攻".ApplyAttack() : "2攻x2".ApplyAttack()) +
                    $"\n不消耗剑阵效果",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            BuffEntry[] buffs = new BuffEntry[] { "素弦", "苦寒", "弱昙", "狂焰" };

                            bool cond = initiator.GetJingJie() < JingJie.HuaShen;
                            int times = cond ? 1 : 2;
                            foreach (BuffEntry b in buffs)
                                if (d.Src.GetStackOfBuff(b) > 0)
                                    await d.Src.GainBuffProcedure(b, times, induced: true);
                        });
                    
                    bool cond = d.J < JingJie.HuaShen;
                    int times = cond ? 1 : 2;
                    await d.AttackProcedure(2, times,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0507",
                name:                       "罗刹",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                withinPool:                 false,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{20 + 20 * dj}攻".ApplyAttack() +
                    $"\n遭受{3 + 2 * dj}腐朽".ApplyDebuff(),
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(20 + 20 * d.Dj);
                    await d.GainBuffProcedure("腐朽", stack: 3 + 2 * d.Dj, induced: true);
                }),
            
            new(id:                         "0509",
                name:                       "正域彼四方",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                withinPool:                 false,
                costGenerator:              CostResult.ManaFromValue(8),
                costDescription:            CostDescription.ManaFromValue(8),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{40 + 20 * dj}攻".ApplyAttack(),
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(40 + 20 * d.Dj);
                }),
            
            new(id:                         "0421",
                name:                       "多段测试",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                withinPool:                 false,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n每耗1灵气，多1次".ApplyAttack(),
                castGenerator:              async d =>
                {
                    int times = d.Caster.GetStackOfBuff("灵气").ClampLower(1);
                    await d.LoseBuffProcedure("灵气", times, induced: true);
                    await d.AttackProcedure(1, times: times);
                }),
            
            // new(id:                         "0101",
            //     name:                       "乘风",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{5 + dj}攻\n" +
            //         $"若有锋锐：{3 + dj}攻".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         bool cond = caster.GetStackOfBuff("锋锐") > 0 || await caster.IsFocused();
            //         int add = cond ? 3 + skill.Dj : 0;
            //         await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
            //
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0104",
            //     name:                       "掠影",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"{5 + 2 * dj}攻".ApplyOdd(castResult) +
            //         $"/" +
            //         $"护甲+{5 + 2 * dj}".ApplyEven(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         int value = 5 + 2 * skill.Dj;
            //         bool odd = skill.IsOdd || await caster.IsFocused();
            //         if (odd)
            //             await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
            //         bool even = skill.IsEven || await caster.IsFocused();
            //         if (even)
            //             await caster.GainArmorProcedure(value, induced: false);
            //         return Style.CastResultFromOddEven(odd, even);
            //     }),
            //
            // new(id:                         "0107",
            //     name:                       "飞絮",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         null,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"施加{8 + 2 * dj}破甲".ApplyOdd(castResult) +
            //         $"/" +
            //         $"锋锐+{1 + dj}".ApplyEven(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         bool odd = skill.IsOdd || await caster.IsFocused();
            //         if (odd)
            //             await caster.RemoveArmorProcedure(8 + 2 * skill.Dj);
            //         bool even = skill.IsEven || await caster.IsFocused(); 
            //         if (even)
            //             await caster.GainBuffProcedure("锋锐", 1 + skill.Dj);
            //         return Style.CastResultFromOddEven(odd, even);
            //     }),
            //
            // new(id:                         "0120",
            //     name:                       "千里神行符",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust | SkillType.Swift,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"升华".ApplyOdd(castResult) +
            //         $"/" +
            //         $"二动".ApplyEven(castResult) +
            //         $"\n灵气+4",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         bool odd = skill.IsOdd || await caster.IsFocused();
            //         if (odd)
            //             await skill.ExhaustProcedure();
            //         bool even = skill.IsEven || await caster.IsFocused();
            //         if (even)
            //             caster.SetActionPoint(2);
            //
            //         await caster.GainBuffProcedure("灵气", 4);
            //         return Style.CastResultFromOddEven(odd, even);
            //     }),
            //
            // new(id:                         "0204",
            //     name:                       "归意",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{10 + 2 * dj}攻\n" +
            //         $"终结：吸血".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         bool cond = await skill.IsEnd(useFocus: true);
            //         await caster.AttackProcedure(10 + 2 * skill.Dj, lifeSteal: cond,
            //             wuXing: skill.Entry.WuXing);
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0207",
            //     name:                       "勤能补拙",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"护甲+{10 + 4 * dj}\n" +
            //         $"初次：遭受1跳行动".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(10 + 4 * skill.Dj, induced: false);
            //         bool cond = !await skill.IsFirstTime();
            //         if (!cond)
            //             await caster.GainBuffProcedure("跳行动");
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0314",
            //     name:                       "旧飞龙在天",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"升华" +
            //         $"\n每轮：闪避补至1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("轮闪避");
            //         return null;
            //     }),
            //
            // new(id:                         "0307",
            //     name:                       "回马枪",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"下次受攻击时：{12 + 4 * dj}攻",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("回马枪", 12 + 4 * skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "0401",
            //     name:                       "化焰",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{4 + 2 * dj}攻\n" +
            //         $"灼烧+{1 + dj / 2}",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
            //         await caster.GainBuffProcedure("灼烧", 1 + skill.Dj / 2);
            //         return null;
            //     }),
            //
            // new(id:                         "0405",
            //     name:                       "聚火",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"升华" +
            //         $"\n灼烧+{2 + dj}",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "0503",
            //     name:                       "地龙",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{7 + 2 * dj}攻\n" +
            //         $"击伤：护甲+{7 + 2 * dj}",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         int value = 7 + 2 * skill.Dj;
            //         bool cond = false;
            //         await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
            //             didDamage: async d =>
            //             {
            //                 cond = true;
            //                 await caster.GainArmorProcedure(value, induced: true);
            //             });
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0505",
            //     name:                       "点星",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{8 + 2 * dj}攻\n" +
            //         $"相邻牌都非攻击：翻倍".ApplyStyle(castResult, "0") +
            //         $"\n" +
            //         $"消耗1灵气：翻倍".ApplyStyle(castResult, "1"),
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         bool cond0 = skill.NoAttackAdjacents || await caster.IsFocused();
            //         bool cond1 = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
            //         int bitShift = 0;
            //         bitShift += cond0 ? 1 : 0;
            //         bitShift += cond1 ? 1 : 0;
            //         await caster.AttackProcedure((8 + 2 * skill.Dj) << bitShift);
            //         return Style.CastResultFromBools(cond0, cond1);
            //     }),

            #endregion

            #region 特殊牌
            
            new(id:                         "0000",
                name:                       "卡池已空",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    "卡池已空",
                withinPool:                 false),

            new(id:                         "0001",
                name:                       "聚气术",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    "灵气+1".ApplyMana(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气");
                }),

            new(id:                         "0002",
                name:                       "灵气匮乏",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    "灵气+1".ApplyMana(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气");
                }),

            new(id:                         "0003",
                name:                       "幻化",
                wuXing:                     null,
                jingJieBound:               JingJie.HuaShenOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    "模仿对手对位的牌",
                withinPool:                 false,
                overridingMergeRule:        NoMerge),

            new(id:                         "0004",
                name:                       "作弊",
                wuXing:                     null,
                jingJieBound:               JingJie.HuaShenOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    "对手气血变成0",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = d.Caster.Opponent().Hp;
                    await d.Caster.Opponent().LoseHealthProcedure(value, causedByAttack: false);
                }),

            new(id:                         "0005",
                name:                       "发呆",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    "就真的只是发呆",
                withinPool:                 false),

            #endregion

            #region 事件牌

            new(id:                         "0603",
                name:                       "遗憾",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"对手失去{3 + dj}灵气",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.RemoveBuffProcedure("灵气", 3 + d.Dj, false);
                }),

            new(id:                         "0604",
                name:                       "爱恋",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                }),

            new(id:                         "0606",
                name:                       "春雨",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                costGenerator:              CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"双方气血+{20 + 5 * dj}".ApplyHeal(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.HealProcedure(20 + 5 * d.Dj, induced: false);
                    await d.HealOppoProcedure(20 + 5 * d.Dj, induced: false);
                }),

            new(id:                         "0607",
                name:                       "枯木",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"双方遭受{5 + dj}腐朽".ApplyDebuff(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("腐朽", 5 + d.Dj);
                    await d.GiveBuffProcedure("腐朽", 5 + d.Dj);
                }),

            new(id:                         "0600",
                name:                       "须臾",
                wuXing:                     null,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Swift | SkillType.Health,
                costGenerator:              CostResult.HealthFromDj(dj => 8 - 2 * dj),
                costDescription:            CostDescription.HealthFromDj(dj => 8 - 2 * dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    j <= JingJie.ZhuJi ? "二动" : "三动",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    bool cond = d.J <= JingJie.ZhuJi;
                    d.Caster.SetActionPoint(cond ? 2 : 3);
                }),

            new(id:                         "0601",
                name:                       "永远",
                wuXing:                     null,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"治疗{18 + dj * 6}".ApplyHeal(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.HealProcedure(18 + d.Dj * 6, induced: false);
                }),

            new(id:                         "0610",
                name:                       "童趣",
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Swift | SkillType.Health,
                costGenerator:              CostResult.HealthFromDj(dj => 8 - 2 * dj),
                costDescription:            CostDescription.HealthFromDj(dj => 8 - 2 * dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    j <= JingJie.ZhuJi ? "二动" : "三动",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    bool cond = d.J <= JingJie.ZhuJi;
                    d.Caster.SetActionPoint(cond ? 2 : 3);
                }),

            new(id:                         "0611",
                name:                       "一心",
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Health,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"治疗{24 + dj * 6}".ApplyHeal(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.HealProcedure(24 + d.Dj * 6, induced: false);
                }),

            new(id:                         "0605",
                name:                       "射落金乌",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"5攻x4".ApplyAttack(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(5, times: 4);
                }),

            new(id:                         "0609",
                name:                       "毒性",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加3内伤".ApplyDebuff(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GiveBuffProcedure("内伤", 3);
                }),
            
            new(id:                         "0612",
                name:                       "观棋烂柯",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加1跳行动",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.Caster.GiveBuffProcedure("跳行动");
                }),

            #endregion
            
            #region 教程
            
            new(id:                         "0701",
                name:                       "冲撞",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻".ApplyAttack(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(3 + d.Dj);
                }),
            
            new(id:                         "0702",
                name:                       "劈砍",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{4 + 2 * dj}攻".ApplyAttack(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(4 + 2 * d.Dj);
                }),
            
            new(id:                         "0703",
                name:                       "冰弹",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                skillTypeComposite:         SkillType.Attack,
                costGenerator:              CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{8 + dj}攻".ApplyAttack(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(8 + d.Dj);
                }),
            
            new(id:                         "0704",
                name:                       "不演了",
                wuXing:                     null,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{100}攻".ApplyAttack(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(100);
                }),
            
            // 脱变，凝水，流霰，养气丹，燎原
            // 金刃，寻猎，激流，空幻
            // 吐纳，恋花，明神，回春，小松，潜龙在渊
            // 剑王行，战意
            
            // new(id:                         "0101",
            //     name:                       "金刃",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() +
            //         $"\n施加{Fib.ToValue(3 + dj)}破甲",
            //     cast:                       async d =>
            //     {
            //         await d.AttackProcedure(Fib.ToValue(4 + d.Dj));
            //         await d.RemoveArmorProcedure(Fib.ToValue(3 + d.Dj), false);
            //     }),
            //
            // new(id:                         "0102",
            //     name:                       "寻猎",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"2攻".ApplyAttack() +
            //         $"\n击伤：施加{5 + 5 * dj}破甲".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 DamageDetails d = closureDetails as DamageDetails;
            //                 if (listener != d.Initiator) return;
            //                 StageSkill initiator = d.Initiator as StageSkill;
            //                 
            //                 await d.Src.RemoveArmorProcedure(5 + 5 * initiator.Dj, true);
            //                 d.CastResult.AppendCond(true);
            //             });
            //
            //         d.CastResult.AppendCond(false);
            //         await d.AttackProcedure(2,
            //             closures: new [] { closure });
            //     }),
            //
            // new(id:                         "0206",
            //     name:                       "空幻",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack | SkillType.Health,
            //     cost:                       async (env, entity, skill, recursive) => new ManaCostResult(3 + skill.Dj - entity.CountSuch(s => s.Entry.WuXing == WuXing.Shui)),
            //     costDescription:            CostDescription.ManaFromDj(dj => 3 + dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{6 + 6 * dj}攻".ApplyAttack() +
            //         $"\n每携带1水：消耗-1",
            //     cast:                       async d =>
            //     {
            //         await d.AttackProcedure(6 + 6 * d.Dj);
            //     }),
            //
            // new(id:                         "0222",
            //     name:                       "激流",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack | SkillType.Health,
            //     cost:                       CostResult.ManaFromDj(dj => 3 + dj),
            //     costDescription:            CostDescription.ManaFromDj(dj => 3 + dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{8 + 4 * dj}攻".ApplyAttack() +
            //         $"\n灵气+{3 + 1 * dj}".ApplyMana(),
            //     cast:                       async d =>
            //     {
            //         await d.AttackProcedure(8 + 4 * d.Dj);
            //         await d.GainBuffProcedure("灵气", 3 + d.Dj, induced: true);
            //     }),

            // new(id:                         "0413",
            //     name:                       "剑王行",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"1攻x{2 + dj}".ApplyAttack() +
            //         $"\n击伤：灵气+1".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 DamageDetails d = closureDetails as DamageDetails;
            //                 if (listener != d.Initiator) return;
            //                 await d.Src.GainBuffProcedure("灵气", induced: true);
            //                 d.CastResult.AppendCond(true);
            //             });
            //
            //         d.CastResult.AppendCond(false);
            //         await d.AttackProcedure(1, times: 2 + d.Dj,
            //             closures: new [] { closure });
            //     }),
            //
            // new(id:                         "0423",
            //     name:                       "战意",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Defend,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"剑意+{2 + dj}" +
            //         $"\n护甲+{2 + dj}".ApplyDefend(),
            //     cast:                       async d =>
            //     {
            //         await d.GainBuffProcedure("剑意", 2 + d.Dj);
            //         await d.GainArmorProcedure(2 + d.Dj, induced: true);
            //     }),
            //
            new(id:                         "0705",
                name:                       "蜕变",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"坚毅+{1 + dj}" +
                    $"\n失去所有护甲",
                withinPool:                 false,
                overridingMergeRule:        DreamCard,
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Tu, gain: 1 + d.Dj);
                    int value = d.Caster.Armor;
                    if (value > 0)
                        await d.LoseArmorProcedure(value, induced: false);
                }),
            
            new(id:                         "0706",
                name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj}" +
                    $"\n每1锋锐，灵气+1".ApplyMana(),
                withinPool:                 false,
                overridingMergeRule:        DreamCard,
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Jin, gain: 1 + d.Dj);
                    int stack = d.Caster.GetStackOfBuff("锋锐");
                    await d.GainBuffProcedure("灵气", stack, induced: true);
                }),
            
            new(id:                         "0707",
                name:                       "流霰",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                costGenerator:              CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{9 + 3 * dj}攻".ApplyAttack() +
                    $"\n每造成{9 - dj}点伤害，格挡+1".ApplyDefend(),
                withinPool:                 false,
                overridingMergeRule:        DreamCard,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
            
                            int gain = d.Value / (9 - initiator.Dj);
                            await d.Src.CycleProcedure(WuXing.Shui, gain: gain);
                        });
            
                    await d.AttackProcedure(9 + 3 * d.Dj,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0708",
                name:                       "养气丹",
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Deplete,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"力量+{Fib.ToValue(2 + dj)}" +
                    $"\n一次性",
                withinPool:                 false,
                overridingMergeRule:        DreamCard,
                castGenerator:              async d =>
                {
                    await d.CycleProcedure(WuXing.Mu, gain: Fib.ToValue(d.Dj + 2), induced: false);
                }),
            
            new(id:                         "0709",
                name:                       "燎原",
                wuXing:                     WuXing.Huo,
                skillTypeComposite:         SkillType.ZiZhi,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ?
                        $"灼烧+{1 + dj}" +
                        $"\n成长:多1"
                        :
                        $"灼烧+3" +
                        $"\n成长:多2"),
                withinPool:                 false,
                overridingMergeRule:        DreamCard,
                castGenerator:              async d =>
                {
                    int value;
                    if (d.J < JingJie.HuaShen)
                    {
                        value = 1 + d.Dj + d.Cc;
                    }
                    else
                    {
                        value = 3 + d.Cc * 2;
                    }
                    await d.CycleProcedure(WuXing.Huo, gain: value);
                }),

            #endregion

            #region 机关牌

            // // 筑基
            //
            // new(id:                         "0700", // 香
            //     name:                       "醒神香", // 香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灵气+4",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灵气", 4);
            //         return null;
            //     }),
            //
            // new(id:                         "0701", // 刃
            //     name:                       "飞镖", // 刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n12攻",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(12);
            //         return null;
            //     }),
            //
            // new(id:                         "0702", // 匣
            //     name:                       "铁匣", // 匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+12",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(12, induced: false);
            //         return null;
            //     }),
            //
            // new(id:                         "0703", // 轮
            //     name:                       "滑索", // 轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Swift | SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n三动 升华",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         caster.SetActionPoint(3);
            //         await skill.ExhaustProcedure();
            //         return null;
            //     }),
            //
            // // 元婴
            //
            // new(id:                         "0704", // 香香
            //     name:                       "还魂香", // 香香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灵气+8",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灵气", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "0705", // 香刃
            //     name:                       "净魂刀", // 香刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n10攻\n击伤：灵气+1，对手灵气-1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(10,
            //             didDamage: async d =>
            //             {
            //                 await caster.GainBuffProcedure("灵气");
            //                 await caster.RemoveBuffProcedure("灵气");
            //             });
            //         return null;
            //     }),
            //
            // new(id:                         "0706", // 香匣
            //     name:                       "防护罩", // 香匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+8\n每有1灵气，护甲+4",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         int add = caster.GetStackOfBuff("灵气");
            //         await caster.GainArmorProcedure(8 + add, induced: false);
            //         return null;
            //     }),
            //
            // new(id:                         "0707", // 香轮
            //     name:                       "能量饮料", // 香轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下1次灵气减少时，加回",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灵气回收");
            //         return null;
            //     }),
            //
            // new(id:                         "0708", // 刃刃
            //     name:                       "炎铳", // 刃刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n25攻",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(25);
            //         return null;
            //     }),
            //
            // new(id:                         "0709", // 刃匣
            //     name:                       "机关人偶", // 刃匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+12\n10攻",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(12, induced: false);
            //         await caster.AttackProcedure(10);
            //         return null;
            //     }),
            //
            // new(id:                         "0710", // 刃轮
            //     name:                       "铁陀螺", // 刃轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n2攻x6",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(2, times: 6);
            //         return null;
            //     }),
            //
            // new(id:                         "0711", // 匣匣
            //     name:                       "防壁", // 匣匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+20\n坚毅+2",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(20, induced: false);
            //         await caster.GainBuffProcedure("坚毅", 2);
            //         return null;
            //     }),
            //
            // new(id:                         "0712", // 匣轮
            //     name:                       "不倒翁", // 匣轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下2次护甲减少时，加回",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("护甲回收", 2);
            //         return null;
            //     }),
            //
            // new(id:                         "0713", // 轮轮
            //     name:                       "助推器", // 轮轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Swift,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n二动 二重",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         caster.SetActionPoint(2);
            //         await caster.GainBuffProcedure("二重");
            //         return null;
            //     }),
            //
            // // 返虚
            //
            // new(id:                         "0714", // 香香香
            //     name:                       "反应堆", // 香香香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n遭受1不堪一击，永久二重+1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("不堪一击");
            //         await caster.GainBuffProcedure("永久二重");
            //         return null;
            //     }),
            //
            // new(id:                         "0715", // 香香刃
            //     name:                       "烟花", // 香香刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n消耗所有灵气，每1，力量+1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         int stack = caster.GetStackOfBuff("灵气");
            //         await caster.TryConsumeProcedure("灵气", stack);
            //         await caster.GainBuffProcedure("力量", stack);
            //         return null;
            //     }),
            //
            // new(id:                         "0716", // 香香匣
            //     name:                       "长明灯", // 香香匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n获得灵气时：每1，气血+3",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("长明灯", 3);
            //         return null;
            //     }),
            //
            // new(id:                         "0717", // 香香轮
            //     name:                       "大往生香", // 香香轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust | SkillType.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n永久免费+1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("永久免费");
            //         return null;
            //     }),
            //
            // new(id:                         "0718", // 缺少匣
            //     name:                       "地府通讯器", // 缺少匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n失去一半气血，每8，灵气+1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         int gain = caster.Hp / 16;
            //         await caster.LoseHealthProcedure(gain * 8);
            //         await caster.GainBuffProcedure("灵气", gain);
            //         return null;
            //     }),
            //
            // new(id:                         "0719", // 刃刃刃
            //     name:                       "无人机阵列", // 刃刃刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n没有效果",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         return null;
            //     }),
            //
            // new(id:                         "0720", // 刃刃香
            //     name:                       "弩炮", // 刃刃香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n50攻 吸血",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(50, lifeSteal: true);
            //         return null;
            //     }),
            //
            // new(id:                         "0721", // 刃刃匣
            //     name:                       "尖刺陷阱", // 刃刃匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下次受到攻击时，对对方施加等量破甲",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("尖刺陷阱");
            //         return null;
            //     }),
            //
            // new(id:                         "0722", // 刃刃轮
            //     name:                       "暴雨梨花针", // 刃刃轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n1攻x10",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(1, times: 10);
            //         return null;
            //     }),
            //
            // new(id:                         "0723", // 缺少轮
            //     name:                       "炼丹炉", // 缺少轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n每回合力量+1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("回合力量");
            //         return null;
            //     }),
            //
            // new(id:                         "0724", // 匣匣匣
            //     name:                       "浮空艇", // 匣匣匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n回合被跳过时，该回合无法受到伤害\n遭受12跳行动",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("浮空艇");
            //         await caster.GainBuffProcedure("跳行动", 12);
            //         return null;
            //     }),
            //
            // new(id:                         "0725", // 匣匣香
            //     name:                       "动量中和器", // 匣匣香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n格挡+10",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("格挡", 10);
            //         return null;
            //     }),
            //
            // new(id:                         "0726", // 匣匣刃
            //     name:                       "机关伞", // 匣匣刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灼烧+8",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灼烧", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "0727", // 匣匣轮
            //     name:                       "一轮马", // 匣匣轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n闪避+6",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("闪避", 6);
            //         return null;
            //     }),
            //
            // new(id:                         "0728", // 缺少香
            //     name:                       "外骨骼", // 缺少香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n攻击时，护甲+3",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("外骨骼", 3);
            //         return null;
            //     }),
            //
            // new(id:                         "0729", // 轮轮轮
            //     name:                       "永动机", // 轮轮轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust | SkillType.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n力量+8 灵气+8\n8回合后死亡",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("力量", 8);
            //         await caster.GainBuffProcedure("灵气", 8);
            //         await caster.GainBuffProcedure("永动机", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "0730", // 轮轮香
            //     name:                       "火箭靴", // 轮轮香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n使用灵气牌时，获得二动",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("火箭靴");
            //         return null;
            //     }),
            //
            // new(id:                         "0731", // 轮轮刃
            //     name:                       "定龙桩", // 轮轮刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n对方二动时，如果没有暴击，获得1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("定龙桩");
            //         return null;
            //     }),
            //
            // new(id:                         "0732", // 轮轮匣
            //     name:                       "飞行器", // 轮轮匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n成功闪避时，如果对方没有跳行动，施加1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("飞行器");
            //         return null;
            //     }),

            #endregion
            
            #region 怪物专属
            
            // 3 5 8 13 21
            new(id:                         "1001",
                name:                       "攻击",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻".ApplyAttack(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(Fib.ToValue(4 + d.Dj));
                }),
            
            // 3 5 8 13 21
            new(id:                         "1002",
                name:                       "防御",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(4 + dj)}".ApplyDefend(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), false);
                }),
            
            // 3 5 8 13 21
            new(id:                         "1003",
                name:                       "聚灵",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+{1 + (dj / 2)}".ApplyMana(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气", 1 + (d.Dj / 2), induced: false);
                }),
            
            // 3 5 8 13 21
            new(id:                         "1004",
                name:                       "调息",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"气血+{Fib.ToValue(4 + dj)}".ApplyHeal(),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.HealProcedure(Fib.ToValue(4 + d.Dj), false);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1101",
                name:                       "啃咬",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加{6 << dj}破甲",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.RemoveArmorProcedure(6 << d.Dj, false);
                }),

            // 6 12 26 52 102
            new(id:                         "1102",
                name:                       "切割",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{2 << dj}攻".ApplyAttack() +
                    $"\n获得{4 << dj}护甲",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(2 << d.Dj);
                    await d.GainArmorProcedure(4 << d.Dj, true);
                }),

            // 8 24 52 105 204
            new(id:                         "1103",
                name:                       "腐蚀",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+5" +
                    $"\n每有1护甲，施加1破甲",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(5, induced: false);
                    int value = d.Caster.Armor.ClampLower(0);
                    await d.RemoveArmorProcedure(value, induced: true);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1104",
                name:                       "剑芒",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锋锐+{1 << (Mathf.Max(dj - 1, 0))}",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("锋锐", 1 << (Mathf.Max(d.Dj - 1, 0)));
                }),

            // 6 12 26 52 102
            new(id:                         "1105",
                name:                       "祥瑞",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"开局：二动 双发",
                withinPool:                 false,
                // startStageCast:             async d =>
                // {
                //     await d.Caster.GainBuffProcedure("二动");
                //     await d.Caster.GainBuffProcedure("一念无量劫");
                // },
                castGenerator:              async d =>
                {
                }),

            // 8 24 52 105 204
            new(id:                         "1106",
                name:                       "万千光辉",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"锋锐+1" +
                    $"\n每1锋锐，多{2 + dj}攻",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("锋锐");
                    int value = d.Caster.GetStackOfBuff("锋锐") * (2 + d.Dj);
                    await d.AttackProcedure(value);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1107",
                name:                       "恶意",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"之后{1 + dj}次施加破甲时将会造成气血流失",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("恶意", 1 + d.Dj);
                }),

            // 6 12 26 52 102
            new(id:                         "1108",
                name:                       "岁月",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻".ApplyAttack() +
                    $"\n击伤：施加{3 + 2 * dj}腐朽".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            await d.Src.GiveBuffProcedure("腐朽", 3 + 2 * initiator.Dj, induced: true);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(10 + 10 * d.Dj,
                        closures: new [] { closure });
                }),

            // 8 24 52 105 204
            new(id:                         "1109",
                name:                       "恶灵招徕",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"若敌方有腐朽，施加10破甲".ApplyStyle(castResult, "0") +
                    $"\n否则，将破甲转为腐朽".ApplyStyle(castResult, "1"),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    bool cond = d.Caster.Opponent().GetStackOfBuff("腐朽") > 0;
                    
                    if (cond)
                    {
                        await d.RemoveArmorProcedure(10, induced: false);
                    }
                    else
                    {
                        int value = -d.Caster.Opponent().Armor;
                        await d.GiveBuffProcedure("腐朽", value, induced: false);
                        await d.GiveArmorProcedure(value, induced: true);
                    }
                    
                    d.CastResult.AppendBools(cond, !cond);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1201",
                name:                       "浪击",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{4 + 4 * dj}攻" +
                    $"\n击伤：灵气+{2 + dj}".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            await d.Src.GainBuffProcedure("灵气", 2 + initiator.Dj);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(4 + 4 * d.Dj,
                        closures: new [] { closure });
                }),

            // 6 12 26 52 102
            new(id:                         "1202",
                name:                       "惊涛",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"4攻" +
                    $"\n爆能{10 + 2 * dj}：多{4 + 2 * dj}攻，多{1 + dj}次，吸血".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    bool cond = await d.TryConsumeProcedure("灵气", 10 + 2 * d.Dj);
                    d.CastResult.AppendCond(cond);
                    
                    if (cond)
                    {
                        await d.AttackProcedure(4 + 4 + 2 * d.Dj, 1 + 1 + d.Dj,
                            closures: new[] { LifeSteal });
                    }
                    else
                    {
                        await d.AttackProcedure(4);
                    }
                }),

            // 8 24 52 105 204
            new(id:                         "1203",
                name:                       "放血",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加4内伤",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GiveBuffProcedure("内伤", 4);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1204",
                name:                       "高速",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"二动" +
                    $"\n每1格挡，造成{1 + dj}伤害",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = d.Caster.GetStackOfBuff("格挡") * d.Dj;
                    await d.AttackProcedure(value);
                    d.Caster.SetActionPoint(2);
                }),

            // 6 12 26 52 102
            new(id:                         "1205",
                name:                       "逍遥游",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"格挡减半" +
                    $"\n格挡+{2 + 2 * dj}",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = d.Caster.GetStackOfBuff("格挡") / 2;
                    await d.LoseBuffProcedure("格挡", value);
                    await d.GainBuffProcedure("格挡", 2 + 2 * d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1206",
                name:                       "爱睡",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                costGenerator:              CostResult.ChannelFromDj(dj => 4 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"气血回复至上限",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = d.Caster.MaxHp - d.Caster.Hp;
                    await d.HealProcedure(value, induced: false);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1207",
                name:                       "幻雾",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                costGenerator:              async (env, entity, skill, recursive) => new ChannelCostResult(4 * (1 - skill.TotalStageCastedCount.Clamp(0, 1))),
                costDescription:            CostDescription.ChannelFromValue(4),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"30攻 吸血" +
                    $"\n非初次：无需吟唱".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    bool cond = d.Skill.TotalStageCastedCount != 0;
                    d.CastResult.AppendCond(cond);
                    await d.AttackProcedure(30,
                        closures: new[] { LifeSteal });
                }),

            // 6 12 26 52 102
            new(id:                         "1208",
                name:                       "须臾",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"十二动 升华",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    d.Caster.SetActionPoint(12);
                }),

            // 8 24 52 105 204
            new(id:                         "1209",
                name:                       "月华清辉",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"敌方失去所有灵气",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = d.Caster.Opponent().GetStackOfBuff("灵气");
                    await d.RemoveBuffProcedure("灵气", value, induced: false);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1301",
                name:                       "木刺",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                costGenerator:              CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻 穿透",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(10 + 10 * d.Dj,
                        closures: new [] { Penetrate });
                }),

            // 6 12 26 52 102
            new(id:                         "1302",
                name:                       "滑水",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"闪避+{1 + dj}",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("闪避", 1 + d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1303",
                name:                       "驱藤",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"双方坚毅+{2 + dj}",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("坚毅", 2 + d.Dj, induced: false);
                    await d.GiveBuffProcedure("坚毅", 2 + d.Dj, induced: true);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1304",
                name:                       "花海",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n每回合力量+1",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    await d.GainBuffProcedure("花海");
                }),

            // 6 12 26 52 102
            new(id:                         "1305",
                name:                       "啄击",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"1攻" +
                    $"\n成长：多1次",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(1, times: 1 + d.Skill.TotalStageCastedCount);
                }),

            // 8 24 52 105 204
            new(id:                         "1306",
                name:                       "娑婆双树",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"将左边牌的成长次数给右边牌",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageSkill leftSkill = d.Skill.Prev(false);
                    StageSkill rightSkill = d.Skill.Next(false);
                    if (leftSkill == null || rightSkill == null)
                        return;

                    rightSkill.SetRealStageCastedCount(leftSkill.TotalStageCastedCount + rightSkill.TotalStageCastedCount);
                    leftSkill.SetRealStageCastedCount(0);
                    // animation
                }),
            
            // 6 12 26 52 102
            new(id:                         "1307",
                name:                       "灵虚步",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                costGenerator:              CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"闪避+3" +
                    $"\n成功闪避时：双发+1",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("闪避", 3);
                    await d.GainBuffProcedure("灵虚步", induced: true);
                }),

            // 6 12 26 52 102
            new(id:                         "1308",
                name:                       "灵犀剑",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+3" +
                    $"\n1攻 每1灵气，多{1 + dj}",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灵气", 3);
                    
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (listener != d.Listener) return;
                            StageSkill initiator = d.Listener as StageSkill;
                            int mana = d.Src.GetStackOfBuff("灵气");
                            d.Value += mana * (1 + initiator.Dj);
                        });
                    
                    await d.AttackProcedure(1,
                        closures: new [] { closure });
                }),

            // 8 24 52 105 204
            new(id:                         "1309",
                name:                       "他心通",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"下一次对方获得增益时：自己也获得",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("他心通");
                }),
            
            // 6 12 26 52 102
            new(id:                         "1401",
                name:                       "吞炎",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"成为残血" +
                    $"\n护甲+{10 + 20 * dj}",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(10 + 20 * d.Dj, induced: false);
                    await d.BecomeLowHealth(induced: true);
                }),

            // 6 12 26 52 102
            new(id:                         "1402",
                name:                       "焚天",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻" +
                    $"\n残血：多{10 + 10 * dj}攻".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = (10 + 10 * d.Dj) + (d.Caster.IsLowHealth ? (10 + 10 * d.Dj) : 0);
                    await d.AttackProcedure(value);
                }),

            // 8 24 52 105 204
            new(id:                         "1403",
                name:                       "山火",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻 每携带1张火，多1次",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = d.Caster.TraversalSkills().Count(s => s.Entry.WuXing == WuXing.Huo);
                    await d.AttackProcedure(3 + d.Dj, times: 1 + value);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1404",
                name:                       "天劫火",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                costGenerator:              CostResult.HealthFromDj(dj => 4 + 4 * dj),
                costDescription:            CostDescription.HealthFromDj(dj => 4 + 4 * dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灼烧+{1 + dj}",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灼烧", 1 + d.Dj);
                }),

            // 6 12 26 52 102
            new(id:                         "1405",
                name:                       "火墙",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"若在下次使用前，没有遭受{1 + dj}次伤害".ApplyCond(castResult) +
                    $"\n50攻".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int stack = d.Caster.GetStackOfBuff("火墙");
                    bool cond = stack > 0;
                    d.CastResult.AppendCond(cond);

                    if (cond)
                    {
                        await d.Caster.RemoveBuffProcedure("火墙", stack);
                        await d.AttackProcedure(50);
                    }

                    await d.Caster.GainBuffProcedure("火墙", 1 + d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1406",
                name:                       "献祭",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华所有卡牌",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.Caster._skills.Do(async s => await s.ExhaustProcedure());
                }),

            // 6 12 26 52 102
            new(id:                         "1407",
                name:                       "须弥",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{10 + 10 * dj}" +
                    $"\n二动 升华",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(10 + 10 * d.Dj, induced: false);
                    d.Caster.SetActionPoint(2);
                    await d.Skill.ExhaustProcedure();
                }),
            
            // 6 12 26 52 102
            new(id:                         "1408",
                name:                       "仙人抚顶",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                costGenerator:              CostResult.ChannelFromDj(dj => 4 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"使用3次后：将对方气血变为0".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("仙人抚顶");
                    bool cond = d.Caster.GetStackOfBuff("仙人抚顶") >= 3;
                    d.CastResult.AppendCond(cond);
                    if (cond)
                    {
                        d.Caster.Opponent().Hp = 0;
                    }
                }),

            // 8 24 52 105 204
            new(id:                         "1409",
                name:                       "消愁",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"下一张牌取消升华",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageSkill s = d.Skill.Next(loop: false);
                    s.Exhausted = false;
                }),
            
            // 6 12 26 52 102
            new(id:                         "1501",
                name:                       "滚石",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n有护甲：多5攻".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    bool cond = d.Caster.Armor > 0;
                    d.CastResult.AppendCond(cond);

                    int value = 10 + (cond ? 1 : 0) * 5;
                    await d.AttackProcedure(value);
                }),

            // 6 12 26 52 102
            new(id:                         "1502",
                name:                       "瓮城",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                costGenerator:              CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+10" +
                    $"\n每有{6 - dj}气血，多1",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    int value = d.Caster.Hp / (6 - d.Dj) + 10;
                    await d.GainArmorProcedure(value, induced: false);
                }),

            // 8 24 52 105 204
            new(id:                         "1503",
                name:                       "风魔",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n遭受1跳走步",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(10);
                    await d.GainBuffProcedure("跳走步");
                }),
            
            // 6 12 26 52 102
            new(id:                         "1504",
                name:                       "震地",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n击伤：对手灵气-2".ApplyCond(castResult),
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (listener, closure, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (listener != d.Listener) return;
                            await d.Src.RemoveBuffProcedure("灵气", 2);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(10,
                        closures: new [] { closure });
                }),

            // 6 12 26 52 102
            new(id:                         "1505",
                name:                       "硬化",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"坚毅+2" +
                    $"\n失去所有护甲",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("坚毅", 2);
                    int value = d.Caster.Armor;
                    if (value > 0)
                        await d.LoseArmorProcedure(value, induced: true);
                }),

            // 8 24 52 105 204
            new(id:                         "1506",
                name:                       "惊吓",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加4滞气",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GiveBuffProcedure("滞气", 4);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1507",
                name:                       "铁布衫",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华\n受到伤害时：最多20",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("伤害上限", 20);
                    await d.Skill.ExhaustProcedure();
                }),

            // 6 12 26 52 102
            new(id:                         "1508",
                name:                       "钢拳",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                costGenerator:              async (env, entity, skill, recursive) =>
                    new ChannelCostResult(4 - entity.Opponent().TraversalBuffs().Count(b => !b.GetEntry().Friendly)),
                costDescription:            CostDescription.ChannelFromValue(4),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{40 + 40 * dj}攻" +
                    $"\n对手每有1种debuff，吟唱-1",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(40 + 40 * d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1509",
                name:                       "天人五衰",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加滞气，缠绕，软弱，腐朽，内伤各5层",
                withinPool:                 false,
                castGenerator:              async d =>
                {
                    BuffEntry[] buffs = new BuffEntry[] { "滞气", "缠绕", "软弱", "腐朽", "内伤" };
                    for (int i = 0; i < buffs.Length; i++)
                        await d.GiveBuffProcedure(buffs[i], 5, induced: i != 0);
                }),

            #endregion
        });
    }

    public void Init()
    {
        List.Do(entry =>
        {
            entry.GenerateCascade();
            entry.CreateSprite();
        });
    }

    public override SkillEntry DefaultEntry() => this["0000"];
}
