using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class StageWaiGong
{
    private StageEntity _owner;
    private RunChip _runChip;
    private WaiGongEntry _entry;
    public WaiGongEntry Entry => _entry;

    private int _slotIndex;
    public int SlotIndex => _slotIndex;

    public bool Consumed;
    public bool RunConsumed;

    public int GetManaCost()
        => _entry.GetManaCost(Level, GetJingJie(), Dj, _powers);

    // public int RunLevel { get; private set; }
    public int Level { get; private set; }
    public int RunUsedTimes { get; private set; }
    public int RunEquippedTimes { get; private set; }
    public int StageUsedTimes { get; private set; }

    public JingJie GetJingJie()
    {
        if(_runChip == null)
            return JingJie.LianQi;

        return _runChip.JingJie;
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
        => _owner._waiGongList.All(wg => wg == this || !wg.GetWaiGongType().Contains(WaiGongType.Attack));
    public bool NoOtherLingQi
        => _owner._waiGongList.All(wg => wg == this || !wg.GetWaiGongType().Contains(WaiGongType.LingQi));
    public bool NoAttackAdjacents
        => !Prev(false).GetWaiGongType().Contains(WaiGongType.Attack) && !Next(false).GetWaiGongType().Contains(WaiGongType.Attack);

    // run powers

    private static readonly string[] PowerBuffNames = new string[] { "临金", "临水", "临木", "临火", "临土" };

    private int[] _powers;
    public int GetPower(WuXing wuXing) => _powers[wuXing] + _owner.GetStackOfBuff(PowerBuffNames[wuXing]);

    public StageWaiGong(StageEntity owner, RunChip runChip, int[] powers, int slotIndex)
    {
        _owner = owner;
        _runChip = runChip;
        _slotIndex = slotIndex;

        if (_runChip != null)
        {
            _entry = _runChip._entry as WaiGongEntry;
            Consumed = false;
            RunConsumed = false;
            // RunLevel = _runChip.Level;
            Level = _runChip.Level;
            RunUsedTimes = _runChip.RunUsedTimes;
            RunEquippedTimes = _runChip.RunEquippedTimes + 1;
            StageUsedTimes = 0;

            _powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => _powers[wuXing] = powers[wuXing]);
        }
        else
        {
            _entry = Encyclopedia.ChipCategory["聚气术"] as WaiGongEntry;
            Consumed = false;
            RunConsumed = false;
            // RunLevel = 0;
            Level = 0;
            RunUsedTimes = 0;
            RunEquippedTimes = 0;
            StageUsedTimes = 0;

            _powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => _powers[wuXing] = powers[wuXing]);
        }
    }

    public string GetName()
        => _entry.Name;

    public WaiGongType GetWaiGongType()
        => _entry.Type;

    public void Execute(StageEntity caster, bool recursive = true)
    {
        _entry.Execute(caster, this, recursive);
        RunUsedTimes += 1;
        StageUsedTimes += 1;
    }

    public IEnumerable<StageWaiGong> Nexts(bool loop = false)
    {
        StageWaiGong curr = this;
        for (int i = 0; i < _owner._waiGongList.Length - 1; i++)
        {
            curr = curr.Next(loop);
            if (curr == null)
                yield break;

            yield return curr;
        }
    }

    public IEnumerable<StageWaiGong> Prevs(bool loop = false)
    {
        StageWaiGong curr = this;
        for (int i = 0; i < _owner._waiGongList.Length - 1; i++)
        {
            curr = curr.Prev(loop);
            if (curr == null)
                yield break;

            yield return curr;
        }
    }

    public StageWaiGong Next(bool loop)
    {
        int index = _slotIndex + 1;
        if (loop)
            index %= _owner._waiGongList.Length;

        if (index >= _owner._waiGongList.Length)
            return null;

        return _owner._waiGongList[index];
    }

    public StageWaiGong Prev(bool loop)
    {
        int index = _slotIndex - 1;
        if (loop)
            index = (index + _owner._waiGongList.Length) % _owner._waiGongList.Length;

        if (index < 0)
            return null;

        return _owner._waiGongList[index];
    }

    public void Register()
    {
        WaiGongEntry entry = _runChip?._entry as WaiGongEntry;
        if (entry == null) return;

        if (entry._startStage != null) _owner.StartStageEvent += StartStage;
    }

    public void Unregister()
    {
        WaiGongEntry entry = _runChip?._entry as WaiGongEntry;
        if (entry == null) return;

        if (entry._startStage != null) _owner.StartStageEvent -= StartStage;
    }

    private void StartStage()
    {
        WaiGongEntry entry = _runChip?._entry as WaiGongEntry;
        if (entry == null) return;

        entry._startStage(_owner, this);
    }
}
