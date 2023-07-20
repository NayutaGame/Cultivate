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

    private bool _consumed;
    public bool Consumed
    {
        get => _consumed;
        set => _consumed = value;
    }

    private bool _runConsumed;
    public bool RunConsumed
    {
        get => _runConsumed;
        set => _runConsumed = value;
    }

    public async Task ConsumeProcedure(bool forRun = false)
        => await _owner.Env.ConsumeProcedure(_owner, this, forRun);

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

    public JingJie GetJingJie()
    {
        if(_runSkill == null)
            return JingJie.LianQi;

        return _runSkill.JingJie;
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
        => SlotIndex == _owner._waiGongList.Length - 1;
    public bool NoOtherAttack
        => _owner._waiGongList.All(wg => wg == this || !wg.GetWaiGongType().Contains(SkillTypeCollection.Attack));
    public bool NoOtherLingQi
        => _owner._waiGongList.All(wg => wg == this || !wg.GetWaiGongType().Contains(SkillTypeCollection.LingQi));
    public bool NoAttackAdjacents
        => !Prev(false).GetWaiGongType().Contains(SkillTypeCollection.Attack) && !Next(false).GetWaiGongType().Contains(SkillTypeCollection.Attack);

    public StageSkill(StageEntity owner, RunSkill runSkill, int slotIndex)
    {
        _owner = owner;
        _runSkill = runSkill;
        _slotIndex = slotIndex;

        _entry = _runSkill?.Entry ?? Encyclopedia.SkillCategory["聚气术"];

        _consumed = false;
        _runConsumed = false;
        RunUsedTimes = _runSkill?.RunUsedTimes ?? 0;
        RunEquippedTimes = _runSkill?.RunEquippedTimes + 1 ?? 0;
        StageUsedTimes = 0;
    }

    public string GetName()
        => _entry.Name;

    public SkillTypeCollection GetWaiGongType()
        => _entry.SkillTypeCollection;

    public async Task Execute(StageEntity caster, bool recursive = true)
    {
        await _entry.Execute(caster, this, recursive);
        RunUsedTimes += 1;
        StageUsedTimes += 1;
    }

    public async Task ExecuteWithoutTween(StageEntity caster, bool recursive = true)
    {
        await _entry.ExecuteWithoutTween(caster, this, recursive);
        RunUsedTimes += 1;
        StageUsedTimes += 1;
    }

    public IEnumerable<StageSkill> Nexts(bool loop = false)
    {
        StageSkill curr = this;
        for (int i = 0; i < _owner._waiGongList.Length - 1; i++)
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
        for (int i = 0; i < _owner._waiGongList.Length - 1; i++)
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
            index %= _owner._waiGongList.Length;

        if (index >= _owner._waiGongList.Length)
            return null;

        return _owner._waiGongList[index];
    }

    public StageSkill Prev(bool loop)
    {
        int index = _slotIndex - 1;
        if (loop)
            index = (index + _owner._waiGongList.Length) % _owner._waiGongList.Length;

        if (index < 0)
            return null;

        return _owner._waiGongList[index];
    }
}
