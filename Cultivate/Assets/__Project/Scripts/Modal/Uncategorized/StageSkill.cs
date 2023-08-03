using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLLibrary;
using DG.Tweening;
using UnityEngine;

public class StageSkill
{
    private StageEntity _owner;
    private RunSkill _runSkill;
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

    private bool _runExhausted;
    public bool RunExhausted
    {
        get => _runExhausted;
        set => _runExhausted = value;
    }

    public async Task ExhaustProcedure(bool forRun = false)
        => await _owner.Env.ExhaustProcedure(_owner, this, forRun);

    public string GetAnnotatedDescription(string evalutated = null)
        => _entry.GetAnnotatedDescription(evalutated ?? GetDescription());

    public string GetDescription()
        => _entry.Evaluate(GetJingJie(), GetJingJie() - _entry.JingJieRange.Start);

    public SkillTypeCollection GetSkillTypeCollection()
        => _entry.SkillTypeCollection;

    public int GetManaCost()
        => _entry.GetManaCost(GetJingJie(), Dj);

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetAnnotationText()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _entry.GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>  {annotation.GetAnnotatedDescription()}\n");

        return sb.ToString();
    }

    public int RunUsedTimes { get; private set; }
    public int RunEquippedTimes { get; private set; }
    public int StageUsedTimes { get; private set; }

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie) => _jingJie = jingJie;

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
        => _owner._skills.All(wg => wg == this || !wg.GetSkillType().Contains(SkillTypeCollection.Attack));
    public bool NoOtherLingQi
        => _owner._skills.All(wg => wg == this || !wg.GetSkillType().Contains(SkillTypeCollection.LingQi));
    public bool NoAttackAdjacents
        => !Prev(false).GetSkillType().Contains(SkillTypeCollection.Attack) && !Next(false).GetSkillType().Contains(SkillTypeCollection.Attack);

    public StageSkill(StageEntity owner, RunSkill runSkill, int slotIndex) : this(owner, runSkill, "聚气术", null, slotIndex) { }
    public StageSkill(StageEntity owner, SkillEntry skillEntry, JingJie jingJie, int slotIndex) : this(owner, null, skillEntry, jingJie, slotIndex) { }
    private StageSkill(StageEntity owner, RunSkill runSkill, SkillEntry skillEntry, JingJie? jingJie, int slotIndex)
    {
        _owner = owner;
        _runSkill = runSkill;
        _entry = _runSkill?.Entry ?? skillEntry;

        if (jingJie.HasValue) {
            _jingJie = jingJie.Value;
        } else if(_runSkill != null) {
            _jingJie = _runSkill.GetJingJie();
        } else {
            _jingJie = _entry.JingJieRange.Start;
        }

        _slotIndex = slotIndex;

        _exhausted = false;
        _runExhausted = false;
        RunUsedTimes = _runSkill?.RunUsedTimes ?? 0;
        RunEquippedTimes = _runSkill?.RunEquippedTimes + 1 ?? 0;
        StageUsedTimes = 0;
    }

    public string GetName()
        => _entry.Name;

    public SkillTypeCollection GetSkillType()
        => _entry.SkillTypeCollection;

    public async Task Execute(StageEntity caster, bool recursive = true)
    {
        await _entry.Execute(caster, this, recursive);
        if (_owner == caster)
        {
            RunUsedTimes += 1;
            StageUsedTimes += 1;
        }
    }

    public async Task ExecuteWithoutTween(StageEntity caster, bool recursive = true)
    {
        await _entry.ExecuteWithoutTween(caster, this, recursive);
        if (_owner == caster)
        {
            RunUsedTimes += 1;
            StageUsedTimes += 1;
        }
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
