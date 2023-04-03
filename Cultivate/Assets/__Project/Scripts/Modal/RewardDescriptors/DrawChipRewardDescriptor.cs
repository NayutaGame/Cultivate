using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DrawChipRewardDescriptor : RewardDescriptor
{
    private string _description;
    public Predicate<ChipEntry> _pred;
    private JingJie _jingJie;
    public int _count;

    public DrawChipRewardDescriptor(string description, Predicate<ChipEntry> pred, JingJie jingJie, int count = 1)
    {
        _description = description;
        _pred = pred;
        _jingJie = jingJie;
        _count = count;
    }

    public override void Claim()
    {
        _count.Do(i => RunManager.Instance.TryDrawAcquired(_pred, _jingJie));
    }

    public override string GetDescription() => _description;
}
