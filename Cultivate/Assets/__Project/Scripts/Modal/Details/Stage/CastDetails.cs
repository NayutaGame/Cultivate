
using Cysharp.Threading.Tasks;
using CLLibrary;

public class CastDetails : StageClosureDetails
{
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
        ResultDict castResult) : base(env)
    {
        Caster = caster;
        Skill = skill;
        Recursive = recursive;
        FromWanJian = fromWanJian;
        IsStartStage = isStartStage;
        StartStageCastTimes = startStageCastTimes;
        CastResult = castResult;
    }

    public CastDetails Clone()
        => new(Env, Caster, Skill, Recursive, FromWanJian, IsStartStage, StartStageCastTimes, CastResult);

    public int J => Skill.GetJingJie();
    public int Dj => Skill.Dj;
    public int Cc => Skill.TotalStageCastedCount;

    public void Clear()
    {
        CastResult.Clear();
    }

    public async UniTask AttackProcedure(int value,
        int times = 1,
        WuXing wuXing = null,
        bool recursive = true,
        StageClosure[] closures = null,
        bool induced = false)
        => await Env.AttackProcedure(AttackDetails.FromCastDetails(this, value, times, wuXing, recursive, closures, induced));

    public async UniTask IndirectProcedure(
        int value,
        WuXing wuXing = null,
        bool lifeSteal = false,
        bool recursive = true,
        bool induced = false)
        => await Env.IndirectProcedure(new IndirectDetails(Env, Caster, Caster.Opponent(), value, Skill, wuXing ?? Skill.Entry.WuXing, lifeSteal, recursive, CastResult, induced));

    public async UniTask DamageSelfProcedure(
        int value,
        bool recursive = true,
        bool induced = false)
        => await Env.DamageProcedure(DamageDetails.FromCastDetails(this, true, value, recursive, induced));

    public async UniTask DamageOppoProcedure(int value,
        bool recursive = true,
        bool induced = false)
        => await Env.DamageProcedure(DamageDetails.FromCastDetails(this, false, value, recursive, induced));

    public async UniTask LoseHealthProcedure(int value, bool causedByAttack, bool induced)
        => await Env.LoseHealthProcedure(new LoseHealthDetails(Env, Caster, value, causedByAttack, Skill, null, CastResult, false, induced));

    public async UniTask RemoveHealthProcedure(int value, bool induced)
        => await Env.LoseHealthProcedure(new LoseHealthDetails(Env, Caster.Opponent(), value, false, Skill, null, CastResult, false, induced));

    public async UniTask HealProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Env, Caster, Caster, value, false, Skill, null, CastResult, false, induced));

    public async UniTask HealOppoProcedure(int value, bool induced)
        => await Env.HealProcedure(new HealDetails(Env, Caster, Caster.Opponent(), value, false, Skill, null, CastResult, false, induced));

    public async UniTask GainArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Env, Caster, Caster, value, Skill, null, CastResult, false, induced));

    public async UniTask GiveArmorProcedure(int value, bool induced)
        => await Env.GainArmorProcedure(new GainArmorDetails(Env, Caster, Caster.Opponent(), value, Skill, null, CastResult, false, induced));

    public async UniTask LoseArmorProcedure(int value, bool induced)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Env, Caster, Caster, value, Skill, null, CastResult, false, induced, true));

    public async UniTask RemoveArmorProcedure(int value, bool induced)
        => await Env.LoseArmorProcedure(new LoseArmorDetails(Env, Caster, Caster.Opponent(), value, Skill, null, CastResult, false, induced, true));

    public async UniTask GainBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(GainBuffDetails.FromCastDetails(
            this, true, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(GainBuffDetails.FromCastDetails(
            this, true, buffEntry, stack, recursive, induced));
    public async UniTask GiveBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(GainBuffDetails.FromCastDetails(
            this, false, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.GainBuffProcedure(GainBuffDetails.FromCastDetails(
            this, false, buffEntry, stack, recursive, induced));
    public async UniTask LoseBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(LoseBuffDetails.FromCastDetails(
            this, true, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask LoseBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(LoseBuffDetails.FromCastDetails(
            this, true, buffEntry, stack, recursive, induced));
    public async UniTask RemoveBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(LoseBuffDetails.FromCastDetails(
            this, false, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask RemoveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await Env.LoseBuffProcedure(LoseBuffDetails.FromCastDetails(
            this, false, buffEntry, stack, recursive, induced));

    public async UniTask CycleProcedure(WuXing wuXing, bool rotate = true, int gain = 0, int recover = 0, bool induced = false)
        => await Env.CycleProcedure(new CycleDetails(Env, Caster, rotate, wuXing, gain, recover, Skill, null, CastResult, false, induced));
    
    public async UniTask DispelProcedure(int stack, bool induced = false)
        => await Env.DispelProcedure(new DispelDetails(Env, Caster, stack, Skill, null, CastResult, false, induced));

    public async UniTask<bool> TryConsumeProcedure(string buffName, int stack = 1, bool recursive = true)
        => await TryConsumeProcedure(Encyclopedia.BuffCategory.FromName(buffName), stack, recursive);
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

    public async UniTask BecomeLowHealth(bool induced = false)
    {
        int gap = Caster.Hp - Caster.GetLowHealthThreshold();
        if (gap > 0)
            await Env.BurnProcedure(BurnDetails.FromBecomeLowHealth(Env, Caster, gap, induced));
    }
}
