
using System;
using Cysharp.Threading.Tasks;
using CLLibrary;

public class CastDetails : StageClosureDetails
{
    public StageEnvironment Env;
    public StageEntity Caster;
    public StageSkill Skill;
    
    public bool Recursive;
    public bool FromWanJian;
    public bool IsStartStage;
    public int StartStageCastTimes;
    
    public ResultDict CastResult;

    public CastDetails(
        StageEnvironment env,
        StageEntity caster,
        StageSkill skill,
        bool recursive,
        bool fromWanJian,
        bool isStartStage,
        int startStageCastTimes,
        ResultDict castResult)
    {
        Env = env;
        Caster = caster;
        Skill = skill;
        Recursive = recursive;
        FromWanJian = fromWanJian;
        IsStartStage = isStartStage;
        StartStageCastTimes = startStageCastTimes;
        CastResult = castResult;
    }

    public int J => Skill.GetJingJie();
    public int Dj => Skill.Dj;
    public int Cc => Skill.TotalStageCastedCount;

    public void Clear()
    {
        CastResult.Clear();
    }

    public async UniTask AttackProcedure(int value,
        int times = 1,
        WuXing? wuXing = null,
        bool recursive = true,
        StageClosure[] closures = null,
        bool induced = false)
        => await Env.AttackProcedure(new AttackDetails(src: Caster, tgt: Caster.Opponent(), value, times, Skill, wuxing: wuXing ?? Skill.Entry.WuXing,
            crit: false, lifeSteal: false, penetrate: false, doesntConsumeJianYi: false, shatter: false, evade: false, recursive: recursive, castResult: CastResult, closures: closures, induced: induced));

    public async UniTask IndirectProcedure(
        int value,
        WuXing? wuXing = null,
        bool lifeSteal = false,
        bool recursive = true,
        bool induced = false)
        => await Env.IndirectProcedure(new IndirectDetails(Caster, Caster.Opponent(), value, Skill, wuXing ?? Skill.Entry.WuXing, lifeSteal, recursive, CastResult, induced));

    public async UniTask DamageSelfProcedure(
        int value,
        bool recursive = true,
        bool induced = false)
        => await Env.DamageProcedure(new DamageDetails(Caster, Caster, value, crit: false, lifeSteal: false, false, recursive, Skill, null, CastResult, induced));

    public async UniTask DamageOppoProcedure(int value,
        bool recursive = true,
        bool induced = false)
        => await Env.DamageProcedure(new DamageDetails(Caster, Caster.Opponent(), value, crit: false, lifeSteal: false, false, recursive, Skill, null, CastResult, induced));

    public async UniTask LoseHealthProcedure(int value, bool causedByAttack, bool induced)
        => await Env.LoseHealthProcedure(new LoseHealthDetails(Caster, value, causedByAttack, induced));

    public async UniTask RemoveHealthProcedure(int value, bool induced)
        => await Env.LoseHealthProcedure(new LoseHealthDetails(Caster.Opponent(), value, false, induced));

    public async UniTask HealProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Caster, Caster, value, false, Skill, CastResult, null, induced));

    public async UniTask HealOppoProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Caster, Caster.Opponent(), value, false, Skill, CastResult, null, induced));

    public async UniTask GainArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Caster, Caster, value, Skill, CastResult, null, induced));

    public async UniTask GiveArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Caster, Caster.Opponent(), value, Skill, CastResult, null, induced));

    public async UniTask LoseArmorProcedure(int value, bool induced)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Caster, Caster, value, Skill, null, CastResult, induced));

    public async UniTask RemoveArmorProcedure(int value, bool induced)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Caster, Caster.Opponent(), value, Skill, null, CastResult, induced));

    public async UniTask GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(new GainBuffDetails(Caster, Caster, buffEntry, stack, recursive, Skill, CastResult, null, induced));

    public async UniTask GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(new GainBuffDetails(Caster, Caster.Opponent(), buffEntry, stack, recursive, Skill, CastResult, null, induced));

    public async UniTask LoseBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(new LoseBuffDetails(Caster, Caster, buffEntry, stack, recursive, induced));

    public async UniTask RemoveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(new LoseBuffDetails(Caster, Caster.Opponent(), buffEntry, stack, recursive, induced));

    public async UniTask CycleProcedure(WuXing wuXing, bool rotate = true, int gain = 0, int recover = 0, bool induced = false)
        => await Env.CycleProcedure(new CycleDetails(Caster, rotate, wuXing, gain, recover, Skill, null, CastResult, induced));
    
    public async UniTask DispelProcedure(int stack, bool induced = false)
        => await Env.DispelProcedure(new DispelDetails(Caster, stack, induced));

    public async UniTask<bool> TryConsumeProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
    {
        if (stack == 0)
            return true;

        Buff b = Caster.FindBuff(buffEntry);
        if (b != null && b.Stack >= stack)
        {
            await LoseBuffProcedure(buffEntry, stack, recursive);
            return true;
        }

        return false;
    }

    public async UniTask TransferProcedure(
        int fromStack,
        BuffEntry fromBuff,
        int toStack,
        BuffEntry toBuff,
        bool consuming,
        int? maxFlow = null,
        int? upperBound = null)
    {
        int flow = Caster.GetStackOfBuff(fromBuff) / fromStack;
        if (upperBound.HasValue)
        {
            int gap = upperBound.Value - Caster.GetStackOfBuff(toBuff);
            if (gap >= 0)
                flow = flow.ClampUpper(gap);
        }
        
        if (maxFlow.HasValue)
            flow = flow.ClampUpper(maxFlow.Value);
        
        if (consuming)
            await LoseBuffProcedure(fromBuff, flow * fromStack);

        await GainBuffProcedure(toBuff, flow * toStack);
    }

    public async UniTask BecomeLowHealth(bool induced = false)
    {
        int gap = Caster.Hp - Caster.GetLowHealthThreshold();
        if (gap > 0)
            await Env.BurnProcedure(Caster, gap, induced);
    }

    public async UniTask<Tuple<bool, bool>> IsEnd(bool allowDoubleEnd)
    {
        if (allowDoubleEnd)
        {
            if (Skill.IsEnd && await Caster.TryConsumeProcedure("终结"))
                return new(true, true);

            if (await Caster.TryConsumeProcedure("大终结"))
                return new(true, true);
        }
        
        if (Skill.IsEnd)
            return new(true, false);

        if (await Caster.TryConsumeProcedure("终结"))
            return new(true, false);

        return new Tuple<bool, bool>(false, false);
    }
}
