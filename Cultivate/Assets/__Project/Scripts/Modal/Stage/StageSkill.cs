
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StageSkill
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;
    private SkillEntry _entry;
    public SkillEntry Entry => _entry;

    private int _slotIndex;
    public int SlotIndex => _slotIndex;

    public SkillSlot GetSlot()
    {
        RunEntity entity = _owner.RunEntity;
        int index = _owner._p + entity.Start;
        return entity.GetSlot(index);
    }

    private bool _exhausted;
    public bool Exhausted
    {
        get => _exhausted;
        set => _exhausted = value;
    }

    public async Task ExhaustProcedure()
        => await _owner.Env.ExhaustProcedure(_owner, this);

    public SkillTypeComposite GetSkillTypeCollection()
        => _entry.SkillTypeComposite;

    public string GetName()
        => _entry.GetName();

    public string GetHighlight(CastResult castResult)
        => _entry.GetHighlight(GetJingJie(), castResult);

    public string GetExplanation()
        => _entry.GetExplanation();

    public string GetTrivia()
        => _entry.GetTrivia();

    public int RunCastedCount { get; private set; }
    public int RunEquippedTimes { get; private set; }
    public int StageCastedCount { get; private set; }

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie) => _jingJie = jingJie;

    public async Task TryUpgradeJingJie()
    {
        if (_jingJie != _entry.HighestJingJie)
            _jingJie += 1;
    }

    public int Dj
        => GetJingJie() - _entry.LowestJingJie;
    public bool IsFirstTime
        => StageCastedCount == 0;
    public bool IsOdd
        => SlotIndex % 2 == 0 || _owner.GetStackOfBuff("森罗万象") > 0;
    public bool IsEven
        => SlotIndex % 2 == 1 || _owner.GetStackOfBuff("森罗万象") > 0;
    public bool IsEnd
        => SlotIndex == _owner._skills.Length - 1;
    public bool NoOtherAttack
        => _owner._skills.All(skill => skill == this || !skill.GetSkillType().Contains(SkillType.Attack));
    public bool NoOtherLingQi
        => _owner._skills.All(skill => skill == this || !skill.GetSkillType().Contains(SkillType.LingQi));
    public bool NoAttackAdjacents
        => !Prev(false).GetSkillType().Contains(SkillType.Attack) && !Next(false).GetSkillType().Contains(SkillType.Attack);

    public static StageSkill FromPlacedSkill(StageEntity owner, PlacedSkill placedSkill, int slotIndex)
        => new(owner, placedSkill, placedSkill.Entry, placedSkill.JingJie, slotIndex);

    public static StageSkill FromSkillEntry(StageEntity owner, SkillEntry skillEntry, JingJie? jingJie = null, int slotIndex = 0)
        => new(owner, null, skillEntry, jingJie ?? skillEntry.LowestJingJie, slotIndex);

    private StageSkill(StageEntity owner, PlacedSkill placedSkill, SkillEntry skillEntry, JingJie jingJie, int slotIndex)
    {
        _owner = owner;
        _entry = skillEntry;
        _jingJie = jingJie;
        _slotIndex = slotIndex;

        _exhausted = false;
        RunCastedCount = placedSkill?.RunSkill?.GetRunUsedTimes() ?? 0;
        RunEquippedTimes = placedSkill?.RunSkill?.GetRunEquippedTimes() + 1 ?? 0;
        StageCastedCount = 0;
    }

    public SkillTypeComposite GetSkillType()
        => _entry.SkillTypeComposite;

    public void IncreaseCastedCount()
    {
        RunCastedCount += 1;
        StageCastedCount += 1;
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
