
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CLLibrary;
using Cysharp.Threading.Tasks;

public class SkillCategory : Category<SkillEntry>
{
    #region Closures

    public static async UniTask<bool> CheckIsEnd(StageEntity entity, StageSkill skill)
    {
        if (skill.IsEnd)
            return true;

        if (await entity.TryConsumeProcedure("终结"))
            return true;

        return false;
    }

    public static readonly StageClosure Crit = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Crit = true;
                                                    }, key: "CritClosure", rawDescription: "暴击", checkListener: true);

    public static readonly StageClosure LifeSteal = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.LifeSteal = true;
                                                    }, key: "LifeStealClosure", rawDescription: "吸血", checkListener: true);

    public static readonly StageClosure Penetrate = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Penetrate = true;
                                                    }, key: "PenetrateClosure", rawDescription: "穿透", checkListener: true);

    private static readonly StageClosure Shatter = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Shatter = true;
                                                    }, key: "ShatterClosure", rawDescription: "碎防", checkListener: true);

    private static readonly StageClosure QiShiClosure = new(StageClosureDict.DID_DAMAGE, 0,
        async (listener, closure, closureDetails) =>
        {
            DamageDetails d = closureDetails as DamageDetails;
            StageSkill skill = listener as StageSkill;
            int qiShiLingQiGain = 1 + skill.J;
            d.CastResult["QiShiLingQiGain"] = qiShiLingQiGain.ToString();
            await d.Src.GainBuffProcedure("灵气", qiShiLingQiGain, induced: true);
        }, key: "QiShiClosure", rawDescription: "击伤：灵气+[QiShiLingQiGain]", checkListener: true);

    private static readonly StageClosure QiShi2Closure = new(StageClosureDict.DID_DAMAGE, 0,
        async (listener, closure, closureDetails) =>
        {
            DamageDetails d = closureDetails as DamageDetails;
            StageSkill skill = listener as StageSkill;
            // int qiShiLingQiGain = 1 + skill.J;
            // d.CastResult["QiShiLingQiGain"] = qiShiLingQiGain.ToString();
            await d.Src.GainBuffProcedure("灵气", d.Value, induced: true);
        }, key: "QiShi2Closure", rawDescription: "击伤：获得等量灵气", checkListener: true);

    private static readonly StageClosure LianXiClosure = new(StageClosureDict.WIL_DAMAGE, 0,
        async (listener, closure, closureDetails) =>
        {
            DamageDetails d = closureDetails as DamageDetails;
            await d.Src.RemoveArmorProcedure(d.Value, induced: true);
            d.Cancel = true;
        }, key: "LianXiClosure", rawDescription: "击伤：伤害转为破甲", checkListener: true);

    private static readonly StageClosure MoDaoClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
        async (listener, closure, closureDetails) =>
        {
            GainBuffDetails d = closureDetails as GainBuffDetails;
            StageSkill skill = listener as StageSkill;

            int maxReturn = skill.J;
            int actualReturn = d.Src.Memory.TryGetVariable(StageEntity.TriggeredCritTimesKey, 0).ClampUpper(maxReturn);

            d.CastResult["MoDaoCritReturn"] = maxReturn.ToString();

            d.Stack += actualReturn;
        }, key: "MoDaoClosure", rawDescription: "返还至多[MoDaoCritReturn]层已触发的暴击", checkListener: true);

    private static readonly StageClosure ZhenJiaoClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.GainArmorProcedure(d.Value, induced: true);
                                                    }, key: "ZhenJiaoClosure", rawDescription: "击伤：获得等量护甲", checkListener: true);

    private static readonly StageClosure LuoChaClosure = new(StageClosureDict.WIL_ATTACK, -2,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int highestAttackRecord = d.Src.Memory.TryGetVariable(StageEntity.HighestAttackKey, 0);
                                                        d.Value += highestAttackRecord;
                                                    }, key: "LuoChaClosure", rawDescription: "", checkListener: true);

    private static readonly StageClosure DuanSuiClosure = new(StageClosureDict.WIL_HEAL, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        HealDetails d = closureDetails as HealDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int duanSuiConvert = 1 + skill.Dj;

                                                        bool tianRen = d.Src.GetStackOfBuff("天人形态") > 0;
                                                        int stack = tianRen ? 50 : d.Src.GetStackOfBuff("锻体");
                                                        d.Value += duanSuiConvert * stack;
                                                        d.CastResult["DuanSuiConvert"] = duanSuiConvert.ToString();
                                                    }, key: "DuanSuiClosure", rawDescription: "每1锻体，气血+[DuanSuiConvert]", checkListener: true);

    private static readonly StageClosure YangShengClosure = new(StageClosureDict.WIL_HEAL, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        HealDetails d = closureDetails as HealDetails;
                                                        StageSkill skill = listener as StageSkill;

                                                        int yangShengPercent = skill.J switch { 0 => 8, 1 => 8, 2 => 8, 3 => 8, _ => 10 };
                                                        int yangShengHealth = d.Tgt.MaxHp * yangShengPercent / 100;
                                                        d.Value += yangShengHealth;
                                                        d.CastResult["YangShengHealth"] = yangShengHealth.ToString();
                                                    }, key: "YangShengClosure", rawDescription: "治疗[YangShengPercent]%气血", checkListener: true);

    private static readonly StageClosure TuiBianClosure = new(StageClosureDict.WIL_LOSE_ARMOR, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        LoseArmorDetails d = closureDetails as LoseArmorDetails;
                                                        int value = d.Src.Armor;
                                                        if (value > 0)
                                                            d.Value += value;
                                                    }, key: "TuiBianClosure", rawDescription: "失去所有护甲", checkListener: true);

    private static readonly StageClosure LiuXianClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill skill = d.Listener as StageSkill;
                                                        int liuXianConvert = 10 - skill.J;
                                                        int gain = d.Value / liuXianConvert;
                                                        await d.Src.CycleProcedure(WuXing.Shui, gain: gain);
                                                        d.CastResult["LiuXianConvert"] = liuXianConvert.ToString();
                                                    }, key: "LiuXianClosure", rawDescription: $"每造成[LiuXianConvert]点伤害，格挡+1", checkListener: true);

    private static readonly StageClosure LiaoYuanClosure = new(StageClosureDict.WIL_CYCLE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CycleDetails d = closureDetails as CycleDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int liaoYuanGrow = skill.GetJingJie() >= JingJie.HuaShen ? 2 : 1;
                                                        d.Gain += skill.TotalStageCastedCount * liaoYuanGrow;
                                                        d.CastResult["LiaoYuanGrow"] = liaoYuanGrow.ToString();
                                                    }, key: "LiaoYuanClosure", rawDescription: $"成长:多[LiaoYuanGrow]", checkListener: true);

    private static readonly StageClosure NingShuiClosure = new(StageClosureDict.DID_CYCLE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CycleDetails d = closureDetails as CycleDetails;
                                                        BuffEntry elementaryBuff = d.WuXing.GetElementaryBuff();
                                                        int stack = d.Owner.GetStackOfBuff(elementaryBuff);
                                                        await d.Owner.GainBuffProcedure("灵气", stack: stack, induced: true);
                                                    }, key: "NingShuiClosure", rawDescription: "", checkListener: true);

    private static readonly StageClosure RenYuClosure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        bool prevIsJin = skill.PrevSkill(true).Entry.GetWuXing() == WuXing.Jin;
                                                        bool nextIsJin = skill.NextSkill(true).Entry.GetWuXing() == WuXing.Jin;
                                                        d.Times += (prevIsJin ? 1 : 0) + (nextIsJin ? 1 : 0);
                                                    }, key: "RenYuClosure", rawDescription: "每相邻1张金，多1次", checkListener: true);

    private static readonly StageClosure TanZhiClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
        async (listener, closure, closureDetails) =>
        {
            GainBuffDetails d = closureDetails as GainBuffDetails;
            StageSkill skill = listener as StageSkill;

            bool cond = await skill.Owner.TryConsumeProcedure("暴击");
            if (!cond)
            {
                d.CastResult.Append("TanZhiClosure", false);
                return;
            }

            int tanZhiExtra = skill.J switch { 0 => 3, 1 => 3, 2 => 3, 3 => 4, 4 => 5, _ => 6 };
            d.Stack += tanZhiExtra;
            d.CastResult["TanZhiExtra"] = tanZhiExtra.ToString();
        }, key: "TanZhiClosure", rawDescription: "消耗1暴击：多[TanZhiExtra]", checkListener: true);

    private static readonly StageClosure PanXuanClosure = new(StageClosureDict.WIL_GAIN_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainArmorDetails d = closureDetails as GainArmorDetails;
                                                        int gain = (-d.Src.Opponent().Armor).ClampLower(0);
                                                        if (gain <= 0) return; // write as cond
                                                        d.Value += gain;
                                                    }, key: "PanXuanClosure", rawDescription: "每1破甲，护甲+1", checkListener: true);

    private static readonly StageClosure BaiRenClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        d.Src.SetActionPoint(2);
                                                    }, key: "BaiRenClosure", rawDescription: "击伤：二动", checkListener: true);

    private static readonly StageClosure ShanFengClosure = new(StageClosureDict.WIL_CYCLE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CycleDetails d = closureDetails as CycleDetails;
                                                        StageSkill skill = d.Listener as StageSkill;
                                                        int shanFengConvert = skill.J switch { 0 => 15, 1 => 15, 2 => 15, 3 => 15, 4 => 10, _ => 8 };
                                                        int gain = d.Owner.Armor / shanFengConvert;
                                                        d.Gain += gain;
                                                        d.CastResult["ShanFengConvert"] = shanFengConvert.ToString();
                                                    }, key: "ShanFengClosure", rawDescription: $"每[ShanFengConvert]护甲，锋锐+1", checkListener: true);

    private static readonly StageClosure XunLieClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill skill = d.Listener as StageSkill;
                                                        int xunLieFragile = 5 + 5 * skill.J;
                                                        await d.Src.RemoveArmorProcedure(xunLieFragile, induced: true);
                                                        d.CastResult["XunLieFragile"] = xunLieFragile.ToString();
                                                    }, key: "XunLieClosure", rawDescription: "击伤：施加[XunLieFragile]破甲", checkListener: true);

    private static readonly StageClosure ZhiShuiClosure = new(StageClosureDict.UNDAMAGED, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill skill = d.Listener as StageSkill;
                                                        int zhiShuiHealth = 4 + 4 * skill.Dj;
                                                        await d.Src.HealProcedure(zhiShuiHealth, induced: true);
                                                        d.CastResult["ZhiShuiHealth"] = zhiShuiHealth.ToString();
                                                    }, key: "ZhiShuiClosure", rawDescription: $"未击伤：气血+[ZhiShuiHealth]", checkListener: true);

    private static readonly StageClosure TiaoHeClosure = new(StageClosureDict.WIL_HEAL, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        HealDetails d = closureDetails as HealDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        bool isEnd = await CheckIsEnd(d.Src, skill);
                                                        if (!isEnd)
                                                        {
                                                            d.CastResult.Append("TiaoHeClosure", false);
                                                            return;
                                                        }
                                                        
                                                        int tiaoHeGain = 1 + 4 * skill.Dj;
                                                        d.Value += tiaoHeGain;
                                                        d.CastResult["TiaoHeGain"] = tiaoHeGain.ToString();
                                                    }, key: "TiaoHeClosure", rawDescription: $"终结：多[TiaoHeGain]", checkListener: true);

    private static readonly StageClosure QiuLuBaiClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int qiuLuBaiConvert = (int)skill.GetJingJie() switch { 0 => 1, 1 => 2, 2 => 3, 3 => 4, _ => 5 };
                                                        int mul = d.Src.Memory.TryGetVariable(StageEntity.OppoLoseArmorTimesKey, 0);
                                                        d.Value += qiuLuBaiConvert * mul;
                                                        d.CastResult["QiuLuBaiConvert"] = qiuLuBaiConvert.ToString();
                                                    }, key: "QiuLuBaiClosure", rawDescription: $"对手护甲每降低过1次，多[QiuLuBaiConvert]攻", checkListener: true);

    private static readonly StageClosure WuWangClosure = new(StageClosureDict.WIL_DAMAGE, -2,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.GiveBuffProcedure("腐朽", d.Value, induced: true);
                                                        d.Cancel = true;
                                                    }, key: "WuWangClosure", rawDescription: "击伤：伤害转为腐朽", checkListener: true);

    private static readonly StageClosure WuWang2Closure = new(StageClosureDict.WIL_DAMAGE, -2,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.GiveBuffProcedure("腐朽", d.Value, induced: true);

                                                        Buff b = d.Tgt.FindBuff("腐朽");
                                                        if (b != null)
                                                        {
                                                            await b.Owner.LoseArmorProcedure(b.Stack, induced: true);
                                                            b.Emphasize();
                                                            await b.LoseStackProcedure();
                                                        }
                                                        
                                                        d.Cancel = true;
                                                    }, key: "WuWangClosure", rawDescription: "击伤：伤害转为腐朽，触发一次", checkListener: true);

    private static readonly StageClosure TianDiTongShouClosure = new(StageClosureDict.WIL_LOSE_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        LoseArmorDetails d = closureDetails as LoseArmorDetails;
                                                        int fragile = -d.Src.Armor.ClampUpper(0);
                                                        d.Value += fragile;
                                                    }, key: "TianDiTongShouClosure", rawDescription: $"自身每1破甲，多1", checkListener: true);

    private static readonly StageClosure TianDiTongShou2Closure = new(StageClosureDict.WIL_LOSE_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        LoseArmorDetails d = closureDetails as LoseArmorDetails;
                                                        int fragile = -d.Src.Armor.ClampUpper(0);
                                                        d.Value += fragile;
                                                    }, key: "TianDiTongShou2Closure", rawDescription: $"自身每1破甲，施加1破甲", checkListener: true);

    // private static readonly StageClosure YiLianTuoShengClosure = new(StageClosureDict.WIL_DAMAGE, 0,
    //                                                 async (listener, closure, closureDetails) =>
    //                                                 {
    //                                                     DamageDetails d = closureDetails as DamageDetails;
    //                                                     int critStack = d.Src.GetStackOfBuff("暴击");
    //                                                     await d.Src.TryConsumeProcedure("暴击", critStack);
    //                                                     d.Value *= 1 + critStack;
    //                                                 }, key: "YiLianTuoShengClosure", description: "暴击释放", checkListener: true);

    private static readonly StageClosure ShanJiClosureBeforeAttack1 = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        int oppoFragileBeforeAttack = skill.Owner.Opponent().Armor;
                                                        if (oppoFragileBeforeAttack < 0)
                                                            skill.Owner.Memory.SetVariable("OppoFragileBeforeAttack", oppoFragileBeforeAttack);
                                                    }, key: "ShanJiClosureBeforeAttack1", rawDescription: "", checkListener: true);

    private static readonly StageClosure ShanJiClosureBeforeAttack2 = new(StageClosureDict.DID_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        int oppoFragileBeforeAttack =
                                                            skill.Owner.Memory.TryGetVariable("OppoFragileBeforeAttack", 0);

                                                        int gap = skill.Owner.Opponent().Armor - oppoFragileBeforeAttack;
                                                        if (oppoFragileBeforeAttack < 0 && gap > 0)
                                                        {
                                                            await skill.Owner.Opponent().LoseArmorProcedure(gap, induced: true);
                                                        }
                                                                        
                                                        skill.Owner.Memory.SetVariable("OppoFragileBeforeAttack", 0);
                                                    }, key: "ShanJiClosureBeforeAttack2", rawDescription: "破甲将补至攻击前", checkListener: true);

    private static readonly StageClosure ShanJiClosureHighest1 = new(StageClosureDict.DID_LOSE_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        LoseArmorDetails d = closureDetails as LoseArmorDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        if (d.Tgt != skill.Owner.Opponent()) return;

                                                        string OppoHighestFragileKey = "OppoHighestFragile";
                                                        int oppoFragile = skill.Owner.Opponent().Armor;
                                                        skill.Owner.Memory.PerformOperation(OppoHighestFragileKey, 0, record => Mathf.Min(record, oppoFragile));

                                                    }, key: "ShanJiClosureHighest1", rawDescription: "", checkListener: true);

    private static readonly StageClosure ShanJiClosureHighest2 = new(StageClosureDict.DID_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        string OppoHighestFragileKey = "OppoHighestFragile";
                                                        int oppoHighestFragile = skill.Owner.Memory.TryGetVariable(OppoHighestFragileKey, 0);

                                                        int gap = skill.Owner.Opponent().Armor - oppoHighestFragile;
                                                        if (gap > 0)
                                                        {
                                                            await skill.Owner.Opponent().LoseArmorProcedure(gap, induced: true);
                                                        }

                                                    }, key: "ShanJiClosureHighest2", rawDescription: "破甲将补至本局最高", checkListener: true);

    private static readonly StageClosure CaiHongClosure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = d.Listener as StageSkill;
                                                        int haiXiaoConvert = skill.J switch { 0 => 3, 1 => 3, 2 => 3, 3 => 4, 4 => 5, _ => 1 };
                                                        d.CastResult["CaiHongConvert"] = haiXiaoConvert.ToString();
                                                        
                                                        int mana = d.Src.GetStackOfBuff("灵气");
                                                        await d.Src.TryConsumeProcedure("灵气", mana);
                                                        d.Value += mana * haiXiaoConvert;
                                                    }, key: "CaiHongClosure", rawDescription: "消耗每1灵气，多[CaiHongConvert]攻", checkListener: true);

    private static readonly StageClosure CaiHong2Closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = d.Listener as StageSkill;
                                                        int haiXiaoConvert = skill.J switch { 0 => 3, 1 => 3, 2 => 3, 3 => 4, 4 => 5, _ => 1 };
                                                        d.CastResult["CaiHongConvert"] = haiXiaoConvert.ToString();
                                                        
                                                        int mana = d.Src.GetStackOfBuff("灵气");
                                                        d.Value += mana * haiXiaoConvert;
                                                    }, key: "CaiHong2Closure", rawDescription: "每1灵气，多[CaiHongConvert]攻", checkListener: true);

    private static readonly StageClosure HaiXiao2Closure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        int times = skill.J <= JingJie.YuanYing ? 1 : 2;
                                                        int currMana = d.Src.GetStackOfBuff("灵气");
                                                        int currStack = d.Stack;

                                                        d.Stack = times * (currStack + currMana);
                                                    }, key: "HaiXiao2Closure", rawDescription: "灵气翻倍", checkListener: true);

    private static readonly StageClosure HaiXiao3Closure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        int times = skill.J <= JingJie.YuanYing ? 1 : 2;
                                                        int currMana = d.Src.GetStackOfBuff("灵气");
                                                        int currStack = d.Stack;

                                                        d.Stack = times * (currStack + currMana);
                                                    }, key: "HaiXiao3Closure", rawDescription: "灵气变成三倍", checkListener: true);

    private static readonly StageClosure YiMengRuShiClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill skill = d.Listener as StageSkill;

                                                        int times = skill.GetJingJie() <= JingJie.YuanYing ? 1 : 2;
                                                        d.CastResult["YiMengRuShiTimes"] = times.ToString();
                                                        
                                                        await d.Src.GainBuffProcedure("一梦如是", stack: times, induced: true);
                                                    }, key: "YiMengRuShiClosure", "击伤：下[YiMengRuShiTimes]次受伤转为治疗", checkListener: true);
    
    private static readonly StageClosure KongHuanClosure = new(StageClosureDict.WIL_MANA_COST, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CostDetails d = closureDetails as CostDetails;
                                                        d.Value -= d.Entity.Skills.Count(s => s.Entry.WuXing == WuXing.Shui);
                                                    }, key: "KongHuanClosure", "每携带1水：消耗-1", checkListener: true);

    private static readonly StageClosure XieYiClosure = new(StageClosureDict.DID_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        string critKey = StageEntity.TriggeredCritTimesKey;
                                                        string lifestealKey = StageEntity.TriggeredLifestealTimesKey;
                                                        string penetrateKey = StageEntity.TriggeredPenetrateTimesKey;
                                                        int critTimes = d.Src.Memory.TryGetVariable(critKey, 0);
                                                        int lifeStealTimes = d.Src.Memory.TryGetVariable(lifestealKey, 0);
                                                        int penetrateTimes = d.Src.Memory.TryGetVariable(penetrateKey, 0);
                                                        if (critTimes > 0) await d.Src.GainBuffProcedure("暴击", induced: true);
                                                        if (lifeStealTimes > 0) await d.Src.GainBuffProcedure("吸血", induced: true);
                                                        if (penetrateTimes > 0) await d.Src.GainBuffProcedure("穿透", induced: true);
                                                        
                                                        d.CastResult["XieYiCrit"] = critTimes > 0 ? "暴击" : "暴击".ApplyInactive();
                                                        d.CastResult["XieYiLifeSteal"] = lifeStealTimes > 0 ? "吸血" : "吸血".ApplyInactive();
                                                        d.CastResult["XieYiPenetrate"] = penetrateTimes > 0 ? "穿透" : "穿透".ApplyInactive();
                                                    }, key: "XieYiClosure", rawDescription: "返还触发过的[XieYiCrit]/[XieYiLifeSteal]/[XieYiPenetrate]", checkListener: true);

    private static readonly StageClosure QiTunShanHeClosure = new(StageClosureDict.WIL_GAIN_BUFF, -2,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill skill = listener as StageSkill;

                                                        if (d.Tgt != skill.Owner) return;
                                                        if (d.BuffEntry != Encyclopedia.BuffCategory.FromName("灵气")) return;

                                                        int target = d.Src.Memory.TryGetVariable(StageEntity.HighestManaKey, 0) + d.Stack;
                                                        int current = d.Src.GetStackOfBuff("灵气");
                                                        d.Stack = target - current;
                                                    }, key: "QiTunShanHeClosure", rawDescription: $"", checkListener: true);

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
                                                    }, key: "TunTianClosure", rawDescription: "每[TunTianConvert]累计治疗，多1攻", checkListener: true);

    private static readonly StageClosure XiaoSongClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int xiaoSongGrow = Fib.ToValue(4 + skill.J);
                                                        d.Value += xiaoSongGrow * skill.TotalStageCastedCount;
                                                        d.CastResult["XiaoSongGrow"] = xiaoSongGrow.ToString();
                                                    }, key: "XiaoSongClosure", rawDescription: $"成长：多[XiaoSongGrow]", checkListener: true);

    private static readonly StageClosure RuMuSanFenClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Value += d.Src.Opponent().Armor / 2;
                                                    }, key: "RuMuSanFenClosure", rawDescription: "对方每有2护甲，多1", checkListener: true);

    private static readonly StageClosure MingShenClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int mingShenGrow = skill.GetJingJie() >= JingJie.HuaShen ? 2 : 1;
                                                        d.Stack += mingShenGrow * skill.TotalStageCastedCount;
                                                        d.CastResult["MingShenGrow"] = mingShenGrow.ToString();
                                                    }, key: "MingShenClosure", rawDescription: "成长：多[MingShenGrow]", checkListener: true);

    // private static readonly StageClosure LuoYingClosure = new(StageClosureDict.WIL_CYCLE, 0,
    //                                                 async (listener, closure, closureDetails) =>
    //                                                 {
    //                                                     CycleDetails d = closureDetails as CycleDetails;
    //                                                     StageSkill skill = listener as StageSkill;
    //                                                     int luoYingConvert = 5 - skill.Dj;
    //
    //                                                     int manaStack = d.Owner.GetStackOfBuff("灵气");
    //                                                     int flow = manaStack / luoYingConvert;
    //
    //                                                     int consume = flow * luoYingConvert;
    //                                                     await d.Owner.TryConsumeProcedure("灵气", consume);
    //
    //                                                     d.Gain += flow;
    //
    //                                                     d.CastResult["LuoYingConvert"] = luoYingConvert.ToString();
    //                                                 }, key: "LuoYingClosure", rawDescription: $"消耗每[LuoYingConvert]灵气，多1", checkListener: true);

    private static readonly StageClosure LuoYingClosure = new(StageClosureDict.WIL_CYCLE, 0,
        async (listener, closure, closureDetails) =>
        {
            CycleDetails d = closureDetails as CycleDetails;
            StageSkill skill = listener as StageSkill;
            int luoYingGrow = skill.GetJingJie() >= JingJie.HuaShen ? 2 : 1;
            d.Gain += skill.TotalStageCastedCount * luoYingGrow;
            d.CastResult["LuoYingGrow"] = luoYingGrow.ToString();
        }, key: "LuoYingClosure", rawDescription: $"成长:多[LuoYingGrow]", checkListener: true);

    private static readonly StageClosure YiXinYiJianClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int power = d.Src.GetStackOfBuff("力量") + d.Src.GetStackOfBuff("剑意");
                                                        int yiXinYiJianConvert = skill.J switch { 0 => 5, 1 => 5, 2 => 5, 3 => 5, 4 => 6, _ => 8 };
                                                        d.Value += (yiXinYiJianConvert - 1) * power;
                                                        d.CastResult["YiXinYiJianConvert"] = yiXinYiJianConvert.ToString();
                                                    }, key: "YiXinYiJianClosure", rawDescription: $"力量/剑意具有[YiXinYiJianConvert]倍效果", checkListener: true);

    private static readonly StageClosure JianLongZaiTianClosure = new(StageClosureDict.WIL_CHANNEL_COST, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CostDetails d = closureDetails as CostDetails;
                                                        d.Value -= d.Skill.TotalStageCastedCount;
                                                    }, key: "JianLongZaiTianClosure", rawDescription: $"成长：吟唱-1", checkListener: true);

    private static readonly StageClosure ShengJiClosure = new(StageClosureDict.WIL_HEAL, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        HealDetails d = closureDetails as HealDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int shengJiGrow = Fib.ToValue(2 + skill.Dj);
                                                        d.Value += skill.TotalStageCastedCount * shengJiGrow;
                                                        d.CastResult["ShengJiGrow"] = shengJiGrow.ToString();
                                                    }, key: "ShengJiClosure", rawDescription: $"成长：多[ShengJiGrow]", checkListener: true);

    private static readonly StageClosure QingQuanClosure = new(StageClosureDict.WIL_CHANNEL_COST, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CostDetails d = closureDetails as CostDetails;
                                                        if (d.Skill.IsFirstTime)
                                                            return;
                                                        d.Value = 0;
                                                    }, key: "QingQuanClosure", rawDescription: $"非初次：无需吟唱", checkListener: true);

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
                                                    }, key: "YiNianWuLiangJieClosure", rawDescription: $"消耗每8灵气，多重+1", checkListener: true);
    
    private static readonly StageClosure YiQieJieKuClosure = new(StageClosureDict.WIL_MANA_COST, 0,
        async (listener, closure, closureDetails) =>
        {
            CostDetails d = closureDetails as CostDetails;
            bool noOtherLingQi = d.Skill.NoOtherLingQi;
            if (!noOtherLingQi)
            {
                d.CostResult.Append("YiQieJieKuClosure", false);
                return;
            }
            d.Value = 0;
        }, key: "YiQieJieKuClosure", rawDescription: $"唯一灵气牌：免除消耗", checkListener: true);

    private static readonly StageClosure JianWangXingClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.GainBuffProcedure("灵气", induced: true);
                                                    }, key: "JianWangXingClosure", rawDescription: "击伤：灵气+1", checkListener: true);

    private static readonly StageClosure JianWangXing2Closure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        await d.Src.GainBuffProcedure("剑意", induced: true);
                                                    }, key: "JianWangXing2Closure", rawDescription: "击伤：剑意+1", checkListener: true);

    private static readonly StageClosure JianWangXing3Closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill s = listener as StageSkill;
                                                        if (s.Owner != d.Src) return;
                                                        d.DoesntConsumeJianYi = true;
                                                    }, key: "JianWangXing2Closure", rawDescription: "不消耗剑意", checkListener: true);

    private static readonly StageClosure ZhanYiClosure = new(StageClosureDict.WIL_GAIN_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainArmorDetails d = closureDetails as GainArmorDetails;
                                                        d.Value += d.Src.GetStackOfBuff("剑意");
                                                    }, key: "ZhanYiClosure", rawDescription: "每1剑意，护甲+1", checkListener: true);

    private static readonly StageClosure ChangMingClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        if (d.BuffEntry.GetName() != "剑意") return; // write as cond
                                                        int stack = d.Src.GetStackOfBuff("灼烧");
                                                        d.Stack += stack;
                                                    }, key: "ChangMingClosure", rawDescription: "每1灼烧，剑意+1", checkListener: true);

    private static readonly StageClosure YiWuJingHongClosure = new(StageClosureDict.WIL_FULL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Times += d.Src.ExhaustedCount;
                                                    }, key: "YiWuJingHongClosure", rawDescription: "每1已升华牌，多1次", checkListener: true);

    private static readonly StageClosure QianXiangClosure = new(StageClosureDict.WIL_FULL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        d.Times += d.Src.GetStackOfBuff("闪避");
                                                    }, key: "YiWuJingHongClosure", rawDescription: "每1闪避，多1次", checkListener: true);

    private static readonly StageClosure WanXiaClosure = new(StageClosureDict.WIL_GAIN_BUFF, -2,
        async (listener, closure, closureDetails) =>
        {
            GainBuffDetails d = closureDetails as GainBuffDetails;
            StageSkill skill = listener as StageSkill;

            if (d.Tgt != skill.Owner) return;
            if (d.BuffEntry != Encyclopedia.BuffCategory.FromName("剑意")) return;

            int target = d.Src.Memory.TryGetVariable(StageEntity.HighestJianYiKey, 0) + d.Stack;
            int current = d.Src.GetStackOfBuff("剑意");
            d.Stack = target - current;
            
            d.Src.Memory.SetVariable(StageEntity.ThisTurnAttackedKey, false);
            
        }, key: "WanXiaClosure", rawDescription: "", checkListener: true);

    private static readonly StageClosure HongTianClosure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
        async (listener, closure, closureDetails) =>
        {
            AttackDetails d = closureDetails as AttackDetails;
            StageSkill skill = listener as StageSkill;

            bool cond = skill.IsFirstTime;
            if (!cond)
            {
                d.CastResult.Append("HongTianClosure", false);
                return;
            }

            int hongTianExtra = Fib.ToValue(4 + skill.J) * 3;
            d.Value += hongTianExtra;
            d.CastResult["HongTianExtra"] = hongTianExtra.ToString();
        }, key: "HongTianClosure", rawDescription: "初次：多[HongTianExtra]", checkListener: true);

    private static readonly StageClosure NuTongClosure = new(StageClosureDict.WIL_HEALTH_COST, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CostDetails d = closureDetails as CostDetails;
                                                        if (!d.Entity.IsLowHealth)
                                                        {
                                                            d.CostResult.Append("NuTongClosure", false);
                                                            return;
                                                        }
                                                        d.Value = 1;
                                                    }, key: "NuTongClosure", rawDescription: "残血：只需1消耗", checkListener: true);

    private static readonly StageClosure AXiuLuoClosure = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int value = d.Src.Memory.TryGetVariable(StageEntity.BurnTimesKey, 0);
                                                        d.Value += value;
                                                    }, key: "AXiuLuoClosure", rawDescription: "每燃命过1次，多1攻", checkListener: true);

    private static readonly StageClosure HuaBuClosure = new(StageClosureDict.WIL_ATTACK, -1,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        if (d.Src.Armor <= 0)
                                                        {
                                                            d.CastResult.Append("HuaBuClosure", false);
                                                            return;
                                                        }

                                                        int j = skill.GetJingJie();
                                                        int value = j switch { 0 => 2, 1 => 3, 2 => 5, 3 => 13, _ => 30 };
                                                        d.Value += value;
                                                        d.CastResult["HuaBuExtra"] = value.ToString();
                                                    }, key: "HuaBuClosure", rawDescription: "有护甲：多[HuaBuExtra]", checkListener: true);

    private static readonly StageClosure BaJiQuanClosure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int toConsume = Mathf.Max(0, d.Src.Armor);
                                                        if (toConsume > 0)
                                                        {
                                                            await d.Src.LoseArmorProcedure(toConsume, induced: true);
                                                            d.Value += toConsume;
                                                        }
                                                    }, key: "BaJiQuanClosure", rawDescription: "消耗每1护甲，1攻", checkListener: true);

    private static readonly StageClosure WuWeiClosure = new(StageClosureDict.WIL_DISPEL, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DispelDetails d = closureDetails as DispelDetails;
                                                        int stack = d.Entity.GetStackOfBuff("坚毅");
                                                        d.Value += stack;
                                                    }, key: "WuWeiClosure", rawDescription: "每1坚毅，净化1", checkListener: true);

    private static readonly StageClosure JiaShiClosure = new(StageClosureDict.WIL_GAIN_BUFF, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainBuffDetails d = closureDetails as GainBuffDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        if (d.BuffEntry.GetName() != "架势")
                                                        {
                                                            d.CastResult.Append("JiaShiClosure", false);
                                                            return;
                                                        }

                                                        bool isEnd = await CheckIsEnd(d.Src, skill);
                                                        if (!isEnd)
                                                        {
                                                            d.CastResult.Append("JiaShiClosure", false);
                                                            return;
                                                        }

                                                        d.Stack += 1;
                                                    }, key: "JiaShiClosure", rawDescription: "终结：2次", checkListener: true);

    private static readonly StageClosure SunJiClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int sunJiTimes = skill.GetJingJie() == JingJie.HuaShen ? 2 : 1;
                                                        d.CastResult["SunJiTimes"] = skill.GetJingJie() == JingJie.HuaShen ? "2倍" : "";
                                                        d.Tgt.MaxHp -= d.Value * sunJiTimes;
                                                    }, key: "SunJiClosure", rawDescription: "击伤：移除[SunJiTimes]气血上限", checkListener: true);

    private static readonly StageClosure BaWangClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int baWangConvert = 6 - skill.Dj.ClampUpper(4);
                                                        d.CastResult["BaWangConvert"] = baWangConvert.ToString();
                                                        int value = (d.Src.MaxHp - d.Tgt.MaxHp).ClampLower(0);
                                                        d.Value += value / baWangConvert;
                                                    }, key: "BaWangClosure", rawDescription: "每[BaWangConvert]气血上限差，多1", checkListener: true);

    private static readonly StageClosure WengChengClosure = new(StageClosureDict.WIL_GAIN_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainArmorDetails d = closureDetails as GainArmorDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        
                                                        int wengChengConvert = 12 - 2 * skill.GetJingJie();
                                                        d.CastResult["WengChengConvert"] = wengChengConvert.ToString();
                                                        d.Value += d.Tgt.MaxHpDiff / wengChengConvert;
                                                    }, key: "WengChengClosure", rawDescription: "每[WengChengConvert]气血上限差，护甲+1", checkListener: true);

    private static readonly StageClosure FengHouClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        int extra = (d.Tgt.MaxHp - d.Tgt.Hp).ClampLower(0);
                                                        await d.Tgt.LoseMaxHealthProcedure(extra);
                                                    }, key: "FengHouClosure", rawDescription: "击伤：移除对手额外气血上限", checkListener: true);

    private static readonly StageClosure ShouShiClosure = new(StageClosureDict.WIL_GAIN_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainArmorDetails d = closureDetails as GainArmorDetails;
                                                        StageSkill skill = listener as StageSkill;

                                                        int value = d.Tgt.Armor.ClampLower(0);
                                                        d.Value += value;
                                                    }, key: "ShouShiClosure", rawDescription: "护甲翻倍", checkListener: true);

    private static readonly StageClosure JianJiBuClosure = new(StageClosureDict.WIL_GAIN_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        GainArmorDetails d = closureDetails as GainArmorDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int jianJiBuConvert = 1 + skill.Dj;
                                                        int stack = d.Src.GetStackOfBuff("灵气");
                                                        d.Value += stack * jianJiBuConvert;
                                                        d.CastResult["JianJiBuConvert"] = jianJiBuConvert.ToString();
                                                    }, key: "JianJiBuClosure", rawDescription: $"每1灵气，护甲+[JianJiBuConvert]", checkListener: true);

    private static readonly StageClosure YaoTuClosure = new(StageClosureDict.DID_CYCLE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CycleDetails d = closureDetails as CycleDetails;
                                                        BuffEntry elementaryBuff = d.WuXing.GetElementaryBuff();
                                                        int stack = d.Owner.GetStackOfBuff(elementaryBuff);
                                                        await d.Owner.GainArmorProcedure(stack * 2, induced: true);
                                                    }, key: "YaoTuClosure", rawDescription: $"", checkListener: true);

    private static readonly StageClosure DuoDuanCeShiClosure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        int stack = d.Src.GetStackOfBuff("灵气");
                                                        await d.Src.LoseBuffProcedure("灵气", stack);
                                                        d.Times += stack - 1;
                                                    }, key: "DuoDuanCeShiClosure", rawDescription: $"每耗1灵气，多1次", checkListener: true);

    private static readonly StageClosure FuShiClosure = new(StageClosureDict.WIL_LOSE_ARMOR, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        LoseArmorDetails d = closureDetails as LoseArmorDetails;
                                                        int value = d.Src.Armor.ClampLower(0);
                                                        d.Value += value;
                                                    }, key: "FuShiClosure", rawDescription: $"每有1护甲，施加1破甲", checkListener: true);

    private static readonly StageClosure WanQianGuangHuiClosure = new(StageClosureDict.WIL_ATTACK, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        AttackDetails d = closureDetails as AttackDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int stack = d.Src.GetStackOfBuff("锋锐");
                                                        int wanQianGuangHuiConvert = 2 + skill.Dj;
                                                        d.Value += stack * wanQianGuangHuiConvert;
                                                        d.CastResult["WanQianGuangHuiConvert"] = wanQianGuangHuiConvert.ToString();
                                                    }, key: "WanQianGuangHuiClosure", rawDescription: $"每1锋锐，多[WanQianGuangHuiConvert]攻", checkListener: true);

    private static readonly StageClosure SuiYueClosure = new(StageClosureDict.DID_DAMAGE, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        DamageDetails d = closureDetails as DamageDetails;
                                                        StageSkill skill = listener as StageSkill;
                                                        int suiYueGain = 3 + 2 * skill.Dj;
                                                        await d.Src.GiveBuffProcedure("腐朽", suiYueGain, induced: true);
                                                        d.CastResult["SuiYueGain"] = suiYueGain.ToString();
                                                    }, key: "SuiYueClosure", rawDescription: "击伤：施加[SuiYueGain]腐朽", checkListener: true);

    private static readonly StageClosure HuanWuClosure = new(StageClosureDict.WIL_CHANNEL_COST, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CostDetails d = closureDetails as CostDetails;
                                                        if (d.Skill.IsFirstTime)
                                                            return;
                                                        d.Value = 0;
                                                    }, key: "HuanWuClosure", rawDescription: $"非初次：无需吟唱", checkListener: true);

    private static readonly StageClosure GangQuanClosure = new(StageClosureDict.WIL_CHANNEL_COST, 0,
                                                    async (listener, closure, closureDetails) =>
                                                    {
                                                        CostDetails d = closureDetails as CostDetails;
                                                        d.Value -= d.Entity.Opponent().TraversalBuffs().Count(b => !b.GetEntry().Friendly);
                                                    }, key: "GangQuanClosure", rawDescription: $"对手每有1种debuff，吟唱-1", checkListener: true);

    #endregion

    public SkillCategory()
    {
        AddRange(new List<SkillEntry>()
        {
            #region 01金

            new(id:                         "Skill01_001",
                name:                       "金刃",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(j switch { 0 => 3, 1 => 5, 2 => 8, 3 => 13, 4 => 21, _ => 34 }),
                    new RemoveArmorProcedureDefinition(j switch { 0 => 2, 1 => 3, 2 => 5, 3 => 8, 4 => 13, _ => 21 }, induced: false),
                }),

            new(id:                         "Skill01_002",
                name:                       "起势",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("QiShiLingQiGain", (1 + j).ToString())
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen),
                    new AttackProcedureDefinition(4)
                        .AddClosure(QiShiClosure)
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen),
                    
                    new AttackProcedureDefinition(4)
                        .AddClosure(QiShi2Closure)
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu),
                    
                    new GainBuffProcedureDefinition("灵气", 1 + j, induced: false)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "Skill01_003",
                name:                       "敛息",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ManaCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(6 + 4 * j)
                        .AddClosure(LianXiClosure),
                }),

            new(id:                         "Skill01_019",
                name:                       "磨刀",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("MoDaoCritReturn", (j).ToString()),
                    new GainBuffProcedureDefinition("暴击")
                        .SetPreCondDefinition(PreCondDefinition.LeLianQi),
                    new GainBuffProcedureDefinition("暴击", stack: j switch {0 => 1, 1 => 1, 2 => 1, 3 => 1, 4 => 1, _ => 2})
                        .AddClosure(MoDaoClosure)
                        .SetPreCondDefinition(PreCondDefinition.GeZhuJi),
                }),
            
            new(id:                         "Skill01_005",
                name:                       "刃雨",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(6 + 4 * j)
                        .AddClosure(RenYuClosure),
                }),

            new(id:                         "Skill01_020",
                name:                       "弹指",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("TanZhiExtra", (1 + j).ToString()),
                    new GainBuffProcedureDefinition("灵气", 1 + j)
                        .AddClosure(TanZhiClosure),
                }),
            
            new(id:                         "Skill01_014",
                name:                       "摇曳",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: j switch { 0 => 2, 1 => 2, 2 => 2, 3 => 2, 4 => 3, _ => 5}),
                    new GainBuffProcedureDefinition("摇曳")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("锋锐变为施加破甲")),
                }),

            new(id:                         "Skill01_011",
                name:                       "秋露白",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ManaCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("QiuLuBaiConvert", (j switch { 0 => 1, 1 => 2, 2 => 3, 3 => 4, 4 => 5, _ => 6 }).ToString()),
                    new AttackProcedureDefinition(j switch { 0 => 4, 1 => 8, 2 => 12, 3 => 16, 4 => 20, _ => 24 })
                        .AddClosure(QiuLuBaiClosure),
                }),

            new(id:                         "Skill01_009",
                name:                       "无妄",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ManaCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1)
                        .AddClosure(WuWangClosure)
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen),
                    
                    new AttackProcedureDefinition(1)
                        .AddClosure(WuWang2Closure)
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu),
                }),

            new(id:                         "Skill01_010",
                name:                       "寻猎",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("XunLieFragile", (5 + 5 * j).ToString()),
                    new AttackProcedureDefinition(2)
                        .AddClosure(XunLieClosure),
                }),

            new(id:                         "Skill01_013",
                name:                       "醉意",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(j switch{ 0 => 3, 1 => 5, 2 => 8, 3 => 13, 4 => 21, _ => 34 })
                        .AddClosure(Crit),
                }),

            new(id:                         "Skill01_004",
                name:                       "流云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(3 + 3 * j, induced: false),
                    new RemoveArmorProcedureDefinition(3 + 3 * j, induced: true),
                }),

            new(id:                         "Skill01_012",
                name:                       "天地同寿",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new RemoveArmorProcedureDefinition(j switch{ 0 => 3, 1 => 5, 2 => 8, 3 => 13, 4 => 21, _ => 34 })
                        .AddClosure(TianDiTongShouClosure)
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen),
                    new RemoveArmorProcedureDefinition(j switch{ 0 => 3, 1 => 5, 2 => 8, 3 => 13, 4 => 21, _ => 34 })
                        .SetPostCondDefinition(PostCondDefinition.StartStage)
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen),
                    
                    new LoseArmorProcedureDefinition(j switch{ 0 => 3, 1 => 5, 2 => 8, 3 => 13, 4 => 21, _ => 34 })
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu),
                    new RemoveArmorProcedureDefinition(0, induced: true)
                        .AddClosure(TianDiTongShou2Closure)
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu),
                    new DirectProcedureDefinition(async d =>
                        {
                            if (!d.Recursive)
                                return;
                            await d.Caster.CastProcedure(d.Skill, false);
                        }).SetPostCondDefinition(PostCondDefinition.StartStage)
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"开局：使用一次")),
                }),

            new(id:                         "Skill01_018",
                name:                       "闪击",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ChannelCostDefinition.FromJ(j => j switch { 0 => 2, 1 => 2, 2 => 2, 3 => 1, 4 => 0, _ => 0 }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1)
                        .AddClosure(ShanJiClosureBeforeAttack1)
                        .AddClosure(ShanJiClosureBeforeAttack2)
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen),
                    
                    new AttackProcedureDefinition(1)
                        .AddClosure(ShanJiClosureHighest1)
                        .AddClosure(ShanJiClosureHighest2)
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu),
                }),

            new(id:                         "Skill01_016",
                name:                       "袖里乾坤",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("暴击", j switch { 0 => 1, 1 => 1, 2 => 1, 3 => 2, 4 => 2, _ => 3 }),
                    new GainArmorProcedureDefinition(6, induced: true),
                    new GainBuffProcedureDefinition("暴击", j switch { 0 => 1, 1 => 1, 2 => 1, 3 => 1, 4 => 2, _ => 3 })
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),
            
            new(id:                         "Skill01_008",
                name:                       "山风",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("ShanFengConvert", (j switch { 0 => 15, 1 => 15, 2 => 15, 3 => 15, 4 => 10, _ => 8 }).ToString()),
                    new GainArmorProcedureDefinition(j switch { 0 => 15, 1 => 15, 2 => 15, 3 => 15, 4 => 20, _ => 25 }, induced: false),
                    new CycleProcedureDefinition(WuXing.Jin)
                        .AddClosure(ShanFengClosure),
                }),
            
            new(id:                         "Skill01_006",
                name:                       "盘旋",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromJ(j => j switch { 0 => 1, 1 => 1, 2 => 1, 3 => 1, 4 => 0, _ => 0 }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new RemoveArmorProcedureDefinition(j switch { 0 => 10, 1 => 10, 2 => 10, 3 => 10, 4 => 20, _ => 30 }, induced: false),
                    new GainArmorProcedureDefinition(0, induced: true)
                        .AddClosure(PanXuanClosure),
                }),

            new(id:                         "Skill01_017",
                name:                       "一莲托生",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition()
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    new GiveBuffProcedureDefinition("跳行动", j <= JingJie.HuaShen ? 1 : 2)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                    new ExhaustProcedureDefinition()
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),
            
            #endregion

            #region 02水
            
            new(id:                         "Skill02_001",
                name:                       "恋花",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Health,
                cost:                       ManaCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(3 + dj) * 2)
                        .AddClosure(LifeSteal),
                }),

            new(id:                         "Skill02_002",
                name:                       "吐纳",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 2 + dj),
                    new GainMaxHealthProcedureDefinition(4 + 4 * dj, induced: true),
                    new GainBuffProcedureDefinition("玄武吐息法", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("治疗可以穿上限"))
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),
            
            new(id:                         "Skill02_003",
                name:                       "止水",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Health,
                cost:                       ManaCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("ZhiShuiHealth", (4 + 4 * dj).ToString()),
                    new AttackProcedureDefinition(4 + 4 * dj)
                        .AddClosure(ZhiShuiClosure),
                }),
            
            new(id:                         "Skill02_005",
                name:                       "大鱼",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Mana | TagCategory.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(2 + 4 * dj),
                    new GainBuffProcedureDefinition("灵气", dj switch
                    {
                        0 => 1,
                        1 => 1,
                        2 => 1,
                        3 => 2,
                        _ => 3,
                    }),
                    new SetActionPointProcedureDefinition(2)
                        .SetPreCondDefinition(PreCondDefinition.GeJinDan),
                }),

            new(id:                         "Skill02_009",
                name:                       "一梦如是",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Health,
                cost:                       ManaCostDefinition.FromJ(j => j <= JingJie.JinDan ? 4 : 0),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("YiMengRuShiTimes", (j <= JingJie.YuanYing ? 1 : 2).ToString()),
                    new AttackProcedureDefinition(1)
                        .AddClosure(YiMengRuShiClosure),
                }),
            
            new(id:                         "Skill02_006",
                name:                       "彩虹",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("CaiHongConvert", (j switch{0 => 3, 1 => 3, 2 => 3, 3 => 4, 4 => 5, _ => 1}).ToString()),
                    
                    new AttackProcedureDefinition(14)
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen)
                        .AddClosure(CaiHongClosure),
                    
                    new AttackProcedureDefinition(14)
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu)
                        .AddClosure(CaiHong2Closure),
                }),

            new(id:                         "Skill02_007",
                name:                       "海啸",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Mana,
                cost:                       ChannelCostDefinition.FromJ(j => j <= JingJie.HuaShen ? 3 : 1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 0)
                        .AddClosure(HaiXiao2Closure)
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    new GainBuffProcedureDefinition("灵气", 0)
                        .AddClosure(HaiXiao3Closure)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new GainBuffProcedureDefinition("禁止灵气")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("之后无法获得灵气")),
                }),
            
            new(id:                         "Skill02_008",
                name:                       "飞鸿踏雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Defend | TagCategory.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("飞鸿踏雪")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("二动时：获得1格挡"))
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new CycleProcedureDefinition(WuXing.Shui, gain: 1),
                    new SetActionPointProcedureDefinition(2),
                }),
            
            new(id:                         "Skill02_019",
                name:                       "奔腾",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetActionPointProcedureDefinition(3),
                    new GainBuffProcedureDefinition("滞气", 3),
                }),
            
            new(id:                         "Skill02_010",
                name:                       "空幻",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       (j, dj) => new ManaCostDefinition(3 + dj, closures: new []{ KongHuanClosure }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(6 + 6 * dj),
                }),
            
            new(id:                         "Skill02_011",
                name:                       "激流",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Mana,
                cost:                       ManaCostDefinition.FromDj(dj => 3 + dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(8 + 4 * dj),
                    new GainBuffProcedureDefinition("灵气", 3 + dj, induced: true),
                }),
            
            new(id:                         "Skill02_012",
                name:                       "潮汐",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Mana | TagCategory.Attack | TagCategory.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", stack: 1 + dj),
                    new AttackProcedureDefinition(20 + 10 * dj)
                        .AddClosure(LifeSteal)
                        .SetPostCondDefinition(PostCondDefinition.ManaBurst(5 + 5 * dj)),
                }),
            
            new(id:                         "Skill02_013",
                name:                       "踏浪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Mana,
                cost:                       ManaCostDefinition.FromDj(dj => 2 + dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 5 + 2 * dj),
                }),
            
            new(id:                         "Skill02_014",
                name:                       "写意",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("XieYiCrit", "暴击"),
                    new TrySetValueProcedureDefinition("XieYiLifeSteal", "吸血"),
                    new TrySetValueProcedureDefinition("XieYiPenetrate", "穿透"),
                    new AttackProcedureDefinition(10 + 4 * dj)
                        .AddClosure(XieYiClosure),
                }),

            new(id:                         "Skill02_015",
                name:                       "气吞山河",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 1 + dj)
                        .AddClosure(QiTunShanHeClosure)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"灵气补至本局最高+{(procedureDefinition as GainBuffProcedureDefinition).Stack}")),
                }),
            
            new(id:                         "Skill02_016",
                name:                       "吞天",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("TunTianConvert", (5 - dj).ToString()),
                    new AttackProcedureDefinition(1)
                        .AddClosure(TunTianClosure),
                }),
            
            new(id:                         "Skill02_017",
                name:                       "瑞雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ManaCostDefinition.FromValue(3),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Shui, gain: 1 + dj),
                    new GainBuffProcedureDefinition("瑞雪", induced: false)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("格挡变成治疗")),
                    new GainBuffProcedureDefinition("禁止二动", induced: false)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("无法二动")),
                }),
            
            new(id:                         "Skill02_018",
                name:                       "镜花水月",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Mana | TagCategory.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
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
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"每耗40气血，灵气+1\n每耗1灵气，气血+10")),
                }),
            
            #endregion

            #region 03木
            
            new(id:                         "Skill03_019",
                name:                       "若竹",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(6 + dj))
                        .AddClosure(Penetrate),
                }),
            
            new(id:                         "Skill03_001",
                name:                       "小松",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Growth,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("XiaoSongGrow", Fib.ToValue(4 + j).ToString()),
                    new AttackProcedureDefinition(Fib.ToValue(3 + j))
                        .AddClosure(XiaoSongClosure),
                }),

            new(id:                         "Skill03_002",
                name:                       "入木三分",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(4 + 4 * dj)
                        .AddClosure(RuMuSanFenClosure),
                }),

            new(id:                         "Skill03_004",
                name:                       "回春",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Defend | TagCategory.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(Fib.ToValue(4 + dj), induced: false)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"双方护甲+{(procedureDefinition as GainArmorProcedureDefinition).Value}")),
                    new GiveArmorProcedureDefinition(Fib.ToValue(4 + dj), induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => { }),
                    new HealProcedureDefinition(Fib.ToValue(4 + dj), induced: false)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"双方气血+{(procedureDefinition as HealProcedureDefinition).Value}")),
                    new HealOppoProcedureDefinition(Fib.ToValue(4 + dj), induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => { }),
                }),

            new(id:                         "Skill03_005",
                name:                       "落英",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("LuoYingGrow", (j <= JingJie.YuanYing ? 1 : 2).ToString()),
                    new CycleProcedureDefinition(WuXing.Mu, gain: j <= JingJie.JinDan ? 1 : 2)
                        .AddClosure(LuoYingClosure),
                }),
            
            new(id:                         "Skill03_006",
                name:                       "钟声",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("钟声", 1 + dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"使下{(procedureDefinition as GainBuffProcedureDefinition).Stack}张牌升级")),
                }),

            new(id:                         "Skill03_007",
                name:                       "一心一剑",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("YiXinYiJianConvert", (5 + dj).ToString()),
                    new AttackProcedureDefinition(4 + 4 * dj)
                        .AddClosure(YiXinYiJianClosure),
                }),
            
            new(id:                         "Skill03_008",
                name:                       "梅开二度",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cost:                       ChannelCostDefinition.FromDj(dj => 1 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("二重", 1 + dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}张牌使用两次")),
                }),

            new(id:                         "Skill03_009",
                name:                       "一叶知秋",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_FULL_ATTACK, -1, async (listener, closure, closureDetails) =>
                    {
                        StageSkill yiYeZhiQiu = listener as StageSkill;
                        AttackDetails d = (AttackDetails)closureDetails;

                        if (yiYeZhiQiu.Owner != d.Src) return;
                        StageSkill invokingSkill = d.Listener as StageSkill;
                        if (invokingSkill == null) return;
                        if (invokingSkill.Entry == yiYeZhiQiu.Entry) return;

                        string key = "UsedClosureDict";
                        yiYeZhiQiu.Owner.Memory.PerformOperation(key, new List<StageClosure>(), record =>
                        {
                            foreach (StageClosure closure in d.Closures)
                            {
                                if (record.Contains(closure))
                                    continue;
                                record.Add(closure);
                            }
                            return record;
                        });
                    }),
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("CopiedClosures", "||具有所有已触发的攻击描述"),
                    new DirectProcedureDefinition(async d =>
                        {
                            string key = "UsedClosureDict";
                            List<StageClosure> list = d.Caster.Memory.TryGetVariable(key, new List<StageClosure>());
                            StageClosure[] closures = list.ToArray();
                            
                            foreach (StageClosure c in closures)
                            {
                                if (c.Description == null)
                                    return;
                                d.CastResult.Append(c.Key, false);
                            }
                            
                            await d.AttackProcedure(1, closures: closures);

                            Description extraDescription = new Description();
                            foreach (StageClosure c in closures)
                            {
                                extraDescription.AppendSoftReturn();
                                Description closureDescription = c.Description;
                                closureDescription.ApplyReplaceValues(d.CastResult);
                                closureDescription.ApplyResult(d.CastResult, c.Key);
                                extraDescription.Join(closureDescription);
                            }
                            
                            d.CastResult["CopiedClosures"] = extraDescription.GetSymbolizedString();
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) =>
                        {
                            DirectProcedureDefinition pd = procedureDefinition as DirectProcedureDefinition;
        
                            d.Join(pd.PostCondDefinition.Description);
                            
                            d.Join($"1攻");
                            Description closureDescription = "[CopiedClosures]";
                            closureDescription.ApplyReplaceValues(castResult);
                            d.Join(closureDescription);
                            
                            d.ApplyStyle(castResult, pd);
                        }),
                }),

            new(id:                         "Skill03_010",
                name:                       "潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Defend | TagCategory.Growth,
                cost:                       ManaCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避"),
                    new AttackProcedureDefinition((4 + 2 * dj) * (4 + 2 * dj))
                        .SetPostCondDefinition(PostCondDefinition.FromCc(2 + dj, true)),
                }),

            new(id:                         "Skill03_011",
                name:                       "见龙在田",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Defend | TagCategory.Growth,
                cost:                       (j, dj) => new ChannelCostDefinition(5 - dj, closures: new []{ JianLongZaiTianClosure }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(10),
                    new GainBuffProcedureDefinition("闪避", 2),
                }),
            
            new(id:                         "Skill03_020",
                name:                       "时光",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 1 + dj),
                    new GainBuffProcedureDefinition("时光", j <= JingJie.YuanYing ? 1 : 2)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下次使用成长牌前，先成长{(procedureDefinition as GainBuffProcedureDefinition).Stack}次")),
                }),
            
            new(id:                         "Skill03_003",
                name:                       "明神",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Mana | TagCategory.Growth,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("MingShenGrow", (j >= JingJie.HuaShen ? 2 : 1).ToString()),
                    new GainBuffProcedureDefinition("灵气", 1 + dj)
                        .AddClosure(MingShenClosure),
                }),

            new(id:                         "Skill03_014",
                name:                       "飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Defend | TagCategory.Growth,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避"),
                    new GainBuffProcedureDefinition("飞龙在天", 2 + 2 * dj, induced: true)
                        .SetPostCondDefinition(PostCondDefinition.FirstTime)
                        .SetDescription((d, procedureDefinition, costResult, castResult) =>
                        {
                            GainBuffProcedureDefinition pd = procedureDefinition as GainBuffProcedureDefinition;
                            d.Join(pd.PostCondDefinition.Description);
                            d.Join($"跳过下{(procedureDefinition as GainBuffProcedureDefinition).Stack}张牌，使其成长");
        
                            if (pd.Closures != null)
                                foreach (StageClosure c in pd.Closures)
                                {
                                    d.AppendSoftReturn();
                                    Description closureDescription = c.Description;
                                    closureDescription.ApplyReplaceValues(castResult);
                                    closureDescription.ApplyResult(castResult, c.Key);
                                    d.Join(closureDescription);
                                }
        
                            d.ApplyStyle(castResult, pd);
                        }),
                }),

            new(id:                         "Skill03_015",
                name:                       "回响",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2FanXu,
                cost:                       ManaCostDefinition.FromJ(j => j <= JingJie.JinDan ? 2 : 0),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            if (!d.Recursive)
                                return;
                            if (!d.Caster.Skills[0].Exhausted)
                                await d.Caster.CastProcedure(d.Caster.Skills[0], false);
                        
                            if (d.J >= JingJie.HuaShen)
                                if (!d.Caster.Skills[1].Exhausted)
                                    await d.Caster.CastProcedure(d.Caster.Skills[1], false);
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join(j < JingJie.HuaShen
                            ? $"使用第一张牌\n已升华的牌无效"
                            : $"使用前两张牌\n已升华的牌无效")),
                }),

            new(id:                         "Skill03_016",
                name:                       "亢龙有悔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Mu, gain: 2 + dj, induced: true),
                    new GainBuffProcedureDefinition("闪避", 2 + dj, induced: true),
                    new AttackProcedureDefinition(2 + dj, times: 2 + dj),
                    new GainBuffProcedureDefinition("不堪一击", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("遭受不堪一击")),
                }),
    
            new(id:                         "Skill03_017",
                name:                       "鹤回翔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            int leftIndex = d.Skill.PrevSkill(true).SlotIndex;
                            int rightIndex = d.Skill.NextSkill(true).SlotIndex;
                        
                            var tempStageSkill = d.Caster.Skills[leftIndex];
                            d.Caster.Skills[leftIndex] = d.Caster.Skills[rightIndex];
                            d.Caster.Skills[rightIndex] = tempStageSkill;

                            var tempIndex = d.Caster.Skills[leftIndex].SlotIndex;
                            d.Caster.Skills[leftIndex].SlotIndex = d.Caster.Skills[rightIndex].SlotIndex;
                            d.Caster.Skills[rightIndex].SlotIndex = tempIndex;
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("交换左右牌")),
                    new SetActionPointProcedureDefinition(2)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),

            new(id:                         "Skill03_018",
                name:                       "一念无量劫",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShen2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("多重", 0)
                        .AddClosure(YiNianWuLiangJieClosure),
                },
                trivia:"在个人量子超算还没普及的时代，凡人只能体验个二十劫意思意思"),
            
            #endregion

            #region 04火
            
            new(id:                         "Skill04_001",
                name:                       "云袖",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(2 + 2 * dj, times: 2),
                    new GainArmorProcedureDefinition(2 + 2 * dj, induced: false),
                }),

            new(id:                         "Skill04_019",
                name:                       "一切皆苦",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Mana,
                cost:                       (j, dj) => new ManaCostDefinition(9 - 2 * dj, closures: new []{ YiQieJieKuClosure }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("抱朴")
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"每回合获得{(procedureDefinition as GainBuffProcedureDefinition).Stack}灵气")),
                }),

            new(id:                         "Skill04_003",
                name:                       "剑王行",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1, times: 2 + dj)
                        .AddClosure(JianWangXingClosure)
                        .SetPreCondDefinition(PreCondDefinition.LeHuaShen),
                    
                    new AttackProcedureDefinition(1, times: 2 + dj)
                        .AddClosure(JianWangXing2Closure)
                        .AddClosure(JianWangXing3Closure)
                        .SetPreCondDefinition(PreCondDefinition.GeFanXu),
                }),

            new(id:                         "Skill04_012",
                name:                       "拂晓",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(2),
                    new GainBuffProcedureDefinition("剑意", Fib.ToValue(3 + dj))
                        .SetPostCondDefinition(PostCondDefinition.FirstTime),
                }),
            
            new(id:                         "Skill04_005",
                name:                       "天衣无缝",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ManaCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(2, induced: true),
                    new GainBuffProcedureDefinition("天衣无缝", 1 + 4 * dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"直到使用攻击牌：每回合{(procedureDefinition as GainBuffProcedureDefinition).Stack}攻，不消耗剑意")),
                }),
            
            new(id:                         "Skill04_020",
                name:                       "窑土",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    
                    new CycleProcedureDefinition(WuXing.Huo, gain: 1 + dj)
                        .AddClosure(YaoTuClosure)
                        .SetDescription((d, procedureDefinition, costResult, castResult) =>
                        {
                            CycleProcedureDefinition pd = procedureDefinition as CycleProcedureDefinition;
                            string elementaryBuffName = pd.WuXing.GetElementaryBuff().GetName();
                            d.Join($"{elementaryBuffName}+{pd.Gain}||每1{elementaryBuffName}，护甲+2");
                        }),
                }),
            
            new(id:                         "Skill04_009",
                name:                       "一舞惊鸿",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Exhaust,
                cost:                       ManaCostDefinition.FromJ(j => j switch{ 0 => 4, 1 => 4, 2 => 4, 3 => 4, 4 => 2, _ => 2 }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            int times = d.J <= JingJie.HuaShen ? 1 : 2;
                            for (int i = 0; i < times; i++)
                            {
                                int slotIndex = d.Skill.SlotIndex;
                                StageSkill skill = d.Caster.PrevSkills(slotIndex, loop: false)
                                    .FirstObj(skill => !skill.Exhausted);
                                if (skill != null)
                                    await skill.ExhaustProcedure();
                            }
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"升华左边牌" + (j <= JingJie.HuaShen ? "" : "，两次"))),
                    new AttackProcedureDefinition(1)
                        .AddClosure(YiWuJingHongClosure),
                }),
            
            new(id:                         "Skill04_006",
                name:                       "晚霞",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("剑意", j <= JingJie.YuanYing ? 3 : 5)
                        .AddClosure(WanXiaClosure)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"剑意补至本局最高+{(procedureDefinition as GainBuffProcedureDefinition).Stack}")),
                }),

            new(id:                         "Skill04_015",
                name:                       "藏锋",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Defend | TagCategory.Exhaust,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GainBuffProcedureDefinition("藏锋")
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"攻击的效果变成获得护甲")),
                }),

            new(id:                         "Skill04_011",
                name:                       "轰天",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Health,
                cost:                       HealthCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("HongTianExtra", (Fib.ToValue(4 + dj) * 3).ToString()),
                    new AttackProcedureDefinition(Fib.ToValue(4 + dj))
                        .AddClosure(HongTianClosure),
                }),

            new(id:                         "Skill04_013",
                name:                       "怒瞳",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Defend | TagCategory.Health,
                cost:                       (j, dj) => new HealthCostDefinition(Fib.ToValue(5 + dj), closures: new []{ NuTongClosure }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(Fib.ToValue(5 + dj)),
                }),

            new(id:                         "Skill04_010",
                name:                       "断罪",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(dj switch { 0 => 10, 1 => 25, 2 => 45, 3 => 70, _ => 100 }),
                    new GainBuffProcedureDefinition("跳行动", 4)
                        .SetPostCondDefinition(PostCondDefinition.HasOtherAttack),
                }),

            new(id:                         "Skill04_014",
                name:                       "浴火",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Health | TagCategory.Defend,
                cost:                       HealthCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(2),
                    new CycleProcedureDefinition(WuXing.Huo, gain: Fib.ToValue(2 + dj))
                        .SetPostCondDefinition(PostCondDefinition.FirstTime),
                    new GainBuffProcedureDefinition("浴火")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("燃命时：根据灼烧造成伤害")),
                }),

            new(id:                         "Skill04_018",
                name:                       "阿修罗",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ChannelCostDefinition.FromValue(5),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(7 + 2 * dj, times: 7 + 2 * dj)
                        .AddClosure(AXiuLuoClosure),
                    new GainBuffProcedureDefinition("禁止行动")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("禁止行动")),
                },
                trivia: "用完之后就会体力耗尽动弹不得"),

            new(id:                         "Skill04_016",
                name:                       "明镜",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Health | TagCategory.Defend,
                cost:                       HealthCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("明镜", j switch { 0 => 1, 1 => 1, 2=> 1, 3 => 2, _ => 3 })
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}次失去气血时，获得护甲")),
                }),
            
            new(id:                         "Skill04_021",
                name:                       "罗刹",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cost:                       HealthCostDefinition.FromValue(1),
                tagComposite:               TagCategory.Health | TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1, times: j <= JingJie.YuanYing ? 1 : 2)
                        .AddClosure(LuoChaClosure)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            =>
                        {
                            AttackProcedureDefinition pd = procedureDefinition as AttackProcedureDefinition;
                            d.Join($"造成本局最高攻+{pd.Value}");
                            if (pd.Times > 1)
                            {
                                d.Join($"x{pd.Times}");
                            }
                        }),
                }),
            
            new(id:                         "Skill04_022",
                name:                       "舍生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Health,
                cost:                       (j, dj) => new HealthCostDefinition(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new BecomeLowHealProcedureDefinition(),
                    new GainBuffProcedureDefinition("不屈", stack: j switch { 0 => 2, 1 => 2, 2 => 2, 3 => 2, 4 => 3, _ => 4 }, induced: true)
                        .SetPostCondDefinition(PostCondDefinition.FirstTime)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"初次：{(procedureDefinition as GainBuffProcedureDefinition).Stack}回合，气血无法降低至0")),
                }),

            new(id:                         "Skill04_023",
                name:                       "不动明王诀",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Health | TagCategory.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new BecomeLowHealProcedureDefinition(),
                    new GainBuffProcedureDefinition("不动明王决")
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"使用非攻击牌后将其升华")),
                }),
            
            #endregion

            #region 05土

            new(id:                         "Skill05_001",
                name:                       "寸劲",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(6 + dj)),
                    new GainBuffProcedureDefinition("软弱", j switch { 0 => 4, 1 => 6, 2 => 8, 3 => 11, _ => 15 }, induced: true),
                }),

            new(id:                         "Skill05_002",
                name:                       "滑步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("HuaBuExtra", (dj switch { 0 => 2, 1 => 3, 2 => 5, 3 => 13, _ => 30 }).ToString()),
                    new AttackProcedureDefinition(4 + 4 * dj)
                        .AddClosure(HuaBuClosure),
                }),

            new(id:                         "Skill05_003",
                name:                       "守势",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromJ(j => j == JingJie.HuaShen ? 2 : 1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(j switch { 0 => 10, 1 => 15, 2 => 25, 3 => 40, _ => 30 }),
                    new GainArmorProcedureDefinition(0, induced: true)
                        .AddClosure(ShouShiClosure)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),

            new(id:                         "Skill05_004",
                name:                       "澄心",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DispelProcedureDefinition(2 + dj, induced: true),
                    new AttackProcedureDefinition(4 + 4 * dj),
                }),

            new(id:                         "Skill05_005",
                name:                       "泰山落",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(Fib.ToValue(6 + dj)),
                    new AttackProcedureDefinition(0)
                        .SetPostCondDefinition(PostCondDefinition.IsEnd)
                        .AddClosure(BaJiQuanClosure),
                }),
            
            new(id:                         "Skill05_006",
                name:                       "龟息",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Tu, gain: 3 + dj),
                    new GainBuffProcedureDefinition("内伤", 6 + 6 * dj),
                }),

            new(id:                         "Skill05_007",
                name:                       "震脚",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(12 + 8 * dj)
                        .AddClosure(ZhenJiaoClosure),
                }),

            new(id:                         "Skill05_008",
                name:                       "崩山掌",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(20),
                    new GainBuffProcedureDefinition("护甲返还", 1 + dj, induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}次失去护甲时，返还")),
                }),
            
            new(id:                         "Skill05_009",
                name:                       "须弥结界",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避"),
                    new GainArmorProcedureDefinition(100)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                    // new GainBuffProcedureDefinition("禁止护甲")
                    //     .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"无法获得护甲"))
                    //     .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),
            
            new(id:                         "Skill05_010",
                name:                       "隼击",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("SunJiTimes", "")
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    new TrySetValueProcedureDefinition("SunJiTimes", "2倍")
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new AttackProcedureDefinition(4 + 4 * dj)
                        .AddClosure(SunJiClosure),
                }),
            
            new(id:                         "Skill05_011",
                name:                       "霸王",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("BaWangConvert", (6 - dj).ToString()),
                    new AttackProcedureDefinition(4 + 3 * dj)
                        .AddClosure(BaWangClosure),
                }),

            new(id:                         "Skill05_012",
                name:                       "锻骨",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("内伤", 4 + 2 * dj),
                    new GainArmorProcedureDefinition(15 + 15 * dj),
                }),

            new(id:                         "Skill05_013",
                name:                       "固元",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Health,
                overridingMergeRule:        MergeRule.GuYuanMergeRule,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：气血上限增加{Fib.ToValue(4 + dj)}")),
                }),

            new(id:                         "Skill05_014",
                name:                       "封喉",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(j + 5))
                        .AddClosure(FengHouClosure),
                }),
            
            new(id:                         "Skill05_015",
                name:                       "瓮城",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainMaxHealthProcedureDefinition(16 + 8 * dj, induced: true),
                    new TrySetValueProcedureDefinition("WengChengConvert", (12 - 2 * j).ToString()),
                    new GainArmorProcedureDefinition(0, induced: false)
                        .AddClosure(WengChengClosure),
                }),

            new(id:                         "Skill05_016",
                name:                       "养生",
                wuXing:                     WuXing.Tu,
                tagComposite:               TagCategory.Health,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("YangShengPercent", (j switch { 0 => 8, 1 => 8, 2 => 8, 3 => 8, _ => 10 }).ToString()),
                    new HealProcedureDefinition(0)
                        .AddClosure(YangShengClosure),
                }),

            new(id:                         "Skill05_017",
                name:                       "磐石",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Tu, gain: 1 + dj),
                    new GainBuffProcedureDefinition("磐石", 1 + dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult) =>
                        {
                            
                            GainBuffProcedureDefinition pd = procedureDefinition as GainBuffProcedureDefinition;
                            d.Join(pd.PostCondDefinition.Description);
                            
                            d.Join($"本局中吟唱时：坚毅+{1 + dj}");
        
                            if (pd.Closures != null)
                                foreach (StageClosure c in pd.Closures)
                                {
                                    d.AppendSoftReturn();
                                    Description closureDescription = c.Description;
                                    closureDescription.ApplyReplaceValues(castResult);
                                    closureDescription.ApplyResult(castResult, c.Key);
                                    d.Join(closureDescription);
                                }
        
                            d.ApplyStyle(castResult, pd);
                        })
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "Skill05_018",
                name:                       "疯魔",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Attack,
                cost:                       ChannelCostDefinition.FromValue(3),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(50),
                    new GainBuffProcedureDefinition("跳走步")
                        .SetPostCondDefinition(PostCondDefinition.IsEnd),
                }),
            
            #endregion

            #region 06无色卡包

            new(id:                         "Skill06_001",
                name:                       "硬化蛊",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new RemoveHealthProcedureDefinition(10 + 4 * dj),
                    new GiveArmorProcedureDefinition(10),
                    new DepleteProcedureDefinition(),
                }),

            new(id:                         "Skill06_002",
                name:                       "雷火弹",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(5 + dj))
                        .AddClosure(Shatter),
                    new DepleteProcedureDefinition(),
                }),

            new(id:                         "Skill06_003",
                name:                       "养气丹",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Mu, gain: Fib.ToValue(2 + dj)),
                    new DepleteProcedureDefinition(),
                }),
            
            new(id:                         "Skill06_004",
                name:                       "天雷丸",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:              TagCategory.Attack | TagCategory.Exhaust | TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(dj switch { 0 => 10, 1 => 25, 2 => 45, 3 => 70, _ => 100 }),
                    new ExhaustProcedureDefinition(),
                    new DepleteProcedureDefinition(),
                }),

            new(id:                         "Skill06_005",
                name:                       "同心蛊",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("同心蛊", stack: 1 + dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}次受治疗时，对敌方造成等量伤害")),
                    new DepleteProcedureDefinition(),
                }),

            new(id:                         "Skill06_006",
                name:                       "七彩蛊",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("腐朽", 20 - 5 * dj),
                    new GainBuffProcedureDefinition("暴击", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"下次攻击具有暴击/吸血/穿透")),
                    new GainBuffProcedureDefinition("吸血", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"")),
                    new GainBuffProcedureDefinition("穿透", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"")),
                    new DepleteProcedureDefinition(),
                }),
            
            new(id:                         "Skill06_007",
                name:                       "破境丹",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Swift | TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetActionPointProcedureDefinition(2)
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    new SetActionPointProcedureDefinition(3)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new ExhaustProcedureDefinition()
                        .SetDescription((description, procedureDefinition, costResult, castResult) => { }),
                    new GainBuffProcedureDefinition("升华", stack: 1)
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing)
                        .SetDescription((description, procedureDefinition, costResult, castResult) => description.Join("升华两次")),
                    new GainBuffProcedureDefinition("升华", stack: 2)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen)
                        .SetDescription((description, procedureDefinition, costResult, castResult) => description.Join("升华三次")),
                    new DepleteProcedureDefinition(),
                }),

            new(id:                         "Skill06_008",
                name:                       "小雷劫",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Deplete,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            float ratio = d.Skill.GetJingJie() <= JingJie.YuanYing ? 0.25f : 0.333f;
                    
                            int selfValue = (int)(d.Caster.MaxHp * ratio);
                            int oppoValue = (int)(d.Caster.Opponent().MaxHp * ratio);
                    
                            await d.Caster.LoseHealthProcedure(selfValue, causedByAttack: false, induced: false);
                            await d.Caster.Opponent().LoseHealthProcedure(oppoValue, causedByAttack: false, induced: false);
                        })
                        .SetPostCondDefinition(PostCondDefinition.StartStage)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join(j <= JingJie.YuanYing ? 
                            "开局：双方失去1/4气血" : 
                            "开局：双方失去1/3气血")),
                    new DepleteProcedureDefinition(),
                }),

            new(id:                         "Skill06_009",
                name:                       "补天丹",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.HuaShen2FanXu,
                overridingMergeRule:        MergeRule.BuTianDanMergeRule,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult)
                        => d.Join($"合成：另外一张牌变成{(j <= JingJie.HuaShen ? "化神" : "返虚")}")),
                }),

            new(id:                         "Skill06_010",
                name:                       "缭乱",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(2 + 4 * dj),
                    new CycleProcedureDefinition(WuXing.Mu, gain: Fib.ToValue(2 + dj))
                        .SetPostCondDefinition(PostCondDefinition.FirstTime),
                }),
            
            new(id:                         "Skill06_011",
                name:                       "蜕变",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2FanXu,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Tu, gain: 1 + dj),
                    new LoseArmorProcedureDefinition(0)
                        .AddClosure(TuiBianClosure),
                }),
            
            new(id:                         "Skill06_012",
                name:                       "流霰",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2FanXu,
                tagComposite:               TagCategory.Attack | TagCategory.Defend,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("LiuXianConvert", (10 - j).ToString()),
                    new AttackProcedureDefinition(6 + 3 * j)
                        .AddClosure(LiuXianClosure),
                }),

            new(id:                         "Skill06_013",
                name:                       "燎原",
                wuXing:                     WuXing.Huo,
                tagComposite:               TagCategory.Growth,
                jingJieBound:               JingJie.ZhuJi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("LiaoYuanGrow", 1.ToString())
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    new CycleProcedureDefinition(WuXing.Huo, gain: 1 + dj)
                        .AddClosure(LiaoYuanClosure)
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    
                    new TrySetValueProcedureDefinition("LiaoYuanGrow", 2.ToString())
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new CycleProcedureDefinition(WuXing.Huo, gain: 3)
                        .AddClosure(LiaoYuanClosure)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),

            new(id:                         "Skill06_014",
                name:                       "百草集",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.JinDan2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new FollowingCycleProcedureDefinition(gain: 1 + dj),
                }),
            
            new(id:                         "Skill06_015",
                name:                       "停云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: 1 + dj),
                    new CycleProcedureDefinition(WuXing.Jin, gain: 1 + dj)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "Skill06_016",
                name:                       "常仪",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("常仪")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"流转时：造成伤害")),
                    new FollowingCycleProcedureDefinition(),
                    new SetActionPointProcedureDefinition(2)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),

            new(id:                         "Skill06_017",
                name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2FanXu,
                tagComposite:               TagCategory.Mana,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: 1 + dj)
                        .AddClosure(NingShuiClosure)
                        .SetDescription((d, procedureDefinition, costResult, castResult) =>
                        {
                            CycleProcedureDefinition pd = procedureDefinition as CycleProcedureDefinition;
                            string elementaryBuffName = pd.WuXing.GetElementaryBuff().GetName();
                            d.Join($"{elementaryBuffName}+{pd.Gain}||每1{elementaryBuffName}，灵气+1");
                        }),
                }),
                
            new(id:                         "Skill06_018",
                name:                       "羲和",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.HuaShen2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new FollowingCycleProcedureDefinition(),
                    new GainBuffProcedureDefinition("羲和")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"使用牌时：流转对应五行")),
                }),
            
            #endregion

            #region 07墨染
            
            new(id:                         "Skill07_001",
                name:                       "守护墨染",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQiOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.ProtectMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：目标护甲/气血基础数值+8")),
                }),
            
            new(id:                         "Skill07_002",
                name:                       "破坏墨染",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQiOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.DestroyMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：目标攻击/破甲基础数值+5")),
                }),
            
            new(id:                         "Skill07_003",
                name:                       "净化墨染",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQiOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.RemoveDebuffMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：移除目标负面效果")),
                }),
            
            new(id:                         "Skill07_004",
                name:                       "燃命墨染",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQiOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.CostHealthMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：将消耗变成气血消耗，数值根据某种规则")),
                }),
            
            new(id:                         "Skill07_005",
                name:                       "流转墨染",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.ZhuJiOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.CycleMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult)
                        => d.Join($"合成：获得基础五行Buff时改为相生五行的Buff")),
                }),
            
            new(id:                         "Skill07_007",
                name:                       "灵气墨染",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJiOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.ManaMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：灵气消耗-2，灵气行为多2")),
                }),
            
            new(id:                         "Skill07_008",
                name:                       "暴击墨染",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJiOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.CritMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：目标变得可以暴击")),
                }),
            
            new(id:                         "Skill07_009",
                name:                       "条件墨染",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDanOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.RemoveCondMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：移除目标的条件||不是所有条件都可以移除")),
                }),
            
            new(id:                         "Skill07_010",
                name:                       "穿透墨染",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDanOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.PenetrateMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：目标变得可以穿透")),
                }),
            
            new(id:                         "Skill07_011",
                name:                       "升华墨染",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDanOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.ExhaustMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：使目标使用后升华")),
                }),
            
            new(id:                         "Skill07_012",
                name:                       "二动墨染",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDanOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.SwiftMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：目标变得可以二动")),
                }),
            
            new(id:                         "Skill07_013",
                name:                       "开局墨染",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYingOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.StartStageMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：开局使用一次")),
                }),
            
            new(id:                         "Skill07_014",
                name:                       "吸血墨染",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYingOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.LifeStealMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：目标变得可以吸血")),
                }),
            
            new(id:                         "Skill07_015",
                name:                       "汇聚墨染",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.YuanYingOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.AccumulateMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：获得Buff的数值+1")),
                }),
            
            new(id:                         "Skill07_016",
                name:                       "吟唱墨染",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYingOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.CostChannelMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：吟唱消耗-1")),
                }),
            
            new(id:                         "Skill07_017",
                name:                       "禁忌墨染",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShenOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.RemoveForbiddenMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：移除一个禁忌词条")),
                }),
            
            new(id:                         "Skill07_018",
                name:                       "狂战墨染",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                mutate:                     (j, dj) => new MutateDefinition[]
                {
                    MutateDefinition.BerserkMutate,
                },
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：攻击次数+1")),
                }),

            #endregion
            
            #region 00特殊 & 角色专属
            
            new(id:                         "Skill00_001",
                name:                       "卡池已空",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQiOnly,
                overridingMergeRule:        MergeRule.NoMerge,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"卡池已空")),
                }),

            new(id:                         "Skill00_002",
                name:                       "聚气术",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQiOnly,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气"),
                }),

            new(id:                         "Skill00_003",
                name:                       "灵气匮乏",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQiOnly,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气"),
                }),

            new(id:                         "Skill00_004",
                name:                       "幻化",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) =>
                    {
                        if (j == JingJie.FanXu)
                            d.Join($"模仿对手对位的牌||境界为返虚");
                        else
                            d.Join($"模仿对手对位的牌||境界为两者较低||低于初始境界时失效");
                    }),
                }),

            new(id:                         "Skill00_005",
                name:                       "伺机而动",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQiOnly,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"正在准备着")),
                }),

            new(id:                         "Skill00_006",
                name:                       "命石",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.HuaShenOnly,
                overridingMergeRule:        MergeRule.MingShiMergeRule,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"能感受到生命之力")),
                }),

            new(id:                         "Skill00_007",
                name:                       "小零食",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.JinDan2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("力量", 1 + dj),
                    new AttackProcedureDefinition(2),
                }),

            new(id:                         "Skill00_008",
                name:                       "长生不老药",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.HuaShen2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            d.Caster.TraversalBuffs().Do(b =>
                            {
                                if (b.GetEntry().BuffStackRule != BuffStackRule.One)
                                    d.Caster.GainBuffProcedure(b.GetEntry(), 1 + dj);
                            });
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) =>
                        {
                            d.Join($"所有Buff获得{1 + dj}层");
                        }),
                }),

            new(id:                         "Skill00_009",
                name:                       "斩断",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2FanXu,
                overridingMergeRule:        MergeRule.ZhanDuanMergeRule,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：将目标从牌山移除")),
                }),

            new(id:                         "Skill00_010",
                name:                       "千象",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避", stack: j == JingJie.FanXu ? 2 : 1),
                    new AttackProcedureDefinition(6 + 2 * dj)
                        .AddClosure(QianXiangClosure),
                }),

            new(id:                         "Skill00_011",
                name:                       "天机",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DescriptionProcedureDefinition((d, procedureDefinition, costResult, castResult) => d.Join($"合成：")),
                }),

            #endregion

            #region 08事件

            new(id:                         "Skill08_001",
                name:                       "遗憾",
                wuXing:                     WuXing.Mu, 
                jingJieBound:               JingJie.LianQi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避"),
                }),

            new(id:                         "Skill08_002",
                name:                       "爱恋",
                wuXing:                     WuXing.Mu, 
                jingJieBound:               JingJie.LianQi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            d.Caster.Skills.Do(skill => skill.IncreaseBonusCastedCount());
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"所有牌成长一次")),
                }),

            new(id:                         "Skill08_003",
                name:                       "春雨",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2FanXu,
                tagComposite:               TagCategory.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GiveBuffProcedureDefinition("跳行动", 2)
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            new(id:                         "Skill08_004",
                name:                       "枯木",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("腐朽", 5),
                    new GiveBuffProcedureDefinition("腐朽", 5),
                }),

            new(id:                         "Skill08_005",
                name:                       "须臾",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2FanXu,
                tagComposite:               TagCategory.Exhaust,
                cost:                       ChannelCostDefinition.FromValue(5),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            foreach (StageSkill skill in d.Caster.Skills)
                            {
                                if (skill.GetTagComposite().Contains(TagCategory.Exhaust))
                                    await skill.ExhaustProcedure();
                            }
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"升华带有升华描述的牌")),
                }),

            new(id:                         "Skill08_006",
                name:                       "永远",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            StageSkill toCast = d.Skill.PrevSkill(true);
                            await d.Caster.CastProcedure(toCast);
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"使用上一张牌\n即使已经升华")),
                }),

            new(id:                         "Skill08_007",
                name:                       "童趣",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("童趣", stack: 1 + dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"复制对手下{(procedureDefinition as GainBuffProcedureDefinition).Stack}次获得的增益")),
                }),

            new(id:                         "Skill08_008",
                name:                       "一心",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("一心", stack: 1 + dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}次吟唱无需消耗")),
                }),

            new(id:                         "Skill08_009",
                name:                       "玄武吐息法",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShen2FanXu,
                tagComposite:               TagCategory.Health | TagCategory.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new DirectProcedureDefinition(async d =>
                        {
                            int gap = d.Caster.MaxHp - d.Caster.Hp;
                            await d.Caster.HealProcedure(gap);
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"气血回复至上限")),
                    new GainBuffProcedureDefinition("玄武吐息法", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"治疗可以穿上限")),
                }),
            
            new(id:                         "Skill08_010",
                name:                       "观棋烂柯",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShen2FanXu,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GiveBuffProcedureDefinition("跳行动"),
                }),

            #endregion
            
            #region 09教程
            
            new(id:                         "Skill09_001",
                name:                       "冲撞",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(3 + dj),
                }),
            
            new(id:                         "Skill09_002",
                name:                       "劈砍",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(4 + 2 * dj),
                }),
            
            new(id:                         "Skill09_003",
                name:                       "冰弹",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQiOnly,
                tagComposite:               TagCategory.Attack,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(8 + dj, wuXing: WuXing.Shui),
                }),
            
            new(id:                         "Skill09_004",
                name:                       "不演了",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.HuaShenOnly,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(100),
                }),
            
            new(id:                         "Skill09_005",
                name:                       "蜕变",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Defend,
                overridingMergeRule:        MergeRule.DreamCard,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Tu, gain: 1 + dj),
                    new LoseArmorProcedureDefinition(0)
                        .AddClosure(TuiBianClosure),
                }),
            
            new(id:                         "Skill09_006",
                name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                tagComposite:               TagCategory.Mana,
                overridingMergeRule:        MergeRule.DreamCard,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: 1 + dj)
                        .AddClosure(NingShuiClosure)
                        .SetDescription((d, procedureDefinition, costResult, castResult) =>
                        {
                            CycleProcedureDefinition pd = procedureDefinition as CycleProcedureDefinition;
                            string elementaryBuffName = pd.WuXing.GetElementaryBuff().GetName();
                            d.Join($"{elementaryBuffName}+{pd.Gain}||每1{elementaryBuffName}，灵气+1");
                        }),
                }),
            
            new(id:                         "Skill09_007",
                name:                       "流霰",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                tagComposite:               TagCategory.Attack | TagCategory.Defend,
                overridingMergeRule:        MergeRule.DreamCard,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("LiuXianConvert", (10 - j).ToString()),
                    new AttackProcedureDefinition(6 + 3 * j)
                        .AddClosure(LiuXianClosure),
                }),
            
            new(id:                         "Skill09_008",
                name:                       "养气丹",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Deplete,
                overridingMergeRule:        MergeRule.DreamCard,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Mu, gain: Fib.ToValue(2 + dj)),
                    new DepleteProcedureDefinition(),
                }),
            
            new(id:                         "Skill09_009",
                name:                       "燎原",
                wuXing:                     WuXing.Huo,
                tagComposite:               TagCategory.Growth,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                overridingMergeRule:        MergeRule.DreamCard,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("LiaoYuanGrow", 1.ToString())
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    new CycleProcedureDefinition(WuXing.Huo, gain: 1 + dj)
                        .AddClosure(LiaoYuanClosure)
                        .SetPreCondDefinition(PreCondDefinition.LeYuanYing),
                    
                    new TrySetValueProcedureDefinition("LiaoYuanGrow", 2.ToString())
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                    new CycleProcedureDefinition(WuXing.Huo, gain: 3)
                        .AddClosure(LiaoYuanClosure)
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),
            
            new(id:                         "Skill09_010",
                name:                       "冲击",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(3 + dj),
                }),
            
            new(id:                         "Skill09_011",
                name:                       "劈斩",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(4 + 2 * dj),
                }),

            #endregion
            
            #region 怪物专属
            
            // 3 5 8 13 21
            new(id:                         "Skill1001",
                name:                       "打击",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(4 + dj)),
                }),
            
            // 3 5 8 13 21
            new(id:                         "Skill1002",
                name:                       "防卫",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(Fib.ToValue(4 + dj)),
                }),
            
            // 3 5 8 13 21
            new(id:                         "Skill1003",
                name:                       "聚灵",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", stack: 1 + (dj / 2)),
                }),
            
            // 3 5 8 13 21
            new(id:                         "Skill1004",
                name:                       "调息",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new HealProcedureDefinition(Fib.ToValue(4 + dj)),
                }),
            
            // 3 5 8 13 21
            new(id:                         "Skill1005",
                name:                       "高速",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new SetActionPointProcedureDefinition(j <= JingJie.YuanYing ? 2 : 3),
                }),
            
            // 3 5 8 13 21
            new(id:                         "Skill1006",
                name:                       "啃咬",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new RemoveArmorProcedureDefinition(Fib.ToValue(4 + dj)),
                }),

            // 3 5 8 13 21
            new(id:                         "Skill1007",
                name:                       "切割",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(Fib.ToValue(4 + dj))
                        .AddClosure(Crit),
                }),
            new(id:                         "Skill1008",
                name:                       "看穿",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       ChannelCostDefinition.FromDj(dj => 5 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("穿透"),
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1104",
                name:                       "剑芒",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: 1 << (Mathf.Max(dj - 1, 0))),
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1206",
                name:                       "爱睡",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       ChannelCostDefinition.FromDj(dj => 5 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            int gap = d.Caster.MaxHp - d.Caster.Hp;
                            await d.Caster.HealProcedure(gap);
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"气血回复至上限")),
                }),
            
            new(id:                         "Skill1301",
                name:                       "木刺",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       ChannelCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(10 + 10 * dj)
                        .AddClosure(Penetrate),
                }),

            new(id:                         "Skill1302",
                name:                       "滑水",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("闪避", 1 + dj),
                }),

            new(id:                         "Skill1303",
                name:                       "驱藤",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("坚毅", 2 + dj, induced: false),
                    new GiveBuffProcedureDefinition("坚毅", 2 + dj, induced: true),
                }),
            
            new(id:                         "Skill1304",
                name:                       "花海",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       ChannelCostDefinition.FromDj(dj => 5 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GainBuffProcedureDefinition("花海")
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"每回合力量+{(procedureDefinition as GainBuffProcedureDefinition).Stack}")),
                }),

            new(id:                         "Skill1310",
                name:                       "小春",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Mu, gain: 1),
                    new AttackProcedureDefinition(Fib.ToValue(3 + dj)),
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1103",
                name:                       "腐蚀",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(5),
                    new RemoveArmorProcedureDefinition(0, induced: true)
                        .AddClosure(FuShiClosure),
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1105",
                name:                       "祥瑞",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("二动")
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                    new GainBuffProcedureDefinition("多重")
                        .SetPostCondDefinition(PostCondDefinition.StartStage),
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1106",
                name:                       "万千光辉",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: 1),
                    new TrySetValueProcedureDefinition("WanQianGuangHuiConvert", (2 + dj).ToString()),
                    new AttackProcedureDefinition(0)
                        .AddClosure(WanQianGuangHuiClosure),
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1107",
                name:                       "恶意",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("恶意", 1 + dj)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}次施加破甲时将会造成气血流失")),
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1108",
                name:                       "岁月",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("SuiYueGain", (3 + 2 * dj).ToString()),
                    new AttackProcedureDefinition(10 + 10 * dj)
                        .AddClosure(SuiYueClosure),
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1109",
                name:                       "恶灵招徕",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"若敌方有腐朽，施加10破甲".ApplyStyle(castResult, "0") +
                    $"\n否则，将破甲转为腐朽".ApplyStyle(castResult, "1"),
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
            new(id:                         "Skill1201",
                name:                       "浪击",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{4 + 4 * dj}攻" +
                    $"\n击伤：灵气+{2 + dj}".ApplyCond(castResult),
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
            new(id:                         "Skill1202",
                name:                       "惊涛",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"4攻" +
                    $"\n爆能{10 + 2 * dj}：多{4 + 2 * dj}攻，多{1 + dj}次，吸血".ApplyCond(castResult),
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
            new(id:                         "Skill1203",
                name:                       "放血",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加4内伤",
                castGenerator:              async d =>
                {
                    await d.GiveBuffProcedure("内伤", 4);
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1204",
                name:                       "高速冲撞",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"二动" +
                    $"\n每1格挡，造成{1 + dj}伤害",
                castGenerator:              async d =>
                {
                    int value = d.Caster.GetStackOfBuff("格挡") * d.Dj;
                    await d.AttackProcedure(value);
                    d.Caster.SetActionPoint(2);
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1205",
                name:                       "逍遥游",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"格挡减半" +
                    $"\n格挡+{2 + 2 * dj}",
                castGenerator:              async d =>
                {
                    int value = d.Caster.GetStackOfBuff("格挡") / 2;
                    await d.LoseBuffProcedure("格挡", value);
                    await d.GainBuffProcedure("格挡", 2 + 2 * d.Dj);
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1207",
                name:                       "幻雾",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       (j, dj) => new ChannelCostDefinition(4, closures: new []{ HuanWuClosure }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(30)
                        .AddClosure(LifeSteal),
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1208",
                name:                       "逝者如斯",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"十二动 升华",
                castGenerator:              async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    d.Caster.SetActionPoint(12);
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1209",
                name:                       "月华清辉",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"敌方失去所有灵气",
                castGenerator:              async d =>
                {
                    int value = d.Caster.Opponent().GetStackOfBuff("灵气");
                    await d.RemoveBuffProcedure("灵气", value, induced: false);
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1305",
                name:                       "啄击",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"1攻" +
                    $"\n成长：多1次",
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(1, times: 1 + d.Skill.TotalStageCastedCount);
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1306",
                name:                       "娑婆双树",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"将左边牌的成长次数给右边牌",
                castGenerator:              async d =>
                {
                    StageSkill leftSkill = d.Skill.PrevSkill(false);
                    StageSkill rightSkill = d.Skill.NextSkill(false);
                    if (leftSkill == null || rightSkill == null)
                        return;

                    rightSkill.SetRealStageCastedCount(leftSkill.TotalStageCastedCount + rightSkill.TotalStageCastedCount);
                    leftSkill.SetRealStageCastedCount(0);
                    // animation
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1307",
                name:                       "灵虚步",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       ManaCostDefinition.FromValue(3),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"闪避+3" +
                    $"\n成功闪避时：双发+1",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("闪避", 3);
                    await d.GainBuffProcedure("灵虚步", induced: true);
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1308",
                name:                       "灵犀剑",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灵气+3" +
                    $"\n1攻 每1灵气，多{1 + dj}",
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
            new(id:                         "Skill1309",
                name:                       "他心通",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"下一次对方获得增益时：自己也获得",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("他心通");
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1401",
                name:                       "吞炎",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"成为残血" +
                    $"\n护甲+{10 + 20 * dj}",
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(10 + 20 * d.Dj, induced: false);
                    await d.BecomeLowHealth(induced: true);
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1402",
                name:                       "焚天",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻" +
                    $"\n残血：多{10 + 10 * dj}攻".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    int value = (10 + 10 * d.Dj) + (d.Caster.IsLowHealth ? (10 + 10 * d.Dj) : 0);
                    await d.AttackProcedure(value);
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1403",
                name:                       "山火",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻 每携带1张火，多1次",
                castGenerator:              async d =>
                {
                    int value = d.Caster.Skills.Count(s => s.Entry.WuXing == WuXing.Huo);
                    await d.AttackProcedure(3 + d.Dj, times: 1 + value);
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1404",
                name:                       "天劫火",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Health,
                cost:                       HealthCostDefinition.FromDj(dj => 4 + 4 * dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"灼烧+{1 + dj}",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("灼烧", 1 + d.Dj);
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1405",
                name:                       "火墙",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"若在下次使用前，没有遭受{1 + dj}次伤害".ApplyCond(castResult) +
                    $"\n50攻".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    int stack = d.Caster.GetStackOfBuff("火墙");
                    bool cond = stack > 0;
                    d.CastResult.AppendCond(cond);

                    if (cond)
                    {
                        await d.RemoveBuffProcedure("火墙", stack);
                        await d.AttackProcedure(50);
                    }

                    await d.Caster.GainBuffProcedure("火墙", 1 + d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1406",
                name:                       "献祭",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华所有卡牌",
                castGenerator:              async d =>
                {
                    await d.Caster.Skills.Do(async s => await s.ExhaustProcedure());
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1407",
                name:                       "须弥",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+{10 + 10 * dj}" +
                    $"\n二动 升华",
                castGenerator:              async d =>
                {
                    await d.GainArmorProcedure(10 + 10 * d.Dj, induced: false);
                    d.Caster.SetActionPoint(2);
                    await d.Skill.ExhaustProcedure();
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1408",
                name:                       "仙人抚顶",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       ChannelCostDefinition.FromDj(dj => 4 - dj),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"使用3次后：将对方气血变为0".ApplyCond(castResult),
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
            new(id:                         "Skill1409",
                name:                       "消愁",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"下一张牌取消升华",
                castGenerator:              async d =>
                {
                    StageSkill s = d.Skill.NextSkill(loop: false);
                    s.Exhausted = false;
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1501",
                name:                       "滚石",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n有护甲：多5攻".ApplyCond(castResult),
                castGenerator:              async d =>
                {
                    bool cond = d.Caster.Armor > 0;
                    d.CastResult.AppendCond(cond);

                    int value = 10 + (cond ? 1 : 0) * 5;
                    await d.AttackProcedure(value);
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1502",
                name:                       "土墙",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       ChannelCostDefinition.FromValue(3),
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"护甲+10" +
                    $"\n每有{6 - dj}气血，多1",
                castGenerator:              async d =>
                {
                    int value = d.Caster.Hp / (6 - d.Dj) + 10;
                    await d.GainArmorProcedure(value, induced: false);
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1503",
                name:                       "风魔",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n遭受1跳走步",
                castGenerator:              async d =>
                {
                    await d.AttackProcedure(10);
                    await d.GainBuffProcedure("跳走步");
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1504",
                name:                       "震地",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n击伤：对手灵气-2".ApplyCond(castResult),
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
            new(id:                         "Skill1505",
                name:                       "硬化",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"坚毅+2" +
                    $"\n失去所有护甲",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("坚毅", 2);
                    int value = d.Caster.Armor;
                    if (value > 0)
                        await d.LoseArmorProcedure(value, induced: true);
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1506",
                name:                       "惊吓",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加4滞气",
                castGenerator:              async d =>
                {
                    await d.GiveBuffProcedure("滞气", 4);
                }),
            
            // 6 12 26 52 102
            new(id:                         "Skill1507",
                name:                       "铁布衫",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"升华\n受到伤害时：最多20",
                castGenerator:              async d =>
                {
                    await d.GainBuffProcedure("伤害上限", 20);
                    await d.Skill.ExhaustProcedure();
                }),

            // 6 12 26 52 102
            new(id:                         "Skill1508",
                name:                       "钢拳",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       (j, dj) => new ChannelCostDefinition(4, closures: new []{ GangQuanClosure }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(40 + 40 * dj),
                }),

            // 8 24 52 105 204
            new(id:                         "Skill1509",
                name:                       "天人五衰",
                wuXing:                     WuXing.Wu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                descriptionGenerator:       (j, dj, costResult, castResult) =>
                    $"施加滞气，缠绕，软弱，腐朽，内伤各5层",
                castGenerator:              async d =>
                {
                    BuffEntry[] buffs = Encyclopedia.BuffCategory.GetDebuffs();
                    for (int i = 0; i < buffs.Length; i++)
                        await d.GiveBuffProcedure(buffs[i], 5, induced: i != 0);
                }),

            #endregion
            
            #region 待选池子

            new(id:                         "Skill01_007",
                name:                       "白刃",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                tagComposite:               TagCategory.Attack | TagCategory.Swift,
                cost:                       ManaCostDefinition.FromValue(2),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(10 + 8 * dj)
                        .AddClosure(BaiRenClosure),
                }),
            
            new(id:                         "Skill01_015",
                name:                       "刺穴",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                tagComposite:               TagCategory.Mana | TagCategory.Swift,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 4),
                    new GainBuffProcedureDefinition("滞气", 4 - dj),
                    new SetActionPointProcedureDefinition(2),
                }),
            
            new(id:                         "Skill02_004",
                name:                       "调和",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Mana | TagCategory.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("TiaoHeGain", (1 + 4 * dj).ToString()),
                    new GainBuffProcedureDefinition("灵气", stack: 1),
                    new HealProcedureDefinition(1 + 4 * dj)
                        .AddClosure(TiaoHeClosure),
                }),
            
            new(id:                         "Skill03_012",
                name:                       "生机",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new TrySetValueProcedureDefinition("ShengJiGrow", Fib.ToValue(2 + dj).ToString()),
                    new HealProcedureDefinition(2 + 4 * dj)
                        .AddClosure(ShengJiClosure),
                }),
            
            new(id:                         "Skill03_013",
                name:                       "清泉",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Mana,
                cost:                       (j, dj) => new ChannelCostDefinition(3, closures: new []{ QingQuanClosure }),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 4 + 2 * dj),
                }),

            new(id:                         "Skill04_002",
                name:                       "正念",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Defend | TagCategory.Exhaust,
                cost:                       ChannelCostDefinition.FromDj(dj => 5 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GainArmorProcedureDefinition(10 + 10 * dj),
                }),
            
            new(id:                         "Skill04_004",
                name:                       "战意",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("剑意", 2 + dj),
                    new GainArmorProcedureDefinition(0, induced: true)
                        .AddClosure(ZhanYiClosure),
                }),

            new(id:                         "Skill04_008",
                name:                       "登宝塔",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                tagComposite:               TagCategory.Exhaust,
                cost:                       ChannelCostDefinition.FromDj(dj => 1 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new ExhaustProcedureDefinition(),
                    new GainBuffProcedureDefinition("升华")
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}张牌具有升华")),
                }),

            new(id:                         "Skill04_017",
                name:                       "观众生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                tagComposite:               TagCategory.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            for (int i = 0; i < 1 + d.Dj; i++)
                            {
                                StageSkill skill = d.Caster.PrevSkills(d.Caster._p, loop: false)
                                    .FirstObj(skill => !skill.Exhausted) ?? d.Caster.Skills[d.Caster._p];
                                await skill.ExhaustProcedure();
                            }
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"升华左边牌" + (j == JingJie.HuaShen ? "，两次" : ""))),
                }),

            new(id:                         "Skill04_007",
                name:                       "常夏",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                tagComposite:               TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Huo, gain: 1),
                    new GainBuffProcedureDefinition("剑意", 0, induced: true)
                        .AddClosure(ChangMingClosure),
                    new GainBuffProcedureDefinition("保留剑意", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"下{(procedureDefinition as GainBuffProcedureDefinition).Stack}次攻击保留剑意"))
                        .SetPreCondDefinition(PreCondDefinition.GeHuaShen),
                }),
            
            new(id:                         "Skill0419",
                name:                       "净天地",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                tagComposite:               TagCategory.Exhaust,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new DirectProcedureDefinition(async d =>
                        {
                            if (!d.Recursive)
                                return;

                            if (d.Skill.Exhausted)
                                return;

                            await d.Skill.ExhaustProcedure();
                    
                            foreach (StageSkill s in d.Caster.Skills)
                            {
                                if (!s.Exhausted)
                                    continue;
                                await d.Caster.CastProcedure(s, recursive: false);
                            }
                        })
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"升华\n使用所有已升华牌")),
                }),
            
            new(id:                         "Skill0605",
                name:                       "射落金乌",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(5, times: 4),
                }),

            new(id:                         "Skill0609",
                name:                       "毒性",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GiveBuffProcedureDefinition("内伤", 3),
                }),
            
            new(id:                         "Skill0522",
                name:                       "活步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Defend,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainArmorProcedureDefinition(Fib.ToValue(5 + dj), induced: false),
                    new GainBuffProcedureDefinition("延迟护甲", stack: Fib.ToValue(5 + dj), induced: true),
                }),

            new(id:                         "Skill0530",
                name:                       "架势",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       ChannelCostDefinition.FromDj(dj => 1 - dj),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("终结")
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join("激活下1次终结\n终结：2次"))
                        .AddClosure(JiaShiClosure),
                }),

            new(id:                         "Skill0536",
                name:                       "无畏",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Tu, gain: 1 + dj),
                    new DispelProcedureDefinition(0)
                        .AddClosure(WuWeiClosure),
                }),

            // new(id:                         "Skill_DTSZ_002",
            //     name:                       "锻骨",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         TagCategory.Health,
            //     cost:                       ChannelCostDefinition.FromValue(1),
            //     cast:                       (j, dj) => new ProcedureDefinition[]
            //     {
            //         // 满血/残血：
            //         new HealProcedureDefinition(6 + dj),
            //         new GainBuffProcedureDefinition("锻体", 3 + (dj / 2), induced: true),
            //         new GainBuffProcedureDefinition("锻体", 3 + (dj / 2), induced: true)
            //             .SetPreCondDefinition(PreCondDefinition.GeZhuJi)
            //             .SetPostCondDefinition(PostCondDefinition.FullHealth),
            //         new GainBuffProcedureDefinition("锻体", 3 + (dj / 2), induced: true)
            //             .SetPreCondDefinition(PreCondDefinition.GeYuanYing)
            //             .SetPostCondDefinition(PostCondDefinition.LowHealth),
            //     }),

            new(id:                         "Skill_DTSZ_005",
                name:                       "锻髓",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                tagComposite:               TagCategory.Health,
                cost:                       ChannelCostDefinition.FromValue(1),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("锻体", 6 + dj),
                    new TrySetValueProcedureDefinition("DuanSuiConvert", (1 + dj).ToString()),
                    new HealProcedureDefinition(0)
                        .AddClosure(DuanSuiClosure),
                    // new LoseExtraMaxHealthProcedureDefinition(),
                }),

            new(id:                         "Skill0512",
                name:                       "箭疾步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                tagComposite:               TagCategory.Mana | TagCategory.Defend,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new GainBuffProcedureDefinition("灵气", 2 + dj),
                    new TrySetValueProcedureDefinition("JianJiBuConvert", (1 + dj).ToString()),
                    new GainArmorProcedureDefinition(0)
                        .AddClosure(JianJiBuClosure),
                }),
            
            new(id:                         "Skill0111",
                name:                       "凛冽",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                tagComposite:               TagCategory.Attack | TagCategory.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new CycleProcedureDefinition(WuXing.Jin, gain: 2),
                    new GainBuffProcedureDefinition("凛冽", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"锋锐具有吸血")),
                    new GainBuffProcedureDefinition("禁止攻击", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"禁止攻击")),
                }),
            
            new(id:                         "Skill0525",
                name:                       "金刚不坏",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                tagComposite:               TagCategory.Health,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new HealProcedureDefinition(40),
                    new GainBuffProcedureDefinition("伤害上限", stack: 40, induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult)
                            => d.Join($"至多受到{(procedureDefinition as GainBuffProcedureDefinition).Stack}伤害")),
                    new GainBuffProcedureDefinition("禁止护甲", induced: true)
                        .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"无法获得护甲")),
                }),
            
            new(id:                         "Skill0509",
                name:                       "正域彼四方",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                tagComposite:               TagCategory.Attack,
                cost:                       ManaCostDefinition.FromValue(8),
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(40 + 20 * dj),
                }),
            
            new(id:                         "Skill0421",
                name:                       "多段测试",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                tagComposite:               TagCategory.Attack,
                cast:                       (j, dj) => new ProcedureDefinition[]
                {
                    new AttackProcedureDefinition(1)
                        .AddClosure(DuoDuanCeShiClosure),
                }),

            #endregion

            #region 注释

            // new(id:                         "Skill0114",
            //     name:                       "素弦",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack | TagCategory.Mana,
            //     cost:                       ChannelCostDefinition.FromValue(1),
            //     descriptionGenerator:       (j, dj, costResult, castResult) =>
            //         $"2攻".ApplyAttack() +
            //         $"\n灵气+3".ApplyMana() +
            //         $"\n下{1 + dj}次攻击也触发",
            //     castGenerator:              async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 AttackDetails d = closureDetails as AttackDetails;
            //                 if (listener != d.Listener) return;
            //                 StageSkill initiator = d.Listener as StageSkill;
            //                 await d.Src.GainBuffProcedure("灵气", 3);
            //                 await d.Src.GainBuffProcedure("素弦", 1 + initiator.Dj);
            //             });
            //
            //         await d.AttackProcedure(2,
            //             closures: new [] { closure });
            //     }),
            //
            // new(id:                         "Skill0214",
            //     name:                       "苦寒",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack | TagCategory.Swift,
            //     cost:                       ManaCostDefinition.FromValue(8),
            //     descriptionGenerator:       (j, dj, costResult, castResult) =>
            //         $"2攻".ApplyAttack() +
            //         $"\n二动+1" +
            //         $"\n下{2 + dj}次攻击也触发",
            //     castGenerator:              async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 AttackDetails d = closureDetails as AttackDetails;
            //                 if (listener != d.Listener) return;
            //                 StageSkill initiator = d.Listener as StageSkill;
            //                 if (d.Src.GetActionPoint() < 2)
            //                     d.Src.SetActionPoint(2);
            //                 else
            //                     await d.Src.GainBuffProcedure("二动");
            //                 await d.Src.GainBuffProcedure("苦寒", 2 + initiator.Dj);
            //             });
            //
            //         await d.AttackProcedure(2,
            //             closures: new [] { closure });
            //     }),
            //
            // new(id:                         "Skill0313",
            //     name:                       "弱昙",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     descriptionGenerator:       (j, dj, costResult, castResult) =>
            //         $"2攻".ApplyAttack() +
            //         $"\n力量+1" +
            //         $"\n下{1 + dj}次攻击也触发",
            //     castGenerator:              async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 AttackDetails d = closureDetails as AttackDetails;
            //                 if (listener != d.Listener) return;
            //                 StageSkill initiator = d.Listener as StageSkill;
            //                 await d.Src.GainBuffProcedure("力量");
            //                 await d.Src.GainBuffProcedure("弱昙", 1 + initiator.Dj);
            //             });
            //
            //         await d.AttackProcedure(2,
            //             closures: new [] { closure });
            //     }),
            //
            // new(id:                         "Skill0414",
            //     name:                       "狂焰",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     descriptionGenerator:       (j, dj, costResult, castResult) =>
            //         $"2攻".ApplyAttack() +
            //         $"\n攻击多8攻".ApplyAttack() +
            //         $"\n下{1 + dj}次攻击也触发",
            //     castGenerator:              async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 AttackDetails d = closureDetails as AttackDetails;
            //                 if (listener != d.Listener) return;
            //                 d.Value += 8;
            //                 // await d.Src.GainBuffProcedure("狂焰", 1 + d.SrcSkill.Dj);
            //             });
            //         
            //         await d.AttackProcedure(2,
            //             closures: new [] { closure });
            //         
            //         await d.GainBuffProcedure("狂焰", 1 + d.Dj);
            //     }),
            //
            // new(id:                         "Skill0514",
            //     name:                       "孤山",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     descriptionGenerator:       (j, dj, costResult, castResult) =>
            //         (j < JingJie.HuaShen ? $"2攻".ApplyAttack() : "2攻x2".ApplyAttack()) +
            //         $"\n不消耗剑阵效果",
            //     castGenerator:              async d =>
            //     {
            //         StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
            //             async (listener, closure, closureDetails) =>
            //             {
            //                 AttackDetails d = closureDetails as AttackDetails;
            //                 if (listener != d.Listener) return;
            //                 StageSkill initiator = d.Listener as StageSkill;
            //                 BuffEntry[] buffs = new BuffEntry[] { "素弦", "苦寒", "弱昙", "狂焰" };
            //
            //                 bool cond = initiator.GetJingJie() < JingJie.HuaShen;
            //                 int times = cond ? 1 : 2;
            //                 foreach (BuffEntry b in buffs)
            //                     if (d.Src.GetStackOfBuff(b) > 0)
            //                         await d.Src.GainBuffProcedure(b, times, induced: true);
            //             });
            //         
            //         bool cond = d.J < JingJie.HuaShen;
            //         int times = cond ? 1 : 2;
            //         await d.AttackProcedure(2, times,
            //             closures: new [] { closure });
            //     }),
            //
            // new(id:                         "Skill0101",
            //     name:                       "乘风",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{5 + dj}攻\n" +
            //         $"若有锋锐：{3 + dj}攻".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         bool cond = caster.GetStackOfBuff("锋锐") > 0 || await caster.IsFocused();
            //         int add = cond ? 3 + skill.Dj : 0;
            //         await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
            //
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "Skill0104",
            //     name:                       "掠影",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"{5 + 2 * dj}攻".ApplyOdd(castResult) +
            //         $"/" +
            //         $"护甲+{5 + 2 * dj}".ApplyEven(castResult),
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
            // new(id:                         "Skill0107",
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
            // new(id:                         "Skill0120",
            //     name:                       "千里神行符",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         TagCategory.Exhaust | TagCategory.Swift,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"升华".ApplyOdd(castResult) +
            //         $"/" +
            //         $"二动".ApplyEven(castResult) +
            //         $"\n灵气+4",
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
            // new(id:                         "Skill0204",
            //     name:                       "归意",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{10 + 2 * dj}攻\n" +
            //         $"终结：吸血".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         bool cond = await skill.IsEnd(useFocus: true);
            //         await caster.AttackProcedure(10 + 2 * skill.Dj, lifeSteal: cond,
            //             wuXing: skill.Entry.WuXing);
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "Skill0207",
            //     name:                       "勤能补拙",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"护甲+{10 + 4 * dj}\n" +
            //         $"初次：遭受1跳行动".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(10 + 4 * skill.Dj, induced: false);
            //         bool cond = !await skill.IsFirstTime();
            //         if (!cond)
            //             await caster.GainBuffProcedure("跳行动");
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "Skill0314",
            //     name:                       "旧飞龙在天",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"升华" +
            //         $"\n每轮：闪避补至1",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("轮闪避");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0401",
            //     name:                       "化焰",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{4 + 2 * dj}攻\n" +
            //         $"灼烧+{1 + dj / 2}",
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
            //         await caster.GainBuffProcedure("灼烧", 1 + skill.Dj / 2);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0405",
            //     name:                       "聚火",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         TagCategory.Exhaust,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"升华" +
            //         $"\n灼烧+{2 + dj}",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0503",
            //     name:                       "地龙",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{7 + 2 * dj}攻\n" +
            //         $"击伤：护甲+{7 + 2 * dj}",
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
            // new(id:                         "Skill0505",
            //     name:                       "点星",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{8 + 2 * dj}攻\n" +
            //         $"相邻牌都非攻击：翻倍".ApplyStyle(castResult, "0") +
            //         $"\n" +
            //         $"消耗1灵气：翻倍".ApplyStyle(castResult, "1"),
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
            //
            // new(id:                         "Skill0121",
            //     name:                       "贪狼",
            //     wuXing:                     WuXing.Jin,
            //     skillTypeComposite:         TagCategory.Attack | TagCategory.Mana,
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
            // new(id:                         "Skill0210",
            //     name:                       "无念无想",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         TagCategory.Health,
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
            //
            // new(id:                         "Skill0311",
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
            // new(id:                         "Skill0305",
            //     name:                       "水滴石穿",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
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
            // new(id:                         "Skill0319",
            //     name:                       "刹那芳华",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         TagCategory.Defend | TagCategory.Exhaust | SkillType.ZiZhi,
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
            // new(id:                         "Skill0410",
            //     name:                       "坐忘",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         TagCategory.Exhaust | TagCategory.Defend,
            //     cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
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
            // new(id:                         "Skill0517",
            //     name:                       "一诺五岳",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         TagCategory.Attack,
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
            // new(id:                         "Skill0503",
            //     name:                       "点穴",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
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
            // new(id:                         "Skill0504",
            //     name:                       "流沙",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         TagCategory.Defend | TagCategory.Mana,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"护甲+{3 + 2 * dj}".ApplyDefend() +
            //         $"\n灵气+{1 + dj / 2}".ApplyMana(),
            //     cast:                       async d =>
            //     {
            //         await d.GainArmorProcedure(3 + 2 * d.Dj, induced: false);
            //         await d.GainBuffProcedure("灵气", 1 + d.Dj / 2);
            //     }),
            //
            // new(id:                         "Skill0516",
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
            // new(id:                         "Skill0511",
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
            //
            // new(id:                         "Skill0405",
            //     name:                       "炎爆",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         TagCategory.Attack,
            //     cost:                       ChannelCostDefinition.FromValue(5),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{9 + 2 * dj}攻x{9 + 2 * dj}".ApplyAttack() +
            //         $"\n禁止行动".ApplyCond(castResult),
            //     cast:                       async d =>
            //     {
            //         await d.AttackProcedure(9 + 2 * d.Dj, times: 9 + 2 * d.Dj);
            //         await d.GainBuffProcedure("禁止行动");
            //     },
            //     trivia: "用完之后就会体力耗尽动弹不得"),
            //
            // new(id:                         "Skill_DTSZ_006",
            //     name:                       "塑魂",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         TagCategory.Health | TagCategory.Mana,
            //     descriptionGenerator:       (j, dj, costResult, castResult) =>
            //         $"锻体+{Fib.ToValue(4 + dj)}" +
            //         $"\n持续：灵气不足时，可消耗3锻体代替1灵气",
            //     castGenerator:              async d =>
            //     {
            //         await d.GainBuffProcedure("锻体", Fib.ToValue(4 + d.Dj));
            //         await d.GainBuffProcedure("塑魂", 3, induced: true);
            //     }),
            //
            // new(id:                         "Skill_DTSZ_007",
            //     name:                       "天人五衰",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     descriptionGenerator:       (j, dj, costResult, castResult) =>
            //     {
            //         BuffEntry[] debuffs = new BuffEntry[] { "滞气", "缠绕", "软弱", "腐朽", "内伤", "脆弱" };
            //
            //         // int slotIndex = costResult?.Skill.SlotIndex ?? 0;
            //         int slotIndex = 0;
            //         slotIndex = slotIndex % debuffs.Length;
            //         BuffEntry debuff = debuffs[slotIndex];
            //         return $"幻影：遭受{3 + 2 * dj}{debuff.GetName().ApplyDebuff()}" +
            //                $"\n每1{debuff.GetName()}，锻体+1";
            //     },
            //     castGenerator:              async d =>
            //     {
            //         BuffEntry[] debuffs = new BuffEntry[] { "滞气", "缠绕", "软弱", "腐朽", "内伤", "脆弱" };
            //         
            //         int slotIndex = d.Skill.SlotIndex % debuffs.Length;
            //         BuffEntry debuff = debuffs[slotIndex];
            //
            //         await d.Caster.GainBuffProcedure(debuff, 3 + 2 * d.Dj);
            //         int value = d.Caster.GetStackOfBuff(debuff);
            //         await d.Caster.GainBuffProcedure("锻体", value);
            //     }),
            //
            // // 筑基
            //
            // new(id:                         "Skill0700", // 香
            //     name:                       "醒神香", // 香
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灵气+4",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灵气", 4);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0701", // 刃
            //     name:                       "飞镖", // 刃
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n12攻",
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(12);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0702", // 匣
            //     name:                       "铁匣", // 匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+12",
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(12, induced: false);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0703", // 轮
            //     name:                       "滑索", // 轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Swift | TagCategory.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n三动 升华",
            //     cast:                       async d =>
            //     {
            //         caster.SetActionPoint(3);
            //         await skill.ExhaustProcedure();
            //         return null;
            //     }),
            //
            // // 元婴
            //
            // new(id:                         "Skill0704", // 香香
            //     name:                       "还魂香", // 香香
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灵气+8",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灵气", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0705", // 香刃
            //     name:                       "净魂刀", // 香刃
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Mana | TagCategory.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n10攻\n击伤：灵气+1，对手灵气-1",
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
            // new(id:                         "Skill0706", // 香匣
            //     name:                       "防护罩", // 香匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+8\n每有1灵气，护甲+4",
            //     cast:                       async d =>
            //     {
            //         int add = caster.GetStackOfBuff("灵气");
            //         await caster.GainArmorProcedure(8 + add, induced: false);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0707", // 香轮
            //     name:                       "能量饮料", // 香轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下1次灵气减少时，加回",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灵气回收");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0708", // 刃刃
            //     name:                       "炎铳", // 刃刃
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n25攻",
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(25);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0709", // 刃匣
            //     name:                       "机关人偶", // 刃匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+12\n10攻",
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(12, induced: false);
            //         await caster.AttackProcedure(10);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0710", // 刃轮
            //     name:                       "铁陀螺", // 刃轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n2攻x6",
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(2, times: 6);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0711", // 匣匣
            //     name:                       "防壁", // 匣匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+20\n坚毅+2",
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(20, induced: false);
            //         await caster.GainBuffProcedure("坚毅", 2);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0712", // 匣轮
            //     name:                       "不倒翁", // 匣轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下2次护甲减少时，加回",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("护甲回收", 2);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0713", // 轮轮
            //     name:                       "助推器", // 轮轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Swift,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n二动 二重",
            //     cast:                       async d =>
            //     {
            //         caster.SetActionPoint(2);
            //         await caster.GainBuffProcedure("二重");
            //         return null;
            //     }),
            //
            // // 返虚
            //
            // new(id:                         "Skill0714", // 香香香
            //     name:                       "反应堆", // 香香香
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n遭受1不堪一击，永久二重+1",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("不堪一击");
            //         await caster.GainBuffProcedure("永久二重");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0715", // 香香刃
            //     name:                       "烟花", // 香香刃
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n消耗所有灵气，每1，力量+1",
            //     cast:                       async d =>
            //     {
            //         int stack = caster.GetStackOfBuff("灵气");
            //         await caster.TryConsumeProcedure("灵气", stack);
            //         await caster.GainBuffProcedure("力量", stack);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0716", // 香香匣
            //     name:                       "长明灯", // 香香匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n获得灵气时：每1，气血+3",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("长明灯", 3);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0717", // 香香轮
            //     name:                       "大往生香", // 香香轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust | TagCategory.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n永久免费+1",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("永久免费");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0718", // 缺少匣
            //     name:                       "地府通讯器", // 缺少匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n失去一半气血，每8，灵气+1",
            //     cast:                       async d =>
            //     {
            //         int gain = caster.Hp / 16;
            //         await caster.LoseHealthProcedure(gain * 8);
            //         await caster.GainBuffProcedure("灵气", gain);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0719", // 刃刃刃
            //     name:                       "无人机阵列", // 刃刃刃
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n没有效果",
            //     cast:                       async d =>
            //     {
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0720", // 刃刃香
            //     name:                       "弩炮", // 刃刃香
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Attack,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n50攻 吸血",
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(50, lifeSteal: true);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0721", // 刃刃匣
            //     name:                       "尖刺陷阱", // 刃刃匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下次受到攻击时，对对方施加等量破甲",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("尖刺陷阱");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0722", // 刃刃轮
            //     name:                       "暴雨梨花针", // 刃刃轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Attack,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n1攻x10",
            //     cast:                       async d =>
            //     {
            //         await caster.AttackProcedure(1, times: 10);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0723", // 缺少轮
            //     name:                       "炼丹炉", // 缺少轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n每回合力量+1",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("回合力量");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0724", // 匣匣匣
            //     name:                       "浮空艇", // 匣匣匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n回合被跳过时，该回合无法受到伤害\n遭受12跳行动",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("浮空艇");
            //         await caster.GainBuffProcedure("跳行动", 12);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0725", // 匣匣香
            //     name:                       "动量中和器", // 匣匣香
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n格挡+10",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("格挡", 10);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0726", // 匣匣刃
            //     name:                       "机关伞", // 匣匣刃
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灼烧+8",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("灼烧", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0727", // 匣匣轮
            //     name:                       "一轮马", // 匣匣轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n闪避+6",
            //     cast:                       async d =>
            //     {
            //         await caster.GainBuffProcedure("闪避", 6);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0728", // 缺少香
            //     name:                       "外骨骼", // 缺少香
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n攻击时，护甲+3",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("外骨骼", 3);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0729", // 轮轮轮
            //     name:                       "永动机", // 轮轮轮
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust | TagCategory.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n力量+8 灵气+8\n8回合后死亡",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("力量", 8);
            //         await caster.GainBuffProcedure("灵气", 8);
            //         await caster.GainBuffProcedure("永动机", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0730", // 轮轮香
            //     name:                       "火箭靴", // 轮轮香
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n使用灵气牌时，获得二动",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("火箭靴");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0731", // 轮轮刃
            //     name:                       "定龙桩", // 轮轮刃
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n对方二动时，如果没有暴击，获得1",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("定龙桩");
            //         return null;
            //     }),
            //
            // new(id:                         "Skill0732", // 轮轮匣
            //     name:                       "飞行器", // 轮轮匣
            //     wuXing:                     WuXing.Wu,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         TagCategory.Deplete | TagCategory.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n成功闪避时，如果对方没有跳行动，施加1",
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("飞行器");
            //         return null;
            //     }),

            #endregion
        });
    }

    public SkillEntry Default() => FromName("卡池已空");

    private SkillEntry[] _mutators;

    public SkillEntry[] Mutators
    {
        get
        {
            if (_mutators != null)
                return _mutators;

            _mutators = new SkillEntry[]
            {
                Encyclopedia.SkillCategory.FromId("Skill07_001"),
                Encyclopedia.SkillCategory.FromId("Skill07_002"),
                Encyclopedia.SkillCategory.FromId("Skill07_003"),
                Encyclopedia.SkillCategory.FromId("Skill07_004"),
                Encyclopedia.SkillCategory.FromId("Skill07_005"),
                // Encyclopedia.SkillCategory.FromId("Skill07_006"),
                Encyclopedia.SkillCategory.FromId("Skill07_007"),
                Encyclopedia.SkillCategory.FromId("Skill07_008"),
                Encyclopedia.SkillCategory.FromId("Skill07_009"),
                Encyclopedia.SkillCategory.FromId("Skill07_010"),
                Encyclopedia.SkillCategory.FromId("Skill07_011"),
                Encyclopedia.SkillCategory.FromId("Skill07_012"),
                Encyclopedia.SkillCategory.FromId("Skill07_013"),
                Encyclopedia.SkillCategory.FromId("Skill07_014"),
                Encyclopedia.SkillCategory.FromId("Skill07_015"),
                Encyclopedia.SkillCategory.FromId("Skill07_016"),
                Encyclopedia.SkillCategory.FromId("Skill07_017"),
                Encyclopedia.SkillCategory.FromId("Skill07_018"),
            };
            return _mutators;
        }
    }
}
