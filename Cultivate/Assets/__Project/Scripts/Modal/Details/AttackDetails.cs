using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class AttackDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public int Times;
    public bool Recursive;

    public Action<DamageDetails> Damaged;
    public Action<DamageDetails> Undamaged;

    public bool Cancel;

    public AttackDetails(StageEntity src, StageEntity tgt, int value, int times, bool recursive = true,
        Action<DamageDetails> damaged = null,
        Action<DamageDetails> undamaged = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Times = times;
        Recursive = recursive;

        Damaged = damaged;
        Undamaged = undamaged;

        Cancel = false;
    }
}
