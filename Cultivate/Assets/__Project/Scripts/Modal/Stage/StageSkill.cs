
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class StageSkill
{
    private readonly StageEntity _owner;
    public StageEntity Owner => _owner;

    private readonly int _slotIndex;
    public int SlotIndex => _slotIndex;

    private int _runSlotIndex;
    public int RunSlotIndex
    {
        get => _runSlotIndex;
        set => _runSlotIndex = value;
    }
    public SkillSlot GetSlot()
        => _owner.RunEntity.GetSlot(_runSlotIndex + 0);
    
    private readonly SkillEntry _entry;
    public SkillEntry Entry => _entry;

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public async Task<bool> TryUpgradeJingJie()
    {
        if (_jingJie == _entry.HighestJingJie)
            return false;
        _jingJie += 1;
        return true;
    }

    private bool _exhausted;
    public bool Exhausted
    {
        get => _exhausted;
        set => _exhausted = value;
    }
    public async Task ExhaustProcedure()
        => await _owner.Env.ExhaustProcedure(_owner, this);

    public int StageCastedCount { get; private set; }
    public void IncreaseCastedCount() => StageCastedCount += 1;

    public static StageSkill FromPlacedSkill(StageEntity owner, int slotIndex, PlacedSkill placedSkill)
        => new(owner, slotIndex, slotIndex, placedSkill.Entry, placedSkill.JingJie);

    public static StageSkill FromSkillEntry(StageEntity owner, SkillEntry skillEntry, JingJie? jingJie = null, int slotIndex = 0)
        => new(owner, slotIndex, slotIndex, skillEntry, jingJie ?? skillEntry.LowestJingJie);

    public StageSkill Clone()
        => new(_owner, _slotIndex, _runSlotIndex, _entry, _jingJie, _exhausted, StageCastedCount);

    private StageSkill(
        StageEntity owner,
        int slotIndex,
        int runSlotIndex,
        SkillEntry skillEntry,
        JingJie jingJie,
        bool exhausted = false,
        int stageCastedCount = 0)
    {
        _owner = owner;
        _slotIndex = slotIndex;
        _runSlotIndex = runSlotIndex;
        _entry = skillEntry;
        _jingJie = jingJie;
        _exhausted = exhausted;
        StageCastedCount = stageCastedCount;
    }

    public SkillTypeComposite GetSkillType()
        => _entry.GetSkillTypeComposite();
    public int Dj
        => GetJingJie() - _entry.LowestJingJie;
    public bool IsOdd
        => SlotIndex % 2 == 0 || _owner.GetStackOfBuff("森罗万象") > 0;
    public bool IsEven
        => SlotIndex % 2 == 1 || _owner.GetStackOfBuff("森罗万象") > 0;

    public async Task<bool> IsFirstTime(bool useFocus = false)
    {
        bool isFirstTime = StageCastedCount == 0;
        if (!isFirstTime)
            isFirstTime = useFocus && await _owner.IsFocused();

        if (isFirstTime)
            _owner.TriggeredFirstTimeRecord = true;
        return isFirstTime;
    }

    public async Task<bool> IsEnd(bool useFocus = false)
    {
        bool isEnd = SlotIndex == _owner._skills.Length - 1;
        if (!isEnd)
            isEnd = useFocus && await _owner.IsFocused();

        if (isEnd)
            _owner.TriggeredEndRecord = true;
        return isEnd;
    }
    
    public bool NoOtherAttack
        => _owner._skills.All(skill => skill == this || !skill.GetSkillType().Contains(SkillType.Attack));
    public bool NoOtherLingQi
        => _owner._skills.All(skill => skill == this || !skill.GetSkillType().Contains(SkillType.LingQi));
    public bool NoAttackAdjacents
        => !Prev(false).GetSkillType().Contains(SkillType.Attack) && !Next(false).GetSkillType().Contains(SkillType.Attack);

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
