using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawChipRewardDescriptor : RewardDescriptor
{
    private string _description;
    public Predicate<ChipEntry> _pred;
    public int _count;

    public DrawChipRewardDescriptor(string description, Predicate<ChipEntry> pred, int count = 1)
    {
        _description = description;
        _pred = pred;
        _count = count;
    }

    public override void Claim()
    {
        RunManager.Instance.DrawChip(_pred, _count);
    }

    public override string GetDescription() => _description;
}
