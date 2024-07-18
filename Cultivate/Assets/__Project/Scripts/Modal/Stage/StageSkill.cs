
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class StageSkill
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;
    private SkillEntry _entry;
    public SkillEntry Entry => _entry;

    private int _slotIndex;
    public int SlotIndex => _slotIndex;

    public SkillSlot GetSlot()
        => _owner.RunEntity.GetSlot(_owner._p + 0);

    private bool _exhausted;
    public bool Exhausted
    {
        get => _exhausted;
        set => _exhausted = value;
    }

    public async Task ExhaustProcedure()
        => await _owner.Env.ExhaustProcedure(_owner, this);

    public int RunCastedCount { get; private set; }
    public int RunEquippedTimes { get; private set; }
    public int StageCastedCount { get; private set; }

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;

    public async Task<bool> TryUpgradeJingJie()
    {
        if (_jingJie == _entry.HighestJingJie)
            return false;
        _jingJie += 1;
        return true;
    }

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

    public static StageSkill FromPlacedSkill(StageEntity owner, PlacedSkill placedSkill, int slotIndex)
        => new(owner, placedSkill.Entry, placedSkill.JingJie, slotIndex);

    public static StageSkill FromSkillEntry(StageEntity owner, SkillEntry skillEntry, JingJie? jingJie = null, int slotIndex = 0)
        => new(owner, skillEntry, jingJie ?? skillEntry.LowestJingJie, slotIndex);

    public StageSkill Clone()
        => new(_owner, _entry, _jingJie, _slotIndex, _exhausted, StageCastedCount);

    private StageSkill(
        StageEntity owner,
        SkillEntry skillEntry,
        JingJie jingJie,
        int slotIndex,
        bool exhausted = false,
        int stageCastedCount = 0)
    {
        _owner = owner;
        _entry = skillEntry;
        _jingJie = jingJie;
        _slotIndex = slotIndex;
        _exhausted = exhausted;
        StageCastedCount = stageCastedCount;
    }

    public SkillTypeComposite GetSkillType()
        => _entry.GetSkillTypeComposite();

    public void IncreaseCastedCount()
    {
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
