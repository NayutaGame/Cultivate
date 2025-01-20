
using Cysharp.Threading.Tasks;
using CLLibrary;

public class CastDetails : ClosureDetails
{
    public StageEnvironment Env;
    public StageEntity Caster;
    public StageSkill Skill;
    public bool Recursive;
    public CastResult CastResult;

    public CastDetails(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive, CastResult castResult)
    {
        Env = env;
        Caster = caster;
        Skill = skill;
        Recursive = recursive;
        CastResult = castResult;
    }

    public int J => Skill.GetJingJie();
    public int Dj => Skill.Dj;
    public int Cc => Skill.StageCastedCount;

    public async UniTask AttackProcedure(int value,
        int times = 1,
        WuXing? wuXing = null,
        bool recursive = true,
        StageClosure[] closures = null,
        bool induced = false)
        => await Env.AttackProcedure(new AttackDetails(src: Caster, tgt: Caster.Opponent(), value, times, Skill, wuxing: wuXing ?? Skill.Entry.WuXing,
            crit: false, lifeSteal: false, penetrate: false, evade: false, recursive: recursive, castResult: CastResult, closures: closures, induced: induced));

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
        => await Env.DamageProcedure(new DamageDetails(Caster, Caster, value, Skill, crit: false, lifeSteal: false, recursive, CastResult, induced));

    public async UniTask DamageOppoProcedure(int value,
        bool recursive = true,
        bool induced = false)
        => await Env.DamageProcedure(new DamageDetails(Caster, Caster.Opponent(), value, Skill, crit: false, lifeSteal: false, recursive, CastResult, induced));

    public async UniTask LoseHealthProcedure(int value, bool induced)
        => await Env.LoseHealthProcedure(new LoseHealthDetails(Caster, value, induced));

    public async UniTask HealProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Caster, Caster, value, false, induced));

    public async UniTask HealOppoProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Caster, Caster.Opponent(), value, false, induced));

    public async UniTask GainArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Caster, Caster, value, induced));

    public async UniTask GiveArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Caster, Caster.Opponent(), value, induced));

    public async UniTask LoseArmorProcedure(int value, bool induced)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Caster, Caster, value, induced));

    public async UniTask RemoveArmorProcedure(int value, bool induced)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Caster, Caster.Opponent(), value, induced));

    public async UniTask GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(new GainBuffDetails(Caster, Caster, buffEntry, stack, recursive, induced));

    public async UniTask GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(new GainBuffDetails(Caster, Caster.Opponent(), buffEntry, stack, recursive, induced));

    public async UniTask LoseBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(new LoseBuffDetails(Caster, Caster, buffEntry, stack, recursive, induced));

    public async UniTask RemoveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(new LoseBuffDetails(Caster, Caster.Opponent(), buffEntry, stack, recursive, induced));

    public async UniTask CycleProcedure(WuXing wuXing, int gain = 0, int recover = 0, bool induced = false)
        => await Env.CycleProcedure(new CycleDetails(Caster, wuXing, gain, recover, induced));
    
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

    public async UniTask TransferProcedure(int fromStack, BuffEntry fromBuff, int toStack, BuffEntry toBuff, bool consuming, int? maxFlow = null, int? upperBound = null)
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

    public async UniTask<bool> JiaShiProcedure()
    {
        if (Caster.GetStackOfBuff("架势") > 0)
        {
            await LoseBuffProcedure("架势");
            Caster.TriggeredJiaShiRecord = true;
            return true;
        }

        // if (await IsFocused())
        // {
        //     TriggeredJiaShiRecord = true;
        //     return true;
        // }

        await GainBuffProcedure("架势");
        return false;
    }

    public async UniTask BecomeLowHealth(bool induced = false)
    {
        int gap = Caster.Hp - Caster.GetLowHealthThreshold();
        if (gap > 0)
            await LoseHealthProcedure(gap, induced);
    }
}
