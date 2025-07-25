
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

public class StageSkill : StageClosureListener
{
    private readonly StageEntity _owner;
    public StageEntity Owner => _owner;

    private int _slotIndex;
    public int SlotIndex
    {
        get => _slotIndex;
        set => _slotIndex = value;
    }

    private readonly int? _runSlotIndex;
    public SkillSlot GetSlot()
        => _runSlotIndex == null ? null : _owner.RunEntity.GetSlot(_runSlotIndex.Value + 0);
    public SkillDefinition GetSkillDefinition()
        => GetSlot()?.Skill?.GetSkillDefinitionFromDj(Dj) ?? Entry.GetSkillDefinitionFromDj(Dj);
    
    private readonly SkillEntry _entry;
    public SkillEntry Entry => _entry;

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public async UniTask<bool> TryUpgradeJingJie()
    {
        if (_jingJie >= _entry.HighestJingJie)
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
    public async UniTask ExhaustProcedure()
        => await _owner.Env.ExhaustProcedure(_owner, this);

    private int _realStageCastedCount;
    public void SetRealStageCastedCount(int value) => _realStageCastedCount = value;
    public void IncreaseRealCastedCount() => _realStageCastedCount += 1;
    private int _bonusStageCastedCount;
    public void SetBonusStageCastedCount(int value) => _bonusStageCastedCount = value;
    public void IncreaseBonusCastedCount() => _bonusStageCastedCount += 1;

    public int TotalStageCastedCount => _realStageCastedCount + _bonusStageCastedCount;

    private StageSkill(
        StageEntity owner,
        int slotIndex,
        int? runSlotIndex,
        SkillEntry skillEntry,
        JingJie jingJie,
        bool exhausted = false,
        int realStageCastedCount = 0,
        int bonusStageCastedCount = 0)
    {
        _owner = owner;
        _slotIndex = slotIndex;
        _runSlotIndex = runSlotIndex;
        _entry = skillEntry;
        _jingJie = jingJie;
        _exhausted = exhausted;
        _realStageCastedCount = realStageCastedCount;
        _bonusStageCastedCount = bonusStageCastedCount;
    }

    public static StageSkill FromPlacedSkill(StageEntity owner, int slotIndex, PlacedSkill placedSkill)
        => new(owner, slotIndex, slotIndex, placedSkill.Entry, placedSkill.JingJie);

    public static StageSkill FromSkillEntry(StageEntity owner, SkillEntry skillEntry, JingJie? jingJie = null, int slotIndex = 0)
        => new(owner, slotIndex, null, skillEntry, jingJie ?? skillEntry.LowestJingJie);

    public StageSkill Clone()
        => new(_owner, _slotIndex, _runSlotIndex, _entry, _jingJie, _exhausted, _realStageCastedCount, _bonusStageCastedCount);

    public TagComposite GetTagComposite()
        => _entry.GetTagComposite();
    public int J
        => GetJingJie();
    public int Dj
        => GetJingJie() - _entry.LowestJingJie;
    public bool IsOdd
        => SlotIndex % 2 == 0 || _owner.GetStackOfBuff("森罗万象") > 0;
    public bool IsEven
        => SlotIndex % 2 == 1 || _owner.GetStackOfBuff("森罗万象") > 0;

    public bool IsFirstTime
    {
        get {
            bool isFirstTime = _realStageCastedCount == 0;

            if (isFirstTime)
                _owner.TriggeredFirstTimeRecord = true;
            return isFirstTime;
        }
    }

    public bool IsNotFirstTime
        => TotalStageCastedCount > 0;

    public bool IsEnd
    {
        get
        {
            bool lianYue = _owner.GetStackOfBuff("连岳") > 0;

            int slot = 0;
            
            int capacity = lianYue ? 2 : 1;
            for (int i = _owner._skills.Length - 1; i >= 0; i--)
            {
                StageSkill skill = _owner._skills[i];
                if (skill.Exhausted)
                    continue;
                capacity--;
                if (capacity == 0)
                {
                    slot = i;
                    break;
                }
            }
            
            return SlotIndex >= slot;
        }
    }
    
    public bool NoOtherAttack
        => _owner._skills.All(skill => skill == this || !skill.GetTagComposite().Contains(TagCategory.Attack) || skill._exhausted);
    public bool NoOtherLingQi
        => _owner._skills.All(skill => skill == this || !skill.GetTagComposite().Contains(TagCategory.Mana));
    public bool NoAttackAdjacents
        => !PrevSkill(false).GetTagComposite().Contains(TagCategory.Attack) && !NextSkill(false).GetTagComposite().Contains(TagCategory.Attack);

    public IEnumerable<StageSkill> NextSkills(bool loop = false)
    {
        foreach (var ret in _owner.NextSkills(_slotIndex, loop))
            yield return ret;
    }

    public IEnumerable<StageSkill> PrevSkills(bool loop = false)
    {
        foreach (var ret in _owner.PrevSkills(_slotIndex, loop))
            yield return ret;
    }

    public StageSkill NextSkill(bool loop)
        => _owner.NextSkill(_slotIndex, loop);

    public StageSkill PrevSkill(bool loop)
        => _owner.PrevSkill(_slotIndex, loop);

    public override string ToString()
    {
        return $"{_jingJie} {_entry.GetName()}";
    }
}
