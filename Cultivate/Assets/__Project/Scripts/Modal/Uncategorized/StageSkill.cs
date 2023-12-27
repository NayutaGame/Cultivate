
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StageSkill
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;
    private EmulatedSkill _runSkill;
    private SkillEntry _entry;
    public SkillEntry Entry => _entry;

    private int _slotIndex;
    public int SlotIndex => _slotIndex;

    private bool _exhausted;
    public bool Exhausted
    {
        get => _exhausted;
        set => _exhausted = value;
    }

    public async Task ExhaustProcedure()
        => await _owner.Env.ExhaustProcedure(_owner, this);

    public string GetAnnotatedDescription(string evalutated = null)
        => _entry.GetAnnotatedDescription(evalutated ?? GetDescription());

    public string GetDescription()
        => _entry.Evaluate(GetJingJie(), GetJingJie() - _entry.JingJieRange.Start);

    public SkillTypeComposite GetSkillTypeCollection()
        => _entry.SkillTypeComposite;

    public int GetLiteralCost()
        => _entry.GetManaCost(GetJingJie(), Dj, false);

    public int GetManaCost()
        => _entry.GetManaCost(GetJingJie(), Dj, JiaShi);

    public int GetChannelTime()
        => _entry.GetChannelTime(GetJingJie(), Dj, JiaShi);

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetAnnotationText()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _entry.GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>\n{annotation.GetAnnotatedDescription()}\n\n");

        return sb.ToString();
    }

    public string GetTrivia()
        => _entry.GetTrivia();

    public int RunUsedTimes { get; private set; }
    public int RunEquippedTimes { get; private set; }
    public int StageUsedTimes { get; private set; }

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie) => _jingJie = jingJie;

    public async Task TryUpgradeJingJie()
    {
        if (_jingJie != _entry.JingJieRange.End - 1)
            _jingJie += 1;
    }

    public int Dj
        => GetJingJie() - _entry.JingJieRange.Start;
    public bool JiaShi
        => Next(true).Entry.Name == "收刀" || Prev(true).Entry.Name == "拔刀" || _owner.GetStackOfBuff("天人合一") > 0;
    public bool IsFirstTime
        => StageUsedTimes == 0;
    public bool IsOdd
        => SlotIndex % 2 == 0 || _owner.GetStackOfBuff("森罗万象") > 0;
    public bool IsEven
        => SlotIndex % 2 == 1 || _owner.GetStackOfBuff("森罗万象") > 0;
    public bool IsEnd
        => SlotIndex == _owner._skills.Length - 1;
    public bool NoOtherAttack
        => _owner._skills.All(wg => wg == this || !wg.GetSkillType().Contains(SkillType.Attack));
    public bool NoOtherLingQi
        => _owner._skills.All(wg => wg == this || !wg.GetSkillType().Contains(SkillType.LingQi));
    public bool NoAttackAdjacents
        => !Prev(false).GetSkillType().Contains(SkillType.Attack) && !Next(false).GetSkillType().Contains(SkillType.Attack);

    public static StageSkill FromRunSkill(StageEntity owner, EmulatedSkill runSkill, int slotIndex)
        => new(owner, runSkill, runSkill.GetEntry(), runSkill.GetJingJie(), slotIndex);

    public static StageSkill FromSkillEntry(StageEntity owner, SkillEntry skillEntry, JingJie? jingJie = null, int slotIndex = 0)
        => new(owner, null, skillEntry, jingJie ?? skillEntry.JingJieRange.Start, slotIndex);

    private StageSkill(StageEntity owner, EmulatedSkill runSkill, SkillEntry skillEntry, JingJie jingJie, int slotIndex)
    {
        _owner = owner;
        _runSkill = runSkill;
        _entry = skillEntry;
        _jingJie = jingJie;
        _slotIndex = slotIndex;

        _exhausted = false;
        RunUsedTimes = _runSkill?.GetRunEquippedTimes() ?? 0;
        RunEquippedTimes = _runSkill?.GetRunEquippedTimes() + 1 ?? 0;
        StageUsedTimes = 0;
    }

    public string GetName()
        => _entry.Name;

    public SkillTypeComposite GetSkillType()
        => _entry.SkillTypeComposite;

    public async Task Channel(StageEntity caster, ChannelDetails d)
    {
        await caster.Env.EventDict.SendEvent(StageEventDict.WILL_CHANNEL, d.Clone());
        await _entry.Channel(caster, d);
        await caster.Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL, d.Clone());
    }

    public async Task ChannelWithoutTween(StageEntity caster, ChannelDetails d)
    {
        await caster.Env.EventDict.SendEvent(StageEventDict.WILL_CHANNEL, d);
        await _entry.ChannelWithoutTween(caster, d);
        await caster.Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL, d);
    }

    public async Task Execute(StageEntity caster, bool recursive = true)
    {
        await caster.Env.EventDict.SendEvent(StageEventDict.WILL_EXECUTE, new ExecuteDetails(caster, this));
        await _entry.Execute(caster, this, recursive);
        if (_owner == caster)
        {
            RunUsedTimes += 1;
            StageUsedTimes += 1;
        }
        await caster.Env.EventDict.SendEvent(StageEventDict.DID_EXECUTE, new ExecuteDetails(caster, this));
    }

    public async Task ExecuteWithoutTween(StageEntity caster, bool recursive = true)
    {
        await caster.Env.EventDict.SendEvent(StageEventDict.WILL_EXECUTE, new ExecuteDetails(caster, this));
        await _entry.ExecuteWithoutTween(caster, this, recursive);
        if (_owner == caster)
        {
            RunUsedTimes += 1;
            StageUsedTimes += 1;
        }
        await caster.Env.EventDict.SendEvent(StageEventDict.DID_EXECUTE, new ExecuteDetails(caster, this));
    }

    public IEnumerable<StageSkill> Nexts(bool loop = false)
    {
        StageSkill curr = this;
        for (int i = 0; i < _owner._skills.Length - 1; i++)
        {
            curr = curr.Next(loop);
            if (curr == null)
                yield break;

            yield return curr;
        }
    }

    public IEnumerable<StageSkill> Prevs(bool loop = false)
    {
        StageSkill curr = this;
        for (int i = 0; i < _owner._skills.Length - 1; i++)
        {
            curr = curr.Prev(loop);
            if (curr == null)
                yield break;

            yield return curr;
        }
    }

    public StageSkill Next(bool loop)
    {
        int index = _slotIndex + 1;
        if (loop)
            index %= _owner._skills.Length;

        if (index >= _owner._skills.Length)
            return null;

        return _owner._skills[index];
    }

    public StageSkill Prev(bool loop)
    {
        int index = _slotIndex - 1;
        if (loop)
            index = (index + _owner._skills.Length) % _owner._skills.Length;

        if (index < 0)
            return null;

        return _owner._skills[index];
    }
}
