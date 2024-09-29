
using System;
using System.Threading.Tasks;
using CLLibrary;

public class CastDetails : EventDetails
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

    public async Task AttackProcedure(int value,
        WuXing? wuXing = null,
        bool crit = false,
        bool lifeSteal = false,
        bool penetrate = false,
        bool recursive = true,
        Func<AttackDetails, CastResult, Task> wilAttack = null,
        Func<AttackDetails, CastResult, Task> didAttack = null,
        Func<DamageDetails, CastResult, Task> wilDamage = null,
        Func<DamageDetails, CastResult, Task> undamaged = null,
        Func<DamageDetails, CastResult, Task> didDamage = null,
        int times = 1,
        bool induced = false)
        => await Env.AttackProcedure(new AttackDetails(Caster, Caster.Opponent(), value, Skill, wuXing ?? Skill.Entry.WuXing, crit, lifeSteal, penetrate, false, recursive, wilAttack, didAttack, wilDamage, undamaged, didDamage), times, CastResult, induced);

    public async Task IndirectProcedure(
        int value,
        WuXing? wuXing = null,
        bool recursive = true,
        bool induced = false)
        => await Env.IndirectProcedure(new IndirectDetails(Caster, Caster.Opponent(), value, Skill, wuXing ?? Skill.Entry.WuXing, recursive), CastResult, induced);

    public async Task DamageSelfProcedure(
        int value,
        bool recursive = true,
        Func<DamageDetails, CastResult, Task> wilDamage = null,
        Func<DamageDetails, CastResult, Task> undamaged = null,
        Func<DamageDetails, CastResult, Task> didDamage = null,
        bool induced = false)
        => await Env.DamageProcedure(new DamageDetails(Caster, Caster, value, Skill, crit: false, lifeSteal: false, recursive, wilDamage, undamaged, didDamage), CastResult, induced);

    public async Task DamageOppoProcedure(int value,
        bool recursive = true,
        Func<DamageDetails, CastResult, Task> wilDamage = null,
        Func<DamageDetails, CastResult, Task> undamaged = null,
        Func<DamageDetails, CastResult, Task> didDamage = null,
        bool induced = false)
        => await Env.DamageProcedure(new DamageDetails(Caster, Caster.Opponent(), value, Skill, crit: false, lifeSteal: false, recursive, wilDamage, undamaged, didDamage), CastResult, induced);

    public async Task LoseHealthProcedure(int value)
        => await Env.LoseHealthProcedure(new LoseHealthDetails(Caster, value));

    public async Task HealProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Caster, Caster, value), induced);

    public async Task HealOppoProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Caster, Caster.Opponent(), value), induced);

    public async Task GainArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Caster, Caster, value), induced);

    public async Task GiveArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Caster, Caster.Opponent(), value), induced);

    public async Task LoseArmorProcedure(int value)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Caster, Caster, value));

    public async Task RemoveArmorProcedure(int value)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Caster, Caster.Opponent(), value));

    public async Task GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(new GainBuffDetails(Caster, Caster, buffEntry, stack, recursive), induced);

    public async Task GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(new GainBuffDetails(Caster, Caster.Opponent(), buffEntry, stack, recursive), induced);

    public async Task LoseBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await Env.LoseBuffProcedure(new LoseBuffDetails(Caster, Caster, buffEntry, stack, recursive));

    public async Task RemoveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await Env.LoseBuffProcedure(new LoseBuffDetails(Caster, Caster.Opponent(), buffEntry, stack, recursive));

    public async Task FormationProcedure(RunFormation runFormation, bool recursive = true)
        => await Env.FormationProcedure(Caster, runFormation, recursive);

    public async Task CycleProcedure(WuXing wuXing, int gain = 0, int recover = 0, bool induced = false)
        => await Env.CycleProcedure(Caster, wuXing, gain, recover, induced);
    
    public async Task DispelProcedure(int stack)
        => await Env.DispelProcedure(Caster, stack);

    public async Task<bool> TryConsumeProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
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

    public async Task TransferProcedure(int fromStack, BuffEntry fromBuff, int toStack, BuffEntry toBuff, bool consuming, int? maxFlow = null, int? upperBound = null)
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

    public async Task<bool> JiaShiProcedure()
    {
        // if (GetStackOfBuff("天人合一") > 0)
        // {
        //     TriggeredJiaShiRecord = true;
        //     return true;
        // }

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

    public async Task BecomeLowHealth()
    {
        int gap = Caster.Hp - Caster.GetLowHealthThreshold();
        if (gap > 0)
            await LoseHealthProcedure(gap);
    }
}
