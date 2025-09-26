
public class LoseBuffDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;

    private LoseBuffDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        BuffEntry buffEntry,
        int stack,
        bool recursive,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Src = src;
        Tgt = tgt;
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
    }

    public static LoseBuffDetails FromCastDetails(CastDetails d, bool tgtIsSrc, BuffEntry buffEntry, int stack, bool recursive, bool induced)
        => new(
            env: d.Env,
            src: d.Caster,
            tgt: tgtIsSrc ? d.Caster : d.Caster.Opponent(),
            buffEntry: buffEntry,
            stack: stack,
            recursive: recursive,
            listener: d.Skill,
            closures: null,
            castResult: d.CastResult,
            closureHasRegistered: false,
            induced: induced);

    public static LoseBuffDetails FromEntity(StageEntity e, bool tgtIsSrc, BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => new(
            env: e.Env,
            src: e,
            tgt: tgtIsSrc ? e : e.Opponent(),
            buffEntry: buffEntry,
            stack: stack,
            recursive: recursive,
            listener: null,
            closures: null,
            castResult: null,
            closureHasRegistered: false,
            induced: induced);
    
    public static LoseBuffDetails FromBuff(Buff b, int stack)
        => new(
            env: b.Owner.Env,
            src: b.Owner,
            tgt: b.Owner,
            buffEntry: b.GetEntry(),
            stack: stack,
            recursive: true,
            listener: null,
            closures: null,
            castResult: null,
            closureHasRegistered: false,
            induced: true);
}
